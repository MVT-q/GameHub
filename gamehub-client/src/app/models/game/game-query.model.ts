import { GameGenre } from '../../enums/game-genre';
import { GameSortBy } from '../../enums/game-sort-by';

export interface GameQuery {
  search?: string;
  genre?: GameGenre;
  hasDedicatedServer?: boolean;
  sortBy?: GameSortBy;
  descending?: boolean;
  page?: number;
  pageSize?: number;
}
