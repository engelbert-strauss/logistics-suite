import {Component, Input, OnInit} from '@angular/core';
import {SignalRService} from '../../services/signalr.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-queue',
  templateUrl: './queue.component.html',
  styleUrls: ['./queue.component.scss']
})
export class QueueComponent implements OnInit {
  public queueSize = 0;
  @Input() public debugMode = false;
  @Input() public headline: string;
  @Input() public subject: string;
  @Input() public maxQueueSize = 80;

  constructor(private signalRService: SignalRService) {
  }

  public ngOnInit(): void {
    this.signalRService.getSubjectByName(this.subject).subscribe((count) => {
      this.onQueueChanged(count);
    });

    if (this.debugMode) {
      this.queueSize = this.maxQueueSize * 0.5;
    }
  }

  private onQueueChanged(count: number) {
    this.queueSize = Math.min(count, this.maxQueueSize);
  }
}
