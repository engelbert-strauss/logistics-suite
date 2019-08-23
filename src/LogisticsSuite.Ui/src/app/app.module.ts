import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {SignalRService} from './services/signalr.service';
import {BackendService} from './services/backend.service';
import {HttpClientModule} from '@angular/common/http';
import {NodeComponent} from './components/node/node.component';
import {StocksComponent} from './components/stocks/stocks.component';
import {QueueComponent} from './components/queue/queue.component';

@NgModule({
  declarations: [
    AppComponent,
    NodeComponent,
    StocksComponent,
    QueueComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    SignalRService,
    BackendService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
