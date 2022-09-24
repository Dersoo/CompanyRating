import {Component, OnInit} from "@angular/core";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {CustomvalidationService} from "./register-validation.service";

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  form!: FormGroup;
  submitted = false;

  constructor(private router: Router,
              private http: HttpClient,
              private formBuilder: FormBuilder,
              private customValidator: CustomvalidationService) {
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      //id : [''],
      firstName : ['', [Validators.required, Validators.minLength(5), Validators.maxLength(12)]],
      lastName : ['', [Validators.required, Validators.minLength(5), Validators.maxLength(12)]],
      email : ['', [Validators.required, Validators.email, Validators.maxLength(25)]],
      age : ['', [Validators.required, Validators.min(12), Validators.max(99)]],
      login : ['', [Validators.required, Validators.minLength(5), Validators.maxLength(15)],[ this.customValidator.existingLoginValidator()]],
      password : ['', [Validators.required,Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{8,}'), Validators.minLength(5), Validators.maxLength(20)]]
    });
  }

  register(): void{
    this.submitted = true;
    if (this.form.valid) {
      alert('You have successfully registered');
      this.http.post("https://localhost:7234/api/auth/register", this.form.getRawValue())
        .subscribe(() => this.router.navigate(["login"]));
    }
  }
}
