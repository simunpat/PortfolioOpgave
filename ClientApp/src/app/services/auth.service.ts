import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    name: string;
    email: string;
    password: string;
}

export interface AuthResponse {
    userId: number;
    name: string;
    email: string;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private apiUrl = `${environment.apiUrl}/api/Auth`;
    private userIdKey = 'user_id';

    constructor(private http: HttpClient, private router: Router) {}

    login(loginRequest: LoginRequest): Observable<AuthResponse> {
        // Ensure data format matches exactly what the API expects
        const requestData = {
            email: loginRequest.email,
            password: loginRequest.password,
        };

        const url = `${this.apiUrl}/login`;

        return this.http.post<AuthResponse>(url, requestData).pipe(
            tap((response) => {
                if (response && response.userId && response.userId > 0) {
                    localStorage.setItem(
                        this.userIdKey,
                        response.userId.toString()
                    );
                }
            })
        );
    }

    register(registerRequest: RegisterRequest): Observable<AuthResponse> {
        return this.http
            .post<AuthResponse>(`${this.apiUrl}/register`, registerRequest)
            .pipe(
                tap((response) => {
                    if (response && response.userId) {
                        localStorage.setItem(
                            this.userIdKey,
                            response.userId.toString()
                        );
                    }
                })
            );
    }

    logout(): void {
        localStorage.removeItem(this.userIdKey);
        this.router.navigate(['/']);
    }

    getUserId(): number | null {
        const userId = localStorage.getItem(this.userIdKey);
        return userId ? parseInt(userId, 10) : null;
    }

    isAuthenticated(): boolean {
        const isAuth = !!localStorage.getItem(this.userIdKey);
        return isAuth;
    }

    getUserByEmail(email: string): Observable<AuthResponse> {
        const url = `${
            environment.apiUrl
        }/api/User/byEmail/${encodeURIComponent(email)}`;

        return this.http.get<AuthResponse>(url).pipe(
            tap((response) => {
                if (response && response.userId) {
                    localStorage.setItem(
                        this.userIdKey,
                        response.userId.toString()
                    );
                }
            })
        );
    }
}
