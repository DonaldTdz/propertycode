import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from './httpclient';
import { DeptInfo, ApiResult, Community, Navigation, ClientUser, Bill, ClientSubject, QRCode } from '../models';
import { Subject } from 'rxjs/Subject';
import { Storage } from '@ionic/storage';

@Injectable()
export class HouseService {

    community: Community;
    readonly pageSize: any = 25;

    constructor(private http: HttpClient, private storage: Storage) { }

    /**
     * 获取楼宇列表
     * 
     * @param {string} comDeptId 
     * 
     * @memberOf AuthService
     */
    getBuildList(comDeptId: string): Observable<any> {
        return this.http.get('PropertyClient/GetBuildListByComDeptId', {
            ComDeptId: comDeptId,
            PageSize: this.pageSize
        }).map(r => {
            let result = r.json() as ApiResult<any>;
            if (result.Code >= 0 && result.Data) {
                return result.Data;
            }
            let blist = { NavigationList: new Array<Navigation>(), Items: new Array<DeptInfo>() };
            return blist;
        }).catch(x => this.http.handleError(x));
    }
    /**
     * 设置小区
     * @param comDeptId 小区Id
     * @param comName 小区名称
     */
    setCommunity(comDeptId: number, comName: string) {
        this.community = new Community();
        this.community.Id = comDeptId;
        this.community.Name = comName;
        this.storage.set("community", this.community).then(() => console.log('community info saved. comId:' + comDeptId + ' comName:' + comName));;
    }

    getCommunity(): Observable<Community> {
        // this.storage.get("community").then(c => this._community = c);
        // return this._community;
        let emitter = new Subject<Community>();
        if (this.community) {
            setTimeout(() => emitter.next(this.community), 0); // mac os 中直接next无法获取数据
        } else {
            this.storage.get('community').then(u => { this.community = u; emitter.next(u) });
        }
        return emitter.asObservable();
    }

    /**
     * 获取单元和房屋列表
     * 
     * @param {string} comDeptId 
     * 
     * @memberOf AuthService
     */
    getUnitList(buildId: string): Observable<any> {
        return this.http.get('PropertyClient/GetUnitListByBuildId', {
            BuildId: buildId,
            PageSize: this.pageSize
        }).map(r => {
            let result = r.json() as ApiResult<any>;
            if (result.Code >= 0 && result.Data) {
                return result.Data;
            }
            let blist = { NavigationList: new Array<Navigation>(), Items: new Array<DeptInfo>() };
            return blist;
        }).catch(x => this.http.handleError(x));
    }

    /**
     * 通过房屋Id获取用户信息
     * @param houseDeptId 房屋DeptId
     */
    getUserInfoByHouseDeptId(houseDeptId: string): Observable<any> {
        return this.http.get('PropertyClient/GetUserInfoByHouseDeptId', {
            HouseDeptId: houseDeptId
        }).map(r => {
            let result = r.json() as ApiResult<any>;
            //if (result.Code >= 0 && result.Data) {
            //    return result.Data;
            //}
            //let user = new ClientUser
            //return user;
            return result;
        }).catch(x => this.http.handleError(x));
    }

    /**
     * 通过电话号码查找用户
     * @param phoneNum 电话号码
     */
    getUserInfoByPhoneNum(phoneNum: any): Observable<any> {
        return this.http.get('PropertyClient/GetUserInfoByPhoneNum', {
            ComDeptId: this.community.Id,
            PhoneNum: phoneNum
        }).map(r => {
            let result = r.json() as ApiResult<any>;
            if (result.Code >= 0) {
                return result;
            }
            let user = new ClientUser
            return user;
        }).catch(x => this.http.handleError(x));
    }

    /**
     * 通过房屋deptId查找账单列表
     * @param houseDeptId 房屋deptId
     */
    getBillListByHouseDeptId(houseDeptId: any): Observable<any> {
        return this.http.get('PropertyClient/GetBillListByHouseDeptId', {
            HouseDeptId: houseDeptId
        }).map(r => {
            let result = r.json() as ApiResult<any>;
            if (result.Code >= 0) {
                return result.Data;
            }
            let bill = new Array<Bill>();
            return bill;
        }).catch(x => this.http.handleError(x));
    }

    /**
     * 根据房屋deptId获取预存收费项目
     * @param houseDeptId 房屋的DeptId
     */
    getSubjectListByHouseDeptId(houseDeptId: any): Observable<any> {
        return this.http.get('PropertyClient/GetSubjectListByHouseDeptId', {
            HouseDeptId: houseDeptId
        }).map(r => {
            let result = r.json() as ApiResult<any>;
            if (result.Code >= 0) {
                return result.Data;
            }
            let subjects = new Array<ClientSubject>();
            return subjects;
        }).catch(x => this.http.handleError(x));
    }

    /**
     * 获取支付二维码
     * @param order 
     */
    QRCodePost(order: any): Observable<any> {
        order.CommunityName = this.community.Name;
        return this.http.post('PropertyClient/QRCodePost', order).map(r => {
            let result = r.json() as ApiResult<any>;
            if (result.Code >= 0) {
                return result.Data;
            }
            let qrcode = new QRCode();
            return qrcode;
        }).catch(x => this.http.handleError(x));
    }

    /**
     * 获取支付状态
     * 待支付 = 0,支付中 = 1,支付成功 = 2,支付失败 = 3,冻结中 = 4,取消 = 5
     * 若订单号不存在，会返回 订单号'ZNSB20178917240319FG1VOS41'不存在, 请核实
     * @param numericalNumber 支付流水号
     */
    getNumericalState(numericalNumber: any): Observable<any> {
        return this.http.get('PropertyClient/GetNumericalState', {
            NumericalNumber: numericalNumber
        }, false).map(r => {
            let result = r.json() as ApiResult<any>;
            if (result.Code >= 0) {
                return result.Data;
            }
            return '';
        }).catch(x => this.http.handleError(x));
    }

    /**
     * 取消扫描支付
     * @param numericalNumber 支付流水号
     */
    cancelPay(numericalNumber: any): Observable<any> {
        return this.http.postEx('PropertyClient/CancelPayPost', {
            NumericalNumber: numericalNumber
        }, false).map(r => {
            let result = r.json() as ApiResult<any>;
            return result;
        }).catch(x => this.http.handleError(x));
    }

    /**
     * 支付验证
     * @param order 支付订单信息
     */
    checkPaymentPost(order: any): Observable<any> {
        order.CommunityName = this.community.Name;
        return this.http.post('PropertyClient/CheckPaymentPost', order).map(r => {
            let result = r.json() as ApiResult<any>;
            return result;
        }).catch(x => this.http.handleError(x));
    }
}
