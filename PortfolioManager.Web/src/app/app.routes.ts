import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Portfolios } from './pages/portfolios/portfolios';
import { authGuard } from './guards/auth.guard';
import { PortfolioDetail } from './pages/portfolio-detail/portfolio-detail';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: Login
  },
  {
    path: 'portfolios',
    component: Portfolios,
    canActivate: [authGuard]
  },
  {
    path: 'portfolios/:id',
    component: PortfolioDetail,
    canActivate: [authGuard]
  }
];
