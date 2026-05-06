import { MenuItem } from 'primeng/api';
import { MvMedia } from './media.model';

export const mediaMenuItem = (
  onArchive: (media: MvMedia) => void,
  onUnarchive: (media: MvMedia) => void,
  media: MvMedia,
): MenuItem[] => [
  ...(media.status === 'ARCHIVED'
    ? [{ label: 'Unarchive', icon: 'pi pi-inbox', command: () => onUnarchive(media) }]
    : [{ label: 'Archive', icon: 'pi pi-inbox', command: () => onArchive(media) }]),
];
