using CompaniesRatingWebApi.Models;

namespace CompaniesRatingWebApi.Services.ReviewServices;

public interface IReviewService
{
    List<Review> Get();

    List<Review> GetAllReviewsOfCompany(string companyId);

    Review Get(string id);

    Review Create(Review review);

    void Update(string id, Review review);

    void Remove(string id);

    bool HasUserReviewedTheSingleCompany(string userId, string companyId);

    void RateReview(string id, string userId, bool isScorePositive);
}