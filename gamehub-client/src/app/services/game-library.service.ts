import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GameLibraryQuery } from '../models/game-library/game-library-query.model';
import { Observable } from 'rxjs';
import { PagedResult } from '../common/paged-result.model';
import { GameLibraryEntry } from '../models/game-library/game-library-entry.model';

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
}
