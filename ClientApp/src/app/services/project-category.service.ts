import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ProjectCategoryDto {
    id: number;
    name: string;
}

@Injectable({
    providedIn: 'root',
})
export class ProjectCategoryService {
    private apiUrl = `${environment.apiUrl}/api/ProjectCategory`;

    constructor(private http: HttpClient) {}

    getAll(): Observable<ProjectCategoryDto[]> {
        return this.http.get<ProjectCategoryDto[]>(this.apiUrl, {
            withCredentials: true,
        });
    }

    getById(id: number): Observable<ProjectCategoryDto> {
        return this.http.get<ProjectCategoryDto>(`${this.apiUrl}/${id}`, {
            withCredentials: true,
        });
    }

    getByProjectId(projectId: number): Observable<ProjectCategoryDto> {
        return this.http.get<ProjectCategoryDto>(
            `${this.apiUrl}/project/${projectId}`,
            {
                withCredentials: true,
            }
        );
    }
}
