/**
 * Configuration options for the confirmation dialog.
 */
export interface ConfirmationOptions {
  /** main text content of the confirmation message */
  message: string;
  /** title text displayed in the dialog header */
  header: string;
  /** primeng icon class to display. Defaults 'pi pi-exclamation-triangle' */
  icon: string;
  /** CSS class for the accept button. Defualt 'p-button-danger' */
  acceptButtonStyleClass?: string;
  /** 'Escape' key closes the dialog. Defualts to false */
  closeOnEscape?: boolean;
  /** Callback function executed when the 'Accept button is clicked.' */
  onAccept?: () => void;
  /** Optional callback function executed when the cancels or close btn is clicked. */
  onReject?: () => void;
}
