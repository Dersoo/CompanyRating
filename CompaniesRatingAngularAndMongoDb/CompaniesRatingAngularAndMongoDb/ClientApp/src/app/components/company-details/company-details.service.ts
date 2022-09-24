import { Injectable } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import { Observable} from "rxjs";
import {Review} from "../../models/review.model";

@Injectable({
  providedIn: 'root'
})
export class CompanyDetailsService{

  constructor(private http: HttpClient) {
  }

  getAllReviews(companyId: String): Observable<Review[]> {
    return this.http.get<Review[]>('https://localhost:7234/api/review/' + companyId);
  }

  GetUserFeedbackAcceptabilityForCurrentCompany(companyId: String){
    return this.http.get<boolean>("https://localhost:7234/api/review/userreviewstatus/" + companyId);
  }
}
