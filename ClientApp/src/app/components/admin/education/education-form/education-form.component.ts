import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {
    EducationService,
    Education,
    EducationCreateDto,
} from '../../../../services/education.service';
import { AuthService } from '../../../../services/auth.service';

@Component({
    selector: 'app-education-form',
    templateUrl: './education-form.component.html',
    styleUrls: ['./education-form.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatProgressSpinnerModule,
    ],
})
export class EducationFormComponent implements OnInit {
    educationForm: FormGroup;
    loading = false;
    error = '';
    isEditMode = false;
    educationId: number | null = null;

    constructor(
        private formBuilder: FormBuilder,
        private educationService: EducationService,
        private authService: AuthService,
        private route: ActivatedRoute,
        private router: Router
    ) {
        this.educationForm = this.formBuilder.group({
            institution: ['', Validators.required],
            degree: ['', Validators.required],
            startDate: ['', Validators.required],
            endDate: [''],
        });
    }

    ngOnInit(): void {
        // Check if we're in edit mode
        this.route.params.subscribe((params) => {
            if (params['id']) {
                this.isEditMode = true;
                this.educationId = +params['id'];
                this.loadEducation(this.educationId);
            }
        });
    }

    loadEducation(id: number): void {
        this.loading = true;
        this.educationService.getEducationById(id).subscribe({
            next: (education) => {
                this.educationForm.patchValue({
                    institution: education.institution,
                    degree: education.degree,
                    startDate: education.startDate,
                    endDate: education.endDate || '',
                });
                this.loading = false;
            },
            error: (err) => {
                this.error = 'Failed to load education details.';
                this.loading = false;
                console.error('Error loading education', err);
            },
        });
    }

    onSubmit(): void {
        if (this.educationForm.invalid) {
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
        const formValues = this.educationForm.value;

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

        const educationData: EducationCreateDto = {
            institution: formValues.institution,
            degree: formValues.degree,
            startDate: startDate,
            endDate: endDate,
            userId: userId,
        };

        console.log('Submitting education data:', educationData);

        if (this.isEditMode && this.educationId) {
            this.updateEducation(this.educationId, educationData);
        } else {
            this.createEducation(educationData);
        }
    }

    createEducation(educationData: EducationCreateDto): void {
        this.educationService.createEducation(educationData).subscribe({
            next: () => {
                this.router.navigate(['/admin/education']);
            },
            error: (err) => {
                console.error('Error creating education', err);
                // Show more detailed error message if available
                if (err.error && typeof err.error === 'string') {
                    this.error = err.error;
                } else if (err.error && err.error.message) {
                    this.error = err.error.message;
                } else {
                    this.error =
                        'Failed to create education entry. Please try again.';
                }
                this.loading = false;
            },
        });
    }

    updateEducation(id: number, educationData: EducationCreateDto): void {
        this.educationService.updateEducation(id, educationData).subscribe({
            next: () => {
                this.router.navigate(['/admin/education']);
            },
            error: (err) => {
                this.error = 'Failed to update education entry.';
                this.loading = false;
                console.error('Error updating education', err);
            },
        });
    }
}
