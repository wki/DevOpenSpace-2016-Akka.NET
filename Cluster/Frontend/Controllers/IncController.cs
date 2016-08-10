using Frontend;
using System.Threading.Tasks;
using System.Web.Http;

namespace Frontend.Controllers
{
    /// <summary>
    /// Zugriff auf das "Read Model" und "Write Model"
    /// </summary>
    [RoutePrefix("api/inc")]
    public class IncController : ApiController
    {
        private readonly IBackendService backendService;

        public IncController(IBackendService backendService)
        {
            this.backendService = backendService;
        }
        
        /// <summary>
        /// Erhöhen der angegebenen Zahl
        /// </summary>
        /// <param name="i">Zahl.</param>
        [HttpPost, Route("{i}")]
        public Task<int> MoveUpToDo([FromUri] int i)
        {
            return backendService.Increment(i);
        }
    }
}