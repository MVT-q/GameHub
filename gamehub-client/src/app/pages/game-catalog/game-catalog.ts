import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { Game } from '../../models/game/game.model';
import { GameService } from '../../services/game.service';
import { FormsModule } from '@angular/forms';
import { GameQuery } from '../../models/game/game-query.model';
import { Observable } from 'rxjs';
import { GameGenre } from '../../enums/game-genre';
import { GameSortBy } from '../../enums/game-sort-by';

@Component({
  selector: 'app-game-catalog',
  imports: [FormsModule],
  templateUrl: './game-catalog.html',
  styleUrl: './game-catalog.css',
})
export class GameCatalog implements OnInit {
  games: Game[] = [];

  search = '';

  gameGenre: number | undefined = undefined;
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

  dedicatedServer: boolean | undefined = undefined;

  sortBy: GameSortBy | undefined;
  sorts = [
    { value: GameSortBy.Title, label: 'Sort by game title' },
    { value: GameSortBy.ReleaseDate, label: 'Sort by release date' },
  ];

  descending: boolean | undefined;

  currentPage = 1;
  pageSize = 2;
  totalCount = 0;

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly gameService: GameService,
  ) {}

  ngOnInit(): void {
    this.getGameList();
  }

  getGameList(): void {
    let query: GameQuery = {
      search: this.search,
      genre: this.gameGenre,
      hasDedicatedServer: this.dedicatedServer,
      sortBy: this.sortBy,
      descending: this.descending,
      page: this.currentPage,
      pageSize: this.pageSize,
    };

    this.gameService.getGameList(query).subscribe({
      next: (games) => {
        if (games.items.length === 0 && this.currentPage > 1) {
          this.currentPage--;

          this.getGameList();
          return;
        }

        this.games = games.items;
        this.totalCount = games.totalCount;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  deleteGame(gameId: number): void {
    this.gameService.deleteGame(gameId).subscribe({
      next: () => {
        this.getGameList();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  nextPage(): void {
    if (this.currentPage >= this.totalPages) {
      return;
    }

    this.currentPage++;
    this.getGameList();
  }

  previousPage(): void {
    if (this.currentPage === 1) {
      return;
    }

    this.currentPage--;
    this.getGameList();
  }

  toGameDetails(gameId: number): void {
    this.router.navigate(['/game-catalog', gameId]);
  }

  toAddGame(): void {
    this.router.navigate(['/game-catalog/add']);
  }

  logout(): void {
    this.authService.logout();

    this.router.navigate(['/login']);
  }
}
