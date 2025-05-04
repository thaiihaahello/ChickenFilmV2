using ChickenFilmV2.Models;

namespace ChickenFilmV2.Services
{
    public class BlogService : IBlogService
    {
        private readonly MovieDbContext _context;

        public BlogService(MovieDbContext context)
        {
            _context = context;
        }

        public List<Blog> GetAll()
        {
            return _context.Blogs
                .OrderByDescending(b => b.BlogId)
                .ToList();
        }


        public Blog? GetMainBlog()
        {
            return _context.Blogs.OrderByDescending(b => b.BlogId).FirstOrDefault();
        }

        public List<Blog> GetSideBlogs(int count = 3)
        {
            return _context.Blogs.OrderByDescending(b => b.BlogId).Skip(1).Take(count).ToList();
        }

        public Blog? GetBlogById(int id)
        {
            return _context.Blogs.FirstOrDefault(b => b.BlogId == id);
        }

        public void Update(Blog blog)
        {
            _context.Blogs.Update(blog);
            _context.SaveChanges(); // 👈 Đảm bảo lưu thay đổi
        }
        public List<Blog> GetOtherBlogs(int currentBlogId)
        {
            return _context.Blogs
                           .Where(b => b.BlogId != currentBlogId)
                           .OrderByDescending(b => b.BlogId)
                           .Take(4)
                           .ToList();
        }

    }
}
