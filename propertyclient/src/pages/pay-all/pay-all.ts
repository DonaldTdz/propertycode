import { PaymentPage } from './../payment/payment';
import { MonthListPage } from './month-list/month-list';
import { HomePage } from './../home/home';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '../component/app-component-base';
import { NavController, NavParams, ModalController, Modal } from 'ionic-angular';
import { DeptInfo, Bill, ClientSubject, ClientUser } from '../../app/core/index';
import { CountDownComponent } from '../component/count-down/count-down.component';
import { ProprietorPage } from '../house-choose-all/proprietor/proprietor';
import { ToSubmitPage } from '../mobile/to-submit/to-submit';

@Component({
  selector: 'page-pay-all',
  templateUrl: 'pay-all.html',
})
export class PayAllPage extends AppComponentBase implements OnInit {
  // temp:string;
  //缴费部分
  house: DeptInfo;
  user: ClientUser;
  bills = Array<Bill>();
  payamount = 0;
  isCheckedAll = false;
  clickGroup = false;
  clickAll = false;
  clickItem = false;
  //预存费部分
  subjects = Array<ClientSubject>();
  month = 3;
  isSubjectCheckedAll = false;
  preamount = 0;
  clicksubject = false;
  clickallsubject = false;
  modal: Modal;
  returnPage: number;
  backParam: any;

  // 初始化为账
  pay = 'account';

  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController, public navParams: NavParams, public modalCtrl: ModalController) {
    super(injector);
    this.house = navParams.get('houseInfo');
    this.user = navParams.get('userInfo');
    this.returnPage = navParams.get('returnPage');
    this.backParam = navParams.get('backParam');
    this.pay = navParams.get('pay');
    let bs = navParams.get('bills');
    if (bs !== undefined) {
      this.bills = bs;
    }
    let ss = navParams.get('subjects');
    if (ss !== undefined) {
      this.subjects = ss;
    }
    let m = navParams.get('month');
    if (m !== undefined) {
      this.month = m;
    }
    let ca = navParams.get('isCheckedAll');
    if (ca !== undefined) {
      this.isCheckedAll = ca;
    }
    let sa = navParams.get('isSubjectCheckedAll');
    if (sa !== undefined) {
      this.isSubjectCheckedAll = sa;
    }
  }

  //页面初始化
  ngOnInit() {
    if (this.pay !== undefined) {
      if (this.bills.length > 0) {
        this.setPayAmount();
      } else {
        this.getBillList();
      }
      if (this.subjects.length > 0) {
        this.setPreAmount();
      } else {
        this.getSubjectList();
      }
    } else {
      this.pay = 'account';
      this.getBillList();
      this.getSubjectList();
    }

  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad PayAllPage');
  }

  // 跳转月份列表
  goMonthList() {
    this.modal = this.modalCtrl.create(MonthListPage);
    this.modal.onDidDismiss(data => {
      this.changeMonth(data);
    });
    this.modal.present();
  }

  // 跳转支付界面
  goPayment() {
    if (this.payamount === 0) {
      this.msgBox.alert('请选择账单');
      return;
    }
    let order = {
      PayType: 1, PayAmount: this.payamount,
      Ids: this.getSelectedBillIds(), HouseDeptId: this.house.Id, HouseNo: this.house.Name, FName: this.house.FName, UserId: this.user.UserId
    };
    // 检查验证
    this.houseService.checkPaymentPost(order).subscribe((result) => {
      if (result.Code === 0) {
        this.countDown.clearCountDownInterval();
        let param = {
          houseInfo: this.house,
          userInfo: this.user,
          returnPage: this.returnPage,
          backParam: this.backParam,
          bills: this.bills,
          pay: 'account',
          isCheckedAll: this.isCheckedAll
        }
        this.navCtrl.push(PaymentPage, { data: order, backParam: param });
      } else {
        this.msgBox.alert(result.Message);
        return;
      }
    });
  }

  // 下拉菜单
  // showAll(index, temp) {
  //  this.temp = temp + index;
  // }
  showAll(item: Bill) {
    item.IsUp = !item.IsUp;
  }

  getBillList() {
    this.houseService.getBillListByHouseDeptId(this.house.Id).subscribe((result) => {
      this.bills = result;
      this.isCheckedAll = true;
      this.setPayAmount();
    });
  }

  billGroupChange(bill: Bill) {
    if (this.clickGroup) {
      bill.BillData.forEach(element => {
        element.IsChecked = bill.IsChecked;
      });
      this.setPayAmount();
      this.clickGroup = false;
    }

    let bcheckall = true;
    this.bills.forEach(bi => {
      if (!bi.IsChecked) {
        bcheckall = false;
        return;
      }
    });
    this.isCheckedAll = bcheckall;
  }

  billChange(bill: Bill, bi: any) {
    if (this.clickItem) {
      if (bi.IsChecked) {
        let allChecked = true;
        bill.BillData.forEach(element => {
          if (!element.IsChecked) {
            allChecked = false;
            return;
          }
        });
        if (allChecked) {
          bill.IsChecked = true;
        }
      } else {
        bill.IsChecked = false;
      }
      this.setPayAmount();
      this.clickItem = false;
    }
  }

  billCheckAll() {
    if (this.clickAll) {
      this.bills.forEach(element => {
        element.IsChecked = this.isCheckedAll;
        element.BillData.forEach(item => {
          item.IsChecked = this.isCheckedAll;
        });
      });
      this.setPayAmount();
      this.clickAll = false;
    }
  }

  setPayAmount() {
    let total = 0;
    this.bills.forEach(bill => {
      if (bill.IsChecked) {
        total += bill.TotalAmount;
      } else {
        bill.BillData.forEach(element => {
          if (element.IsChecked) {
            total += element.Amount;
          }
        });
      }
    });
    this.payamount = Math.round(total * 100) / 100;//JavaScript数字处理
  }

  getSelectedBillIds(): Array<any> {
    let selectedBillIds = new Array<any>();
    this.bills.forEach(bill => {
      bill.BillData.forEach(element => {
        if (element.IsChecked) {
          selectedBillIds.push(element.BillId);
        }
      });
    });
    return selectedBillIds;
  }

  // 预存费部分
  changeMonth(m: any) {
    if (m < 1 || m > 24) {
      return;
    }
    this.month = m;
    this.subjects.forEach(s => {
      if (s.MonthAmount !== 0) {
        s.PreAmount = Math.round(s.MonthAmount * this.month * 100) / 100;
      }
    });
    this.setPreAmount();
  }

  changePreAmount(suj: ClientSubject, type: number) {
    if (!suj.IsChecked) {
      return;
    }
    if (type === 1) {
      if (suj.PreAmount < 9950) {
        suj.PreAmount += 50;
      } else {
        suj.PreAmount = 9950;
      }
    } else {
      if (suj.PreAmount > 50) {
        suj.PreAmount -= 50;
      } else {
        suj.PreAmount = 50;
      }
    }
    this.setPreAmount();
  }

  initPreAmount() {
    this.subjects.forEach(s => {
      if (s.MonthAmount === 0) {
        s.PreAmount = 100;//默认100
      } else {
        s.PreAmount = Math.round(s.MonthAmount * this.month * 100) / 100;
      }
    });
    this.setPreAmount();
  }

  getSubjectList() {
    this.houseService.getSubjectListByHouseDeptId(this.house.Id).subscribe((result) => {
      this.subjects = result;
      this.isSubjectCheckedAll = true;
      this.initPreAmount();
    });
  }

  subjectCheck(s: ClientSubject) {
    if (this.clicksubject) {
      if (!s.IsChecked) {
        this.isSubjectCheckedAll = false;
      } else {
        let checkAll = true;
        this.subjects.forEach(s => {
          if (!s.IsChecked) {
            checkAll = false;
            return;
          }
        });
        this.isSubjectCheckedAll = checkAll;
      }
      this.setPreAmount();
      this.clicksubject = false;
    }
  }

  subjectCheckAll() {
    if (this.clickallsubject) {
      this.subjects.forEach(s => {
        s.IsChecked = this.isSubjectCheckedAll;
      });
      this.setPreAmount();
      this.clickallsubject = false;
    }
  }

  setPreAmount() {
    let total = 0;
    this.subjects.forEach(sub => {
      if (sub.IsChecked) {
        total += sub.PreAmount;
      }
    });
    this.preamount = Math.round(total * 100) / 100; // JavaScript数字处理
  }

  getSelectedSubjectIds(): Array<any> {
    let selectedSubjectIds = new Array<any>();
    this.subjects.forEach(sub => {
      if (sub.IsChecked) {
        selectedSubjectIds.push(sub.SubjectId);
      }
    });
    return selectedSubjectIds;
  }

  getSelectedSubject(): Array<any> {
    let selectedSubject = new Array<any>();
    this.subjects.forEach(sub => {
      if (sub.IsChecked) {
        selectedSubject.push(sub);
      }
    });
    return selectedSubject;
  }

  // 预存费跳转支付界面
  goPrePayment() {
    if (this.preamount === 0) {
      this.msgBox.alert("请选择预存收费项目");
      return;
    }
    let order = {
      PayType: 2, PayAmount: this.preamount,
      Ids: [], Subjects: this.getSelectedSubject(), HouseDeptId: this.house.Id,
      HouseNo: this.house.Name, FName: this.house.FName, UserId: this.user.UserId, Month: this.month
    };
    // 检查验证
    this.houseService.checkPaymentPost(order).subscribe((result) => {
      if (result.Code === 0) {
        this.countDown.clearCountDownInterval();
        let param = {
          houseInfo: this.house,
          userInfo: this.user,
          returnPage: this.returnPage,
          backParam: this.backParam,
          subjects: this.subjects,
          pay: 'pre',
          month: this.month,
          isSubjectCheckedAll: this.isSubjectCheckedAll
        }
        this.navCtrl.push(PaymentPage, { data: order, backParam: param });
      } else {
        this.msgBox.alert(result.Message);
        return;
      }
    });
  }

  /**
   * 当跳转到首页时 关闭弹出框
   */
  countDownGoHome() {
    if (this.modal && this.modal !== null) {
      this.modal.dismiss();
    }
  }

  /**
   * 返回上一页
   */
  goPrevious() {
    if (this.returnPage === 1) {
      this.navCtrl.push(ProprietorPage, { userInfo: this.user });
    } else {
      this.navCtrl.push(ToSubmitPage, { userInfo: this.user, returnPage: this.backParam.returnPage, backParam: this.backParam.backParam });
    }
  }
}
