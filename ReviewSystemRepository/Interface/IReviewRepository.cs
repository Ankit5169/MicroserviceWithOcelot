using ReviewSystemRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReviewSystemRepository.Interface
{
    public interface IReviewRepository
    {
        Task<IEnumerable<RestaurantReview>> ViewComments(int id);
        Task<bool> UpdateReviewComment(int id, UpdateComment comment);
        Task<bool> AddReviewComment(CustomerReview review);
    }
}
