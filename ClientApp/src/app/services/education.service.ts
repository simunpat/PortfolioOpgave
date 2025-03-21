import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';

export interface Education {
    id: number;
    institution: string;
    degree: string;
    startDate: string;
    endDate: string;
    userId: number;
}

export interface EducationCreateDto {
    institution: string;
    degree: string;
    startDate: string;
    endDate: string;
    userId?: number;
}

@Injectable({
    providedIn: 'root',
})
export class EducationService {
    private apiUrl = `${environment.apiUrl}/api/Education`;

    constructor(private http: HttpClient, private authService: AuthService) {}

    getAllEducation(): Observable<Education[]> {
        return this.http
            .get<Education[]>(this.apiUrl)
            .pipe(catchError(this.handleError));
    }

    getEducationById(id: number): Observable<Education> {
        return this.http
            .get<Education>(`${this.apiUrl}/${id}`)
            .pipe(catchError(this.handleError));
    }

    getUserEducation(userId: number): Observable<Education[]> {
        return this.http
            .get<Education[]>(`${this.apiUrl}/User/${userId}`)
            .pipe(catchError(this.handleError));
    }

    getMyEducation(): Observable<Education[]> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        return this.http
            .get<Education[]>(`${this.apiUrl}/User/${userId}`, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    createEducation(education: EducationCreateDto): Observable<Education> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        // Ensure userId is included in the request body
        const educationWithUserId = {
            ...education,
            userId: userId,
        };

        console.log('Creating education with data:', educationWithUserId);

        return this.http
            .post<Education>(this.apiUrl, educationWithUserId, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    updateEducation(
        id: number,
        education: EducationCreateDto
    ): Observable<any> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        // Ensure userId is included in the request body
        const educationWithUserId = {
            ...education,
            userId: userId,
        };

        return this.http
            .put(`${this.apiUrl}/${id}`, educationWithUserId, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    deleteEducation(id: number): Observable<any> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        return this.http
            .delete(`${this.apiUrl}/${id}`, {
                headers: {
                    UserId: userId.toString(),
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

        console.error('Education service error:', error);
        return throwError(() => new Error(errorMessage));
    }
}
