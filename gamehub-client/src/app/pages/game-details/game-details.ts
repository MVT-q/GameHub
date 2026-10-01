import { Component, OnInit } from '@angular/core';
import { Game } from '../../models/game/game.model';
import { ActivatedRoute } from '@angular/router';
import { GameService } from '../../services/game.service';
import { GameGenre } from '../../enums/game-genre';

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

  constructor(
    private readonly route: ActivatedRoute,
    private readonly gameService: GameService,
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
  }

  getGenreLabel(genre: GameGenre): string {
    return this.genres.find(item => item.value === genre)?.label ?? 'Unknown';
  }
}
