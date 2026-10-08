import { Component } from '@angular/core';
import { GameGenre } from '../../enums/game-genre';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { GameService } from '../../services/game.service';
import { CreateGame } from '../../models/game/game-create.model';

@Component({
  selector: 'app-add-game',
  imports: [FormsModule],
  templateUrl: './add-game.html',
  styleUrl: './add-game.css',
})
export class AddGame {
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
  ) {}

  addGame(): void {
    const game: CreateGame = {
      title: this.gameTitle,
      releaseDate: this.gameReleaseDate,
      description: this.gameDescription,
      hasDedicatedServers: this.hasDedicatedServers,
      genre: this.gameGenre,
    };

    this.gameService.addGame(game).subscribe({
      next: () => {
        this.router.navigate(['game-catalog']);
      },
      error: (error) => {
        console.error(error);
      },
    });
  }
}
