using ArandaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArandaApp.Controllers
{
    [Route("api/rolpermisos")]
    [ApiController]
    public class RolpermisosController : ControllerBase
    {
        private readonly ILogger<RolpermisosController> logger;
        private readonly ApplicationDbContext context;

        public RolpermisosController(ILogger<RolpermisosController> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Rolpermiso>>> Get() 
        {
            return await context.Rolpermisos.ToListAsync();
        }
    }
}
