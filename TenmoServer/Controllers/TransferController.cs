using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TenmoServer.Controllers
{
    [Authorize]
    [Route("transfer")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        //private ITransferDao transferDao;
    }
}
