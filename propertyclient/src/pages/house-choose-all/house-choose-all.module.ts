import { ProprietorPage } from './proprietor/proprietor';
import { OwnerInformationPage } from './owner-information/owner-information';
import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
import { HousingChoicePage } from './housing-choice/housing-choice';
import { CommonComponentModule } from '../component/component.module';

const _modules = [
    IonicModule,
    CommonComponentModule
];

const _pages = [
    HousingChoicePage,
    OwnerInformationPage,
    ProprietorPage
];

@NgModule({
    imports: [
        ..._modules
    ],
    exports: [],
    declarations: [
        ..._pages
    ],
    entryComponents: [
        ..._pages
    ],
    providers: [],
})
export class HouseChooseAllModule { }
