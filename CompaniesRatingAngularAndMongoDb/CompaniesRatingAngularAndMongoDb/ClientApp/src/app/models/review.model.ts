import {ScoreOfReview} from "./score.of.review.model";

type Assessments = {
  salary: number;
  office: number;
  education: number;
  career: number;
  community: number;
};

export class Review {
  id: string = "";
  userId: string = "";
  companyId: string = "";
  dateOfReview: Date = new Date();
  assessments!: Assessments;
  comment: string = "";
  countOfLikes: number = 0;
  countOfDislikes: number = 0;
  isDisabled: boolean = false;

  public constructor(init?: Partial<Review>) {
    Object.assign(this, init);
  }
}
