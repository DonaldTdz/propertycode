import { HomePage } from './../../home/home';
import { Component, Injector, ViewChild } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AppComponentBase } from '../../component/app-component-base';
import { ClientUser } from '../../../app/core/index';
import { PayAllPage } from './../../pay-all/pay-all';
import { CountDownComponent } from '../../component/count-down/count-down.component';
import { OwnerInformationPage } from '../owner-information/owner-information';

@Component({
  selector: 'page-proprietor',
  templateUrl: 'proprietor.html'
})
export class ProprietorPage extends AppComponentBase {
  time: number = 60;
  user: ClientUser;
  phone1 = '';
  phone2 = '';
  phone3 = '';
  phone4 = '';
  readonly numbers = [{ code: 1, text: '1', type: '1' }, { code: 2, text: '2', type: '1' }, { code: 3, text: '3', type: '1' },
  { code: 4, text: '4', type: '1' }, { code: 5, text: '5', type: '1' }, { code: 6, text: '6', type: '1' },
  { code: 7, text: '7', type: '1' }, { code: 8, text: '8', type: '1' }, { code: 9, text: '9', type: '1' },
  { code: -1, text: '重置', type: '2' }, { code: 0, text: '0', type: '1' }, { code: -2, text: '删除', type: '2' }];

  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController, public navParams: NavParams) {
    super(injector);
    this.user = navParams.get('userInfo');
  }

  // 点击按键
  clickNumber(num: any) {
    if (num.type === '1') {
      this.setPhone4(num.text);
    } else {
      if (num.code === -1) {
        this.clearPhone4();
      } else {
        this.removePhone4();
      }
    }
  }

  setPhone4(num: any) {

    if (this.phone1 === '') {
      this.phone1 = num;
      return;
    }
    if (this.phone2 === '') {
      this.phone2 = num;
      return;
    }
    if (this.phone3 === '') {
      this.phone3 = num;
      return;
    }
    if (this.phone4 === '') {
      this.phone4 = num;
      return;
    }
  }

  clearPhone4() {
    this.phone1 = '';
    this.phone2 = '';
    this.phone3 = '';
    this.phone4 = '';
  }

  removePhone4() {
    if (this.phone4 !== '') {
      this.phone4 = '';
      return;
    }
    if (this.phone3 !== '') {
      this.phone3 = '';
      return;
    }
    if (this.phone2 !== '') {
      this.phone2 = '';
      return;
    }
    if (this.phone1 !== '') {
      this.phone1 = '';
      return;
    }
  }

  // 跳转物业缴费页面
  goPayAll() {
    if (this.phone1 === '' || this.phone2 === '' || this.phone3 === '' || this.phone4 === '') {
      this.msgBox.alert('请输入业主手机号最后四位');
      return;
    }
    let phone = this.phone1 + this.phone2 + this.phone3 + this.phone4;
    // 验证手机号最后四位
    let end4char = this.user.PhoneNumber.substring(7);
    // alert(phone)
    // alert(end4char)

    if (phone != end4char) {
      this.msgBox.alert('输入后四位手机号有误');
      return;
    }
    this.countDown.clearCountDownInterval();
    this.navCtrl.push(PayAllPage, { houseInfo: this.user.HouseData, userInfo: this.user, returnPage: 1 });
  }

  goPrevious() {
    this.navCtrl.push(OwnerInformationPage, { houseInfo : this.user.HouseData });
  }

}
