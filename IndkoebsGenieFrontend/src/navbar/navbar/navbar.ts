import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../app/services/Security/auth.service';
import { LoginResponse } from '../../app/models/login.model/login.model';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css'], 
})
export class Navbar {
  isLoggedIn: boolean = false;
  currentUser: LoginResponse | null = null; 
  roleChecker: string = 'Admin';
  isAdmin: boolean = false;
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
  }
}
