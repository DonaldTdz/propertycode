import { Injectable } from '@angular/core';
import { Platform, ToastController, Nav, App, ViewController, Toast, ActionSheet, Alert, Loading, Modal, Popover } from 'ionic-angular';
//import { TabsPage } from '../../../pages/tabs/tabs';

@Injectable()
export class PlatformService {
    /**
     * 用于判断返回键是否触发
     * 
     * @private
     * @type {boolean}
     * @memberOf PlatformService
     */
    private _backButtonPressed: boolean = false;

    private _rootNav: Nav;

    /**
     * 在首页加载完成后，设置根导航组件
     * 
     * @memberOf PlatformService
     */
    public set rootNav(nav: Nav) {
        this._rootNav = nav;
    }

    constructor(
        private appCtrl: App,
        private platform: Platform,
        private toastCtrl: ToastController
    ) {}

    /**
     * 注册返回键事件
     * 
     * @memberOf PlatformService
     */
    registerBackButton() {
        if (this.platform.is('android')) {
            this._registerAndroidExit();
        } else if (this._isWechat()) {
            // 添加一条返回历史
            this._pushHistory();
            // 注册返回事件
            this._registerBrowserBack();
            // 进入Tabs之外的子页面时，添加页面历史
            this.appCtrl.viewDidLoad.subscribe(this._onViewLoaded);
        }
    }


    /**
     * 全局页面加载事件
     * 
     * @private
     * @param {ViewController} viewCtl 
     * 
     * @memberOf PlatformService
     */
    private _onViewLoaded(viewCtl: ViewController) {
        console.log(viewCtl);
        if (
            viewCtl instanceof Toast
            || viewCtl instanceof ActionSheet
            || viewCtl instanceof Alert
            || viewCtl instanceof Loading
            || viewCtl instanceof Modal
            || viewCtl instanceof Popover) {
            return;
        }
        window.history.pushState({
            title: viewCtl.component.name,
            url: viewCtl.component.name
        }, viewCtl.component.name, viewCtl.component.name);
    }

    /**
     * 根据UserAgent判断是否微信浏览器
     * 
     * 
     * @memberOf PlatformService
     */
    private _isWechat() {
        let ua = navigator.userAgent.toLowerCase();
        let isWechat = /micromessenger/.test(ua);
        return isWechat;
    }

    /**
     * 向页面记录表添加虚拟记录，用来拦截物理返回键
     * 
     * @private
     * 
     * @memberOf PlatformService
     */
    private _pushHistory() {
        var state = {
            title: 'wechat-index',
            url: '#'
        };
        window.history.pushState(state, state.title, '#');
    }


    /**
     * 为安卓注册双击返回退出app
     * 
     * @private
     * 
     * @memberOf MyApp
     */
    private _registerAndroidExit() {
        this.platform.registerBackButtonAction((): any => this._onBackButtonClicked, 101);
    }

    /**
     * 注册浏览器返回事件
     */
    private _registerBrowserBack() {
        window.addEventListener('popstate', (e) => {
            this._onBackButtonClicked();
        }, false);
    }

    /**
     * 双击退出提示框，这里使用Ionic2的ToastController
     * 
     * @private
     * 
     * @memberOf MyApp
     */
    private _showExit() {
        if (this._backButtonPressed) {
            // 当触发标志为true时，即2秒内双击返回按键则退出APP
            if (this._isWechat()) {

            } else {
                this.platform.exitApp();
            }
        } else {
            let toast = this.toastCtrl.create({
                message: '再按一次退出应用',
                duration: 2000,
                position: 'bottom'
            });
            toast.present();
            this._backButtonPressed = true;
            // 2秒内没有再次点击返回则将触发标志标记为false
            setTimeout(() => {
                if (this._isWechat()) {
                    // 再次添加一条记录，否则下一次返回键将退出本页面
                    this._pushHistory();
                }
                this._backButtonPressed = false;
            }, 2000);
        }
    }

    /**
     * 响应返回键点击事件
     * 
     * @private
     * 
     * @memberOf MyApp
     */
    private _onBackButtonClicked() {
        let activeVC = this._rootNav.getActive();
        let page = activeVC.instance;
        /*if (!(page instanceof TabsPage)) {
            if (!this._rootNav.canGoBack()) {
                // 当前页面为tabs，退出APP
                return this._showExit();
            }
            // 当前页面为tabs的子页面，正常返回
            return this._rootNav.pop();
        }*/
        // 获取主Tab组件
        let tabs = page.tabs;
        let activeNav = tabs.getSelected();
        if (!activeNav.canGoBack()) {
            // 当前页面为tab栏，退出APP
            return this._showExit();
        }
        // 当前页面为tab栏的子页面，正常返回
        return activeNav.pop();
    }
}
