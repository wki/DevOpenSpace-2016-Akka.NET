using System;
using System.Web.Http;
using System.Threading.Tasks;

namespace ToDoList.Web.Controllers
{
    [RoutePrefix("api/demo")]
    public class DemoController : ApiController
    {
        public DemoController()
        {
        }

        [HttpGet, Route("bla")]
        public string Bla()
        {
            return "bla";
        }
    }
}
