import { Component, OnInit, OnDestroy } from '@angular/core';
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
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router, ActivatedRoute } from '@angular/router';
import { SkillService } from '../../../../services/skill.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-skill-form',
    templateUrl: './skill-form.component.html',
    styleUrls: ['./skill-form.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatProgressSpinnerModule,
    ],
})
export class SkillFormComponent implements OnInit, OnDestroy {
    skillForm: FormGroup;
    isLoading = false;
    error: string | null = null;
    isEditMode = false;
    skillId: number | null = null;
    pageTitle = 'Add New Skill';
    private valueChangesSubscription: Subscription | undefined;

    constructor(
        private fb: FormBuilder,
        private skillService: SkillService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.skillForm = this.fb.group({
            name: ['', [Validators.required, Validators.minLength(2)]],
            proficiencyLevel: [
                50,
                [Validators.required, Validators.min(1), Validators.max(100)],
            ],
        });

        // Listen for changes to update the slider fill
        this.valueChangesSubscription = this.skillForm
            .get('proficiencyLevel')
            ?.valueChanges.subscribe((value) => {
                this.updateSliderFill(value);
            });

        // Check if we're in edit mode
        const editSkillId = localStorage.getItem('editSkillId');
        if (editSkillId) {
            this.isEditMode = true;
            this.skillId = parseInt(editSkillId, 10);
            this.pageTitle = 'Edit Skill';
        }
    }

    ngOnInit(): void {
        // Check if we're in edit mode
        const skillIdParam = this.route.snapshot.paramMap.get('id');
        if (skillIdParam) {
            this.skillId = parseInt(skillIdParam, 10);
            this.isEditMode = true;
            this.pageTitle = 'Edit Skill';
        }

        if (this.isEditMode && this.skillId) {
            this.loadSkill(this.skillId);
        } else {
            // Set initial slider fill on load
            setTimeout(() => {
                this.updateSliderFill(
                    this.skillForm.get('proficiencyLevel')?.value
                );
            });
        }
    }

    private loadSkill(id: number): void {
        this.isLoading = true;
        this.skillService.getById(id).subscribe({
            next: (skill) => {
                this.skillForm.patchValue({
                    name: skill.name,
                    proficiencyLevel: skill.proficiencyLevel,
                });
                this.isLoading = false;
            },
            error: (error) => {
                this.isLoading = false;
                this.error = 'Failed to load skill data';
                console.error('Error loading skill:', error);
            },
        });
    }

    onSubmit(): void {
        if (this.skillForm.valid) {
            this.isLoading = true;
            this.error = null;

            if (this.isEditMode && this.skillId) {
                this.skillService
                    .update(this.skillId, this.skillForm.value)
                    .subscribe({
                        next: () => {
                            localStorage.removeItem('editSkillId');
                            this.router.navigate(['/admin']);
                        },
                        error: this.handleError,
                    });
            } else {
                this.skillService.create(this.skillForm.value).subscribe({
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

        if (error.status === 0) {
            this.error = 'Network or CORS error. Please check your connection.';
        } else if (error.error && typeof error.error === 'string') {
            this.error = error.error;
        } else if (error.message) {
            this.error = error.message;
        } else {
            this.error =
                'Failed to ' +
                (this.isEditMode ? 'update' : 'create') +
                ' skill. Please try again.';
        }

        console.error(
            'Skill ' + (this.isEditMode ? 'update' : 'creation') + ' error:',
            error
        );
    };

    onCancel(): void {
        if (this.isEditMode) {
            localStorage.removeItem('editSkillId');
        }
        this.router.navigate(['/admin']);
    }

    ngOnDestroy(): void {
        if (this.valueChangesSubscription) {
            this.valueChangesSubscription.unsubscribe();
        }
        // Clean up localStorage if navigating away
        if (this.isEditMode) {
            localStorage.removeItem('editSkillId');
        }
    }

    updateSliderFill(value: number): void {
        document.documentElement.style.setProperty(
            '--slider-value',
            `${value}%`
        );
    }
}
