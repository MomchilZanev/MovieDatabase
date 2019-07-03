﻿using MovieDatabase.Models.InputModels;

namespace MovieDatabase.Services.Contracts
{
    public interface IReviewService
    {
        bool IsValidId(string itemId);

        bool Exists(string userId, string itemId);

        CreateReviewInputModel GetUserReview(string userId, string itemId);

        string CreateUserReview(string userId, string itemId, string content, int rating);

        string UpdateUserReview(string userId, string itemId, string content, int rating);
    }
}
