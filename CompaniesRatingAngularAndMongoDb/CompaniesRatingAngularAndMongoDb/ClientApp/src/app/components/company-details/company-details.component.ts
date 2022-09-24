import {Component, Inject, OnInit} from "@angular/core";
import {FormBuilder} from "@angular/forms";
import {CompaniesService} from "../companies/companies.service";
import {MatDialogRef, MAT_DIALOG_DATA, MatDialog} from "@angular/material/dialog";
import {CompanyDetailsService} from "./company-details.service";
import {Review} from "../../models/review.model";
import {Observable} from "rxjs";
import {User} from "../../models/user.model";
import {UserService} from "../user/user.service";
import {ReviewComponent} from "../review/review.component";
import {ReviewService} from "../review/review.service";
import {ScoreOfReview} from "../../models/score.of.review.model";

@Component({
  selector: 'company-details',
  templateUrl: './company-details.component.html',
  styleUrls: ['./company-details.component.css']
})

export class CompanyDetailsComponent implements OnInit {
  title = "Select location";
  reviews: Review[] = [];
  panelOpenState = false;
  userObs: Observable<User> | undefined;
  canUserAddReview: Observable<boolean> | undefined;

  constructor(private companyDetailsService: CompanyDetailsService,
              private companyService: CompaniesService,
              private formBuilder: FormBuilder,
              private dialogRef: MatDialogRef<CompanyDetailsComponent>,
              @Inject(MAT_DIALOG_DATA) public editData : any,
              public userService: UserService,
              public dialog: MatDialog,
              private reviewService: ReviewService) {
  }

  ngOnInit(): void {
    this.userObs = this.userService.getUserDirect();
    this.canUserAddReview = this.companyDetailsService.GetUserFeedbackAcceptabilityForCurrentCompany(this.editData.id);
    this.getAllReviews();
  }

  onChange() {
    this.getAllReviews();
  }

  getAllReviews() {
    this.companyDetailsService.getAllReviews(this.editData.id)
      .subscribe(
        Response => {
          this.reviews = Response;
        }
      );
  }

  getReviewOfCurrentUser(userId: string)
  {
    return this.reviews.find(review => review.userId == userId);
  }

  editReview(review: Review){
    this.dialog.open(ReviewComponent, {
      width: '30%',
      data: review
    }).afterClosed().subscribe(() => { this.getAllReviews(); } );
  }

  onDelete(id: string) {
    this.reviewService.deleteReview(id)
      .subscribe(
        Response => {
          this.getAllReviews();
        }
      );
  }

  RateReview(reviewId: string, isScorePositive: boolean)
  {
    const scoreOfReview: ScoreOfReview = {
      userId: this.userService.user.id,
      isScorePositive: isScorePositive
    }

    this.reviewService.rateReview(reviewId, scoreOfReview)
      .subscribe(
        Response => {
          this.getAllReviews();
        }
      );
  }
}
