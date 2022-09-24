import { Injectable } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import { Observable} from "rxjs";
import {Review} from "../../models/review.model";
import {ScoreOfReview} from "../../models/score.of.review.model";

@Injectable({
  providedIn: 'root'
})
export class ReviewService{

  baseUrl = 'https://localhost:7234/api/review';

  constructor(private http: HttpClient) {
  }

  addReview (review: Review): Observable<Review> {
    return this.http.post<Review>(this.baseUrl, review);
  }

  deleteReview(id: string): Observable<Review> {
    return this.http.delete<Review>(this.baseUrl + '/' + id)
  }

  updateReview(review: Review): Observable<Review> {
    return this.http.put<Review>(this.baseUrl + '/' + review.id, review)
  }

  rateReview (reviewId: string, scoreOfReview: ScoreOfReview): Observable<Review> {
    return this.http.post<Review>(this.baseUrl + '/ratereview/' + reviewId, scoreOfReview);
  }
}
