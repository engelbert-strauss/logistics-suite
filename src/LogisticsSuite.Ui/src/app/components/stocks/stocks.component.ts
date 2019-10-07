import {Component, Input, OnInit} from '@angular/core';
import {SignalRService} from '../../services/signalr.service';
import {StocksDto} from '../../data/stocks-dto';

@Component({
  selector: 'app-stocks',
  templateUrl: './stocks.component.html',
  styleUrls: ['./stocks.component.scss']
})
export class StocksComponent implements OnInit {
  public stocks: StocksDto[] = [];
  @Input() public debugMode = false;

  constructor(private signalRService: SignalRService) {
  }

  public ngOnInit(): void {
    this.signalRService.onStocksChanged$.subscribe((stocks) => {
      this.onStocksChanged(stocks);
    });

    this.initializeStocks();
  }

  private onStocksChanged(stocks: StocksDto[]) {
    this.stocks = stocks;
  }

  private initializeStocks() {
    let i: number;
    let quantity = 100;

    for (i = 101001; i <= 101010; i++) {
      const dto = new StocksDto();

      dto.articleNo = i;
      dto.quantity = quantity;

      if (this.debugMode) {
        quantity -= 10;
      }

      this.stocks.push(dto);
    }
  }
}
