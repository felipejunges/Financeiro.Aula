using Financeiro.Common.Events;

namespace Financeiro.Boleto.Domain.Interfaces.ApiServices
{
    public interface IGeradorBoletoApiService
    {
        Task<byte[]?> ObterPdfBoleto(string chaveBoleto);
        Task<string?> GerarTokenBoleto(GerarBoletoEvent boleto, string numeroBoleto);
    }
}