import { MenuItem } from 'primeng/api';
import { MvMedia } from './media.model';

export const mediaMenuItem = (
  onArchive: (media: MvMedia) => void,
  media: MvMedia,
): MenuItem[] => [
  {
    label: 'Archive',
    icon: 'pi pi-inbox',
    command: () => onArchive(media),
  },
];
