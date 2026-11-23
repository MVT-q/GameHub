import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GameLibraryNoteQuery } from '../models/game-library/game-library-note-query.model';
import { Observable } from 'rxjs';
import { GameLibraryNote } from '../models/game-library/game-library-note.model';
import { PagedResult } from '../common/paged-result.model';
import { CreateGameLibraryNote } from '../models/game-library/create-game-library-note.model';
import { UpdateGameLibraryNote } from '../models/game-library/update-game-library-note.model';

@Injectable({
  providedIn: 'root',
})
export class GameLibraryNoteService {
  private readonly apiUrl = 'https://localhost:7096/api/library';

  constructor(private readonly http: HttpClient) {}

  getNotes(entryId: number, query: GameLibraryNoteQuery): Observable<PagedResult<GameLibraryNote>> {
    let params = new HttpParams();

    if (query.search) {
      params = params.set('search', query.search);
    }

    params = params.set('descending', query.descending);

    params = params.set('page', query.page);

    params = params.set('pageSize', query.pageSize);

    return this.http.get<PagedResult<GameLibraryNote>>(`${this.apiUrl}/${entryId}/notes`, { params });
  }

  createNote(entryId: number, note: CreateGameLibraryNote): Observable<GameLibraryNote> {
    return this.http.post<GameLibraryNote>(`${this.apiUrl}/${entryId}/notes`, note);
  }

  deleteNote(entryId: number, noteId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${entryId}/notes/${noteId}`);
  }

  updateNote(entryId: number, noteId: number, note: UpdateGameLibraryNote): Observable<GameLibraryNote> {
    return this.http.put<GameLibraryNote>(`${this.apiUrl}/${entryId}/notes/${noteId}`, note);
  }
}
