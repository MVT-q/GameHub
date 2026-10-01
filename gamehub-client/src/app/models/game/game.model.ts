import { GameGenre } from "../../enums/game-genre";

export interface Game {
  id: number;
  title: string;
  releaseDate: string;
  description: string;
  genre: GameGenre;
  updatedAt: string | null;
}