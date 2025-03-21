import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { EducationService, Education } from '../../services/education.service';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-education',
  templateUrl: './education.component.html',
  styleUrls: ['./education.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    RouterModule
  ]
})
export class EducationComponent implements OnInit {
  educationList: Education[] = [];
  loading = true;
  error = false;
  isAdminView = false;
  
  constructor(
    private educationService: EducationService,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {}
  
  ngOnInit(): void {
    // Check if we're in the admin view
    this.isAdminView = this.route.snapshot.url.some(segment => segment.path === 'admin');
    this.loadEducation();
  }
  
  loadEducation(): void {
    // If authenticated and in admin view, get education for the logged-in user
    const userId = this.authService.getUserId();
    
    if (this.isAdminView && userId) {
      this.loadUserEducation(userId);
    } else {
      // For public view, get all education entries
      this.loadAllEducation();
    }
  }
  
  loadUserEducation(userId: number): void {
    this.educationService.getUserEducation(userId).subscribe({
      next: (data) => {
        console.log('User education data from API:', data);
        this.educationList = this.sortEducationByDate(data);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching user education', err);
        this.loading = false;
        this.error = true;
      }
    });
  }
  
  loadAllEducation(): void {
    this.educationService.getAllEducation().subscribe({
      next: (data) => {
        console.log('Education data from API:', data);
        this.educationList = this.sortEducationByDate(data);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching education', err);
        this.loading = false;
        this.error = true;
      }
    });
  }
  
  deleteEducation(id: number): void {
    if (confirm('Are you sure you want to delete this education entry?')) {
      this.loading = true;
      this.educationService.deleteEducation(id).subscribe({
        next: () => {
          // Reload the education list after deletion
          this.loadEducation();
        },
        error: (err) => {
          console.error('Error deleting education', err);
          this.loading = false;
          this.error = true;
        }
      });
    }
  }
  
  // Sort education by end date (most recent first)
  private sortEducationByDate(education: Education[]): Education[] {
    return [...education].sort((a, b) => {
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
} 