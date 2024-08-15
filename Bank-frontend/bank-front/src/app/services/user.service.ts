import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User, UserLoginDto, UserRegistrationDto } from '../shared/models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
private apiUrl = 'https://localhost:7285/api/User';

  constructor(private http:HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}`);
  }

  getUserById(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  register(user: User): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/register`, user);
  }

  assignRole(userId: string, role: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/assign-role`, { userId, role });
  }
}
