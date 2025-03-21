import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { ProjectService } from '../../../../services/project.service';
import { ProjectCategoryService } from '../../../../services/project-category.service';

@Component({
    selector: 'app-project-form',
    templateUrl: './project-form.component.html',
    styleUrls: ['./project-form.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatIconModule,
        MatProgressSpinnerModule,
    ],
})
export class ProjectFormComponent implements OnInit {
    projectForm: FormGroup;
    projectCategories: any[] = [];
    isLoading = false;
    error: string | null = null;
    isEditMode = false;
    projectId: number | null = null;
    pageTitle = 'Create New Project';

    constructor(
        private fb: FormBuilder,
        private projectService: ProjectService,
        private projectCategoryService: ProjectCategoryService,
        private router: Router
    ) {
        this.projectForm = this.fb.group({
            title: ['', [Validators.required, Validators.minLength(3)]],
            liveDemoUrl: ['', [Validators.pattern('https?://.+')]],
            projectCategoryId: ['', Validators.required],
        });

        // Check if we're in edit mode
        const editProjectId = localStorage.getItem('editProjectId');
        if (editProjectId) {
            this.isEditMode = true;
            this.projectId = parseInt(editProjectId, 10);
            this.pageTitle = 'Edit Project';
        }
    }

    ngOnInit(): void {
        this.loadProjectCategories();

        if (this.isEditMode && this.projectId) {
            this.loadProject(this.projectId);
        }
    }

    private loadProject(id: number): void {
        this.isLoading = true;
        this.projectService.getById(id).subscribe({
            next: (project) => {
                this.projectForm.patchValue({
                    title: project.title,
                    liveDemoUrl: project.liveDemoUrl,
                    projectCategoryId: project.projectCategoryId,
                });
                this.isLoading = false;
            },
            error: (error) => {
                this.isLoading = false;
                this.error = 'Failed to load project data';
                console.error('Error loading project:', error);
            },
        });
    }

    private loadProjectCategories(): void {
        this.isLoading = true;
        this.projectCategoryService.getAll().subscribe({
            next: (categories) => {
                this.projectCategories = categories;
                this.isLoading = false;
            },
            error: (error) => {
                this.isLoading = false;

                // Check if this is a network or CORS error
                if (error.status === 0) {
                    this.error =
                        'Network or CORS error while loading categories. Please check your connection.';
                } else {
                    this.error = 'Failed to load project categories';
                }

                console.error('Category loading error:', error);
            },
        });
    }

    onSubmit(): void {
        if (this.projectForm.valid) {
            this.isLoading = true;
            this.error = null;

            if (this.isEditMode && this.projectId) {
                this.projectService
                    .update(this.projectId, this.projectForm.value)
                    .subscribe({
                        next: () => {
                            localStorage.removeItem('editProjectId');
                            this.router.navigate(['/admin']);
                        },
                        error: this.handleError,
                    });
            } else {
                this.projectService.create(this.projectForm.value).subscribe({
                    next: () => {
                        this.router.navigate(['/admin']);
                    },
                    error: this.handleError,
                });
            }
        }
    }

    private handleError = (error: any) => {
        this.isLoading = false;

        // Check if this is a network or CORS error (status 0)
        if (error.status === 0) {
            this.error =
                'Network or CORS error. Please check your connection and server configuration.';
        }
        // Handle specific API errors
        else if (error.error && typeof error.error === 'string') {
            this.error = error.error;
        } else if (error.message) {
            this.error = error.message;
        } else {
            this.error =
                'Failed to ' +
                (this.isEditMode ? 'update' : 'create') +
                ' project. Please try again.';
        }

        console.error(
            'Project ' + (this.isEditMode ? 'update' : 'creation') + ' error:',
            error
        );
    };

    onCancel(): void {
        if (this.isEditMode) {
            localStorage.removeItem('editProjectId');
        }
        this.router.navigate(['/admin']);
    }

    ngOnDestroy(): void {
        // Clean up localStorage if navigating away
        if (this.isEditMode) {
            localStorage.removeItem('editProjectId');
        }
    }
}
