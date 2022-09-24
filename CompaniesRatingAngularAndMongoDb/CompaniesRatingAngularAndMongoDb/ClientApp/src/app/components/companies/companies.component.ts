import {Component,OnInit} from "@angular/core";
import { Company } from "src/app/models/company.model";
import {CompaniesService} from "./companies.service";
import {MatDialog} from '@angular/material/dialog';
import {CompanyFormComponent} from "../company-form/company-form.component";
import {UserService} from "../user/user.service";
import {Observable} from "rxjs";
import {User} from "../../models/user.model";
import {CompanyDetailsComponent} from "../company-details/company-details.component";

@Component({
  selector: 'companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css']
})
export class CompaniesComponent implements OnInit{
  title = "List of companies";
  public current: number = 1;
  public perPage = 10;
  public total = 0;
  company: Company = {
    id: '',
    name: '',
    rating: 0,
    location: {city: '', country: ''},
    description: ''
  }
  companies: Company[] = [];
  public itemsToDisplay: Company[] = [];
  searchTerm = '';

  userObs: Observable<User> | undefined;

  public onGoTo(page: number): void {
    this.current = page
    this.itemsToDisplay = this.paginate(this.current, this.perPage)
  }

  public onNext(page: number): void {
    this.current = page + 1
    this.itemsToDisplay = this.paginate(this.current, this.perPage)
  }

  public onPrevious(page: number): void {
    this.current = page - 1
    this.itemsToDisplay = this.paginate(this.current, this.perPage)
  }

  public paginate(current: number, perPage: number): Company[] {
    return [...this.companies.slice((current - 1) * perPage).slice(0, perPage)]
  }

  constructor(private companyService: CompaniesService,
              public dialog: MatDialog,
              public userService: UserService) {
  }

  ngOnInit(): void {
    this.getAllCompanies();

    this.userService.getUser();

    this.userObs = this.userService.getUserDirect();
  }

  search(searchString: string): void {
    this.getAllCompanies(searchString);
  }

  getAllCompanies(searchString?: string) {
    this.companyService.getAllCompanies()
      .subscribe(
        Response => {
          this.companies = Response;
          if(searchString && searchString != ""){
            this.companies = this.companies.filter((val) =>
               val.name.toLowerCase().includes(searchString.toLowerCase())
             );
          }
          this.total = Math.ceil(this.companies.length / this.perPage);
          this.itemsToDisplay = this.paginate(this.current, this.perPage);
          this.title = `Currently registered ${this.companies.length} companies:`;
        }
      );
  }

  addCompany() {
    this.dialog.open(CompanyFormComponent, {
      width: '30%'
    }).afterClosed().subscribe(() => { this.getAllCompanies(); } );
  }

  editCompany(company: Company){
    this.dialog.open(CompanyFormComponent, {
      width: '30%',
      data: company
    }).afterClosed().subscribe(() => { this.getAllCompanies(); } );
  }

  detailsOfCompany(company: Company){
    this.dialog.open(CompanyDetailsComponent, {
      width: '60%',
      height: '80%',
      data: company
    }).afterClosed().subscribe(() => { this.getAllCompanies(); } );
  }

  onDelete(id: string) {
    this.companyService.deleteCompany(id)
      .subscribe(
        Response => {
          this.getAllCompanies();
        }
      );
  }
}
