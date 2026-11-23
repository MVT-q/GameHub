import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GameLibraryNoteService } from '../../services/game-library-note.service';
import { GameLibraryNoteQuery } from '../../models/game-library/game-library-note-query.model';
import { GameLibraryNote } from '../../models/game-library/game-library-note.model';
import { CreateGameLibraryNote } from '../../models/game-library/create-game-library-note.model';
import { UpdateGameLibraryNote } from '../../models/game-library/update-game-library-note.model';

@Component({
  selector: 'app-game-library-notes',
  imports: [FormsModule],
  templateUrl: './game-library-notes.html',
  styleUrl: './game-library-notes.css',
})
export class GameLibraryNotes {
  @Input() entryId = 0;

  notes: GameLibraryNote[] = [];

  search = '';
  sortByCreateDate = false;
  showNotes = false;

  currentPage = 1;
  pageSize = 10;
  totalCount = 0;

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  noteButtonText = 'Show notes';

  isCreating = false;
  noteTitle = '';
  noteDescription = '';

  editingNoteId: number | null = null;
  editingNoteTitle = '';
  editingNoteDescription: string | null = '';

  constructor(private readonly gameLibraryNoteService: GameLibraryNoteService) {}

  getNotes(): void {
    let query: GameLibraryNoteQuery = {
      search: this.search,
      descending: this.sortByCreateDate,
      page: this.currentPage,
      pageSize: this.pageSize,
    };

    this.gameLibraryNoteService.getNotes(this.entryId, query).subscribe({
      next: (notes) => {
        const lastPage = Math.max(1, Math.ceil(notes.totalCount / this.pageSize));

        if (this.currentPage > lastPage) {
          this.currentPage = lastPage;
          this.getNotes();
          return;
        }

        this.notes = notes.items;
        this.totalCount = notes.totalCount;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  createNote(): void {
    const note: CreateGameLibraryNote = {
      title: this.noteTitle,
      description: this.noteDescription,
    };

    this.gameLibraryNoteService.createNote(this.entryId, note).subscribe({
      next: () => {
        this.noteTitle = '';
        this.noteDescription = '';
        this.isCreating = false;
        this.currentPage = 1;
        this.getNotes();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  deleteNote(noteId: number): void {
    this.gameLibraryNoteService.deleteNote(this.entryId, noteId).subscribe({
      next: () => {
        this.getNotes();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  editNote(note: GameLibraryNote): void {
    this.editingNoteId = note.id;
    this.editingNoteTitle = note.title;
    this.editingNoteDescription = note.description;
  }

  cancelEdit(): void {
    this.editingNoteId = null;
  }

  updateNote(noteId: number): void {
    const note: UpdateGameLibraryNote = {
      title: this.editingNoteTitle,
      description: this.editingNoteDescription,
    };

    this.gameLibraryNoteService.updateNote(this.entryId, noteId, note).subscribe({
      next: () => {
        this.editingNoteId = null;
        this.getNotes();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.getNotes();
  }

  toggleNotes(): void {
    if (this.showNotes) {
      this.noteButtonText = 'Show notes';
      this.showNotes = false;
      return;
    }

    this.noteButtonText = 'Hide notes';
    this.showNotes = true;
    this.getNotes();
  }

  startCreate(): void {
    if (this.isCreating) {
      this.isCreating = false;
      return;
    }

    this.isCreating = true;
  }

  nextPage(): void {
    if (this.currentPage >= this.totalPages) {
      return;
    }

    this.currentPage++;
    this.getNotes();
  }

  previousPage(): void {
    if (this.currentPage === 1) {
      return;
    }

    this.currentPage--;
    this.getNotes();
  }
}
