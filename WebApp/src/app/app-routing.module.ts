import { NgModule } from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LabIComponent} from './modules/pages/lab-i/lab-i.component';

const routes: Routes = [
  {path: '', redirectTo: 'lab-i', pathMatch: 'full'},
  {path: 'lab-i', component: LabIComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
