import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { WorkExperienceService, WorkExperience } from '../../services/work-experience.service';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-work-experience',
  templateUrl: './work-experience.component.html',
  styleUrls: ['./work-experience.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule
  ]
})
export class WorkExperienceComponent implements OnInit {
  experiences: WorkExperience[] = [];
  loading = true;
  error = false;
  isAdminView = false;
  
  constructor(
    private workExperienceService: WorkExperienceService,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {}
  
  ngOnInit(): void {
    // Check if we're in the admin view
    this.isAdminView = this.route.snapshot.url.some(segment => segment.path === 'admin');
    this.loadWorkExperiences();
  }
  
  loadWorkExperiences(): void {
    // If authenticated and in admin view, get work experiences for the logged-in user
    const userId = this.authService.getUserId();
    
    if (this.isAdminView && userId) {
      this.loadUserWorkExperiences(userId);
    } else {
      // For public view, get all work experiences
      this.loadAllWorkExperiences();
    }
  }
  
  loadUserWorkExperiences(userId: number): void {
    this.workExperienceService.getUserWorkExperiences(userId).subscribe({
      next: (data) => {
        console.log('User work experience data from API:', data);
        this.experiences = this.sortExperiencesByDate(data);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching user work experiences', err);
        this.loading = false;
        this.error = true;
      }
    });
  }
  
  loadAllWorkExperiences(): void {
    this.workExperienceService.getAllWorkExperiences().subscribe({
      next: (data) => {
        console.log('Work experience data from API:', data);
        this.experiences = this.sortExperiencesByDate(data);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching work experiences', err);
        this.loading = false;
        this.error = true;
      }
    });
  }
  
  // Sort work experiences by end date (most recent first)
  private sortExperiencesByDate(experiences: WorkExperience[]): WorkExperience[] {
    return [...experiences].sort((a, b) => {
      const dateA = a.endDate ? new Date(a.endDate).getTime() : Date.now();
      const dateB = b.endDate ? new Date(b.endDate).getTime() : Date.now();
      return dateB - dateA;
    });
  }
  
  // Format date for display
  formatDate(dateString: string | undefined): string {
    if (!dateString) return 'Present';
    
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short' });
  }
  
  // Calculate duration of employment
  calculateDuration(startDate: string, endDate: string | undefined): string {
    const start = new Date(startDate);
    const end = endDate ? new Date(endDate) : new Date();
    
    const years = end.getFullYear() - start.getFullYear();
    const months = end.getMonth() - start.getMonth();
    
    let result = '';
    
    if (years > 0) {
      result += `${years} year${years > 1 ? 's' : ''}`;
    }
    
    if (months > 0 || (years === 0 && months === 0)) {
      if (result.length > 0) result += ', ';
      result += `${months > 0 ? months : 1} month${months > 1 || months === 0 ? 's' : ''}`;
    }
    
    return result;
  }
} 