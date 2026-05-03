import {
  Component,
  Injector,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { catchError, Subject, takeUntil } from 'rxjs';
import { AccountService } from '../../../../core/service/account.service';
import { MvLoginInfoParam, MvLoginResponse } from '../../../../core/model/account.model';
import { MvResponse } from '../../../../shared/model/response.model';
import { AppComponent } from '../../../../app.component';
import { ResponseStatusEnum, ROUTE_PATHS } from '../../../../shared';
import { AuthService } from '../../../../core/service/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent extends AppComponent implements OnInit, OnDestroy {
  private __unSubscribeAll$: Subject<any>;
  formGroup!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private acc: AccountService,
    private injector: Injector
  ) {
    super(injector);
    this.__unSubscribeAll$ = new Subject<any>();
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.formGroup = this.fb.group({
      tenantCode: [null, Validators.required],
      userName: [null, Validators.required],
      password: [null, Validators.required],
    });
  }

  onSubmit() {
    if (this.formGroup.valid && this.formGroup.dirty) {
      const param = {
        ...this.formGroup.value
      } as MvLoginInfoParam;  

      this.acc.login(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe({
        next: (response: MvResponse<MvLoginResponse>) => {
          if(response.type === ResponseStatusEnum.success && response.data) {
            this.auth.setSession(response.data);
            this.navigate([ROUTE_PATHS.SCREEN_HOME])
            this.formGroup.reset();
          }
        },
        error: (err: any) => {
          this.showToast('warn', 'Login Failed', err.error?.message ?? 'Invalid credentials.');
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
