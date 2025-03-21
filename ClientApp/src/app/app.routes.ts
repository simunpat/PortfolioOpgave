import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ContactComponent } from './components/contact/contact.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { EducationFormComponent } from './components/admin/education/education-form/education-form.component';
import { ProjectFormComponent } from './components/admin/projects/project-form/project-form.component';
import { SkillFormComponent } from './components/admin/skills/skill-form/skill-form.component';
import { WorkExperienceFormComponent } from './components/admin/work-experience/work-experience-form/work-experience-form.component';
import { AuthGuard } from './shared/auth-guard';
import { PortfolioComponent } from './components/portfolio/portfolio.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'portfolio/:id', component: PortfolioComponent },
    { path: 'contact', component: ContactComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    {
        path: 'admin',
        component: AdminDashboardComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'admin/projects/new',
        component: ProjectFormComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'admin/projects/edit',
        component: ProjectFormComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'admin/skills/new',
        component: SkillFormComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'admin/skills/edit',
        component: SkillFormComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'admin/education/new',
        component: EducationFormComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'admin/education/:id/edit',
        component: EducationFormComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'admin/experience/new',
        component: WorkExperienceFormComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'admin/experience/:id/edit',
        component: WorkExperienceFormComponent,
        canActivate: [AuthGuard],
    },
    { path: '**', redirectTo: '' }, // Redirect to home for any unknown routes
];
