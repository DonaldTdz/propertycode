import { PayAllPage } from './../../pay-all/pay-all';
import { HomePage } from './../../home/home';
import { Component, Injector, ViewChild } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AppComponentBase } from '../../component/app-component-base';
import { ClientUser } from '../../../app/core/index';
import { CountDownComponent } from '../../component/count-down/count-down.component';
import { MobilePage } from '../mobile';
import { CustomerInformationPage } from '../customer-information/customer-information';

/**
 * Generated class for the OwnerInformationPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-to-submit',
  templateUrl: 'to-submit.html',
})
export class ToSubmitPage extends AppComponentBase {
  user: ClientUser;
  returnPage: number;
  backParam: any;
  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController, public navParams: NavParams) {
    super(injector);
    this.user = navParams.get('userInfo');
    this.returnPage = navParams.get('returnPage');
    this.backParam = navParams.get('backParam');
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad ToSubmitPage');
  }

  // 跳转物业缴费页面
  goPayAll() {
    this.countDown.clearCountDownInterval();
    this.navCtrl.push(PayAllPage, { houseInfo: this.user.HouseData, userInfo: this.user, returnPage: 2, backParam: { returnPage: this.returnPage, backParam: this.backParam } });
  }

  /**
   * 返回上一页
   */
  goPrevious() {
    if (this.returnPage === 1) {
      this.navCtrl.push(MobilePage);
    } else {
      this.navCtrl.push(CustomerInformationPage, { userInfo: this.backParam.userInfo });
    }
  }

}
