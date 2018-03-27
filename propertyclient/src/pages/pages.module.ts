import { PaymentPageModule } from './payment/payment.module';
import { MonthListPage } from './pay-all/month-list/month-list';
import { PayAllPage } from './pay-all/pay-all';
import { MobileModule } from './mobile/mobile.module';
import { HouseChooseAllModule } from './house-choose-all/house-choose-all.module';
import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
import { SharedModule } from '../app/shared';
import { MobilePage } from './mobile/mobile';
import { HomePage } from './home/home';
import { CommonComponentModule } from './component/component.module';

import {HouseService, MessageBox} from '../app/core';

const _modules = [
    IonicModule,
    SharedModule,
    HouseChooseAllModule,
    MobileModule,
    PaymentPageModule,
    CommonComponentModule
];

const _pages = [
    MobilePage,
    HomePage,
    PayAllPage,
    MonthListPage
];

const _services = [HouseService, MessageBox];

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
    providers: [
        ..._services
     ],
})
export class PagesModule { }
