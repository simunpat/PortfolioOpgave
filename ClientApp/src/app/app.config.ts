import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { AuthInterceptor } from './shared/auth-interceptor';
import { MaterialModule } from './shared/material.module';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';

export const appConfig: ApplicationConfig = {
    providers: [
        provideRouter(routes),
        provideHttpClient(withInterceptors([AuthInterceptor])),
        provideAnimations(),
        importProvidersFrom(
            MaterialModule,
            MatTabsModule,
            MatCardModule,
            MatButtonModule,
            MatIconModule,
            MatDividerModule,
            MatToolbarModule,
            MatMenuModule
        ),
    ],
};
