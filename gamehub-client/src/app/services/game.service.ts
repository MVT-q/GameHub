import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Game } from '../models/game/game.model';
import { CreateGame } from '../models/game/game-create.model';
import { UpdateGameInfo } from '../models/game/update-game-info.model';
import { GameQuery } from '../models/game/game-query.model';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  private readonly apiUrl = 'https://localhost:7096/api/games';

  constructor(private readonly http: HttpClient) {}

  getGameList(query: GameQuery): Observable<Game[]> {
    let params = new HttpParams();

    if (query.search) {
      params = params.set('search', query.search);
    }

    if (query.genre !== undefined) {
      params = params.set('genre', query.genre);
    }

    if (query.hasDedicatedServer !== undefined) {
      params = params.set('hasDedicatedServer', query.hasDedicatedServer);
    }

    return this.http.get<Game[]>(this.apiUrl, { params });
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
