import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient } from '@angular/common/http';
import { MessageService, ConfirmationService } from 'primeng/api';
import { AppConst } from './app-const';
import { SharedUiModule } from "./shared/module/shared-ui.module";
import { RequestInterceptor } from './core/interceptor/request.interceptor';
import { HttpErrorInterceptor } from './core/interceptor/httpError.interceptor';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    SharedUiModule
],
  providers: [
    AppConst,
    {
      provide: APP_INITIALIZER,
      useFactory: (appConst: AppConst) => () => appConst.load(),
      deps: [AppConst],
      multi: true
    },
    provideHttpClient(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true
    },
    ConfirmationService,
    MessageService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
