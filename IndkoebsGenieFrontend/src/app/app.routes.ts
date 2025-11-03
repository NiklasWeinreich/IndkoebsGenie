import { Routes } from '@angular/router';
import { AdminPanelComponent } from './components/admin-panel-component/admin-panel-component';

export const routes: Routes = [
    { path: 'about', loadComponent: () => import('./components/about-component/about-component').then(m => m.AboutComponent) },
    { path: 'current-list', loadComponent: () => import('./components/currrent-lists-component/currrent-lists-component').then(m => m.CurrrentListsComponent) },
    { path: 'create-list', loadComponent: () => import('./components/create-list-component/create-list-component').then(m => m.CreateListComponent) },
    { path: 'login-registrer', loadComponent: () => import('./components/login-registrer-component/login-registrer-component').then(m => m.LoginRegistrerComponent) },
    { path: 'admin-panel', loadComponent: () => import('./components/admin-panel-component/admin-panel-component').then(m => m.AdminPanelComponent) },
    {
        path: 'admin-panel',
        loadComponent: () => import('./components/admin-panel-component/admin-panel-component')
            .then(m => m.AdminPanelComponent),

        children: [
            {
                path: 'admin-users',
                loadComponent: () => import('./components/admin-panel-component/admin-panel-users-component/admin-panel-users-component')
                    .then(m => m.AdminPanelUsersComponent)
            },
        ]
    }
];
