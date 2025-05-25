import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { LegoSet } from "./models";
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(
    private readonly http: HttpClient
  ) { }

  public getSet(): Observable<LegoSet> {
    return this.http.get<LegoSet>(`${environment.url}/lego-set`)
  }

  public toggleOwned(pieceId: number, owned: boolean): Observable<void>
  {
    return this.http.put<void>(`${environment.url}/lego-pieces/toggle-owned`, {
      legoPieceId: pieceId,
      owned: owned
    });
  }

  public setNumberMissing(pieceId: number, missing: number): Observable<void>
  {
    return this.http.put<void>(`${environment.url}/lego-pieces/number-missing`, {
      legoPieceId: pieceId,
      numberMissing: missing
    });
  }
}
