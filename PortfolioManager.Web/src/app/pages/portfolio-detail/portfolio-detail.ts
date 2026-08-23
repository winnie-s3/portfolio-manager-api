import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PortfolioService } from '../../services/portfolio.service';
import { Portfolio } from '../../models/portfolio';

@Component({
  selector: 'app-portfolio-detail',
  imports: [RouterLink, DatePipe],
  templateUrl: './portfolio-detail.html',
  styleUrl: './portfolio-detail.css',
})
export class PortfolioDetail implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly portfolioService = inject(PortfolioService);

  portfolio = signal<Portfolio | null>(null);
  loading = signal(true);
  errorMessage = signal('');

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.portfolioService.getById(id).subscribe({
      next: portfolio => {
        this.portfolio.set(portfolio);
        this.loading.set(false);
      },

      error: error => {
        console.error('Erro ao carregar carteira', error);
        this.errorMessage.set('Não foi possível carregar a carteira.');
        this.loading.set(false);
      }
    });
  }
}
