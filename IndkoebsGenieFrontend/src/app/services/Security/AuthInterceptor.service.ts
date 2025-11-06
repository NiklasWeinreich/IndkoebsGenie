import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenService } from '../Security/token.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private tokenService: TokenService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Hent token fra din TokenService
    const token = this.tokenService.getToken();

    // Hvis der findes en token, tilføj den som Authorization-header
    if (token) {
      // HttpRequest er immutabel, så vi kloner den med ekstra headers
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    // Giv (evt. opdateret) request videre til næste led i kæden
    return next.handle(request);
  }
}
