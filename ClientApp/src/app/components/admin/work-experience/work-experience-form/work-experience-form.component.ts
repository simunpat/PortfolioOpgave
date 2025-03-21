import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {
    WorkExperienceService,
    WorkExperience,
    WorkExperienceCreateDto,
} from '../../../../services/work-experience.service';
import { AuthService } from '../../../../services/auth.service';

@Component({
    selector: 'app-work-experience-form',
    templateUrl: './work-experience-form.component.html',
    styleUrls: ['./work-experience-form.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatProgressSpinnerModule,
    ],
})
export class WorkExperienceFormComponent implements OnInit {
    workExperienceForm: FormGroup;
    loading = false;
    error = '';
    isEditMode = false;
    workExperienceId: number | null = null;

    constructor(
        private formBuilder: FormBuilder,
        private workExperienceService: WorkExperienceService,
        private authService: AuthService,
        private route: ActivatedRoute,
        private router: Router
    ) {
        this.workExperienceForm = this.formBuilder.group({
            company: ['', Validators.required],
            position: ['', Validators.required],
            startDate: ['', Validators.required],
            endDate: [''],
        });
    }

    ngOnInit(): void {
        // Check if we're in edit mode
        this.route.params.subscribe((params) => {
            if (params['id']) {
                this.isEditMode = true;
                this.workExperienceId = +params['id'];
                this.loadWorkExperience(this.workExperienceId);
            }
        });
    }

    loadWorkExperience(id: number): void {
        this.loading = true;
        this.workExperienceService.getWorkExperienceById(id).subscribe({
            next: (workExperience) => {
                this.workExperienceForm.patchValue({
                    company: workExperience.company,
                    position: workExperience.position,
                    startDate: workExperience.startDate,
                    endDate: workExperience.endDate || '',
                });
                this.loading = false;
            },
            error: (err) => {
                this.error = 'Failed to load work experience details.';
                this.loading = false;
                console.error('Error loading work experience', err);
            },
        });
    }

    onSubmit(): void {
        if (this.workExperienceForm.invalid) {
            return;
        }

        this.loading = true;
        this.error = '';

        const userId = this.authService.getUserId();
        if (!userId) {
            this.error = 'You must be logged in to perform this action.';
            this.loading = false;
            return;
        }

        // Get form values and ensure dates are properly formatted
        const formValues = this.workExperienceForm.value;

        // Ensure start date is a valid date
        let startDate: string;
        if (formValues.startDate) {
            const startDateObj = new Date(formValues.startDate);
            startDate = startDateObj.toISOString();
        } else {
            this.error = 'Start date is required';
            this.loading = false;
            return;
        }

        // For end date, use either the selected date or current date if empty
        let endDate: string;
        if (formValues.endDate) {
            const endDateObj = new Date(formValues.endDate);
            endDate = endDateObj.toISOString();
        } else {
            // Default to current date if empty
            endDate = new Date().toISOString();
        }

        const workExperienceData: WorkExperienceCreateDto = {
            company: formValues.company,
            position: formValues.position,
            startDate: startDate,
            endDate: endDate,
            userId: userId,
        };

        console.log('Submitting work experience data:', workExperienceData);

        if (this.isEditMode && this.workExperienceId) {
            this.updateWorkExperience(
                this.workExperienceId,
                workExperienceData
            );
        } else {
            this.createWorkExperience(workExperienceData);
        }
    }

    createWorkExperience(workExperienceData: WorkExperienceCreateDto): void {
        this.workExperienceService
            .createWorkExperience(workExperienceData)
            .subscribe({
                next: () => {
                    this.router.navigate(['/admin/dashboard']);
                    this.router.navigate(['/admin'], {
                        queryParams: { tab: 'experience' },
                    });
                },
                error: (err) => {
                    console.error('Error creating work experience', err);
                    // Show more detailed error message if available
                    if (err.error && typeof err.error === 'string') {
                        this.error = err.error;
                    } else if (err.error && err.error.message) {
                        this.error = err.error.message;
                    } else {
                        this.error =
                            'Failed to create work experience entry. Please try again.';
                    }
                    this.loading = false;
                },
            });
    }

    updateWorkExperience(
        id: number,
        workExperienceData: WorkExperienceCreateDto
    ): void {
        this.workExperienceService
            .updateWorkExperience(id, workExperienceData)
            .subscribe({
                next: () => {
                    this.router.navigate(['/admin/dashboard']);
                    this.router.navigate(['/admin'], {
                        queryParams: { tab: 'experience' },
                    });
                },
                error: (err) => {
                    console.error('Error updating work experience', err);
                    if (err.error && typeof err.error === 'string') {
                        this.error = err.error;
                    } else if (err.error && err.error.message) {
                        this.error = err.error.message;
                    } else {
                        this.error =
                            'Failed to update work experience entry. Please try again.';
                    }
                    this.loading = false;
                },
            });
    }
}
