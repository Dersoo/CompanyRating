import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgxPaginationModule } from "ngx-pagination";
import { JwtModule } from "@auth0/angular-jwt"

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { CompaniesComponent } from './components/companies/companies.component';
import {CompaniesService} from "./components/companies/companies.service";
import { PaginationComponent } from "src/app/components/pagination/pagination.component"
import { CompanyFormComponent } from "./components/company-form/company-form.component";
import {CompanyFormService} from "./components/company-form/company-form.service";

import {MatToolbarModule} from "@angular/material/toolbar";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {MatDialogModule} from '@angular/material/dialog';
import {MatInputModule} from "@angular/material/input";
import {MatSelectModule} from "@angular/material/select";
import {LoginComponent} from "./components/login/login.component";
import {RegisterComponent} from "./components/register/register.component";
import {AuthGuardService} from "./guards/auth-guard.service";
import {CustomvalidationService} from "./components/register/register-validation.service";
import {UserService} from "./components/user/user.service";
import {CompanyDetailsComponent} from "./components/company-details/company-details.component";
import {CompanyDetailsService} from "./components/company-details/company-details.service";
import {ReviewComponent} from "./components/review/review.component";
import {MatExpansionModule} from "@angular/material/expansion";
import {MatButtonToggleModule} from "@angular/material/button-toggle";
import {ReviewService} from "./components/review/review.service";

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    CompaniesComponent,
    PaginationComponent,
    CompanyFormComponent,
    LoginComponent,
    RegisterComponent,
    CompanyDetailsComponent,
    ReviewComponent
  ],
    imports: [
        BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
        HttpClientModule,
        NgxPaginationModule,
        FormsModule,
        RouterModule.forRoot([
            {path: '', component: CompaniesComponent, canActivate: [AuthGuardService], pathMatch: 'full'},
            {path: 'login', component: LoginComponent},
            {path: 'register', component: RegisterComponent}
        ]),
        BrowserAnimationsModule,
        MatToolbarModule,
        MatIconModule,
        MatButtonModule,
        MatDialogModule,
        MatInputModule,
        MatSelectModule,
        ReactiveFormsModule,
        JwtModule.forRoot({
            config: {
                tokenGetter: tokenGetter,
                allowedDomains: ["localhost:7234"],
                disallowedRoutes: []
            }
        }),
        MatExpansionModule,
        MatButtonToggleModule
    ],
  providers: [CompaniesService, CompanyFormService, AuthGuardService, CustomvalidationService, UserService, CompanyDetailsService, ReviewService],
  exports: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
