using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArandaApp.Filtros
{
    public class BadRequestParsing : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var casteoResult = context.Result as IStatusCodeActionResult;
            if (casteoResult == null) {
                return;
            }

            var statusCode = casteoResult.StatusCode;
            if (statusCode == 400) {
                var respuesta = new List<string>();
                var resultadoActual = context.Result as BadRequestObjectResult;
                var respuestaF = new List<string>();
                if (resultadoActual.Value is string)
                {
                    respuesta.Add(resultadoActual.Value.ToString());
                }
                else
                {
                    foreach (var llave in context.ModelState.Keys)
                    {
                        foreach (var error in context.ModelState[llave].Errors)
                        {
                            respuestaF.Add(error.ErrorMessage);
                        }
                    }

                    if (respuestaF.Count > 0) {
                        respuesta.Add(respuestaF[0]);
                    }


                    context.Result = new BadRequestObjectResult(respuesta);

                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
