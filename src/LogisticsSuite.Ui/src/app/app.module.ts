import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {HttpClientModule} from '@angular/common/http';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {SignalRService} from './services/signalr.service';
import {BackendService} from './services/backend.service';
import {NodeComponent} from './components/node/node.component';
import {StocksComponent} from './components/stocks/stocks.component';
import {QueueComponent} from './components/queue/queue.component';
import {SuiteComponent} from './components/suite/suite.component';

@NgModule({
  declarations: [
    AppComponent,
    NodeComponent,
    StocksComponent,
    QueueComponent,
    SuiteComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
  ],
  providers: [
    SignalRService,
    BackendService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
