
using ChickenFilmV2.Models;
using System.Collections.Generic;

namespace ChickenFilmV2.Services.Interfaces
{
    public interface ICommentService
    {
        List<Comment> GetAllComments();
        void DeleteComment(int id);
    }
}
