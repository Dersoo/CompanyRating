import { Injectable } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import { Location } from "src/app/models/location.model";
import { Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CompanyFormService{

  baseUrl = 'https://localhost:7234/api/location';

  constructor(private http: HttpClient) {
  }

  getAllCountries(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl);
  }

  getAllLocationsOfTheCountry(countryName: string): Observable<Location[]> {
    return this.http.get<Location[]>(this.baseUrl + "/" + countryName);
  }
}
