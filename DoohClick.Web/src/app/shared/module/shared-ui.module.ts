import { NgModule } from "@angular/core";
import { StyleClassModule } from 'primeng/styleclass';
import { MenubarModule } from 'primeng/menubar';
import { BadgeModule } from 'primeng/badge';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { SidebarModule } from 'primeng/sidebar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { TableModule } from 'primeng/table';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { TagModule } from 'primeng/tag';
import { DialogModule } from 'primeng/dialog'
import { InputNumberModule } from 'primeng/inputnumber';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { ChipModule } from 'primeng/chip'
import { MenuModule } from 'primeng/menu';
import { SpeedDialModule } from 'primeng/speeddial';
import { CardModule } from 'primeng/card';
import { ToolbarModule } from 'primeng/toolbar';
import { BlockUIModule } from 'primeng/blockui';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { PasswordModule } from 'primeng/password';
import { MultiSelectModule } from 'primeng/multiselect';
import { TabViewModule } from 'primeng/tabview';

import { ConfirmationService, MessageService } from "primeng/api";

const PRIME_NG_MODULES = [
        StyleClassModule,
        AvatarModule,
        BadgeModule,
        MenubarModule,
        RippleModule,
        ButtonModule,
        InputTextModule,
        SidebarModule,
        ConfirmDialogModule,
        ToastModule,
        TableModule,
        IconFieldModule,
        InputIconModule,
        TagModule,
        DialogModule,
        InputNumberModule,
        DropdownModule,
        FieldsetModule,
        ChipModule,
        MenuModule,
        SpeedDialModule,
        CardModule,
        ToolbarModule,
        BlockUIModule,
        ProgressSpinnerModule,
        PasswordModule,
        MultiSelectModule,
        TabViewModule,
];
 
@NgModule({
    imports: [
        ...PRIME_NG_MODULES
    ],
    providers: [
        ConfirmationService,
        MessageService
    ],
    exports: [
        ...PRIME_NG_MODULES
    ]
})

export class SharedUiModule {}