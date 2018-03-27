import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular';
import { CountDownComponent } from './count-down/count-down.component';

const _modules = [
    IonicModule,
];

const _components = [
    CountDownComponent
];

@NgModule({
    imports: [
        ..._modules
    ],
    exports: [..._components],
    declarations: [
        ..._components
    ],
    entryComponents: [
        ..._components
    ],
    providers: [],
})
export class CommonComponentModule { }
