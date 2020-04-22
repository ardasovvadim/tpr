import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {LabIComponent} from './modules/pages/lab-i/lab-i.component';
import {HeaderComponent} from './modules/common/header/header.component';
import {FooterComponent} from './modules/common/footer/footer.component';
import {RouterModule} from '@angular/router';
import {MatButtonModule} from '@angular/material/button';
import {AppRoutingModule} from './app-routing.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatCardModule} from '@angular/material/card';
import {MatSliderModule} from '@angular/material/slider';
import {CriterionComponent} from './modules/pages/lab-i/criterion/criterion.component';
import {CriterionDirective} from './modules/pages/lab-i/criterion.directive';
import {AlternativeService} from './modules/pages/lab-i/alternative.service';
import {ApiService} from './modules/core/services/api.service';
import {MatTableModule} from '@angular/material/table';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {AccessInterceptor} from './modules/core/interseprors/access.interceptor';
import {LabIIComponent} from './modules/pages/lab-ii/lab-i-i.component';
import { AnswersSequenceComponent } from './modules/pages/lab-ii/answers-sequence/answers-sequence.component';
import {MatTabsModule} from '@angular/material/tabs';
import { ClassificationTableIIComponent } from './modules/pages/lab-ii/classification-table-i-i/classification-table-i-i.component';
import {ClassificationService} from './modules/pages/lab-ii/classification.service';

@NgModule({
  declarations: [
    AppComponent,
    LabIComponent,
    HeaderComponent,
    FooterComponent,
    CriterionComponent,
    CriterionComponent,
    CriterionDirective,
    LabIIComponent,
    AnswersSequenceComponent,
    ClassificationTableIIComponent
  ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        RouterModule,
        MatButtonModule,
        AppRoutingModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatCardModule,
        MatSliderModule,
        FormsModule,
        MatTableModule,
        HttpClientModule,
        MatTabsModule,
    ],
  providers: [
    ApiService,
    AlternativeService,
    ClassificationService,
    {provide: HTTP_INTERCEPTORS, useClass: AccessInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
