import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {SignalRService} from '../../services/signalr.service';
import {DelayChangedEvent} from '../../data/delay-changed-event';
import {ServiceName} from '../../data/service-name';
import {OperationMode} from '../../data/operation-mode';

@Component({
  selector: 'app-node',
  templateUrl: './node.component.html',
  styleUrls: ['./node.component.scss']
})
export class NodeComponent implements OnInit {
  public ltr: any[] = [];
  public rtl: any[] = [];
  public ttb: any[] = [];
  public delay: string = null;
  @Input() public debugMode = false;
  @Input() public serviceId: string = null;
  @Input() public name: ServiceName;
  @Input() public right: string = null;
  @Input() public left: string = null;
  @Input() public bottom: string = null;
  @Input() public leftOffset = false;
  @Input() public rightOffset = false;
  @Input() public buttons: 'None' | 'Minus' | 'Plus' | 'Both' = 'None';
  @Output() public delayChanged: EventEmitter<DelayChangedEvent> = new EventEmitter<DelayChangedEvent>();

  constructor(private signalRService: SignalRService) {
  }

  public ngOnInit(): void {
    if (this.right) {
      this.signalRService.getSubjectByName(this.right).subscribe((message) => {
        this.onLtrLinkAdded(message);
      });

      if (this.debugMode) {
        this.ltr.push('');
      }
    }

    if (this.left) {
      this.signalRService.getSubjectByName(this.left).subscribe((message) => {
        this.onRtlLinkAdded(message);
      });

      if (this.debugMode) {
        this.rtl.push('');
      }
    }

    if (this.bottom) {
      this.signalRService.getSubjectByName(this.bottom).subscribe((message) => {
        this.onTtbLinkAdded(message);
      });

      if (this.debugMode) {
        this.ttb.push('');
      }
    }

    if (this.serviceId) {
      this.signalRService.onDelayChanged$.subscribe((delay) => {
        let index = delay.randomDelays.findIndex((x) => x.service === this.serviceId);

        if (index >= 0) {
          this.delay = `${delay.randomDelays[index].minValue} / ${delay.randomDelays[index].maxValue}`;
        } else {
          index = delay.periodicDelays.findIndex((x) => x.service === this.serviceId);

          if (index >= 0) {
            this.delay = `${delay.periodicDelays[index].value}`;
          }
        }
      });

      if (this.debugMode) {
        this.delay = '100 / 500';
      }
    }
  }

  public onMinusClicked(): void {
    const event = new DelayChangedEvent();

    event.serviceName = this.name;
    event.operationMode = OperationMode.Decrease;

    this.delayChanged.emit(event);
  }

  public onPlusClicked(): void {
    const event = new DelayChangedEvent();

    event.serviceName = this.name;
    event.operationMode = OperationMode.Increase;

    this.delayChanged.emit(event);
  }

  public getName() {
    return ServiceName[this.name];
  }

  private onLtrLinkAdded(message: any) {
    this.ltr.push(message);
    setTimeout(() => {
      this.ltr.shift();
    }, 1000);
  }

  private onRtlLinkAdded(message: any) {
    this.rtl.push(message);
    setTimeout(() => {
      this.rtl.shift();
    }, 1000);
  }

  private onTtbLinkAdded(message: any) {
    this.ttb.push(message);
    setTimeout(() => {
      this.ttb.shift();
    }, 1000);
  }
}
