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
    [Route("api/permisos")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly ILogger<PermisosController> logger;
        private readonly ApplicationDbContext context;

        public PermisosController(ILogger<PermisosController> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Permiso>>> Get()
        {
            return await context.Permisos.ToListAsync();
        }

    }
}
