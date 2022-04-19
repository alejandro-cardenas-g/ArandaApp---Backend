using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArandaApp.ApiBehavior
{
    public static class BehaviorBadRequest
    {

        public static void Parsear(ApiBehaviorOptions options) 
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {

                var respuesta = new List<string>();
                var respuestaF = new List<string>();

                foreach (var llave in actionContext.ModelState.Keys)
                {
                    foreach (var error in actionContext.ModelState[llave].Errors)
                    {
                        respuestaF.Add(error.ErrorMessage);
                    }
                }
                if (respuestaF.Count > 0) {
                    respuesta.Add(respuestaF[0]);
                }


                return new BadRequestObjectResult(respuesta);

            };
        }

    }
}
