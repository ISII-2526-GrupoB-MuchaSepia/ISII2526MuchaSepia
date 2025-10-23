using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CochesController : ControllerBase
    {
        private readonly ApplicationDbContext context; //permite consultar base de datos
        private readonly ILogger<CochesController> _logger; //permite registrar logs

        public CochesController(ApplicationDbContext context, ILogger<CochesController> logger)
        {
            this.context = context;
            this._logger = logger;
        }

       



        }
}