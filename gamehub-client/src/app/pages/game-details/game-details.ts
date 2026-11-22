import { Component, OnInit } from '@angular/core';
import { Game } from '../../models/game/game.model';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from '../../services/game.service';
import { GameGenre } from '../../enums/game-genre';
import { GameLibraryService } from '../../services/game-library.service';
import { AddGameToLibrary } from '../../models/game-library/add-game-to-library.model';

@Component({
  selector: 'app-game-details',
  imports: [],
  templateUrl: './game-details.html',
  styleUrl: './game-details.css',
})
export class GameDetails implements OnInit {
  gameId = 0;
  game: Game | null = null;

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

  isInLibrary = false;
  isAddingToLibrary = false;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly gameService: GameService,
    private readonly router: Router,
    private readonly gameLibraryService: GameLibraryService,
  ) {}

  ngOnInit(): void {
    this.gameId = Number(this.route.snapshot.paramMap.get('gameId'));

    this.gameService.getGameById(this.gameId).subscribe({
      next: (game) => {
        this.game = game;
      },
      error: (error) => {
        console.error(error);
      },
    });

    this.gameLibraryService.isGameInLibrary(this.gameId).subscribe({
      next: (result) => {
        this.isInLibrary = result;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  getGenreLabel(genre: GameGenre): string {
    return this.genres.find((item) => item.value === genre)?.label ?? 'Unknown';
  }

  toEditGame(): void {
    this.router.navigate(['/game-catalog', this.gameId, 'edit']);
  }

  addToLibrary(): void {
    const game: AddGameToLibrary = {
      gameId: this.gameId,
    };

    this.isAddingToLibrary = true;

    this.gameLibraryService.addGameToLibrary(game).subscribe({
      next: () => {
        this.isInLibrary = true;
        this.isAddingToLibrary = false;
      },
      error: (error) => {
        this.isAddingToLibrary = false;
        console.error(error);
      },
    });
  }
}
