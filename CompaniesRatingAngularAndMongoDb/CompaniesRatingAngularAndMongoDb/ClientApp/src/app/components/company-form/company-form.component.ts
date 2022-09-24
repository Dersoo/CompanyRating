import {Component, Inject, OnInit} from "@angular/core";
import { Location } from "src/app/models/location.model";
import {CompanyFormService} from "./company-form.service";
import {Company} from "../../models/company.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {CompaniesService} from "../companies/companies.service";
import {MatDialogRef, MAT_DIALOG_DATA} from "@angular/material/dialog";

@Component({
  selector: 'company-form',
  templateUrl: './company-form.component.html',
  styleUrls: ['./company-form.component.css']
})

export class CompanyFormComponent implements OnInit {
  title = "Select location";
  countries: string[] = [];
  locationsOfCurrentCountry: Location[] = [];
  location: Location = {
    city: '',
    country: ''
  }
  companyForm !: FormGroup;
  actionBtn : string = "Save";

  constructor(private companyFormService: CompanyFormService,
              private companyService: CompaniesService,
              private formBuilder: FormBuilder,
              private dialogRef: MatDialogRef<CompanyFormComponent>,
              @Inject(MAT_DIALOG_DATA) public editData : any) {
  }

  ngOnInit(): void {
    this.companyForm = this.formBuilder.group({
      id : [''],
      name : ['', Validators.required],
      rating : ['', Validators.required],
      companyCountry : ['', Validators.required],
      location : ['', Validators.required]
    })
    this.getAllCountries();

    if (this.editData){
      this.actionBtn = "Update";
      this.getAllLocationsOfTheCountry(this.editData.location.country);
      this.companyForm.controls['id'].setValue(this.editData.id);
      this.companyForm.controls['name'].setValue(this.editData.name);
      this.companyForm.controls['rating'].setValue(this.editData.rating);
      this.companyForm.controls['companyCountry'].setValue(this.editData.location.country);
      //this.companyForm.controls['location'].setValue(this.editData.location);
      this.companyForm.controls['location'].setValue(this.editData.location);
    }
  }

  addCompany(){
    if(!this.editData){
      if(this.companyForm.valid){
        this.companyService.addCompany(new Company(this.companyForm.value))
          .subscribe({
            next: (res) => {
              alert("Company added successfully");
              this.companyForm.reset();
              this.dialogRef.close('save');
            },
            error: () => {
              alert("Error while adding the company");
            }
          });
      }
    }else{
      this.updateCompany();
    }
  }

  updateCompany(){
    this.companyService.updateCompany(new Company(this.companyForm.value))
      .subscribe({
        next: (res) => {
          alert("Company updated successfully");
          this.companyForm.reset();
          this.dialogRef.close('update');
        },
        error: () => {
          alert("Error while updating the company");
        }
      })
  }

  locationComparisonFunction = function( location: any, value: any ) : boolean {
    return location.city === value.city;
  }

  onChange(countryName: string) {
    this.getAllLocationsOfTheCountry(countryName);
  }

  getAllCountries() {
    this.companyFormService.getAllCountries()
      .subscribe(
        Response => {
          this.countries = Response;
        }
      );
  }

  getAllLocationsOfTheCountry(countryName: string) {
    this.companyFormService.getAllLocationsOfTheCountry(countryName)
      .subscribe(
        Response => {
          this.locationsOfCurrentCountry = Response;
        }
      );
  }

}
