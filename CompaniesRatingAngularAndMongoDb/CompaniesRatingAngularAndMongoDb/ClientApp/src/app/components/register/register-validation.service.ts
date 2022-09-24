import { Injectable} from '@angular/core';
import {AbstractControl, AsyncValidatorFn, ValidationErrors} from '@angular/forms';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CustomvalidationService {

  constructor(private router: Router, private http: HttpClient) {
  }

  existingLoginValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
      return this.http.post<boolean>("https://localhost:7234/api/auth/isloginexisting", {"login" : control.value}).pipe(map(
        (isExistingLogin) => {
          return (isExistingLogin) ? { "loginNotAvailable": true } : null;
        }
      ));
    };
  }
}
