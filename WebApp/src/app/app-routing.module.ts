import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LabIComponent} from './modules/pages/lab-i/lab-i.component';
import {LabIIComponent} from './modules/pages/lab-ii/lab-i-i.component';

const routes: Routes = [
  {path: '', redirectTo: 'lab-i', pathMatch: 'full'},
  {path: 'lab-i', component: LabIComponent},
  {path: 'lab-ii', component: LabIIComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
