import {Component} from '@angular/core';
import {BackendService} from './services/backend.service';
import {DelayChangedEvent} from './data/delay-changed-event';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(private backendService: BackendService) {
  }

  public onDelayChanged(event: DelayChangedEvent): void {
    this.backendService.changeDelay(event);
  }
}
