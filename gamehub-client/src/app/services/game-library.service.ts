import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GameLibraryQuery } from '../models/game-library/game-library-query.model';
import { Observable } from 'rxjs';
import { PagedResult } from '../common/paged-result.model';
import { GameLibraryEntry } from '../models/game-library/game-library-entry.model';
import { AddGameToLibrary } from '../models/game-library/add-game-to-library.model';

@Injectable({
  providedIn: 'root',
})
export class GameLibraryService {
  private readonly apiUrl = 'https://localhost:7096/api/library';

  constructor(private readonly http: HttpClient) {}

  getLibraryEntries(query: GameLibraryQuery): Observable<PagedResult<GameLibraryEntry>> {
    let params = new HttpParams();

    if (query.search) {
      params = params.set('search', query.search);
    }

    if (query.genre !== undefined) {
      params = params.set('genre', query.genre);
    }

    if (query.status !== undefined) {
      params = params.set('status', query.status);
    }

    if (query.sortByRating !== undefined) {
      params = params.set('sortByRating', query.sortByRating);
    }

    params = params.set('page', query.page);

    params = params.set('pageSize', query.pageSize);

    return this.http.get<PagedResult<GameLibraryEntry>>(this.apiUrl, { params });
  }

  deleteGameEntry(entryId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${entryId}`);
  }

  addGameToLibrary(game: AddGameToLibrary): Observable<GameLibraryEntry> {
    return this.http.post<GameLibraryEntry>(`${this.apiUrl}`, game);
  }

  isGameInLibrary(gameId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/contains/${gameId}`);
  }
}
