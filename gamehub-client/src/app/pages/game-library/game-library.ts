import { Component, OnInit } from '@angular/core';
import { GameLibraryEntry } from '../../models/game-library/game-library-entry.model';
import { FormsModule } from '@angular/forms';
import { GameGenre } from '../../enums/game-genre';
import { GameProgressStatus } from '../../enums/game-progress-status';
import { GameLibraryQuery } from '../../models/game-library/game-library-query.model';
import { GameLibraryService } from '../../services/game-library.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-game-library',
  imports: [FormsModule],
  templateUrl: './game-library.html',
  styleUrl: './game-library.css',
})
export class GameLibrary implements OnInit {
  entries: GameLibraryEntry[] = [];

  search = '';

  gameGenre: GameGenre | undefined = undefined;
  genres = [
    { value: GameGenre.Action, label: 'Action' },
    { value: GameGenre.Adventure, label: 'Adventure' },
    { value: GameGenre.RPG, label: 'RPG' },
    { value: GameGenre.Strategy, label: 'Strategy' },
    { value: GameGenre.Simulation, label: 'Simulation' },
    { value: GameGenre.Sports, label: 'Sports' },
    { value: GameGenre.Racing, label: 'Racing' },
    { value: GameGenre.Horror, label: 'Horror' },
  ];

  status: GameProgressStatus | undefined = undefined;
  statuses = [
    { value: GameProgressStatus.WantToPlay, label: 'WantToPlay' },
    { value: GameProgressStatus.Playing, label: 'Playing' },
    { value: GameProgressStatus.Completed, label: 'Completed' },
    { value: GameProgressStatus.Paused, label: 'Paused' },
    { value: GameProgressStatus.Dropped, label: 'Dropped' },
  ];

  sortByRating: boolean | undefined = undefined;

  currentPage = 1;
  pageSize = 50;
  totalCount = 0;

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  constructor(private readonly gameLibraryService: GameLibraryService, private readonly router: Router) {}

  ngOnInit(): void {
    this.getLibraryEntries();
  }

  getLibraryEntries(): void {
    let query: GameLibraryQuery = {
      search: this.search,
      genre: this.gameGenre,
      status: this.status,
      sortByRating: this.sortByRating,
      page: this.currentPage,
      pageSize: this.pageSize,
    };

    this.gameLibraryService.getLibraryEntries(query).subscribe({
      next: (entries) => {
        const lastPage = Math.max(1, Math.ceil(entries.totalCount / this.pageSize));

        if (this.currentPage > lastPage) {
          this.currentPage = lastPage;
          this.getLibraryEntries();
          return;
        }

        this.entries = entries.items;
        this.totalCount = entries.totalCount;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  deleteGameEntry(entryId: number): void {
    this.gameLibraryService.deleteGameEntry(entryId).subscribe({
      next: () => {
        this.getLibraryEntries();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  toGameDetails(entryId: number): void {
    this.router.navigate(['/game-library', entryId])
  }

  nextPage(): void {
    if (this.currentPage >= this.totalPages) {
      return;
    }

    this.currentPage++;
    this.getLibraryEntries();
  }

  previousPage(): void {
    if (this.currentPage === 1) {
      return;
    }

    this.currentPage--;
    this.getLibraryEntries();
  }
}
