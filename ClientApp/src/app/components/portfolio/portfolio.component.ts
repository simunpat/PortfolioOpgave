import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { forkJoin, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import {
    UserService,
    User,
    WorkExperience,
    Education,
    Project,
} from '../../services/user.service';
import {
    ProjectCategoryService,
    ProjectCategoryDto,
} from '../../services/project-category.service';
import { AuthService } from '../../services/auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-portfolio',
    templateUrl: './portfolio.component.html',
    styleUrls: ['./portfolio.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        MatIconModule,
        MatProgressSpinnerModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
    ],
})
export class PortfolioComponent implements OnInit {
    user: User | null = null;
    loading = true;
    error = false;
    projectCategories: { [key: number]: ProjectCategoryDto } = {};
    isOwnPortfolio = false;
    contactForm: FormGroup;
    isSubmitting = false;
    contactSuccess = false;
    contactError: string | null = null;

    constructor(
        private route: ActivatedRoute,
        private userService: UserService,
        private projectCategoryService: ProjectCategoryService,
        private authService: AuthService,
        private fb: FormBuilder,
        private http: HttpClient
    ) {
        this.contactForm = this.fb.group({
            name: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            subject: ['', Validators.required],
            message: ['', Validators.required],
        });
    }

    ngOnInit(): void {
        const userId = Number(this.route.snapshot.paramMap.get('id'));
        this.isOwnPortfolio = this.authService.getUserId() === userId;
        this.loadUserPortfolio(userId);
    }

    loadUserPortfolio(userId: number): void {
        this.userService.getByIdWithDetails(userId).subscribe({
            next: (data: User) => {
                this.user = data;
                if (data.projects && data.projects.length > 0) {
                    this.loadProjectCategories(data.projects);
                } else {
                    this.loading = false;
                }
            },
            error: (err: Error) => {
                console.error('Error fetching user portfolio', err);
                this.loading = false;
                this.error = true;
            },
        });
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
                this.loading = false;
            },
            error: (err: Error) => {
                console.error('Error fetching project categories', err);
                this.loading = false;
            },
        });
    }

    getProjectCategoryName(project: Project): string | null {
        return this.projectCategories[project.projectCategoryId]?.name || null;
    }

    submitContactForm(): void {
        if (this.contactForm.invalid) {
            return;
        }

        this.isSubmitting = true;
        this.contactSuccess = false;
        this.contactError = null;

        // Get the portfolio owner ID from the route
        const portfolioUserId = Number(this.route.snapshot.paramMap.get('id'));

        const contactData = {
            ...this.contactForm.value,
            portfolioUserId: portfolioUserId,
            dateSubmitted: new Date(),
        };

        // Typically you would have a real API endpoint to send this to
        // For now, we'll simulate a successful submission after a delay
        setTimeout(() => {
            console.log('Contact form submitted:', contactData);
            this.isSubmitting = false;
            this.contactSuccess = true;
            this.contactForm.reset();
        }, 1000);

        // When you have a real API endpoint, you would use code like this:
        /*
        this.http.post(`${environment.apiUrl}/api/contact`, contactData).subscribe({
            next: () => {
                this.isSubmitting = false;
                this.contactSuccess = true;
                this.contactForm.reset();
            },
            error: (err) => {
                this.isSubmitting = false;
                this.contactError = 'Failed to send message. Please try again later.';
                console.error('Error submitting contact form', err);
            }
        });
        */
    }
}
