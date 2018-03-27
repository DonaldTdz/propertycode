import { CustomerInformationPage } from './customer-information/customer-information';
import { Component, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '../component/app-component-base';
import { ClientUser } from '../../app/core/index';

import { NavController } from 'ionic-angular';
import { ToSubmitPage } from './to-submit/to-submit';
import { CountDownComponent } from '../component/count-down/count-down.component';
import { HomePage } from '../home/home';

@Component({
  selector: 'page-mobile',
  templateUrl: 'mobile.html'
})
export class MobilePage extends AppComponentBase {
  phone: string = '';
  readonly numbers = [{ code: 1, text: '1', type: '1' }, { code: 2, text: '2', type: '1' }, { code: 3, text: '3', type: '1' },
  { code: 4, text: '4', type: '1' }, { code: 5, text: '5', type: '1' }, { code: 6, text: '6', type: '1' },
  { code: 7, text: '7', type: '1' }, { code: 8, text: '8', type: '1' }, { code: 9, text: '9', type: '1' },
  { code: -1, text: '重置', type: '2' }, { code: 0, text: '0', type: '1' }, { code: -2, text: '删除', type: '2' }];

  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController) {
    super(injector);
  }

  // 跳转业主信息页面
  goInformation() {
    if (this.phone.length != 11) {
      this.msgBox.alert('手机号格式有误（11位数字）');
      return;
    }
    this.houseService.getUserInfoByPhoneNum(this.phone).subscribe((result) => {
      if (result.Code != 0) {
        this.msgBox.alert(result.Message);
        return;
      }
      this.countDown.clearCountDownInterval();
      let uInfo = result.Data as ClientUser;
      // 如果房屋数>1
      if (uInfo.HouseData.length > 1) {
        this.navCtrl.push(CustomerInformationPage, { userInfo: uInfo });
      } else {
        // 只有一个房屋
        uInfo.HouseData = uInfo.HouseData[0];
        this.navCtrl.push(ToSubmitPage, { userInfo: uInfo, returnPage: 1 });
      }
    });
  }

  // 点击按键
  clickNumber(num: any) {
    if (num.type == '1') {
      this.setPhone(num.text);
    } else {
      if (num.code == -1) {
        this.clearPhone();
      } else {
        this.removePhone();
      }
    }
  }

  setPhone(num: any) {
    if (this.phone.length < 11) {
      this.phone += num;
    }
  }

  clearPhone() {
    this.phone = '';
  }

  removePhone() {
    let len = this.phone.length;
    this.phone = this.phone.substring(0, len - 1);
  }

  /**
   * 返回上一页
   */
  goPrevious() {
    this.navCtrl.push(HomePage);
  }
}
