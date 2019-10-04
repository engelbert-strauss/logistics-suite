import {Component, Input, OnInit} from '@angular/core';
import {SignalRService} from '../../services/signalr.service';

@Component({
  selector: 'app-queue',
  templateUrl: './queue.component.html',
  styleUrls: ['./queue.component.scss']
})
export class QueueComponent implements OnInit {
  public queueSize = 122;
  @Input() public headline: string;
  @Input() public subject: string;

  constructor(private signalRService: SignalRService) {
  }

  public ngOnInit(): void {
    this.signalRService.getSubjectByName(this.subject).subscribe((count) => {
      this.onQueueChanged(count);
    });
  }

  private onQueueChanged(count: number) {
    this.queueSize = Math.min(count, 122);
  }
}
