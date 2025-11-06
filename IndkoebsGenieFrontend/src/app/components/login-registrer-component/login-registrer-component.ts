import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/Security/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-registrer-component',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login-registrer-component.html',
  styleUrls: ['./login-registrer-component.css'],
})
export class LoginRegistrerComponent implements OnInit {
  loginForm: FormGroup;
  message: string = '';

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });

    // Registering form setup can be added here
  }

  ngOnInit(): void {
    this.ifAlreadyLoggedIn();
  }

 login(): void {
  console.log('loggin in', this.loginForm.value);
  const { email, password } = this.loginForm.value;

  this.authService.login({ email, password }, true).subscribe({
    next: () => this.router.navigate(['/']),
    error: (error) => {
      if (error.status === 0) this.message = error.message;
      else if (error.status === 401) this.message = 'Forkert e-mail eller adgangskode.';
      else this.message = 'Noget gik galt. Prøv igen senere.';
    },
  });
}

  ifAlreadyLoggedIn(): void {
    const currentUser = this.authService.currentUser;
    if (currentUser && currentUser.id > 0) {
      this.router.navigate(['/']);
    } else {
      console.log('Ingen bruger logget ind endnu.');
    }
  }
}
