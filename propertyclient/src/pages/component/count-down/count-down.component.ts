import { HomePage } from '../../home/home';
import { Component, OnInit, Input, OnDestroy, Output, EventEmitter } from '@angular/core';
import { NavController } from 'ionic-angular';

/**
 * Generated class for the OwnerInformationPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'count-down',
  templateUrl: 'count-down.component.html'
})
export class CountDownComponent implements OnInit, OnDestroy {

  @Input() time: number = 60;

  @Input() showBack: boolean = true;

  @Output() onGoHome = new EventEmitter<any>();

  @Output() onPrevious = new EventEmitter<any>();

  countDown: any;

  constructor(public navCtrl: NavController) {
  }

  //页面初始化
  ngOnInit() {
    // 倒计时
    this.startCountDown();
  }

  startCountDown() {
    this.countDown = setInterval(() => {
      if (this.time > 0) {
        this.time--;
      } else {
        this.goHome();
      }
    }, 1000);
  }

  ngOnDestroy() {
    clearInterval(this.countDown);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad CountDownComponent');
  }

  // 跳转首页
  goHome() {
    this.clearCountDownInterval();
    this.navCtrl.push(HomePage);
  }

  // 上一页
  goPrevious() {
    this.clearCountDownInterval();
    this.onPrevious.emit();
  }

  clearCountDownInterval() {
    this.onGoHome.emit();
    clearInterval(this.countDown);
    //console.log('clear count Down Interval');
  }
}
