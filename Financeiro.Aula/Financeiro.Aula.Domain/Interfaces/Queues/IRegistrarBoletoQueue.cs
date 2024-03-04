using Financeiro.Aula.Domain.DTOs;
using Financeiro.Common.Events;

namespace Financeiro.Aula.Domain.Interfaces.Queues
{
    public interface IRegistrarBoletoQueue
    {
        Task EnviarParcelaFilaGerarBoleto(GerarBoletoEvent boletoEvent);
    }
}