namespace WebApi.Features.Reviews.Models;

public class ReviewSummaryResponse
{
    public double AvgReview { get; set; }
    public int NumOfReview { get; set; }
    public int NumOfFiveStar { get; set; }
    public int NumOfFourStar { get; set; }
    public int NumOfThreeStar { get; set; }
    public int NumOfTwoStar { get; set; }
    public int NumOfOneStar { get; set; }
}
