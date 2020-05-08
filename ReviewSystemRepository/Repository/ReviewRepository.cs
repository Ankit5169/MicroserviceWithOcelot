using Dapper;
using Microsoft.Extensions.Configuration;
using ReviewSystemRepository.Interface;
using ReviewSystemRepository.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ReviewSystemRepository.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly IConfiguration _config;
        public ReviewRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<RestaurantReview>> ViewComments(int id)
        {
            var restList = new List<RestaurantReview>();
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");

                string query = "select r.id, r.Restaurant_Name,r.Restaurant_Address,c.City, c.state , dbo.GetReviewComments(r.id) ReviewComments " +
                                "from tbl_Restaurant r left join tbl_City_State c on r.City_id = c.id ";
                if (id > 0)
                    query += "where r.id = " + id;

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<RestaurantReviewModel>(query);
                    if (result != null)
                    {
                        foreach (var row in result)
                        {
                            var restData = new RestaurantReview();

                            restData.id = row.id;
                            restData.Restaurant_Name = row.Restaurant_Name;
                            restData.Restaurant_Address = row.Restaurant_Address;
                            restData.City = row.City;
                            restData.state = row.state;
                            if (!string.IsNullOrEmpty(row.ReviewComments))
                                restData.reviews = ConvertToReviewModel(row.ReviewComments);

                            restList.Add(restData);
                        }
                    }
                }
                return restList;
            }
            catch (Exception ex)
            { return null; }
        }

        private IList<ReviewModel> ConvertToReviewModel(string value)
        {
            var rows = value.Split(',');
            List<ReviewModel> reviewList = new List<ReviewModel>();

            foreach (var val in rows)
            {
                ReviewModel review = new ReviewModel();
                var arr = val.Split('_');
                review.id = arr[0];
                review.rating = arr[1];
                review.comment = arr[2];
                review.first_name = arr[3];
                review.last_name = arr[4];

                reviewList.Add(review);
            }

            return reviewList;
        }

        public async Task<bool> UpdateReviewComment(int id, UpdateComment comment)
        {
            bool commentUpdated = false;
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");

                string query = "update tbl_Review set rating = " + comment.rating + ", Comment = '" + comment.comment + "' where id = " + id + ";";

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                    {
                        commentUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            { }
            return commentUpdated;
        }

        public async Task<bool> AddReviewComment(CustomerReview review)
        {
            bool commentAdded = false;
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");
                string query = "insert into tbl_Review values (" + review.rest_id + "," + review.cust_id + ", " + review.rating + ", '" + review.comment + "')";

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                    {
                        commentAdded = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return commentAdded;
        }
    }
}
