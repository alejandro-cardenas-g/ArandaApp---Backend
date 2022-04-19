using ArandaApp.Models;
using ArandaApp.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArandaApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            this.logger = logger;
            this.context = context;
            this.configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseToken>> Get(int id,[FromHeader(Name = "x-token")] string token)
        {
            ResponseToken response = new ResponseToken();

            try
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
                
                var validate = await validarToken(token);
                logger.LogInformation(validate.ToString());
                if ( validate == true)
                {

                    CredencialesUsuario credenciales = new CredencialesUsuario()
                    {
                        Email = usuario.Email,
                        Password = usuario.Password
                    };

                    response.token = (await ConstruirToken(credenciales)).ToString();
                    response.data = new Auth
                    {
                        Id = usuario.Id,
                        Username = usuario.Username,
                        rol = usuario.Rol
                    };
                    response.ok = true;
                    response.error = null;

                    return response;

                }
                else {

                    response.data = null;
                    response.ok = false;
                    response.token = null;
                    response.error = "Token Invalido";
                    return response;

                }

            }
            catch (Exception e)
            {
                logger.LogWarning(e.ToString());
                response.data = null;
                response.ok = false;
                response.token = null;
                response.error = "Usuario No válido";
                return response;

            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseToken>> Post([FromBody] CredencialesUsuario credenciales)
        {

            ResponseToken response = new ResponseToken();

            try
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == credenciales.Email);

                bool verified = BCrypt.Net.BCrypt.Verify(credenciales.Password, usuario.Password);

                if (verified == true)
                {

                    response.token = (await ConstruirToken(credenciales)).ToString();
                    response.data = new Auth { 
                        Id = usuario.Id,
                        Username = usuario.Username,
                        rol = usuario.Rol
                    };
                    response.ok = true;
                    response.error = null;

                    return response;

                }
                else {
                    response.data = null;
                    response.ok = false;
                    response.token = null;
                    response.error = "Contraseña Incorrecta";
                    return response;
                }


            }
            catch (Exception e)
            {
                logger.LogWarning(e.ToString());
                response.data = null;
                response.ok = false;
                response.token = null;
                response.error = "No existe un usuario con este correo electrónico";
                return response;

            }

        }

        private async Task<string> ConstruirToken(CredencialesUsuario credenciales)
        {

            var claims = new List<Claim>()
            {
                new Claim("email", credenciales.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirity = DateTime.UtcNow.AddDays(1);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirity,
                Issuer = null,
                Audience = null,
                SigningCredentials = creds
            };

            var token =  tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        private async Task<bool> validarToken(string token)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
                return true;
            }
            catch(Exception e)
            {
                logger.LogInformation(e.ToString());
                return false;
            }
        }
    }
}
