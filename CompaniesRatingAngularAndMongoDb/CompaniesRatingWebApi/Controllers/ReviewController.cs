using System.Security.Claims;
using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Models.Nested;
using CompaniesRatingWebApi.Services.ReviewServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompaniesRatingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ReviewController(IReviewService reviewService, IHttpContextAccessor httpContextAccessor)
    {
        this._reviewService = reviewService;
        this._httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<List<Review>> Get()
    {
        return _reviewService.Get();
    }

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<List<Review>> Get(string id)
    {
        return _reviewService.GetAllReviewsOfCompany(id);
    }

    [HttpPost]
    [Authorize]
    public ActionResult<Review> Post([FromBody]Review review)
    {
        _reviewService.Create(review);

        return CreatedAtAction(nameof(Get), new { id = review.Id }, review);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Put(string id, [FromBody]Review review)
    {
        var existingReview = _reviewService.Get(id);

        if (existingReview == null)
        {
            return NotFound($"Review with id = {id} not found");
        }
        
        _reviewService.Update(id, review);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(string id)
    {
        var existingReview = _reviewService.Get(id);

        if (existingReview == null)
        {
            return NotFound($"Review with id = {id} not found");
        }
        
        _reviewService.Remove(existingReview.Id);

        return Ok(existingReview);
    }
    
    [HttpGet, Route("userreviewstatus/{id}")]
    [Authorize]
    public bool GetUserFeedbackAcceptabilityForCurrentCompany(string id)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return !_reviewService.HasUserReviewedTheSingleCompany(userId, id);
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    [HttpPost, Route("ratereview/{id}")]
    //[Authorize]
    public ActionResult RateReview(string id, [FromBody]ScoreOfReview scoreOfReview)
    {
        _reviewService.RateReview(id, scoreOfReview.UserId, scoreOfReview.IsScorePositive);

        return NoContent();
    }
}