import { Component } from '@angular/core';

import { Router } from '@angular/router';
import { UserLoginDto } from '../shared/models/user';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) {
    
  }

  login() {
    const userLoginDto: UserLoginDto = { userName: this.username, password: this.password };
    this.authService.login(userLoginDto).subscribe(
      () => this.router.navigate(['/home']),
      error => console.error('Login failed', error)
    );
  }
}
