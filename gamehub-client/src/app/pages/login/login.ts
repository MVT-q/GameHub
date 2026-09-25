import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  username = '';
  password = '';

  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
  ) {}

  login(): void {
    this.authService
      .login({
        username: this.username,
        password: this.password,
      })
      .subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);

          console.log('logged in');
        },
        error: (error) => {
          console.error(error);
        },
      });
  }

  toRegister(): void {
    this.router.navigate(['/register']);
  }
}
