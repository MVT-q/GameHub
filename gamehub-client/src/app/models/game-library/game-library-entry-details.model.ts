import { GameGenre } from '../../enums/game-genre';
import { GameProgressStatus } from '../../enums/game-progress-status';

export interface GameLibraryEntryDetails {
  id: number;
  gameId: number;
  title: string;
  genre: GameGenre;
  status: GameProgressStatus;
  rating?: number;
  addedAt: string;
}
