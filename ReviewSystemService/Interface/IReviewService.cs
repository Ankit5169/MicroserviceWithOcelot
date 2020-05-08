using ReviewSystemRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReviewSystemService.Interface
{
    public interface IReviewService
    {
        Task<IEnumerable<RestaurantReview>> ViewComments(int id);
        Task<string> UpdateReviewComment(int id, UpdateComment comment);
        Task<string> AddReviewComment(CustomerReview review);
    }
}
