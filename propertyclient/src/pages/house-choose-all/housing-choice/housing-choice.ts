import { OwnerInformationPage } from './../owner-information/owner-information';
import { Component, ViewChild, OnInit, Injector } from '@angular/core';
import { NavController, NavParams, Slides } from 'ionic-angular';
import { DeptInfo, Navigation, DeptType } from '../../../app/core/index';
import { AppComponentBase } from '../../component/app-component-base';
import { CountDownComponent } from '../../component/count-down/count-down.component';
import { HomePage } from '../../home/home';

/**
 * Generated class for the HousingChoicePage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-housing-choice',
  templateUrl: 'housing-choice.html'
})
export class HousingChoicePage extends AppComponentBase implements OnInit {
  deptList: DeptInfo[] = [];               // 内容列表数据
  navigationList: Navigation[] = [];       // 导航缩略菜单数据
  unitHouseList = [];                      // 单元楼层房屋列表数据
  showDeptList = [];                       // 显示到页面的分组数据
  slideDeptType: DeptType = DeptType.Build;// 缩略菜单类型
  selectedBulid: DeptInfo; // 选择的楼栋
  selectedUnit: DeptInfo;  // 选择的单元
  selectedHouse: DeptInfo; // 选择的房屋

  classActiceIndex: number = 0;

  @(ViewChild)('navSlides') navSlides: Slides;          // 导航缩略菜单
  @(ViewChild)('contentSlides') contentSlides: Slides;  // 显示内容区域
  @ViewChild('countDown') countDown: CountDownComponent;

  constructor(injector: Injector, public navCtrl: NavController, public navParams: NavParams) {
    super(injector);
  }

  // 页面初始化
  ngOnInit() {
    this.getBuildList();
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad HousingChoicePage');
  }

  getBuildList() {
    this.houseService.getBuildList(this.community.Id).subscribe((result) => {
      this.deptList = result.Items;
      this.navigationList = result.NavigationList;
      this.setShowDeptList();
    });
  }
  // 向上的按钮 每次跳5个
  goBack() {
    let index = this.navSlides.getActiveIndex();
    if (index < 5) {
      index = 0;
    } else {
      index = (index - 5);
    }
    this.navSlides.slideTo(index);
    // this.navSlides.slideToClickedSlide = true;
    // this.navSlides.slidePrev();
  }

  // 向下的按钮 每次跳5个 
  goForward() {
    let index = this.navSlides.getActiveIndex();
    let tindex = this.navigationList.length - 5;
    if (index > tindex) {
      index = index;
    } else {
      index = (index + 5);
    }
    this.navSlides.slideTo(index);
    // this.navSlides.slideNext();
  }


  // 跳转个人信息确认页面
  goInformation() {
    this.navCtrl.push(OwnerInformationPage);
  }
  /**
   * 点击缩略图
   * @param navigation 
   */
  goIndex(navigation: Navigation) {
    switch (navigation.DeptType) {
      case 1: // 楼栋
        {
          this.contentSlides.slideTo(navigation.Index);
          this.navSlides.slideTo(navigation.Index);
        }
        break;
      case 2: // 单元
        {
          this.contentSlides.slideTo(navigation.Index);
          this.navSlides.slideTo(navigation.Index);
        }
        break;
      case 3: // 楼层
        {
          // 导航菜单设置到 点击的楼层
          this.navSlides.slideTo(navigation.Index);
          // 内容菜单设置到0
          this.contentSlides.slideTo(0);
          this.deptList = this.unitHouseList[navigation.Index];
          this.setShowHouse();
        }
        break;
      default:
        break;
    }

  }
  /**
   * 设置每个slide 的内容
   */
  setShowDeptList() {
    this.showDeptList = [];
    this.navigationList.forEach(element => {
      let slide = [];
      for (let i = element.StartIndex - 1; i < element.EndIndex; i++) {
        slide.push(this.deptList[i]);
      }
      this.showDeptList.push(slide);
    });
  }
  /**
   * 缩略菜单变化 同步内容
   * 注：只有单元和楼栋才需要 缩略菜单和内容联动控制，楼层是实现刷新房屋
   */
  slideChanged() {
    let index = this.navSlides.getActiveIndex();
    // 如果是选择楼层需要单独处理
    if (this.slideDeptType === DeptType.Floor) {
      this.deptList = this.unitHouseList[index];
      this.setShowHouse();
    } else {
      this.contentSlides.slideTo(index);
    }
  }
  /**
   * 内容变化 同步缩略菜单
   * 注：只有单元和楼栋才需要 缩略菜单和内容联动控制，楼层是实现刷新房屋
   */
  slideContentChanged() {
    let index = this.contentSlides.getActiveIndex();
    // 排除楼层 不需要内容联动
    if (this.slideDeptType !== DeptType.Floor) {
      this.navSlides.slideTo(index);
    }
  }

  /** 单元部分 */
  goDept(dinfo: DeptInfo) {
    switch (dinfo.DeptType) {
      case 1: // 楼栋
        {
          this.getUnitList(dinfo.Id.toString());
          this.countDown.time = 60;
          this.slideDeptType = DeptType.Unit;
          this.selectedBulid = dinfo;
          if (this.selectedBulid.Name.indexOf('栋') <= 0) {
            this.selectedBulid.Name = this.selectedBulid.Name + '栋';
          }
        }
        break;
      case 2: // 单元
        {
          this.getHouseList(dinfo.Children);
          this.countDown.time = 60;
          this.slideDeptType = DeptType.Floor;
          this.selectedUnit = dinfo;
        }
        break;
      case 4: // 房屋
        {
          this.countDown.clearCountDownInterval();
          this.selectedHouse = dinfo;
          this.navCtrl.push(OwnerInformationPage, { houseInfo: dinfo });
        }
        break;
      default:
        break;
    }
  }

  getUnitList(buildId: string) {
    this.houseService.getUnitList(buildId).subscribe((result) => {
      this.deptList = result.Items;
      this.navigationList = result.NavigationList;
      this.setShowDeptList();
    });
  }

  getHouseList(data: any) {
    this.navigationList = data.NavigationList;
    this.unitHouseList = data.Items;
    this.deptList = this.unitHouseList[0];
    this.setShowHouse();
  }

  setShowHouse() {
    this.showDeptList = [];
    let slide = [];
    // 如果是单页显示 为25条，否则显示20条
    let pageSize: number = this.navigationList.length === 1 ? 25 : 20;
    for (let i = 0; i < this.deptList.length; i++) {
      slide.push(this.deptList[i]);
      if ((i + 1) % pageSize === 0) {
        this.showDeptList.push(slide);
        slide = [];
      }
    }
    if (slide.length > 0) {
      this.showDeptList.push(slide);
    }
  }

  goPrevious() {
    this.navCtrl.push(HomePage);
  }

}
