import { GameGenre } from '../../enums/game-genre';

export interface UpdateGameInfo {
  title: string;
  releaseDate: string;
  description: string;
  hasDedicatedServers: boolean;
  genre: GameGenre;
}
