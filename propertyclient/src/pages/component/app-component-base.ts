import { Injector } from '@angular/core';
import { HouseService, Community, MessageBox } from '../../app/core/index';
import { NavController } from 'ionic-angular';

export abstract class AppComponentBase {
    community: Community = new Community();
    houseService: HouseService;
    msgBox: MessageBox;

    constructor(injector: Injector) {
        this.houseService = injector.get(HouseService);
        this.msgBox = injector.get(MessageBox);
        this.community = this.houseService.community;
        let navCtrl: NavController = injector.get(NavController);
        //console.log('begin length:'+navCtrl.length());
        navCtrl.remove(0, navCtrl.length());
        //console.log('end length:'+navCtrl.length());
    }
}