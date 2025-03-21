import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { AuthService, LoginRequest } from '../../../services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        MatIconModule,
        RouterModule,
    ],
})
export class LoginComponent {
    loginForm: FormGroup;
    loading = false;
    error = '';
    hidePassword = true;

    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private router: Router
    ) {
        this.loginForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required],
        });
    }

    onSubmit(): void {
        if (this.loginForm.invalid) {
            return;
        }

        this.loading = true;
        this.error = '';

        const loginRequest: LoginRequest = {
            email: this.loginForm.value.email,
            password: this.loginForm.value.password,
        };

        this.authService.login(loginRequest).subscribe({
            next: (response) => {
                // Check if we got a valid userId
                if (response && response.userId && response.userId > 0) {
                    this.navigateToAdmin();
                } else {
                    // Try to get the user by email as a fallback
                    this.authService
                        .getUserByEmail(loginRequest.email)
                        .subscribe({
                            next: (userResponse) => {
                                if (userResponse && userResponse.userId) {
                                    this.navigateToAdmin();
                                } else {
                                    this.error =
                                        'Could not retrieve user information.';
                                    this.loading = false;
                                }
                            },
                            error: (err) => {
                                this.error =
                                    'Login succeeded but user retrieval failed.';
                                this.loading = false;
                            },
                        });
                }
            },
            error: (err) => {
                this.error = 'Login failed. Please check your credentials.';
                this.loading = false;
            },
        });
    }

    private navigateToAdmin(): void {
        // Navigate to admin
        this.router.navigate(['/admin']).then(
            () => {
                this.loading = false;
            },
            () => {
                this.loading = false;
            }
        );
    }
}
