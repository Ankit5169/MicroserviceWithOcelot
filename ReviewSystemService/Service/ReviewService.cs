using ReviewSystemRepository.Interface;
using ReviewSystemRepository.Model;
using ReviewSystemService.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReviewSystemService.Service
{
    public class ReviewService : IReviewService
    {
        private IReviewRepository _repository;

        public ReviewService(IReviewRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> AddReviewComment(CustomerReview review)
        {
            var result = await _repository.AddReviewComment(review);
            if (result)
                return "Review Added Successfully!!";
            else
                return "Failed to add review!";
        }

        public async Task<string> UpdateReviewComment(int id, UpdateComment comment)
        {
            var result = await _repository.UpdateReviewComment(id, comment);
            if (result)
                return "Review Updated Successfully!!";
            else
                return "Review coud not be updated!";
        }

        public async Task<IEnumerable<RestaurantReview>> ViewComments(int id)
        {
            return await _repository.ViewComments(id);
        }
    }
}
