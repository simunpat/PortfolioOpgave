import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css'],
    standalone: true,
    imports: [CommonModule, RouterModule, MatButtonModule, MatIconModule],
})
export class HeaderComponent {
    constructor(private authService: AuthService) {}

    get isLoggedIn(): boolean {
        return this.authService.isAuthenticated();
    }

    logout(): void {
        this.authService.logout();
    }
}
