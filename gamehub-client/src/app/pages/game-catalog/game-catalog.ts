import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { Game } from '../../models/game/game.model';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-game-catalog',
  imports: [],
  templateUrl: './game-catalog.html',
  styleUrl: './game-catalog.css',
})
export class GameCatalog implements OnInit {
  games: Game[] = [];

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly gameService: GameService,
  ) {}

  ngOnInit(): void {
    this.getGameList();
  }

  getGameList(): void {
    this.gameService.getGameList().subscribe({
      next: (games) => {
        this.games = games;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  toGameDetails(gameId: number): void {
    this.router.navigate(['/game-catalog', gameId]);
  }

  logout(): void {
    this.authService.logout();

    this.router.navigate(['/login']);
  }
}
