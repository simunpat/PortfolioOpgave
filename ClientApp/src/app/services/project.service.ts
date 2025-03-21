import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';
import { Project } from './user.service';

export interface CreateProjectDto {
    title: string;
    liveDemoUrl?: string;
    projectCategoryId: number;
}

export interface UpdateProjectDto {
    title: string;
    liveDemoUrl?: string;
    projectCategoryId: number;
}

@Injectable({
    providedIn: 'root',
})
export class ProjectService {
    private apiUrl = `${environment.apiUrl}/api/Project`;

    constructor(private http: HttpClient, private authService: AuthService) {}

    getAll(): Observable<Project[]> {
        return this.http
            .get<Project[]>(this.apiUrl)
            .pipe(catchError(this.handleError));
    }

    getAllByUser(userId: number): Observable<Project[]> {
        return this.http
            .get<Project[]>(`${this.apiUrl}/User/${userId}`)
            .pipe(catchError(this.handleError));
    }

    getMyProjects(): Observable<Project[]> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        return this.http
            .get<Project[]>(`${this.apiUrl}/my`, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    getById(id: number): Observable<Project> {
        return this.http
            .get<Project>(`${this.apiUrl}/${id}`)
            .pipe(catchError(this.handleError));
    }

    create(project: CreateProjectDto): Observable<Project> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        return this.http
            .post<Project>(this.apiUrl, project, {
                headers: {
                    UserId: userId.toString(),
                },
            })
            .pipe(catchError(this.handleError));
    }

    update(id: number, project: UpdateProjectDto): Observable<Project> {
        const userId = this.authService.getUserId();

        if (!userId) {
            return throwError(() => new Error('User not authenticated'));
        }

        return this.http
            .put<Project>(`${this.apiUrl}/${id}`, project, {
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

        return throwError(() => new Error(errorMessage));
    }
}
