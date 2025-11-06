import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TokenService {
  // Holder en reference til valgt storage (sessionStorage som default)
  private storage: Storage | null = null;

  constructor() {
    // Sikrer at vi kun bruger browser-APIs når window findes (SSR-safe)
    if (typeof window !== 'undefined') {
      this.storage = sessionStorage; // standard: kortvarig session
    }
  }

  // Gem token: brug localStorage ved "remember me", ellers sessionStorage
  setToken(token: string, rememberMe: boolean) {
    if (!this.storage) return;
    if (rememberMe) {
      localStorage.setItem('token', token);   // overlever browser-genstart
    } else {
      this.storage.setItem('token', token);   // ryger når fanen/lukning
    }
  }

  // Hent token: tjek først localStorage, derefter sessionStorage
  getToken(): string | null {
    if (!this.storage) return null;
    return localStorage.getItem('token') || this.storage.getItem('token');
  }

  // Fjern token helt fra begge lagre
  removeToken() {
    if (!this.storage) return;
    localStorage.removeItem('token');
    this.storage.removeItem('token');
  }
}
