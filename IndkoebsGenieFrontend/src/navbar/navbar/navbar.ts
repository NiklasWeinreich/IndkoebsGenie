import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../app/services/Security/auth.service';
import { LoginResponse } from '../../app/models/login.model/login.model';
import { User } from '../../app/models/user.model/user.model';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css'],
})
export class Navbar {
  isLoggedIn: boolean = false;
  currentUser: User | null = null;
  roleChecker: string = 'Admin';

  constructor(private authService: AuthService, private router: Router) {
    this.authService.currentUser$.subscribe((user) => {
      this.currentUser = user;
      this.checkLoginStatus();
    });
  }

  ngOnInit(): void {
    this.checkLoginStatus();
  }

  checkLoginStatus(): void {
    const currentUser = this.authService.currentUser;
    this.isLoggedIn = !!currentUser && currentUser.id > 0;

    if (this.isLoggedIn && currentUser) {
      // Tjek om brugerens rolle er 'Admin'
      this.roleChecker = currentUser.role;
    }
  }

  logout(): void {
    console.log('Logging out user...', this.authService.currentUser?.email);
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
