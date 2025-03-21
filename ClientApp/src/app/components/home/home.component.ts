import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule, Router } from '@angular/router';
import { UserService, User } from '../../services/user.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        MatIconModule,
        MatProgressSpinnerModule,
        RouterModule,
    ],
})
export class HomeComponent implements OnInit {
    users: User[] = [];
    loading = true;
    error = false;

    constructor(private userService: UserService, private router: Router) {}

    ngOnInit(): void {
        this.loadUsers();
    }

    loadUsers(): void {
        this.userService.getAll().subscribe({
            next: (data: User[]) => {
                this.users = data;
                this.loading = false;
            },
            error: (err: any) => {
                console.error('Error fetching users', err);
                this.loading = false;
                this.error = true;
            },
        });
    }

    navigateToPortfolio(userId: number): void {
        this.router.navigate(['/portfolio', userId]);
    }
}
