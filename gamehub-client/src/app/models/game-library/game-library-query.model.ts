import { GameGenre } from '../../enums/game-genre';
import { GameProgressStatus } from '../../enums/game-progress-status';

export interface GameLibraryQuery {
  search?: string;
  genre?: GameGenre;
  status?: GameProgressStatus;
  sortByRating?: boolean;
  page: number;
  pageSize: number;
}