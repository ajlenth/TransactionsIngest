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
  status: string;
}

interface Revision {
  id: number,
  field: string,
  from: string,
  to: string
}

@Component({
  selector: 'app-root',
  standalone: false,
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App implements OnInit {
  public readonly transactions = signal<Transaction[] | undefined>(undefined);
  public readonly auditLog = signal<Revision[] | undefined>(undefined);
  public viewMode = "Transactions"

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getTransactions();
  }

  getTransactions() {
    this.http.get<Transaction[]>('/transactions').subscribe({
      next: (result) => this.transactions.set(result),
      error: (error) => console.error(error)
    });
  }

  viewAuditLog(sourceSystem: string, transactionId: number) {
    this.http.get<Revision[]>('/auditLog/' + sourceSystem + "/" + transactionId).subscribe({
      next: (result) => {
        console.log(sourceSystem + "/" + transactionId)
        this.viewMode = "AuditLog"
        this.auditLog.set(result)
        this.transactions.set(undefined)
      },
      error: (error) => console.error(error)
    })
  }

  closeAuditLog(): void {
    this.viewMode = "Transactions"
    this.auditLog.set(undefined)
    this.getTransactions()
  }

  onFileSelected(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {

      var file = target.files[0]

      var fileReader = new FileReader();
      fileReader.onload = (e) => {
        const formData = new FormData();
        formData.append('file', file, file.name);

        const headers = { 'Content-Type': 'application/json' };

        this.http.post<Transaction[]>("/import", fileReader.result, {headers}).subscribe({
          next: (result) => this.transactions.set(result),
          error: (error) => {
            alert("Error importing data! Check console for more details.")
            console.error(error)
          }
        })
      }
      fileReader.readAsText(file);
    }
  }

  protected readonly title = signal('transactionsingest.client');
}
