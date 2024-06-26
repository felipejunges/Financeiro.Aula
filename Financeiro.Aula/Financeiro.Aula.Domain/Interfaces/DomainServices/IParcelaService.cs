using Financeiro.Aula.Domain.DTOs;

namespace Financeiro.Aula.Domain.Interfaces.DomainServices
{
    public interface IParcelaService
    {
        GeracaoParcelamentoDto GerarParcelas(double valorTotal, int numeroParcelas, DateTime primeiroVencimento, long contratoId);
        Task<(bool Sucesso, string Mensagem)> AlterarChaveBoletoParcela(Guid tokenBoleto, string chaveBoleto);
    }
}