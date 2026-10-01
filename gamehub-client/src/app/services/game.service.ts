import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Game } from '../models/game/game.model';

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
}
