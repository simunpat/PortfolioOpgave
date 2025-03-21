import { Component } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    Validators,
    ReactiveFormsModule,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatCardModule,
        MatIconModule,
        MatProgressSpinnerModule,
        RouterModule,
    ],
})
export class RegisterComponent {
    registerForm: FormGroup;
    error: string | null = null;
    loading = false;
    hidePassword = true;

    constructor(
        private authService: AuthService,
        private fb: FormBuilder,
        private router: Router
    ) {
        this.registerForm = this.fb.group({
            name: ['', [Validators.required]],
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(6)]],
        });
    }

    onSubmit(): void {
        if (this.registerForm.invalid) {
            return;
        }

        this.loading = true;
        this.error = null;

        this.authService.register(this.registerForm.value).subscribe({
            next: () => {
                this.loading = false;
                this.router.navigate(['/login'], {
                    queryParams: { registered: 'true' },
                });
            },
            error: (err) => {
                this.loading = false;
                this.error =
                    err.error || 'Registration failed. Please try again.';
            },
        });
    }
}
