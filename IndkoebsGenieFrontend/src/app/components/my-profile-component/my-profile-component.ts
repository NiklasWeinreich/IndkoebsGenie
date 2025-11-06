import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/Security/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { User } from '../../models/user.model/user.model';

@Component({
  selector: 'app-my-profile-component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './my-profile-component.html',
  styleUrl: './my-profile-component.css',
})
export class MyProfileComponent {
  private auth = inject(AuthService);
  user$ = this.auth.currentUser$;   // bind i templaten

  constructor(private http: HttpClient, public AuthService: AuthService) {}
  
  ngOnInit(): void {
    this.GetCurrentUserProfile();
  }
  
  GetCurrentUserProfile(): void {
    const currentUser = this.AuthService.currentUser;
    if (currentUser) {
      console.log('Current user profile:', currentUser);
    } else {
      console.log('Error fetching user profile.');
    }
  }
}
