using System;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIContagem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContadorController : ControllerBase
    {
        private static Contador _CONTADOR = new Contador();
        private static string _FRAMEWORK;

        static ContadorController()
        {
            _FRAMEWORK = Assembly
                .GetEntryAssembly()?
                .GetCustomAttribute<TargetFrameworkAttribute>()?
                .FrameworkName;
        }

        [HttpGet]
        public object Get([FromServices]IConfiguration configuration)
        {
            lock (_CONTADOR)
            {
                _CONTADOR.Incrementar();

                return new
                {
                    _CONTADOR.ValorAtual,
                    Environment.MachineName,
                    Sistema = Environment.OSVersion.VersionString,
                    Hospedagem = "Azure App Service", // Alteração para testar o deployment automatizado
                    Local =  "Repositório GitHub",
                    TargetFramework = _FRAMEWORK
                };
            }
        }
    }
}
