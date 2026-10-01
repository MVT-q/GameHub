import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-game-catalog',
  imports: [],
  templateUrl: './game-catalog.html',
  styleUrl: './game-catalog.css',
})
export class GameCatalog {
  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {}

  logout(): void {
    this.authService.logout();

    this.router.navigate(['/login']);
  }
}
