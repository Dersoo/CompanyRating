import {Component, EventEmitter, Inject, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Review} from "../../models/review.model";
import {ReviewService} from "./review.service";
import {UserService} from "../user/user.service";

@Component({
  selector: 'review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.css']
})

export class ReviewComponent implements OnInit {
  @Input() companyId : string = "";
  @Output() reviewDoneEvent = new EventEmitter();
  title = "Feedback";
  reviewForm !: FormGroup;
  actionBtn : string = "Save";
  review: Review = {
    id: "",
    userId: "",
    companyId: "",
    dateOfReview: new Date(),
    assessments: {
      salary: 0,
      office: 0,
      education: 0,
      career: 0,
      community: 0,
    },
    comment: "",
    countOfLikes: 0,
    countOfDislikes: 0,
    isDisabled: false
  }

  constructor(private reviewService: ReviewService,
              public userService: UserService,
              private formBuilder: FormBuilder,
              private dialogRef: MatDialogRef<ReviewComponent>,
              @Inject(MAT_DIALOG_DATA) public editData : any) {
  }

  ngOnInit(): void {
    this.userService.getUser();
    this.reviewForm = this.formBuilder.group({
      id : [''],
      companyId : [''],
      salary : ['', Validators.required],
      office : ['', Validators.required],
      education : ['', Validators.required],
      career : ['', Validators.required],
      community : ['', Validators.required],
      comment : ['', Validators.required]
    });

    if (this.editData && this.companyId == ""){
      this.actionBtn = "Update";
      this.reviewForm.controls['id'].setValue(this.editData.id);
      this.reviewForm.controls['companyId'].setValue(this.editData.companyId);
      this.reviewForm.controls['salary'].setValue(this.editData.assessments.salary);
      this.reviewForm.controls['office'].setValue(this.editData.assessments.office);
      this.reviewForm.controls['education'].setValue(this.editData.assessments.education);
      this.reviewForm.controls['career'].setValue(this.editData.assessments.career);
      this.reviewForm.controls['community'].setValue(this.editData.assessments.community);
      this.reviewForm.controls['comment'].setValue(this.editData.comment);
    }
  }

  addReview(){
    if((!this.editData) || this.companyId != ""){
      if(this.reviewForm.valid){
        this.review.userId = this.userService.user.id;
        this.review.companyId = this.companyId;
        this.review.dateOfReview = new Date;
        this.review.assessments.salary = this.reviewForm.controls['salary'].value;
        this.review.assessments.office = this.reviewForm.controls['office'].value;
        this.review.assessments.education = this.reviewForm.controls['education'].value;
        this.review.assessments.career = this.reviewForm.controls['career'].value;
        this.review.assessments.community = this.reviewForm.controls['community'].value;
        this.review.comment = this.reviewForm.controls['comment'].value;
        this.reviewService.addReview(this.review)
          .subscribe({
            next: (res) => {
              alert("Review added successfully");
              this.companyId = "";
              this.reviewForm.reset();
              this.reviewDoneEvent.emit();
            },
            error: () => {
              alert("Error while adding review");
            }
          });
      }
    }else{
      this.updateCompany();
    }
  }

  updateCompany(){
    this.review.id = this.reviewForm.controls['id'].value;
    this.review.userId = this.userService.user.id;
    this.review.companyId = this.reviewForm.controls['companyId'].value;;
    this.review.dateOfReview = new Date;
    this.review.assessments.salary = this.reviewForm.controls['salary'].value;
    this.review.assessments.office = this.reviewForm.controls['office'].value;
    this.review.assessments.education = this.reviewForm.controls['education'].value;
    this.review.assessments.career = this.reviewForm.controls['career'].value;
    this.review.assessments.community = this.reviewForm.controls['community'].value;
    this.review.comment = this.reviewForm.controls['comment'].value;
    this.review.countOfLikes = this.editData.countOfLikes;
    this.review.countOfDislikes = this.editData.countOfDislikes;
    this.review.isDisabled = this.editData.isDisabled;
    this.reviewService.updateReview(this.review)
      .subscribe({
        next: (res) => {
          alert("Review updated successfully");
          this.reviewForm.reset();
          this.dialogRef.close('update');
        },
        error: () => {
          alert("Error while updating the review");
        }
      })
  }
}
