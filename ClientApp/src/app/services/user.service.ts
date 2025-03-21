import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export interface Project {
    id: number;
    title: string;
    liveDemoUrl?: string;
    createdAt: Date;
    userId: number;
    projectCategoryId: number;
}

export interface Skill {
    id: number;
    name: string;
    proficiencyLevel: number;
    userId: number;
}

export interface WorkExperience {
    id: number;
    company: string;
    position: string;
    startDate: Date;
    endDate: Date;
    userId: number;
}

export interface Education {
    id: number;
    degree: string;
    institution: string;
    startDate: Date;
    endDate: Date;
    userId: number;
}

export interface User {
    id: number;
    name: string;
    email: string;
    passwordHash: string;
    projects?: Project[];
    skills?: Skill[];
    workExperience?: WorkExperience[];
    education?: Education[];
}

export interface UserCreateDto {
    name: string;
    email: string;
    password: string;
}

@Injectable({
    providedIn: 'root',
})
export class UserService {
    private apiUrl = `${environment.apiUrl}/api/User`;

    constructor(private http: HttpClient) {
        console.log('UserService initialized with API URL:', this.apiUrl);
    }

    getAll(): Observable<User[]> {
        console.log('Fetching all users from:', this.apiUrl);
        return this.http.get<User[]>(this.apiUrl).pipe(
            tap((users) => console.log('Received users:', users)),
            catchError(this.handleError)
        );
    }

    getById(id: number): Observable<User> {
        return this.http
            .get<User>(`${this.apiUrl}/${id}`)
            .pipe(catchError(this.handleError));
    }

    getByIdWithDetails(id: number): Observable<User> {
        return this.http.get<User>(`${this.apiUrl}/${id}`).pipe(
            tap((response) => {
                console.log('User details response:', response);
            }),
            catchError(this.handleError)
        );
    }

    createUser(user: UserCreateDto): Observable<User> {
        return this.http
            .post<User>(this.apiUrl, user)
            .pipe(catchError(this.handleError));
    }

    updateUser(id: number, user: UserCreateDto): Observable<any> {
        return this.http
            .put(`${this.apiUrl}/${id}`, user)
            .pipe(catchError(this.handleError));
    }

    deleteUser(id: number): Observable<any> {
        return this.http
            .delete(`${this.apiUrl}/${id}`)
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

        console.error('API Error:', errorMessage);
        return throwError(() => new Error(errorMessage));
    }
}
