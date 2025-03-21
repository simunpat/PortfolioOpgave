import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';

export interface WorkExperience {
    id: number;
    company: string;
    position: string;
    startDate: string;
    endDate: string;
    userId: number;
}

export interface WorkExperienceCreateDto {
    company: string;
    position: string;
    startDate: string;
    endDate: string;
    userId?: number;
}

@Injectable({
    providedIn: 'root',
})
export class WorkExperienceService {
    private apiUrl = `${environment.apiUrl}/api/WorkExperience`;

    constructor(private http: HttpClient, private authService: AuthService) {}

    getAllWorkExperiences(): Observable<WorkExperience[]> {
        return this.http
            .get<WorkExperience[]>(this.apiUrl)
            .pipe(catchError(this.handleError));
    }

    getWorkExperienceById(id: number): Observable<WorkExperience> {
        return this.http
            .get<WorkExperience>(`${this.apiUrl}/${id}`)
            .pipe(catchError(this.handleError));
    }

    getUserWorkExperiences(userId: number): Observable<WorkExperience[]> {
        return this.http
            .get<WorkExperience[]>(`${this.apiUrl}/User/${userId}`)
            .pipe(catchError(this.handleError));
    }

    getMyWorkExperiences(): Observable<WorkExperience[]> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        return this.http
            .get<WorkExperience[]>(`${this.apiUrl}/User/${userId}`, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    createWorkExperience(
        workExperience: WorkExperienceCreateDto
    ): Observable<WorkExperience> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        // Ensure userId is included in the request body
        const workExperienceWithUserId = {
            ...workExperience,
            userId: userId,
        };

        console.log(
            'Creating work experience with data:',
            workExperienceWithUserId
        );

        return this.http
            .post<WorkExperience>(this.apiUrl, workExperienceWithUserId, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    updateWorkExperience(
        id: number,
        workExperience: WorkExperienceCreateDto
    ): Observable<any> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        // Ensure userId is included in the request body
        const workExperienceWithUserId = {
            ...workExperience,
            userId: userId,
        };

        return this.http
            .put(`${this.apiUrl}/${id}`, workExperienceWithUserId, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    deleteWorkExperience(id: number): Observable<any> {
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

        console.error('Work Experience service error:', error);
        return throwError(() => new Error(errorMessage));
    }
}
