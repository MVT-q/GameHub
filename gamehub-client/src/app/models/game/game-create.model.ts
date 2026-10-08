import { GameGenre } from '../../enums/game-genre';

export interface CreateGame {
  title: string;
  releaseDate: string;
  description: string;
  hasDedicatedServers: boolean;
  genre: GameGenre;
}
