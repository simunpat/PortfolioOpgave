import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SkillService, Skill } from '../../services/skill.service';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-skills',
    templateUrl: './skills.component.html',
    styleUrls: ['./skills.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        MatChipsModule,
        MatProgressBarModule,
        MatDividerModule,
        MatProgressSpinnerModule,
    ],
})
export class SkillsComponent implements OnInit {
    skills: Skill[] = [];
    loading = true;
    error = false;
    isAdminView = false;

    constructor(
        private skillService: SkillService,
        private authService: AuthService,
        private route: ActivatedRoute
    ) {}

    ngOnInit(): void {
        // Check if we're in the admin view
        this.isAdminView = this.route.snapshot.url.some(
            (segment) => segment.path === 'admin'
        );
        this.loadSkills();
    }

    loadSkills(): void {
        // If authenticated and in admin view, get skills for the logged-in user
        const userId = this.authService.getUserId();

        if (this.isAdminView && userId) {
            this.loadUserSkills(userId);
        } else {
            // For public view, get all skills
            this.loadAllSkills();
        }
    }

    loadUserSkills(userId: number): void {
        this.skillService.getAllByUser(userId).subscribe({
            next: (data) => {
                console.log('User skills data from API:', data);
                this.skills = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error fetching user skills', err);
                this.loading = false;
                this.error = true;
            },
        });
    }

    loadAllSkills(): void {
        this.skillService.getAll().subscribe({
            next: (data) => {
                console.log('Skills data from API:', data);
                this.skills = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Error fetching skills', err);
                this.loading = false;
                this.error = true;
            },
        });
    }

    // Helper method to get skill level as a percentage
    getSkillLevelPercentage(proficiencyLevel: number): number {
        return (proficiencyLevel / 5) * 100; // Assuming proficiencyLevel is between 0-5
    }
}
