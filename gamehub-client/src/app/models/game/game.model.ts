import { GameGenre } from "../../enums/game-genre";

export interface Game {
  id: number;
  title: string;
  releaseDate: string;
  description: string;
  hasDedicatedServers: boolean;
  genre: GameGenre;
  updatedAt: string | null;
}