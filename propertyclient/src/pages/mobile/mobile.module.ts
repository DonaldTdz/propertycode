import { ToSubmitPage } from './to-submit/to-submit';
import { CustomerInformationPage } from './customer-information/customer-information';
import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
import { CommonComponentModule } from '../component/component.module';

const _modules = [
    IonicModule,
    CommonComponentModule
];

const _pages = [
    CustomerInformationPage,
    ToSubmitPage
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
export class MobileModule { }
