import { GameGenre } from '../../enums/game-genre';

export interface GameQuery {
  search?: string;
  genre?: GameGenre;
  hasDedicatedServer?: boolean;
}
