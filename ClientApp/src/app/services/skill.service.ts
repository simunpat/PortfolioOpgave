import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';

export interface Skill {
    id: number;
    name: string;
    proficiencyLevel: number;
    userId: number;
}

export interface CreateSkill {
    name: string;
    proficiencyLevel: number;
}

export interface UpdateSkill {
    name: string;
    proficiencyLevel: number;
}

@Injectable({
    providedIn: 'root',
})
export class SkillService {
    private apiUrl = `${environment.apiUrl}/api/Skill`;

    constructor(private http: HttpClient, private authService: AuthService) {}

    getAll(): Observable<Skill[]> {
        return this.http
            .get<Skill[]>(this.apiUrl)
            .pipe(catchError(this.handleError));
    }

    getAllByUser(userId: number): Observable<Skill[]> {
        return this.http
            .get<Skill[]>(`${this.apiUrl}/user/${userId}`)
            .pipe(catchError(this.handleError));
    }

    getMySkills(): Observable<Skill[]> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        return this.http
            .get<Skill[]>(`${this.apiUrl}/user/${userId}`, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    getById(id: number): Observable<Skill> {
        return this.http
            .get<Skill>(`${this.apiUrl}/${id}`)
            .pipe(catchError(this.handleError));
    }

    create(skill: CreateSkill): Observable<Skill> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        // Include userId in the request body
        const skillWithUserId = {
            ...skill,
            userId: userId,
        };

        console.log('Creating skill with data:', skillWithUserId);
        console.log('Sending to URL:', this.apiUrl);

        return this.http
            .post<Skill>(this.apiUrl, skillWithUserId, {
                headers: {
                    'Content-Type': 'application/json',
                    Accept: 'application/json',
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    update(id: number, skill: UpdateSkill): Observable<Skill> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        // Include userId in the request body
        const skillWithUserId = {
            ...skill,
            userId: userId,
        };

        return this.http
            .put<Skill>(`${this.apiUrl}/${id}`, skillWithUserId, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    delete(id: number): Observable<void> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        return this.http
            .delete<void>(`${this.apiUrl}/${id}`, {
                headers: {
                    UserId: userId ? userId.toString() : '',
                },
            })
            .pipe(catchError(this.handleError));
    }

    private handleError(error: HttpErrorResponse): Observable<never> {
        let errorMessage = 'An error occurred';

        if (error.error instanceof ErrorEvent) {
            // Client-side error
            errorMessage = `Error: ${error.error.message}`;
        } else {
            // Server-side error
            errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }

        console.error('Skill service error:', error);
        return throwError(() => new Error(errorMessage));
    }
}
