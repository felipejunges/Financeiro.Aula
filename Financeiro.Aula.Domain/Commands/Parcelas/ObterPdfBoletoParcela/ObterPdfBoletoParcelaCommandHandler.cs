﻿using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.ObterPdfBoletoParcela
{
    public class ObterPdfBoletoParcelaCommandHandler : IRequestHandler<ObterPdfBoletoParcelaCommand, (bool Sucesso, string Mensagem, byte[]? Pdf)>
    {
        private readonly IGeradorBoletoApiService _geradorBoletoApiService;
        private readonly IParcelaRepository _parcelaRepository;

        public ObterPdfBoletoParcelaCommandHandler(IGeradorBoletoApiService geradorBoletoApiService, IParcelaRepository parcelaRepository)
        {
            _geradorBoletoApiService = geradorBoletoApiService;
            _parcelaRepository = parcelaRepository;
        }

        public async Task<(bool Sucesso, string Mensagem, byte[]? Pdf)> Handle(ObterPdfBoletoParcelaCommand request, CancellationToken cancellationToken)
        {
            var parcela = await _parcelaRepository.ObterParcela(request.ParcelaId);

            if (parcela is null)
                return (false, "Parcela não localizada", null);

            if (!parcela.TemBoleto)
                return (false, "A parcela não possui boleto gerado", null);

            var resultado = await _geradorBoletoApiService.ObterPdfBoleto(parcela);

            if (!resultado.Sucesso)
                return (false, "Erro ao obter os dados do boleto", null);

            //string pdf = Convert.ToBase64String(resultado.Pdf);

            return (true, string.Empty, resultado.Pdf);
        }
    }
}