import { Component } from '@angular/core';
import { User } from '../../../models/user.model/user.model';
import { UserService } from '../../../services/user.service/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-panel-users-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-panel-users-component.html',
  styleUrl: './admin-panel-users-component.css',
})
export class AdminPanelUsersComponent {
  Users: User[] = [];

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.GetUsers();
    this.loadUsersPlaceholders();
  }

  GetUsers(): void {
    this.userService.getAllUsers().subscribe((data: User[]) => {
      this.Users = data;
      console.log(this.Users);
    });
  }
  //Optimerer *ngFor ved at spore brugere via ID, så Angular genbruger DOM-elementer.
  trackById = (_: number, u: User) => u.id;

  isLoading = true;

  // 6 skelet-rækker – justér antal efter behov
  skeletonRows = Array.from({ length: 6 });

  trackByIndex = (i: number) => i;

  // Eksempel: toggl loading når data er klar
  loadUsersPlaceholders() {
    this.isLoading = true;
    this.userService.getAllUsers().subscribe({
      next: (users) => {
        this.Users = users;
        this.isLoading = false;
      },
      error: (_) => {
        this.Users = [];
        this.isLoading = false;
      },
    });
  }
}
