import { ProprietorPage } from './../proprietor/proprietor';
import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AppComponentBase } from '../../component/app-component-base';
import { ClientUser, DeptInfo } from '../../../app/core/index';
import { CountDownComponent } from '../../component/count-down/count-down.component';
import { HousingChoicePage } from '../housing-choice/housing-choice';

/**
 * Generated class for the OwnerInformationPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-owner-information',
  templateUrl: 'owner-information.html',
})
export class OwnerInformationPage extends AppComponentBase implements OnInit {
  houseInfo: DeptInfo;
  user: ClientUser;
  showError: boolean = false;
  errorMsg: string = '';

  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController, public navParams: NavParams) {
    super(injector);
    this.houseInfo = navParams.get('houseInfo');
  }

  // 页面初始化
  ngOnInit() {
    this.getUserInfo();
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad OwnerInformationPage');
  }

  // 跳转手机号页面
  goProprietor() {
    this.countDown.clearCountDownInterval();
    this.user.HouseData = this.houseInfo;
    this.navCtrl.push(ProprietorPage, { userInfo: this.user });
  }

  getUserInfo() {
    this.houseService.getUserInfoByHouseDeptId(this.houseInfo.Id.toString()).subscribe((result) => {
      if (result.Code === 0 && result.Data) {
        this.user = result.Data;
      } else {
        this.errorMsg = result.Message;
        this.showError = true;
      }
    });
  }

  goPrevious() {
    this.navCtrl.push(HousingChoicePage);
  }
}
