import {RandomDelayDto} from './random-delay-dto';
import {PeriodicDelayDto} from './periodic-delay-dto';

export class DelayDto {
  public periodicDelays: PeriodicDelayDto[];
  public randomDelays: RandomDelayDto[];
}
