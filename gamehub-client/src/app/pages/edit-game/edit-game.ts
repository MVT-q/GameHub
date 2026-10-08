import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Game } from '../../models/game/game.model';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from '../../services/game.service';
import { GameGenre } from '../../enums/game-genre';
import { UpdateGameInfo } from '../../models/game/update-game-info.model';

@Component({
  selector: 'app-edit-game',
  imports: [FormsModule],
  templateUrl: './edit-game.html',
  styleUrl: './edit-game.css',
})
export class EditGame implements OnInit {
  gameId = 0;
  game!: Game;

  gameTitle = '';
  gameReleaseDate = '';
  gameDescription = '';
  hasDedicatedServers = false;
  gameGenre = 0;
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
    private readonly router: Router,
    private readonly gameService: GameService,
    private readonly route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.gameId = Number(this.route.snapshot.paramMap.get('gameId'));

    this.gameService.getGameById(this.gameId).subscribe({
      next: (game) => {
        this.game = game;

        this.gameTitle = game.title;
        this.gameReleaseDate = game.releaseDate.split('T')[0];
        this.gameDescription = game.description;
        this.hasDedicatedServers = game.hasDedicatedServers;
        this.gameGenre = game.genre;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  saveGame(): void {
    const game: UpdateGameInfo = {
      title: this.gameTitle,
      releaseDate: this.gameReleaseDate,
      description: this.gameDescription,
      hasDedicatedServers: this.hasDedicatedServers,
      genre: this.gameGenre,
    };

    this.gameService.updateGameInfo(this.gameId, game).subscribe({
      next: () => {
        this.router.navigate(['/game-catalog', this.gameId]);
      },
    });
  }
}
