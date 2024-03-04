using Financeiro.Common.Events;

namespace Financeiro.Boleto.Domain.Interfaces.Services
{
    public interface IBoletoService
    {
        Task<string?> ObterPdfBoleto(Guid id);
        Task<Entities.Boleto?> RegistrarBoleto(GerarBoletoEvent boletoEvent);
    }
}