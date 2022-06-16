﻿using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.GerarBoletoParcela
{
    public class GerarBoletoParcelaCommand : IRequest<(bool Sucesso, string Mensagem, string Pdf)>
    {
        public long ParcelaId { get; private set; }

        public GerarBoletoParcelaCommand(long parcelaId)
        {
            ParcelaId = parcelaId;
        }
    }
}