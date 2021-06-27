using BlogLab.Models.BlogComment;
using BlogLab.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCommentController : ControllerBase
    {
        private readonly IBlogCommentRepository _blogCommentRepository;
        private readonly IBlogRepository _blogRepository;

        public BlogCommentController(IBlogCommentRepository blogCommentRepository, IBlogRepository blogRepository)
        {
            _blogCommentRepository = blogCommentRepository;
            _blogRepository = blogRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BlogComment>> Create(BlogComment blogComment)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.Name).Value);

            var createdBlogComment = await _blogCommentRepository.UpsertAsync(blogComment, applicationUserId);

            return Ok(createdBlogComment);
        }

        [HttpGet("{blogId}")]
        public async Task<ActionResult<List<BlogComment>>> GetAll(int blogId)
        {
            var blogcomments = await _blogCommentRepository.GetAllAsync(blogId);

            return Ok(blogcomments);
        }

        [HttpGet("{blogId}")]
        public async Task<ActionResult<BlogComment>> Get(int blogId)
        {
            var blogcomment = await _blogCommentRepository.GetAsync(blogId);

            return Ok(blogcomment);
        }

        [Authorize]
        [HttpDelete("{blogCommentId}")]
        public async Task<ActionResult<int>> Delete(int blogCommentId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.Name).Value);

            var foundBlogcomment = await _blogCommentRepository.GetAsync(blogCommentId);

            if (foundBlogcomment == null) return BadRequest("Comment does not exist");

            if (foundBlogcomment.ApplicationUserId != applicationUserId) return Unauthorized("You are not authorized to delete this comment");

            var affectedRow = await _blogCommentRepository.DeleteAsync(blogCommentId);

            return Ok(affectedRow);
        }
    }
}
