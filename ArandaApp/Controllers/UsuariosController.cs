using ArandaApp.Models;
using ArandaApp.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArandaApp.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ILogger<UsuariosController> logger;
        private readonly ApplicationDbContext context;

        public UsuariosController(ILogger<UsuariosController> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            var usuarios = await context.Usuarios.ToListAsync();
            var us = usuarios.FindAll(user => user.Id != 1 && user.Email != "admin@aranda.com");
            return us;
        }

        [HttpPost]
        public async Task<Response1> Post([FromBody] Usuario usuario)
        {
            try
            {
                
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                usuario.Password = passwordHash;
                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();
                int Id = usuario.Id;
                Response1 response = new Response1();
                response.ok = true;
                response.data = usuario;
                response.error = null;

                return response;
            }
            catch (Exception e) {

                logger.LogError("--------------------------:" + e.ToString());
                Response1 response = new Response1();
                response.ok = false;
                response.data = new Usuario();

                if (e.ToString().Contains("Violation of UNIQUE KEY") == true)
                {
                    response.error = "Ya existe un usuario con este correo electrónico";
                }

                return response;
            }

            //var result = 
            //logger.LogWarning("UsuarioRef: " + usuarioref.ToString());
            
            //logger.LogWarning(usuario.Id.ToString());
            

            //return await context.Usuarios.FindAsync(usuarioref);
        }

        [HttpPut("{Id:int}")]
        public async Task<Response1> Put(int Id, [FromBody] Usuario usuario)
        {
            Response1 response = new Response1();

            try
            {   

                var usuarioToUpd = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == Id);

                bool passwordChange = false;

                if (usuarioToUpd.Password != usuario.Password) {
                    passwordChange = true;
                }

                usuarioToUpd.FullName = usuario.FullName;
                usuarioToUpd.Edad = usuario.Edad;
                usuarioToUpd.Direccion = usuario.Direccion;
                usuarioToUpd.Email = usuario.Email;
                usuarioToUpd.Username = usuario.Username;
                usuarioToUpd.Telefono = usuario.Telefono;
                usuarioToUpd.Rol = usuario.Rol;

                if (passwordChange == true) { 
                    usuarioToUpd.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                }

                await context.SaveChangesAsync();

                response.ok = true;
                response.data = usuarioToUpd;
                response.error = null;

                return response;

                
            }
            catch (Exception e)
            {

                logger.LogError("--------------------------:" + e.ToString());
           
                response.ok = false;
                response.data = new Usuario();

                if (e.ToString().Contains("Violation of UNIQUE KEY") == true)
                {
                    response.error = "Ya existe un usuario con este correo electrónico";
                }

                if(e.ToString().Contains("Object reference not set to an instance") == true)
                {
                    response.error = "El usuario que deseas modificar no existe.";
                }

                return response;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<Response1> Delete(int id)
        {

            Response1 response = new Response1();

            try
            {
                
                var usuarioToDelete = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                Usuario refUser = new Usuario()
                {
                    Id = id
                };

                context.Remove(usuarioToDelete);

                await context.SaveChangesAsync();

                response.ok = true;
                response.data = refUser;
                response.error = null;

                return response;

            }
            catch (Exception e)
            {

                logger.LogError("--------------------------:" + e.ToString());

                response.ok = false;
                response.data = new Usuario();

                if (e.ToString().Contains("Object reference not set to an instance") == true)
                {
                    response.error = "El usuario que deseas eliminar no existe.";
                }

                return response;
            }

        }

    }
}
