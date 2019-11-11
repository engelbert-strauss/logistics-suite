import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {BackendService} from '../../services/backend.service';
import {DelayChangedEvent} from '../../data/delay-changed-event';
import {ServiceName} from '../../data/service-name';

@Component({
  selector: 'app-suite',
  templateUrl: './suite.component.html',
  styleUrls: ['./suite.component.scss']
})
export class SuiteComponent implements OnInit {
  public debugMode = false;
  public maxQueueSize = 80;
  public ServiceName = ServiceName;

  constructor(private backendService: BackendService, private route: ActivatedRoute) {
  }

  public onDelayChanged(event: DelayChangedEvent): void {
    this.backendService.changeDelay(event);
  }

  public ngOnInit(): void {
    this.debugMode = this.route.snapshot.params.mode === 'debug';
    this.route.params.subscribe((params) => {
      this.debugMode = params.mode === 'debug';
    });

    if (this.route.snapshot.queryParamMap.get('queueSize')) {
      this.maxQueueSize = +this.route.snapshot.queryParamMap.get('queueSize');
    }

    this.route.queryParamMap.subscribe((queryParamMap) => {
      if (queryParamMap.get('queueSize')) {
        this.maxQueueSize = +this.route.snapshot.queryParamMap.get('queueSize');
      }
    });
  }
}
