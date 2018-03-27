import { PayResultPage } from './pay-result/pay-result';
import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { PaymentPage } from './payment';
import { CommonComponentModule } from '../component/component.module';

const _pages = [
  PayResultPage
]
@NgModule({
  declarations: [
    PaymentPage,
    ..._pages
  ],
  entryComponents: [
    ..._pages
  ],
  imports: [
    IonicPageModule.forChild(PaymentPage),
    CommonComponentModule
  ],
  exports: [
    PaymentPage,
  ]
})
export class PaymentPageModule { }
