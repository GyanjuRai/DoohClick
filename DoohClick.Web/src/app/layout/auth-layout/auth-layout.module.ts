import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthLayoutComponent } from './component/auth-layout.component';
import { LoginComponent } from './component/login/login.component';
import { RouterModule } from '@angular/router';
import { SharedUiModule } from '../../shared/module/shared-ui.module';
import { authRoutes } from './auth-layout.route';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    AuthLayoutComponent,
    LoginComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(authRoutes),
    SharedUiModule,
    ReactiveFormsModule,
  ]
})
export class AuthLayoutModule { }
