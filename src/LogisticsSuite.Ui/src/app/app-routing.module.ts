import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';

import {SuiteComponent} from './components/suite/suite.component';

const routes: Routes = [
  {
    path: 'suite',
    component: SuiteComponent
  },
  {
    path: 'suite/:mode',
    component: SuiteComponent
  },
  {
    path: '',
    redirectTo: '/suite',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
