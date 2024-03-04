using Financeiro.Common.Events;

namespace Financeiro.Boleto.Domain.Interfaces.Queues
{
    public interface IBoletoRegistradoQueue
    {
        Task EnviarFilaBoletoRegistrado(BoletoRegistradoEvent boletoEvent);
    }
}