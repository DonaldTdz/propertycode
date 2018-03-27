import { Component, ViewChild } from '@angular/core';
import { Platform, Nav } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { HomePage } from '../pages/home/home';
import { PlatformService } from './core';


@Component({
  templateUrl: 'app.html'
})
export class MyApp {

  rootPage = HomePage;

  @ViewChild(Nav) nav: Nav;

  constructor(
    private platform: Platform,
    private statusBar: StatusBar,
    private splashScreen: SplashScreen,
    private platformService: PlatformService) {
    platform.ready().then(() => {
      this.splashScreen.hide();
      //(<any>window).navigator.splashscreen.hide();
      
      if (platform.is('ios') || platform.is('android')) {
        this.statusBar.styleDefault();
      }
      // alert(this.platform.getQueryParam("comId"));
      // 注册返回按键事件
      this.platformService.rootNav = this.nav;
      this.platformService.registerBackButton();
    });
  }
  
}
