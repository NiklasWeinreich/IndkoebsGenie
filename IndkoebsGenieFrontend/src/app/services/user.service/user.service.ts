import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly productItemApiUrl = environment.apiUrl + '/User';

  constructor(private http: HttpClient) {}

  getAllUsers() {
    console.log('Henter brugere: ' + this.productItemApiUrl);
    return this.http.get<any>(this.productItemApiUrl + '/GetAllUsers');
  }

  getUserById(userId: number) {
    console.log('Henter bruger med ID: ' + userId);
    return this.http.get<any>(this.productItemApiUrl + userId);
  }
}
