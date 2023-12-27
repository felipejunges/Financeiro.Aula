using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Boleto.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ParametrosController : ControllerBase
    {
        private readonly IParametroBoletoRepository _parametroBoletoRepository;

        public ParametrosController(IParametroBoletoRepository parametroBoletoRepository)
        {
            _parametroBoletoRepository = parametroBoletoRepository;
        }

        [HttpPut("numero-boleto")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> IncrementarNumeroBoletoAtual()
        {
            var numeroBoletoAtual = await _parametroBoletoRepository.IncrementarNumeroBoletoAtual();

            return Ok(numeroBoletoAtual);
        }
    }
}