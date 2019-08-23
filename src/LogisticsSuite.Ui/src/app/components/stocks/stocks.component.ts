import {Component, OnInit} from '@angular/core';
import {SignalRService} from '../../services/signalr.service';
import {StocksDto} from '../../data/stocks-dto';
import {StocksChangedMessage} from '../../data/stocks-changed-message';

@Component({
  selector: 'app-stocks',
  templateUrl: './stocks.component.html',
  styleUrls: ['./stocks.component.scss']
})
export class StocksComponent implements OnInit {
  public stocks: StocksDto[] = [];

  constructor(private signalRService: SignalRService) {
  }

  public ngOnInit(): void {
    this.signalRService.onStocksChanged$.subscribe((message) => {
      this.onStocksChanged(message);
    });

    this.initializeStocks();
  }

  private onStocksChanged(message: StocksChangedMessage) {
    this.stocks = message.stocks;
  }

  private initializeStocks() {
    let i: number;

    for (i = 101001; i <= 101010; i++ ) {
      const dto = new StocksDto();

      dto.articleNo = i;
      dto.quantity = 100;

      this.stocks.push(dto);
    }
  }
}
