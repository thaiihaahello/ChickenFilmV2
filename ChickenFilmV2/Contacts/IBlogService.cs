using ChickenFilmV2.Models;

namespace ChickenFilmV2.Services
{
    public interface IBlogService
    {
        Blog? GetMainBlog();
        List<Blog> GetSideBlogs(int count = 3);
        Blog? GetBlogById(int id);
        void Update(Blog blog);
        List<Blog> GetOtherBlogs(int currentBlogId);

        List<Blog> GetAll();
    }
}
