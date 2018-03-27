import { ToSubmitPage } from './../to-submit/to-submit';
import { Component, Injector, ViewChild } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AppComponentBase } from '../../component/app-component-base';
import { ClientUser } from '../../../app/core/index';
import { CountDownComponent } from '../../component/count-down/count-down.component';
import { MobilePage } from '../mobile';

@Component({
  selector: 'page-customer-information',
  templateUrl: 'customer-information.html',
})
export class CustomerInformationPage extends AppComponentBase {
  user: ClientUser;

  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController, public navParams: NavParams) {
    super(injector);
    this.user = navParams.get('userInfo');
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad CustomerInformationPage');
  }

  // 确认页面
  goSubmit(house: any) {
    let newUser = new ClientUser();
    newUser.UserName = this.user.UserName;
    newUser.PhoneNumber = this.user.PhoneNumber;
    newUser.FUserName = this.user.FUserName;
    newUser.FPhoneNumber = this.user.FPhoneNumber;
    newUser.HouseData = house;
    this.countDown.clearCountDownInterval();
    this.navCtrl.push(ToSubmitPage, { userInfo: newUser, returnPage: 2, backParam: { userInfo: this.user } });
  }

  /**
   * 返回上一页
   */
  goPrevious() {
    this.navCtrl.push(MobilePage);
  }

}
