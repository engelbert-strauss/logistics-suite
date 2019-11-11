import {Injectable, OnDestroy} from '@angular/core';
import {HubConnection, HubConnectionBuilder, LogLevel} from '@aspnet/signalr';
import {BehaviorSubject, Subject} from 'rxjs';

import {ParcelDispatchedMessage} from '../data/parcel-dispatched-message';
import {OrderReleasedMessage} from '../data/order-released-message';
import {ReplenishedMessage} from '../data/replenished-message';
import {ReplenishmentRequestedMessage} from '../data/replenishment-requested-message';
import {WebOrderReleasedMessage} from '../data/web-order-released-message';
import {CallOrderReleasedMessage} from '../data/call-order-released-message';
import {StocksDto} from '../data/stocks-dto';
import {DelayDto} from '../data/delay-dto';

@Injectable({providedIn: 'root'})
export class SignalRService implements OnDestroy {
  public connectionState$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public onOrderReleased$: Subject<OrderReleasedMessage> = new Subject<OrderReleasedMessage>();
  public onParcelDispatched$: Subject<ParcelDispatchedMessage> = new Subject<ParcelDispatchedMessage>();
  public onReplenished$: Subject<ReplenishedMessage> = new Subject<ReplenishedMessage>();
  public onReplenishmentRequested$: Subject<ReplenishmentRequestedMessage> = new Subject<ReplenishmentRequestedMessage>();
  public onStocksChanged$: Subject<StocksDto[]> = new Subject<StocksDto[]>();
  public onWebOrderReleased$: Subject<WebOrderReleasedMessage> = new Subject<WebOrderReleasedMessage>();
  public onCallOrderReleased$: Subject<CallOrderReleasedMessage> = new Subject<CallOrderReleasedMessage>();
  public onParcelQueueChanged$: Subject<number> = new Subject<number>();
  public onOrderQueueChanged$: Subject<number> = new Subject<number>();
  public onDelayChanged$: Subject<DelayDto> = new Subject<DelayDto>();

  private connection: HubConnection;
  private connected = false;

  constructor() {
    this.connect();
  }

  public ngOnDestroy(): void {
    if (this.connection) {
      this.connection.stop();
      this.connected = false;
    }
  }

  public getSubjectByName(name: string): Subject<any> {
    if (name === 'onWebOrderReleased') {
      return this.onWebOrderReleased$;
    } else if (name === 'onOrderReleased') {
      return this.onOrderReleased$;
    } else if (name === 'onCallOrderReleased') {
      return this.onCallOrderReleased$;
    } else if (name === 'onReplenished') {
      return this.onReplenished$;
    } else if (name === 'onReplenishmentRequested') {
      return this.onReplenishmentRequested$;
    } else if (name === 'onParcelDispatched') {
      return this.onParcelDispatched$;
    } else if (name === 'onOrderQueueChanged') {
      return this.onOrderQueueChanged$;
    } else if (name === 'onParcelQueueChanged') {
      return this.onParcelQueueChanged$;
    }

    return null;
  }

  private connect(): void {
    this.connection = null;
    this.connection = new HubConnectionBuilder()
      .withUrl('logisticssuite.backend/ws')
      .configureLogging(LogLevel.Debug)
      .build();
    this.connection.start()
      .then(() => {
        this.connected = true;
        this.connectionState$.next(true);
        console.log('WebSocket connection established.');
        this.onConnected();
        this.connection.onclose(() => {
          console.log('WebSocket connection closed.');
          this.reconnect();
        });
      })
      .catch((err) => {
        this.connection.stop();
        console.log('Error while establishing connection: ' + err);
        this.connectionState$.next(false);
        this.reconnect();
      });
  }

  private reconnect(): void {
    setTimeout(() => {
      console.log('Try to reconnect to WebSocket');
      this.connect();
    }, 5000);
  }

  private onConnected(): void {
    this.connection.on('OnOrderReleasedMessageReceivedAsync', (message) => {
      this.onOrderReleased$.next(message);
    });

    this.connection.on('OnParcelDispatchedMessageReceivedAsync', (message) => {
      this.onParcelDispatched$.next(message);
    });

    this.connection.on('OnReplenishedMessageReceivedAsync', (message) => {
      this.onReplenished$.next(message);
    });

    this.connection.on('OnReplenishmentRequestedMessageReceivedAsync', (message) => {
      this.onReplenishmentRequested$.next(message);
    });

    this.connection.on('OnStocksChangedAsync', (stocks) => {
      this.onStocksChanged$.next(stocks);
    });

    this.connection.on('OnWebOrderReleasedMessageReceivedAsync', (message) => {
      this.onWebOrderReleased$.next(message);
    });

    this.connection.on('OnCallOrderReleasedMessageReceivedAsync', (message) => {
      this.onCallOrderReleased$.next(message);
    });

    this.connection.on('OnParcelQueueChangedAsync', (count) => {
      this.onParcelQueueChanged$.next(count);
    });

    this.connection.on('OnOrderQueueChangedAsync', (count) => {
      this.onOrderQueueChanged$.next(count);
    });

    this.connection.on('OnDelayChangedAsync', (delay) => {
      this.onDelayChanged$.next(delay);
    });

  }
}
