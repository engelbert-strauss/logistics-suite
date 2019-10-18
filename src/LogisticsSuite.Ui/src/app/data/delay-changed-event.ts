import {OperationMode} from './operation-mode';
import {ServiceName} from './service-name';

export class DelayChangedEvent {
  public serviceName: ServiceName;
  public operationMode: OperationMode;
}
