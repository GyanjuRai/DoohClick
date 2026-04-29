import { MenuItem } from 'primeng/api';
import { MvScreen } from './screen.model';

export const screenMenuItem = (
  onView: (screen: MvScreen) => void,
  onEdit: (screen: MvScreen) => void,
  onDelete: (screen: MvScreen) => void,
  screen: MvScreen,
): MenuItem[] => [
  {
    label: 'Options',
    items: [
      {
        label: 'View',
        icon: 'pi pi-eye',
        iconClass: 'text-blue-500',
        command: () => onView(screen),
      },
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        iconClass: 'text-orange-500',
        command: () => onEdit(screen),
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        iconClass: 'text-red-500',
        command: () => onDelete(screen),
      },
    ],
  },
];
