using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Queues;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Common.Events;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.GerarBoletoParcela
{
    public class GerarBoletoParcelaCommandHandler : IRequestHandler<GerarBoletoParcelaCommand, (bool Sucesso, string Mensagem)>
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly IRegistrarBoletoQueue _boletoQueue;

        public GerarBoletoParcelaCommandHandler(IParcelaRepository parcelaRepository, IRegistrarBoletoQueue boletoQueue)
        {
            _parcelaRepository = parcelaRepository;
            _boletoQueue = boletoQueue;
        }

        public async Task<(bool Sucesso, string Mensagem)> Handle(GerarBoletoParcelaCommand request, CancellationToken cancellationToken)
        {
            var parcela = await _parcelaRepository.ObterParcela(request.ParcelaId);

            if (parcela is null)
                return (false, "Parcela não localizada");

            if (parcela.Paga)
                return (false, "A parcela já está paga");

            if (parcela.BoletoPendente && !request.ConfirmaSobrescrever)
                return (false, "A parcela já possui geração de boleto pendente");

            if (parcela.TemBoleto && !request.ConfirmaSobrescrever)
                return (false, "A parcela já possui boleto gerado");

            parcela.GerarNovoTokenBoleto();
            await _parcelaRepository.AlterarParcela(parcela);

            await _boletoQueue.EnviarParcelaFilaGerarBoleto(CriarEvento(parcela));

            return (true, string.Empty);
        }

        private static GerarBoletoEvent CriarEvento(Parcela parcela)
        {
            return new GerarBoletoEvent(
                tokenRetorno: parcela.TokenBoleto!.Value,
                identificadorContrato: parcela.Contrato!.Id.ToString(),
                dataVencimento: parcela.DataVencimento,
                valor: parcela.Valor,
                cliente: new GerarBoletoCliente(
                    parcela.Contrato!.Cliente!.Nome,
                    parcela.Contrato!.Cliente!.Cpf,
                    parcela.Contrato!.Cliente!.Endereco)
            );
        }
    }
}