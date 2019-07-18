﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IAvatarService
    {
        Task ChangeUserAvatar(string userId, IFormFile avatar);

        Task<string> GetUserAvatarLink(string userId);
    }
}
