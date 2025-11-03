import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { User } from '../../models/user.model/user.model';
import { UserService } from '../../services/user.service/user.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-about-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './about-component.html',
  styleUrl: './about-component.css',
})
export class AboutComponent {


}
