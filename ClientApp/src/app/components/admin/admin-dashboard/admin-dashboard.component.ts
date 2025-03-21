import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../../services/auth.service';
import { ProjectService } from '../../../services/project.service';
import { Project } from '../../../services/user.service';
import { Router } from '@angular/router';
import {
    ProjectCategoryService,
    ProjectCategoryDto,
} from '../../../services/project-category.service';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SkillService, Skill } from '../../../services/skill.service';
import {
    EducationService,
    Education,
} from '../../../services/education.service';
import {
    WorkExperienceService,
    WorkExperience,
} from '../../../services/work-experience.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
    selector: 'app-admin-dashboard',
    templateUrl: './admin-dashboard.component.html',
    styleUrls: ['./admin-dashboard.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        MatButtonModule,
        MatIconModule,
        MatCardModule,
        MatTableModule,
        MatProgressSpinnerModule,
        MatSnackBarModule,
        MatTooltipModule,
    ],
})
export class AdminDashboardComponent implements OnInit {
    activeTab: string = 'projects';
    projects: Project[] = [];
    skills: Skill[] = [];
    educations: Education[] = [];
    workExperiences: WorkExperience[] = [];
    isLoading = false;
    isLoadingSkills = false;
    isLoadingEducation = false;
    isLoadingWorkExperience = false;
    error: string | null = null;
    skillError: string | null = null;
    educationError: string | null = null;
    workExperienceError: string | null = null;
    displayedColumns: string[] = ['title', 'liveDemo', 'category', 'actions'];
    skillColumns: string[] = ['name', 'proficiency', 'actions'];
    educationColumns: string[] = ['institution', 'degree', 'period', 'actions'];
    workExperienceColumns: string[] = [
        'company',
        'position',
        'period',
        'actions',
    ];
    projectCategories: { [key: number]: ProjectCategoryDto } = {};

    constructor(
        private authService: AuthService,
        private projectService: ProjectService,
        private projectCategoryService: ProjectCategoryService,
        private skillService: SkillService,
        private educationService: EducationService,
        private workExperienceService: WorkExperienceService,
        private router: Router,
        private snackBar: MatSnackBar,
        private http: HttpClient
    ) {}

    ngOnInit(): void {
        // Check if user is authenticated
        if (!this.authService.isAuthenticated()) {
            // Redirect to login page or show login form
            console.log('User is not authenticated');
            this.router.navigate(['/login']);
        }

        this.loadProjects();
    }

    setActiveTab(tabName: string): void {
        this.activeTab = tabName;

        // Load data based on active tab
        if (tabName === 'projects' && this.projects.length === 0) {
            this.loadProjects();
        } else if (tabName === 'skills' && this.skills.length === 0) {
            this.loadSkills();
        } else if (tabName === 'education' && this.educations.length === 0) {
            this.loadEducations();
        } else if (
            tabName === 'experience' &&
            this.workExperiences.length === 0
        ) {
            this.loadWorkExperiences();
        }
    }

    loadProjects(): void {
        this.isLoading = true;
        this.error = null;

        // First check if user is authenticated
        if (!this.authService.isAuthenticated()) {
            this.error = 'You must be logged in to view projects';
            this.isLoading = false;
            this.router.navigate(['/login']);
            return;
        }

        this.projectService.getMyProjects().subscribe({
            next: (projects) => {
                this.projects = projects;
                if (projects && projects.length > 0) {
                    this.loadProjectCategories(projects);
                } else {
                    this.isLoading = false;
                }
            },
            error: (err) => {
                console.error('Error loading projects:', err);

                if (err.message === 'User not authenticated') {
                    this.error = 'You must be logged in to view projects';
                    this.router.navigate(['/login']);
                } else {
                    this.error = 'Failed to load projects. Please try again.';
                }

                this.isLoading = false;
            },
        });
    }

    loadSkills(): void {
        this.isLoadingSkills = true;
        this.skillError = null;

        // First check if user is authenticated
        if (!this.authService.isAuthenticated()) {
            this.skillError = 'You must be logged in to view skills';
            this.isLoadingSkills = false;
            this.router.navigate(['/login']);
            return;
        }

        const userId = this.authService.getUserId();
        if (!userId) {
            this.skillError = 'Unable to determine user ID';
            this.isLoadingSkills = false;
            return;
        }

        // Using direct HTTP request instead of service to match backend format
        this.http
            .get<any>(`${environment.apiUrl}/api/Skill/User/${userId}`, {
                withCredentials: true,
                headers: {
                    UserId: userId.toString(),
                },
            })
            .subscribe({
                next: (skills) => {
                    this.skills = skills;
                    this.isLoadingSkills = false;
                },
                error: (err) => {
                    console.error('Error loading skills:', err);
                    this.skillError =
                        'Failed to load skills. Please try again.';
                    this.isLoadingSkills = false;
                },
            });
    }

    editSkill(skill: Skill): void {
        // Store the skill ID in local storage to be retrieved by the form component
        localStorage.setItem('editSkillId', skill.id.toString());
        this.router.navigate(['/admin/skills/edit']);
    }

    deleteSkill(id: number): void {
        if (confirm('Are you sure you want to delete this skill?')) {
            this.isLoadingSkills = true;
            this.skillService.delete(id).subscribe({
                next: () => {
                    this.snackBar.open('Skill deleted successfully', 'Close', {
                        duration: 3000,
                    });
                    this.loadSkills(); // Reload the list
                },
                error: (err) => {
                    console.error('Error deleting skill:', err);
                    this.snackBar.open('Failed to delete skill', 'Close', {
                        duration: 3000,
                    });
                    this.isLoadingSkills = false;
                },
            });
        }
    }

    loadProjectCategories(projects: Project[]): void {
        const categoryRequests = projects.map((project) =>
            this.projectCategoryService.getByProjectId(project.id).pipe(
                catchError(() => {
                    console.warn(
                        `Category for project ${project.id} not found`
                    );
                    return of(null);
                })
            )
        );

        forkJoin(categoryRequests).subscribe({
            next: (categories: (ProjectCategoryDto | null)[]) => {
                this.projectCategories = categories
                    .filter(
                        (category): category is ProjectCategoryDto =>
                            category !== null
                    )
                    .reduce((acc, category) => {
                        acc[category.id] = category;
                        return acc;
                    }, {} as { [key: number]: ProjectCategoryDto });
                this.isLoading = false;
            },
            error: (err: Error) => {
                console.error('Error fetching project categories', err);
                this.isLoading = false;
            },
        });
    }

    getProjectCategoryName(project: Project): string {
        return this.projectCategories[project.projectCategoryId]?.name || 'N/A';
    }

    editProject(project: Project): void {
        // Store the project ID in local storage to be retrieved by the form component
        localStorage.setItem('editProjectId', project.id.toString());
        this.router.navigate(['/admin/projects/edit']);
    }

    deleteProject(id: number): void {
        if (confirm('Are you sure you want to delete this project?')) {
            this.isLoading = true;
            this.projectService.delete(id).subscribe({
                next: () => {
                    this.snackBar.open(
                        'Project deleted successfully',
                        'Close',
                        {
                            duration: 3000,
                        }
                    );
                    this.loadProjects(); // Reload the list
                },
                error: (err) => {
                    console.error('Error deleting project:', err);
                    this.snackBar.open('Failed to delete project', 'Close', {
                        duration: 3000,
                    });
                    this.isLoading = false;
                },
            });
        }
    }

    logout(): void {
        this.authService.logout();
    }

    loadEducations(): void {
        this.isLoadingEducation = true;
        this.educationError = null;

        // First check if user is authenticated
        if (!this.authService.isAuthenticated()) {
            this.educationError =
                'You must be logged in to view education entries';
            this.isLoadingEducation = false;
            this.router.navigate(['/login']);
            return;
        }

        this.educationService.getMyEducation().subscribe({
            next: (educations) => {
                this.educations = educations;
                this.isLoadingEducation = false;
            },
            error: (err) => {
                console.error('Error loading education entries:', err);
                this.educationError =
                    'Failed to load education entries. Please try again.';
                this.isLoadingEducation = false;
            },
        });
    }

    editEducation(education: Education): void {
        this.router.navigate(['/admin/education', education.id, 'edit']);
    }

    deleteEducation(id: number): void {
        if (confirm('Are you sure you want to delete this education entry?')) {
            this.isLoadingEducation = true;
            this.educationService.deleteEducation(id).subscribe({
                next: () => {
                    this.snackBar.open(
                        'Education entry deleted successfully',
                        'Close',
                        {
                            duration: 3000,
                        }
                    );
                    this.loadEducations(); // Reload the list
                },
                error: (err) => {
                    console.error('Error deleting education entry:', err);
                    this.snackBar.open(
                        'Failed to delete education entry',
                        'Close',
                        {
                            duration: 3000,
                        }
                    );
                    this.isLoadingEducation = false;
                },
            });
        }
    }

    loadWorkExperiences(): void {
        this.isLoadingWorkExperience = true;
        this.workExperienceError = null;

        // First check if user is authenticated
        if (!this.authService.isAuthenticated()) {
            this.workExperienceError =
                'You must be logged in to view work experiences';
            this.isLoadingWorkExperience = false;
            this.router.navigate(['/login']);
            return;
        }

        this.workExperienceService.getMyWorkExperiences().subscribe({
            next: (workExperiences) => {
                this.workExperiences = workExperiences;
                this.isLoadingWorkExperience = false;
            },
            error: (err) => {
                console.error('Error loading work experiences:', err);
                this.workExperienceError =
                    'Failed to load work experiences. Please try again.';
                this.isLoadingWorkExperience = false;
            },
        });
    }

    editWorkExperience(workExperience: WorkExperience): void {
        this.router.navigate(['/admin/experience', workExperience.id, 'edit']);
    }

    deleteWorkExperience(id: number): void {
        if (confirm('Are you sure you want to delete this work experience?')) {
            this.isLoadingWorkExperience = true;
            this.workExperienceService.deleteWorkExperience(id).subscribe({
                next: () => {
                    this.snackBar.open(
                        'Work experience deleted successfully',
                        'Close',
                        {
                            duration: 3000,
                        }
                    );
                    this.loadWorkExperiences(); // Reload the list
                },
                error: (err) => {
                    console.error('Error deleting work experience:', err);
                    this.snackBar.open(
                        'Failed to delete work experience',
                        'Close',
                        {
                            duration: 3000,
                        }
                    );
                    this.isLoadingWorkExperience = false;
                },
            });
        }
    }

    // Helper method to format dates for display
    formatDate(dateString: string | null): string {
        if (!dateString) return 'Present';
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'short',
        });
    }
}
