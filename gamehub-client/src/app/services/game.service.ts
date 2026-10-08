import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Game } from '../models/game/game.model';
import { CreateGame } from '../models/game/game-create.model';
import { UpdateGameInfo } from '../models/game/update-game-info.model';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  private readonly apiUrl = 'https://localhost:7096/api/games';

  constructor(private readonly http: HttpClient) {}

  getGameList(): Observable<Game[]> {
    return this.http.get<Game[]>(this.apiUrl);
  }

  getGameById(gameId: number): Observable<Game> {
    return this.http.get<Game>(`${this.apiUrl}/${gameId}`);
  }

  addGame(game: CreateGame): Observable<Game> {
    return this.http.post<Game>(this.apiUrl, game);
  }

  updateGameInfo(gameId: number, game: UpdateGameInfo): Observable<Game> {
    return this.http.put<Game>(`${this.apiUrl}/${gameId}`, game);
  }

  deleteGame(gameId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${gameId}`);
  }
}
