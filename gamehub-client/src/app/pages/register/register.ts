import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { RegisterRequest } from '../../models/register-request.model';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  username = '';
  password = '';

  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
  ) {}

  register(): void {
    const request: RegisterRequest = {
      username: this.username,
      password: this.password,
    };

    this.authService
      .register(request)
      .pipe(switchMap(() => this.authService.login(request)))
      .subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);

          console.log('registered');
        },
        error: (error) => {
          console.error(error);
        },
      });
  }

  toLogin(): void {
    this.router.navigate(['/login']);
  }
}
