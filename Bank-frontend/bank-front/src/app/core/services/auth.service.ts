// src/app/core/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { User, UserLoginDto } from '../../shared/models/user';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7285/api/User';

  constructor(private http: HttpClient) {}

  login(UserLoginDto: UserLoginDto): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/login`, UserLoginDto)
      .pipe(tap((result: any) => {
        if (result && result.token) {
          localStorage.setItem('token', result.token);
          const decodedToken: any = jwtDecode(result.token);
          console.log('Decoded Token:', decodedToken); // Debugging line

          const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
          const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
          if (userId) {
            localStorage.setItem('userId', userId);
            localStorage.setItem('role', role);
          } else {
            console.error('User ID not found in token');
          }
        }
      }));
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('role');
  }

  isLoggedIn() {
    return localStorage.getItem('token') !== null;
  }

  getToken() {
    return localStorage.getItem('token');
  }

  getCurrentUser() {
    return {
      id: localStorage.getItem('userId'),
      role: localStorage.getItem('role')
    };
  }
}
