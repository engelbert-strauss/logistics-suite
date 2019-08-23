import {StocksDto} from './stocks-dto';

export class StocksChangedMessage {
  public stocks: StocksDto[];
}
