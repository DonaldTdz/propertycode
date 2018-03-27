import { HomePage } from './../home/home';
import { PayResultPage } from './pay-result/pay-result';
import { Component, Injector, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { AppComponentBase } from '../component/app-component-base';
import { CountDownComponent } from '../component/count-down/count-down.component';
import { PayAllPage } from '../pay-all/pay-all';

declare var QRCode: any;
//declare var jQuery : any;
/**
 * Generated class for the PaymentPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */
@IonicPage()
@Component({
  selector: 'page-payment',
  templateUrl: 'payment.html',
})
export class PaymentPage extends AppComponentBase implements OnInit, OnDestroy {
  time = 60;
  paydata: any;
  numericalNumber: any;
  payState: any = '';
  payDown: any;
  backParam: any;

  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController, public navParams: NavParams) {
    super(injector);
    this.paydata = navParams.get('data');
    this.backParam = navParams.get('backParam');
  }

  ngOnInit() {
    this.getQRCode();
    this.startPayDown();
  }

  ngOnDestroy() {
    this.storpPayDown();
    this.cancelPay();
    console.log('on destroy page');
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad PaymentPage');
  }

  getQRCode() {
    this.houseService.QRCodePost(this.paydata).subscribe((result) => {
      //设置支付流水号
      this.numericalNumber = result.NumericalNumber;
      //生成支付宝二维码
      this.generateQRcode('alipay_qrcode', result.AlipayUrl);
      //生成微信二维码
      this.generateQRcode('wechat_qrcode', result.WeChatUrl);
    });
  }

  //生成二维码
  generateQRcode(id: string, url: any) {
    /*$('#alipay_qrcode').qrcode({
              text: '支付宝',
              width: '230',
              height: '230',
              src: 'assets/images/pay_icon.png'
          });*/
    let qrcode = new QRCode(id, {
      text: url,
      width: 230,
      height: 230,
      //colorDark : '#000000',
      //colorLight : '#ffffff',
      //src: 'assets/images/pay_icon.png',
      correctLevel: QRCode.CorrectLevel.H
    });
    /*return jQuery('#'+id).qrcode({
      text	: url,
      width: 230,
      height: 230
	  });*/
  }

  getNumericalState() {
    this.houseService.getNumericalState(this.numericalNumber).subscribe((result) => {
      this.payState = result;
    });
  }

  startPayDown() {
    //每循环获取状态
    this.payDown = setInterval(() => {
      //支付成功
      if (this.payState == '2') {
        this.countDown.clearCountDownInterval();
        this.navCtrl.push(PayResultPage, { data: { IsSuccess: true, Msg: '支付成功', Amount: this.paydata.PayAmount, Data: this.paydata } });
      } else if (this.payState == '3' || this.payState == '4' || this.payState == '5' || this.payState == '-1' || this.payState == '7') {//支付失败
        let paymsg = '支付失败';
        if (this.payState == '4') {
          paymsg = '支付账户被冻结';
        }
        if (this.payState == '5') {
          paymsg = '用户取消';
        }
        if (this.payState == '-1') {
          paymsg = '支付异常';
        }
        if (this.payState == '-2') {
          paymsg = '支付账单不存在';
        }
        if (this.payState == '7') {
          paymsg = '同步账单失败，请联系物业管理员';
        }
        this.countDown.clearCountDownInterval();
        this.navCtrl.push(PayResultPage, { data: { IsSuccess: false, Msg: paymsg, Amount: 0, Data: this.paydata } });
      } else {//继续循环
        this.getNumericalState();
      }
    }, 4000);
  }

  storpPayDown() {
    if (this.payDown != null) {
      clearInterval(this.payDown);
      console.log('clear pay Down Interval');
    }
  }

  cancelPay() {
    if ((this.payState == '0' || this.payState == '') && this.numericalNumber) {
      this.houseService.cancelPay(this.numericalNumber).subscribe(result => {
        // alert(result.Message);
      });
    }
  }

  /**
  * 返回上一页
  */
  goPrevious() {
    this.navCtrl.push(PayAllPage, { houseInfo: this.backParam.houseInfo, userInfo: this.backParam.userInfo, 
      returnPage: this.backParam.returnPage, backParam: this.backParam.backParam, pay: this.backParam.pay, 
      bills: this.backParam.bills, subjects: this.backParam.subjects, month: this.backParam.month,
      isCheckedAll: this.backParam.isCheckedAll, isSubjectCheckedAll: this.backParam.isSubjectCheckedAll
     });
  }

}
