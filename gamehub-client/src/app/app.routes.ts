import { Routes } from '@angular/router';
import { Register } from './pages/register/register';
import { Login } from './pages/login/login';
import { GameCatalog } from './pages/game-catalog/game-catalog';
import { authGuard } from './guards/auth-guard';
import { guestGuard } from './guards/guest-guard';
import { GameDetails } from './pages/game-details/game-details';
import { AddGame } from './pages/add-game/add-game';
import { EditGame } from './pages/edit-game/edit-game';

export const routes: Routes = [
  { path: 'register', component: Register, canActivate: [guestGuard] },
  { path: 'login', component: Login, canActivate: [guestGuard] },
  { path: 'game-catalog', component: GameCatalog, canActivate: [authGuard] },
  { path: 'game-catalog/add', component: AddGame, canActivate: [authGuard] },
  { path: 'game-catalog/:gameId/edit', component: EditGame, canActivate: [authGuard] },
  { path: 'game-catalog/:gameId', component: GameDetails, canActivate: [authGuard] }, 
  { path: '', redirectTo: 'login', pathMatch: 'full' },
];
