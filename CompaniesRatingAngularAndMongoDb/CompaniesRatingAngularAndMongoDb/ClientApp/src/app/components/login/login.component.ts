import {Component} from "@angular/core";
import {Router} from "@angular/router";
import {HttpClient, HttpErrorResponse, HttpHeaders} from "@angular/common/http";
import {NgForm} from "@angular/forms";
import {AuthenticatedResponse} from "../../models/authenticated.response.model";

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  invalidLogin: boolean = false;

  constructor(private router: Router, private http: HttpClient) {
  }

  login(form: NgForm){
    const credentials = {
      'userLogin': form.value.login,
      'password': form.value.password
    }

    this.http.post<AuthenticatedResponse>("https://localhost:7234/api/auth/login", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json"})
    }).subscribe({
      next: (response: AuthenticatedResponse) => {
        const token = response.token;
        const refreshToken = response.refreshToken;
        localStorage.setItem("jwt", token);
        localStorage.setItem("refreshToken", refreshToken);
        this.invalidLogin = false;
        this.router.navigate(["/"]);
      },
      error: (err: HttpErrorResponse) => this.invalidLogin = true
    })
  }

  loginAsGuest(){
    this.http.get<AuthenticatedResponse>("https://localhost:7234/api/auth/loginasguest").subscribe({
      next: (response: AuthenticatedResponse) => {
        const token = response.token;
        localStorage.setItem("jwt", token);
        this.invalidLogin = false;
        this.router.navigate(["/"]);
      },
      error: (err: HttpErrorResponse) => this.invalidLogin = true
    })
  }
}
