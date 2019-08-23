import {Injectable, OnDestroy} from '@angular/core';
import {HubConnection, HubConnectionBuilder, LogLevel} from '@aspnet/signalr';
import {BehaviorSubject, Subject} from 'rxjs';
import {ParcelDispatchedMessage} from '../data/parcel-dispatched-message';
import {OrderReleasedMessage} from '../data/order-released-message';
import {ReplenishedMessage} from '../data/replenished-message';
import {ReplenishmentRequestedMessage} from '../data/replenishment-requested-message';
import {StocksChangedMessage} from '../data/stocks-changed-message';
import {WebOrderReleasedMessage} from '../data/web-order-released-message';
import {CallOrderReleasedMessage} from '../data/call-order-released-message';
import {ParcelQueueChangedMessage} from '../data/parce-queue-changed-message';
import {OrderQueueChangedMessage} from '../data/order-queue-changed-message';
import {DelayChangedMessage} from '../data/delay-changed-message';

@Injectable({providedIn: 'root'})
export class SignalRService implements OnDestroy {
  public connectionState$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public onOrderReleased$: Subject<OrderReleasedMessage> = new Subject<OrderReleasedMessage>();
  public onParcelDispatched$: Subject<ParcelDispatchedMessage> = new Subject<ParcelDispatchedMessage>();
  public onReplenished$: Subject<ReplenishedMessage> = new Subject<ReplenishedMessage>();
  public onReplenishmentRequested$: Subject<ReplenishmentRequestedMessage> = new Subject<ReplenishmentRequestedMessage>();
  public onStocksChanged$: Subject<StocksChangedMessage> = new Subject<StocksChangedMessage>();
  public onWebOrderReleased$: Subject<WebOrderReleasedMessage> = new Subject<WebOrderReleasedMessage>();
  public onCallOrderReleased$: Subject<CallOrderReleasedMessage> = new Subject<CallOrderReleasedMessage>();
  public onParcelQueueChanged$: Subject<ParcelQueueChangedMessage> = new Subject<ParcelQueueChangedMessage>();
  public onOrderQueueChanged$: Subject<OrderQueueChangedMessage> = new Subject<OrderQueueChangedMessage>();
  public onDelayChanged$: Subject<DelayChangedMessage> = new Subject<DelayChangedMessage>();

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

    this.connection.on('OnStocksChangedMessageReceivedAsync', (message) => {
      this.onStocksChanged$.next(message);
    });

    this.connection.on('OnWebOrderReleasedMessageReceivedAsync', (message) => {
      this.onWebOrderReleased$.next(message);
    });

    this.connection.on('OnCallOrderReleasedMessageReceivedAsync', (message) => {
      this.onCallOrderReleased$.next(message);
    });

    this.connection.on('OnParcelQueueChangedMessageReceivedAsync', (message) => {
      this.onParcelQueueChanged$.next(message);
    });

    this.connection.on('OnOrderQueueChangedMessageReceivedAsync', (message) => {
      this.onOrderQueueChanged$.next(message);
    });

    this.connection.on('OnDelayChangedMessageReceivedAsync', (message) => {
      this.onDelayChanged$.next(message);
    });

  }
}
