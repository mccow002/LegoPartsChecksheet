export type LegoSet = {
  legoPieces: LegoPiece[];
}

export type LegoPiece = {
  legoPieceId: number;
  name: string;
  imageUrl: string;
  quantity: number;
  elementId: string;
  owned: boolean;
  numberMissing: number;
  index: number;
}

export type PieceStatus = 'Owned' | 'Missing' | 'Own None' | 'All';

export type LegoFilter = {
  pieceNumber: string;
  status: PieceStatus;
};

export const defaultFilter: LegoFilter = {
  pieceNumber: '',
  status: 'All'
};
