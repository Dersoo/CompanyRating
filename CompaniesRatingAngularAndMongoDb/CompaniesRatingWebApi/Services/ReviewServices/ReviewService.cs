using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Models.DatabaseSettingsForStores;
using CompaniesRatingWebApi.Models.Nested;
using CompaniesRatingWebApi.Services.CompanyServices;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CompaniesRatingWebApi.Services.ReviewServices;

public class ReviewService : IReviewService
{
    private readonly IMongoCollection<Review> _reviews;
    private readonly ICompanyService _companyService;

    public ReviewService(IStoresDatabaseSettings settings, IMongoClient mongoClient, ICompanyService companyService)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _reviews = database.GetCollection<Review>(settings.ReviewCollectionName);
        _companyService = companyService;
    }

    public List<Review> Get()
    {
        return _reviews.Find(review => true).ToList();
    }
    
    public List<Review> GetAllReviewsOfCompany(string companyId)
    {
        return _reviews.Find(review => review.CompanyId == companyId && review.IsDisabled == false).ToList();
    }

    public Review Get(string id)
    {
        return _reviews.Find(review => review.Id == id).FirstOrDefault();
    }

    public Review Create(Review review)
    {
        if (review != null)
        {
            review.Scores = new List<ScoreOfReview>();
            _reviews.InsertOne(review);

            var reviewedCompany = _companyService.Get(review.CompanyId);
            reviewedCompany.Rating = (reviewedCompany.Rating + 
                                     ((review.Assessments.Salary +
                                       review.Assessments.Office +
                                       review.Assessments.Education +
                                       review.Assessments.Community +
                                       review.Assessments.Career)/5))/2;
            _companyService.Update(reviewedCompany.Id, reviewedCompany);

            return review;
        }

        return null;
    }

    public void Update(string id, Review review)
    {
        if (review != null)
        {
            var oldReview = Get(id);
            double factorOfOldReview = (oldReview.Assessments.Salary +
                                        oldReview.Assessments.Office +
                                        oldReview.Assessments.Education +
                                        oldReview.Assessments.Community +
                                        oldReview.Assessments.Career) / 5;
            
            var update = Builders<Review>.Update
                .Set(r => r.Assessments, review.Assessments)
                .Set(r => r.Comment, review.Comment)
                .Set(r => r.DateOfReview, review.DateOfReview);

            _reviews.UpdateOne(r => r.Id == id, update);
            
            //_reviews.ReplaceOne(review => review.Id == id, review);
            
            var reviewedCompany = _companyService.Get(review.CompanyId);
            reviewedCompany.Rating = (reviewedCompany.Rating - (factorOfOldReview/2)) / 2;
            reviewedCompany.Rating = (reviewedCompany.Rating + 
                                      ((review.Assessments.Salary +
                                        review.Assessments.Office +
                                        review.Assessments.Education +
                                        review.Assessments.Community +
                                        review.Assessments.Career)/5))/2;
            _companyService.Update(reviewedCompany.Id, reviewedCompany);
        }
    }

    public void Remove(string id)
    {
        _reviews.DeleteOne(review => review.Id == id);
    }

    public bool HasUserReviewedTheSingleCompany(string userId, string companyId)
    {
        return _reviews.Find(review => review.CompanyId == companyId && review.UserId == userId).CountDocuments() > 0;
    }

    public void RateReview(string reviewId, string userId, bool isScorePositive)
    {
        var filterBuilder = Builders<Review>.Filter;
        var filter = filterBuilder.Eq(x => x.Id, reviewId) &
                     filterBuilder.ElemMatch(review => review.Scores, el => el.UserId == userId);
        var updateBuilder = Builders<Review>.Update;
        var update = updateBuilder.Set(doc => doc.Scores[-1].IsScorePositive, isScorePositive);

        void SetReviewScoresValue(bool isFirstRate)
        {
            var scoredReview = Get(reviewId);
            
            if (isScorePositive && isFirstRate)
            {
                scoredReview.CountOfLikes++;
            }
            else if (!isScorePositive && isFirstRate)
            {
                scoredReview.CountOfDislikes++;
            }
            else if (isScorePositive)
            {
                scoredReview.CountOfLikes++;
                scoredReview.CountOfDislikes--;
            }
            else if (!isScorePositive)
            {
                scoredReview.CountOfDislikes++;
                scoredReview.CountOfLikes--;
            }

            if ((scoredReview.CountOfLikes - scoredReview.CountOfDislikes) < -1)
            {
                scoredReview.IsDisabled = true;
            }

            _reviews.ReplaceOne(review => review.Id == reviewId, scoredReview);
        }

        var updateResult = _reviews.UpdateOne(filter, update);

        if (updateResult.ModifiedCount < 1)
        {
            if (updateResult.MatchedCount < 1)
            {
                filter = filterBuilder.Eq(x => x.Id, reviewId);
                update = Builders<Review>.Update
                    .AddToSet(x => x.Scores, new ScoreOfReview
                    {
                        UserId = userId,
                        IsScorePositive = isScorePositive
                    });

                if (_reviews.UpdateOne(filter, update).UpsertedId != "")
                {
                    SetReviewScoresValue(true);
                }
            }
        }
        else
        {
            SetReviewScoresValue(false);
        }
    }
}