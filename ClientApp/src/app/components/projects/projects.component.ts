import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule } from '@angular/router';
import { ProjectService, Project } from '../../services/project.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    RouterModule
  ]
})
export class ProjectsComponent implements OnInit {
  projects: Project[] = [];
  loading = true;
  error = false;
  
  constructor(private projectService: ProjectService) {}
  
  ngOnInit(): void {
    this.loadProjects();
  }
  
  loadProjects(): void {
    // For now, we'll get all projects. In a real app, you might want to get projects for a specific user
    this.projectService.getAll().subscribe({
      next: (data) => {
        console.log('Projects data from API:', data);
        this.projects = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching projects', err);
        this.loading = false;
        this.error = true;
      }
    });
  }
}
