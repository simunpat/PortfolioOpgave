import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FormsModule,
    ReactiveFormsModule,
    FormBuilder,
    FormGroup,
    Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import {
    ProjectService,
    CreateProjectDto,
    UpdateProjectDto,
} from '../../services/project.service';
import {
    ProjectCategoryService,
    ProjectCategoryDto,
} from '../../services/project-category.service';
import { Project } from '../../services/user.service';

@Component({
    selector: 'app-project-management',
    templateUrl: './project-management.component.html',
    styleUrls: ['./project-management.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatCardModule,
        MatButtonModule,
        MatIconModule,
        MatInputModule,
        MatSelectModule,
        MatDialogModule,
        MatSnackBarModule,
    ],
})
export class ProjectManagementComponent implements OnInit {
    projects: Project[] = [];
    categories: ProjectCategoryDto[] = [];
    projectForm: FormGroup;
    isEditing = false;
    currentProjectId: number | null = null;

    constructor(
        private projectService: ProjectService,
        private categoryService: ProjectCategoryService,
        private fb: FormBuilder
    ) {
        this.projectForm = this.fb.group({
            title: ['', [Validators.required, Validators.minLength(3)]],
            liveDemoUrl: [
                '',
                [Validators.required, Validators.pattern('https?://.+')],
            ],
            projectCategoryId: ['', Validators.required],
        });
    }

    ngOnInit(): void {
        this.loadProjects();
        this.loadCategories();
    }

    loadProjects(): void {
        this.projectService.getMyProjects().subscribe({
            next: (projects) => {
                this.projects = projects;
            },
            error: (error) => {
                console.error('Error loading projects:', error);
            },
        });
    }

    loadCategories(): void {
        this.categoryService.getAll().subscribe({
            next: (categories) => {
                this.categories = categories;
            },
            error: (error) => {
                console.error('Error loading categories:', error);
            },
        });
    }

    onSubmit(): void {
        if (this.projectForm.valid) {
            const projectData = this.projectForm.value;
            if (this.isEditing && this.currentProjectId) {
                this.updateProject(this.currentProjectId, projectData);
            } else {
                this.createProject(projectData);
            }
        }
    }

    createProject(projectData: CreateProjectDto): void {
        this.projectService.create(projectData).subscribe({
            next: () => {
                this.loadProjects();
                this.resetForm();
            },
            error: (error) => {
                console.error('Error creating project:', error);
            },
        });
    }

    updateProject(id: number, projectData: UpdateProjectDto): void {
        this.projectService.update(id, projectData).subscribe({
            next: () => {
                this.loadProjects();
                this.resetForm();
            },
            error: (error) => {
                console.error('Error updating project:', error);
            },
        });
    }

    deleteProject(id: number): void {
        if (confirm('Are you sure you want to delete this project?')) {
            this.projectService.delete(id).subscribe({
                next: () => {
                    this.loadProjects();
                },
                error: (error) => {
                    console.error('Error deleting project:', error);
                },
            });
        }
    }

    editProject(project: Project): void {
        this.isEditing = true;
        this.currentProjectId = project.id;
        this.projectForm.patchValue({
            title: project.title,
            liveDemoUrl: project.liveDemoUrl,
            projectCategoryId: project.projectCategoryId,
        });
    }

    resetForm(): void {
        this.isEditing = false;
        this.currentProjectId = null;
        this.projectForm.reset();
    }

    getCategoryName(categoryId: number): string {
        const category = this.categories.find((c) => c.id === categoryId);
        return category ? category.name : 'Unknown Category';
    }
}
