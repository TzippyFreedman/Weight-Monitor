import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {  ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import {HttpClientModule} from '@angular/common/http';
import { RegisterComponent } from './register/register.component';
import { UserComponent } from './user/user.component'
import { RouterModule } from '@angular/router';
import { AddMeasureComponent } from './add-measure/add-measure.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    UserComponent,
    AddMeasureComponent,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
     
         {path:'register',component:RegisterComponent},
        {path:'login',component:LoginComponent},
        {path:'user/:userFileId',component:UserComponent},
        {path:'measure',component:AddMeasureComponent},
        {path:"",redirectTo:'/login',pathMatch:"full"}
  ]),
],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {

  
 }
