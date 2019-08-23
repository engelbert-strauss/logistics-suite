import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {DelayChangedEvent} from '../data/delay-changed-event';

@Injectable()
export class BackendService {
  constructor(private httpClient: HttpClient) {
  }

  public changeDelay(event: DelayChangedEvent): void {
    this.httpClient.post('logisticssuite.backend/delay', event)
      .subscribe(() => {}, (error) => {
        console.log(error);
      });
  }
}
