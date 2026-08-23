import { Component, inject, OnInit, signal } from '@angular/core';
import { PortfolioService } from '../../services/portfolio.service';
import { Portfolio } from '../../models/portfolio';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-portfolios',
  imports: [RouterLink],
  templateUrl: './portfolios.html',
  styleUrl: './portfolios.css',
})
export class Portfolios implements OnInit {
  private readonly portfolioService = inject(PortfolioService);

  portfolios = signal<Portfolio[]>([]);
  loading = signal(true);
  errorMessage = signal('');

  ngOnInit(): void {
    this.loadPortfolios();
  }

  private loadPortfolios(): void {
    this.portfolioService.getAll().subscribe({
      next: portfolios => {
        this.portfolios.set(portfolios);
        this.loading.set(false);
      },

      error: error => {
        console.error('Erro ao carregar carteiras', error);
        this.errorMessage.set('Não foi possível carregar as carteiras.');
        this.loading.set(false);
      }
    });
  }
}
