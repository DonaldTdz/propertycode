import { Component, Injector, ViewChild } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AppComponentBase } from '../../component/app-component-base';

import { PaymentPage } from '../payment';
import { CountDownComponent } from '../../component/count-down/count-down.component';

@Component({
  selector: 'page-pay-result',
  templateUrl: 'pay-result.html',
})
export class PayResultPage extends AppComponentBase {
  resultData: any;
  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController, public navParams: NavParams) {
    super(injector);
    this.resultData = navParams.get('data');
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad PayResultPage');
  }

  // 重试
  reTry() {
    this.countDown.clearCountDownInterval();
    this.navCtrl.push(PaymentPage, { data: this.resultData.Data });
  }
}
