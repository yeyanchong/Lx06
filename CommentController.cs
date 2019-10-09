using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public ActionResult Get()
        {
            return Json(DAL.Comment.Instance.GetCount());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Json(DAL.Comment.Instance.GetWorkCount(id));
        }

        // POST api/<controller>
        [HttpPost("page")]
        public ActionResult getPage([FromBody]Model.Page page)
        {
            var result = DAL.Comment.Instance.GetPage(page);
            if (result.Count() == 0)
                return Json(Result.Err("返回记录数位10"));
            else
                return Json(Result.Ok(result));


        }
        [HttpPost("workPage")]
        public ActionResult getWorkPage([FromBody]Model.CommentPage page)
        {
            var result = DAL.Comment.Instance.GetWorkPage(page);
            if (result.Count() == 0)
                return Json(Result.Err("返回记录数位0"));
            else
                return Json(Result.Ok(result));


        }





        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                var n = DAL.Comment.Instance.Delete(id);
                if (n != 0)
                    return Json(Result.Ok("删除成功"));
                else
                    return Json(Result.Err("用户名不存在"));

            }
            catch (Exception ex)
            {

                return Json(Result.Err(ex.Message));
            }
        }
        [HttpPost]
        public ActionResult Post([FromBody]Model.Comment comment)
        {
            comment.commentTime = DateTime.Now;
            try
            {
                int n = DAL.Comment.Instance.Add(comment);
                return Json(Result.Ok("发布评论成功", n));
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("foreign key"))
                    if (ex.Message.ToLower().Contains("username"))
                        return Json(Result.Err("合法用户才能添加记录"));
                    else
                        return Json(Result.Err("用户所属作品不存在"));
                else  (ex.Message.ToLower().Contains("null"))
                         return Json(Result.Err("评论内容、作品ID 、用户名不能为空"));
                else
                    return Json(Result.Err(ex.Message));

            }
        }
    }
}
