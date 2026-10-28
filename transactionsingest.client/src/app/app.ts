import { HttpClient } from '@angular/common/http';
import { Component, OnInit, signal } from '@angular/core';

interface Transaction {
  sourceSystem: string;
  transactionId: number;
  cardNumber: string;
  locationCode: string;
  productName: string;
  amount: number;
  transactionTime: Date;
}

@Component({
  selector: 'app-root',
  standalone: false,
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App implements OnInit {
  public readonly transactions = signal<Transaction[] | undefined>(undefined);
  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<Transaction[]>('/transactions').subscribe({
      next: (result) => this.transactions.set(result),
      error: (error) => console.error(error)
    });
  }

  protected readonly title = signal('transactionsingest.client');
}
