import { Component, OnInit } from '@angular/core';
import { GameLibraryEntryDetails } from '../../models/game-library/game-library-entry-details.model';
import { GameGenre } from '../../enums/game-genre';
import { GameProgressStatus } from '../../enums/game-progress-status';
import { GameLibraryService } from '../../services/game-library.service';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UpdateGameStatus } from '../../models/game-library/update-game-status.model';
import { UpdateGameRating } from '../../models/game-library/update-game-rating.model';
import { GameLibraryNotes } from '../game-library-notes/game-library-notes';

@Component({
  selector: 'app-game-library-details',
  imports: [FormsModule, GameLibraryNotes],
  templateUrl: './game-library-details.html',
  styleUrl: './game-library-details.css',
})
export class GameLibraryDetails implements OnInit {
  entryId = 0;
  entry: GameLibraryEntryDetails | null = null;

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

  statuses = [
    { value: GameProgressStatus.WantToPlay, label: 'WantToPlay' },
    { value: GameProgressStatus.Playing, label: 'Playing' },
    { value: GameProgressStatus.Completed, label: 'Completed' },
    { value: GameProgressStatus.Paused, label: 'Paused' },
    { value: GameProgressStatus.Dropped, label: 'Dropped' },
  ];

  isEditingStatus = false;
  isEditingRating = false;

  entryStatus: GameProgressStatus = 0;
  entryRating: number | null = null;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly gameLibraryService: GameLibraryService,
  ) {}

  ngOnInit(): void {
    this.entryId = Number(this.route.snapshot.paramMap.get('entryId'));

    this.gameLibraryService.getGameEntryById(this.entryId).subscribe({
      next: (entry) => {
        this.entry = entry;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  editStatus(): void {
    if (!this.entry) return;

    if (this.isEditingStatus) {
      this.isEditingStatus = false;
      return;
    }

    this.entryStatus = this.entry.status;
    this.isEditingStatus = true;
  }

  editRating(): void {
    if (!this.entry) return;

    if (this.isEditingRating) {
      this.isEditingRating = false;
      return;
    }

    this.entryRating = this.entry.rating ?? null;
    this.isEditingRating = true;
  }

  saveStatus(): void {
    const entry: UpdateGameStatus = {
      status: this.entryStatus,
    };

    this.gameLibraryService.updateGameStatus(this.entryId, entry).subscribe({
      next: (entry) => {
        this.entry = entry;
        this.isEditingStatus = false;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  saveRating(): void {
    const entry: UpdateGameRating = {
      rating: this.entryRating,
    };

    this.gameLibraryService.updateGameRating(this.entryId, entry).subscribe({
      next: (entry) => {
        this.entry = entry;
        this.isEditingRating = false;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  resetRating(): void {
    this.entryRating = null;
    this.saveRating();
  }

  getGenreLabel(genre: GameGenre): string {
    return this.genres.find((item) => item.value === genre)?.label ?? 'Unknown';
  }

  getStatusLabel(status: GameProgressStatus): string {
    return this.statuses.find((item) => item.value === status)?.label ?? 'Unknown';
  }
}
