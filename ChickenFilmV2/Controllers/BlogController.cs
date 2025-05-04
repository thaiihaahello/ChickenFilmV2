using ChickenFilmV2.Models;
using ChickenFilmV2.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChickenFilmV2.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public IActionResult Index()
        {
            var mainBlog = _blogService.GetMainBlog();
            var sideBlogs = _blogService.GetSideBlogs();

            ViewBag.MainBlog = mainBlog;
            ViewBag.SideBlogs = sideBlogs;

            return View();
        }

        public IActionResult All()
        {
            var allBlogs = _blogService.GetAll(); // Lấy toàn bộ blog
            return View(allBlogs);
        }


        public IActionResult Detail(int id)
        {
            var blog = _blogService.GetBlogById(id);
            var otherBlogs = _blogService.GetOtherBlogs(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewBag.Blog = blog;
            ViewBag.OtherBlogs = otherBlogs;

            return View(blog);
        }

        [HttpPost]
        public IActionResult Like(int id)
        {
            var blog = _blogService.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }

            blog.BlogLike = (blog.BlogLike ?? 0) + 1;
            _blogService.Update(blog);

            return Ok(blog.BlogLike); // Có thể trả về số lượt like mới nếu cần
        }

        [HttpPost]
        public IActionResult Dislike(int id)
        {
            var blog = _blogService.GetBlogById(id);
            if (blog == null)
            {
                return NotFound();
            }

            blog.BlogDislike = (blog.BlogDislike ?? 0) + 1;
            _blogService.Update(blog);

            return Ok(blog.BlogDislike);
        }

    }
}
