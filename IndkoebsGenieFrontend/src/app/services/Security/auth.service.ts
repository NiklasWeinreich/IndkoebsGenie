import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, switchMap, map } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environment/environment';
import { LoginResponse, LoginModel } from '../../models/login.model/login.model';
import { User } from '../../models/user.model/user.model';
import { TokenService } from './token.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  // Nøgle til at gemme basis-logininfo i storage
  private readonly BASIC_INFO_KEY = 'basicUserInfo';

  // Holder den aktuelle bruger i hukommelsen (null = ikke logget ind)
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  // Observable, så komponenter kan lytte på brugerens tilstand
  public currentUser$ = this.currentUserSubject.asObservable();

  // Simpel global besked, fx til notifikationer/alerts
  private globalMessageSubject = new BehaviorSubject<string | null>(null);
  public globalMessage$ = this.globalMessageSubject.asObservable();

  constructor(private http: HttpClient, private tokenService: TokenService) {
    // Ved opstart: prøv at genskabe login-tilstand fra tidligere session
    const token = this.tokenService.getToken();
    const basic = this.readBasicInfo();

    // Hvis der findes token + basisinfo, så indlæs fulde brugerdata
    if (token && basic) {
      this.getUserDetails(basic.id).subscribe({
        next: (u) => this.currentUserSubject.next(u), // sæt bruger som logget ind
        error: () => this.logout(), // hvis noget fejler, ryd tilstand
      });
    }
  }

  // Giver den aktuelle bruger synkront (praktisk i guards osv.)
  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  // Hent nuværende token (fx til guards/interceptors)
  get token(): string | null {
    return this.tokenService.getToken();
  }

  // Sæt/fjern en global besked (kan bindes til en toast/alert)
  setGlobalMessage(msg: string | null) {
    this.globalMessageSubject.next(msg);
  }

  // Læs basis-logininfo fra local/session storage
  private readBasicInfo(): LoginResponse | null {
    const raw =
      localStorage.getItem(this.BASIC_INFO_KEY) ??
      sessionStorage.getItem(this.BASIC_INFO_KEY);

    if (!raw) return null;
    try {
      return JSON.parse(raw) as LoginResponse;
    } catch {
      return null; // hvis data ikke kan læses, ignoreres den
    }
  }

  // Gem basis-logininfo i valgt storage (rememberMe styrer typen)
  private writeBasicInfo(data: LoginResponse, remember: boolean) {
    const store = remember ? localStorage : sessionStorage;
    store.setItem(this.BASIC_INFO_KEY, JSON.stringify(data));
  }

  // Fjern al gemt basis-logininfo
  private clearBasicInfo() {
    localStorage.removeItem(this.BASIC_INFO_KEY);
    sessionStorage.removeItem(this.BASIC_INFO_KEY);
  }

  // Logger ind:
  // 1) får basis-logininfo
  // 2) henter fulde brugerdata
  // 3) gemmer token + basisinfo og udsender brugeren
  login(model: LoginModel, rememberMe: boolean): Observable<LoginResponse> {
    const url = `${environment.apiUrl}/Auth/login`;

    return this.http.post<LoginResponse>(url, model).pipe(
      // Når basisinfo er modtaget, hent fuld bruger
      switchMap((resp) =>
        this.getUserDetails(resp.id).pipe(map((user) => ({ resp, user })))
      ),
      // Når fuld bruger er klar, opdater lokal tilstand og storage
      switchMap(({ resp, user }) => {
        this.tokenService.setToken(resp.token, rememberMe); // gem token
        this.writeBasicInfo(resp, rememberMe);              // gem basisinfo
        this.currentUserSubject.next(user);                 // udsend bruger
        return of(resp);                                    // retur basisrespons
      })
    );
  }

  // Logger ud: ryd token, storage og bruger-tilstand
  logout() {
    this.tokenService.removeToken();
    this.clearBasicInfo();
    this.currentUserSubject.next(null);
  }

  getUserDetails(userId: number): Observable<User> {
    return this.http.get<User>(`${environment.apiUrl}/User/${userId}`);
  }

  registerUser(newUser: User): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/Auth/register`, newUser);
  }

  forgotPassword(email: string): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/Auth/forgot-password`, { email });
  }

  resetPassword(model: { email: string; token: string; newPassword: string }): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/Auth/reset-password`, model);
  }
}
