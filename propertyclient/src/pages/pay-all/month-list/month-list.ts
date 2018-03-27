import { Component } from '@angular/core';
import { NavController, NavParams, ViewController } from 'ionic-angular';

@Component({
  selector: 'page-month-list',
  templateUrl: 'month-list.html',
})
export class MonthListPage {

  constructor(public navCtrl: NavController, public navParams: NavParams, public viewCtrl: ViewController) {
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad MonthListPage');
  }

  // 月份列表
  readonly monthList = [{ code: 1, text: '1个月' }, { code: 2, text: '2个月' }, { code: 3, text: '3个月' }, { code: 4, text: '4个月' },
  { code: 5, text: '5个月' }, { code: 6, text: '6个月' }, { code: 7, text: '7个月' }, { code: 8, text: '8个月' },
  { code: 9, text: '9个月' }, { code: 10, text: '10个月' }, { code: 11, text: '11个月' }, { code: 12, text: '12个月' },
  { code: 13, text: '13个月' }, { code: 14, text: '14个月' }, { code: 15, text: '15个月' }, { code: 16, text: '16个月' },
  { code: 17, text: '17个月' }, { code: 18, text: '18个月' }, { code: 19, text: '19个月' }, { code: 20, text: '20个月' },
  { code: 21, text: '21个月' }, { code: 22, text: '22个月' }, { code: 23, text: '23个月' }, { code: 24, text: '24个月' }];

  // 关闭窗口
  close(month: any) {
    let m = month.code;
    this.viewCtrl.dismiss(m);
  }
}
