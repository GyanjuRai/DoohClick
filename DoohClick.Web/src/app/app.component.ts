import { Component, Injector } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ROUTE_PATHS } from './shared';
import { ConfirmationOptions } from './shared/model/confirmation.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  private _confirmationService: ConfirmationService;
  private _messageService: MessageService;

  protected readonly routes = ROUTE_PATHS;

  constructor(injector: Injector) {
    this._confirmationService = injector.get(ConfirmationService);
    this._messageService = injector.get(MessageService);
  }

  /**
   * Displays a toast notification to the user.
   * @param severity visual theme of the toast.
   * @param summary (default) Success. short, bold title for notification.
   * @param details descriptive body text providing more context.
   * @param life (Optional) duration in millisecond before the toast auto-dismissed.
   */
  protected showToast(
    severity: 'success' | 'info' | 'warn' | 'error',
    summary: string = 'Success',
    details: string,
    life: number = 3000,
  ) {
    this._messageService.add({
      severity: severity,
      summary: summary,
      detail: details,
      life: life,
    });
  }

  /**
   * Triggers a global confirmation dialog with the provided configuration.
   * @param confirmationOptions object containing labels, icons, and callback actions.
   */
  protected openConfirmationBox(confirmationOptions: ConfirmationOptions) {
    this._confirmationService.confirm({
      message: confirmationOptions.message,
      header: confirmationOptions.header,
      icon: confirmationOptions?.icon ?? 'pi pi-exclamation-triangle',
      acceptButtonStyleClass:
        confirmationOptions?.acceptButtonStyleClass ?? 'p-button-danger',
      closeOnEscape: confirmationOptions?.closeOnEscape ?? false,
      accept: confirmationOptions.onAccept,
      reject: confirmationOptions.onReject,
    });
  }

}
