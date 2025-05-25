import { Component, OnInit } from '@angular/core';
import { HttpService } from "./services/http.service";
import { defaultFilter, LegoFilter, LegoPiece, LegoSet, PieceStatus } from "./services/models";
import { FormBuilder } from "@angular/forms";
import { BehaviorSubject, debounce, debounceTime, map, switchMap, tap } from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'lego-checksheet';

  filter$ = new BehaviorSubject<LegoFilter>(defaultFilter);
  filteredPieces$ = new BehaviorSubject<LegoPiece[]>([]);
  missingCount$ = new BehaviorSubject<number>(0);

  setInfo: LegoSet | undefined;

  pieceArr = this.fb.array([]);
  ownedArr = this.fb.array([]);

  piecePartFilter = this.fb.control(null);
  piecePartStatus = this.fb.control('All');

  constructor(
    private readonly http: HttpService,
    private readonly fb: FormBuilder
  ) {
  }

  ngOnInit() {
    this.http.getSet()
      .subscribe(x => {
        this.setInfo = x;
        this.filteredPieces$.next(x.legoPieces);
        this.countMissing();
        for (let i = 0; i < x.legoPieces.length; i++){
          const p = x.legoPieces[i];
          p.index = i;
          const missingCtrl = this.fb.control(p.numberMissing || null);
          this.pieceArr.push(missingCtrl);
          missingCtrl.valueChanges.pipe(
            debounceTime(500),
            switchMap(x => this.http.setNumberMissing(p.legoPieceId, !x ? 0 : x).pipe(
              map(() => x)
            ))
          )
            .subscribe(x => {
              p.numberMissing = !x ? 0 : x;
              this.countMissing();
            });

          const ownedCtrl = this.fb.control(p.owned);
          this.ownedArr.push(ownedCtrl);
          ownedCtrl.valueChanges.pipe(
            switchMap(x => this.http.toggleOwned(p.legoPieceId, !!x).pipe(map(() => x)))
          ).subscribe(x => {
            p.owned = !!x;
            if(x) {
              p.numberMissing = 0;
              this.pieceArr.controls[i].setValue(0, { emitEvent: false });
            }
          });
        }
      });

    this.filter$.subscribe(x => {
      if(!this.setInfo) {
        return;
      }

      let filtered = [...this.setInfo.legoPieces];
      if(x.pieceNumber) {
        filtered = filtered.filter(p => p.legoPieceId.toString().startsWith(x.pieceNumber));
      }

      if(x.status !== 'All') {
        if(x.status === 'Owned') {
          filtered = filtered.filter(x => x.owned);
        } else if(x.status === 'Missing') {
          filtered = filtered.filter(x => x.numberMissing > 0);
        }
      }

      console.log(filtered);

      this.filteredPieces$.next(filtered);
    });

    this.piecePartFilter.valueChanges.subscribe(x => {
      this.filter$.next({
        ...this.filter$.value,
        pieceNumber: x ?? ''
      })
    });

    this.piecePartStatus.valueChanges.subscribe(x => {
      this.filter$.next({
        ...this.filter$.value,
        status: x as PieceStatus ?? 'All'
      })
    });
  }

  trackBy(idx: number, item: LegoPiece){
    return item.legoPieceId;
  }

  toggleMissing(part: LegoPiece, index: number) {
    this.ownedArr.controls[index].setValue(!part.owned);
  }

  missingAll(part: LegoPiece, index: number) {
    this.pieceArr.controls[index].setValue(part.quantity);
  }

  private countMissing(){
    const missing = this.setInfo?.legoPieces.reduce((agg, x) => agg + x.numberMissing, 0) ?? 0;
    this.missingCount$.next(missing);
  }
}
