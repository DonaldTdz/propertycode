import { MobilePage } from './../mobile/mobile';
import { Component, Injector } from '@angular/core';
import { NavController, Platform } from 'ionic-angular';
import { HousingChoicePage } from './../house-choose-all/housing-choice/housing-choice';
import { AppComponentBase } from '../component/app-component-base';

// declare var $ : any;

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage extends AppComponentBase {

  constructor(injector: Injector, public navCtrl: NavController, private platform: Platform) {
    super(injector);
    if (!this.community || !this.community.Id || this.community.Id === undefined) {
      // let comId = platform.getQueryParam('comId');
      // let comName = decodeURI(platform.getQueryParam('comName'));
      // this.houseService.setCommunity(comId, comName);
      this.houseService.setCommunity(14438,"物业社区1");
      // this.houseService.setCommunity(113344,"万树森林二期");
      // this.houseService.setCommunity(3866,"逸社区_锦心苑");
      this.houseService.getCommunity();
    }
  }

  // 跳转手机页
  goMobile() {
    this.navCtrl.push(MobilePage);
    // this.navCtrl.push(CountDownComponent);
  }
  // 跳转房屋选择
  goHousingChoice() {
    this.navCtrl.push(HousingChoicePage);
  }
}
