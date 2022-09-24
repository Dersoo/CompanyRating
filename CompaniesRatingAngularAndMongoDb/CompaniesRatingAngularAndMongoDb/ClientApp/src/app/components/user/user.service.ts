import { Injectable } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {User} from "../../models/user.model";
import {JwtHelperService} from "@auth0/angular-jwt";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class UserService{

  user: User = {
    id: '',
    firstName: '',
    lastName: '',
    email: '',
    age: 0,
    login: '',
    password: '',
    isAdmin: false
  };

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, private router: Router) {
    if(this.isUserAuthenticated()) {
      this.getUser();
    }
  }

  isUserAuthenticated(){
    const token: string =  localStorage.getItem("jwt") ?? '';

    return !!(token && !this.jwtHelper.isTokenExpired(token));
  }

  logOut() {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");

    this.user = {
      id: '',
      firstName: '',
      lastName: '',
      email: '',
      age: 0,
      login: '',
      password: '',
      isAdmin: false
    };

    this.router.navigate(["login"]);
  }

  getUser(){
    this.http.get<User>("https://localhost:7234/api/auth/user")
      .subscribe((Response) => {
        this.user = Response;
      });
  }

  getUserDirect(){
    return this.http.get<User>("https://localhost:7234/api/auth/user");
  }
}
