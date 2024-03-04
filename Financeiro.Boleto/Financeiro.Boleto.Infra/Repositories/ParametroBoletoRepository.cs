using Financeiro.Boleto.Domain.Entities;
using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Financeiro.Boleto.Infra.Context;
using Financeiro.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Boleto.Infra.Repositories
{
    public class ParametroBoletoRepository : IParametroBoletoRepository
    {
        private const long PARAMETRO_BOLETO_PADRAO = 1;

        private readonly BoletoDb _context;

        public ParametroBoletoRepository(BoletoDb context)
        {
            _context = context;
        }

        public async Task<ParametroBoleto> ObterParametrosBoleto()
        {
            var parametros = await _context.ParametrosBoleto
                            .Where(p => p.Id == PARAMETRO_BOLETO_PADRAO)
                            .FirstOrDefaultAsync();

            if (parametros is not null)
                return parametros;

            return await CriarParametrosBoleto();
        }

        public async Task<int> IncrementarNumeroBoletoAtual()
        {
            var parametros = await ObterParametrosBoleto();

            parametros.IncrementarNumeroBoletoAtual();

            await _context.SaveChangesAsync();

            return parametros.NumeroBoletoAtual;
        }

        private async Task<ParametroBoleto> CriarParametrosBoleto()
        {
            var parametros = new ParametroBoleto(
                                    id: PARAMETRO_BOLETO_PADRAO,
                                    descricao: "Boleto Bradesco",
                                    banco: "237",
                                    agencia: "1234-5",
                                    numeroConta: "123456-0",
                                    carteira: "12",
                                    numeroBoletoAtual: 0,
                                    nomeBeneficiario: "Financeiro Aula Solutions",
                                    cnpjBeneficiario: "09.934.582/0001-58",
                                    enderecoBeneficiario: new Endereco(
                                        cep: "93000-000",
                                        logradouro: "Rua das Empresas",
                                        numero: "112",
                                        complemento: "",
                                        bairro: "Centro",
                                        municipio: "Porto Alegre",
                                        uf: "RS")
                                    );

            await _context.ParametrosBoleto.AddAsync(parametros);
            await _context.SaveChangesAsync();

            return parametros;
        }
    }
}