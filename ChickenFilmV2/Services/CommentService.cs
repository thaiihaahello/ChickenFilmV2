using ChickenFilmV2.Models;
using ChickenFilmV2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ChickenFilmV2.Services
{
    public class CommentService : ICommentService
    {
        private readonly MovieDbContext _context;

        public CommentService(MovieDbContext context)
        {
            _context = context;
        }

        public List<Comment> GetAllComments()
        {
            return _context.Comments
                .Include(c => c.User)
                .Include(c => c.Movie)
                .ToList();
        }

        public void DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }
    }
}
