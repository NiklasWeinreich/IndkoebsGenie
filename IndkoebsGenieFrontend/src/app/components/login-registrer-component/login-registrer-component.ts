import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/Security/auth.service';

@Component({
  selector: 'app-login-registrer-component',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login-registrer-component.html',
  styleUrls: ['./login-registrer-component.css'],
})
export class LoginRegistrerComponent {
  constructor(private authService: AuthService, private fb: FormBuilder) {}
  
}
