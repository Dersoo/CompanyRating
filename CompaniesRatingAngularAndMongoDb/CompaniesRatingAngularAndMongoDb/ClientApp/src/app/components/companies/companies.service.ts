import { Injectable } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import { Company } from "src/app/models/company.model";
import { Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CompaniesService{

  baseUrl = 'https://localhost:7234/api/company';

  constructor(private http: HttpClient) {
  }

  getAllCompanies(): Observable<Company[]> {
    return this.http.get<Company[]>(this.baseUrl);
  }

  addCompany (company: Company): Observable<Company> {
    return this.http.post<Company>(this.baseUrl, company);
  }

  deleteCompany(id: string): Observable<Company> {
    return this.http.delete<Company>(this.baseUrl + '/' + id)
  }

  updateCompany(company: Company): Observable<Company> {
    return this.http.put<Company>(this.baseUrl + '/' + company.id, company)
  }
}
