﻿using Financeiro.Boleto.Domain.DTOs;
using Financeiro.Boleto.Domain.Entities;
using Financeiro.Boleto.Domain.Interfaces.ApiServices;
using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Financeiro.Boleto.Infra.Services.ApiServices
{
    public class BoletoCloudApiService : IGeradorBoletoApiService
    {
        private readonly ILogger<BoletoCloudApiService> _logger;
        private readonly HttpClient _client;
        private readonly ParametroBoleto? _parametroBoleto;

        public BoletoCloudApiService(
            ILogger<BoletoCloudApiService> logger,
            HttpClient client,
            IParametroBoletoRepository parametroBoletoRepository)
        {
            _logger = logger;
            _client = client;
            _parametroBoleto = parametroBoletoRepository.ObterParametrosBoleto().Result;
        }

        public async Task<byte[]?> ObterPdfBoleto(string chaveBoleto)
        {
            var response = await _client.GetAsync($"boletos/{chaveBoleto}");

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                _logger.LogError("Requisição de obtenção de boleto mal sucedida. {StatusCode} - {Erro}", response.StatusCode, erro);

                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<string?> GerarTokenBoleto(BoletoGerarDto boleto, string numeroBoleto)
        {
            var body = MontarBodyDaParcela(boleto, numeroBoleto);

            _logger.LogInformation(
                    "Enviando registro de boleto para Boleto.Cloud. Boleto: {boleto} - Numero: {numeroBoleto}",
                    boleto.TokenRetorno,
                    numeroBoleto);

            using (var conteudo = new FormUrlEncodedContent(body))
            {
                conteudo.Headers.Clear();
                conteudo.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var response = await _client.PostAsync("boletos", conteudo);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Requisição de registro de boleto mal sucedida. {StatusCode} - {Erro}", response.StatusCode, erro);

                    return null;
                }

                return ObterTokenDoHeader(response);
            }
        }

        private Dictionary<string, string> MontarBodyDaParcela(BoletoGerarDto boleto, string numeroBoleto)
        {
            var cliente = boleto.Cliente;

            var body = new Dictionary<string, string>
            {
                ["boleto.conta.banco"] = _parametroBoleto!.Banco,
                ["boleto.conta.agencia"] = _parametroBoleto.Agencia,
                ["boleto.conta.numero"] = _parametroBoleto.NumeroConta,
                ["boleto.conta.carteira"] = _parametroBoleto.Carteira,
                ["boleto.beneficiario.nome"] = _parametroBoleto.NomeBeneficiario,
                ["boleto.beneficiario.cprf"] = _parametroBoleto.CnpjBeneficiario,
                ["boleto.beneficiario.endereco.cep"] = _parametroBoleto.EnderecoBeneficiario.Cep,
                ["boleto.beneficiario.endereco.uf"] = _parametroBoleto.EnderecoBeneficiario.Uf,
                ["boleto.beneficiario.endereco.localidade"] = _parametroBoleto.EnderecoBeneficiario.Municipio,
                ["boleto.beneficiario.endereco.bairro"] = _parametroBoleto.EnderecoBeneficiario.Bairro,
                ["boleto.beneficiario.endereco.logradouro"] = _parametroBoleto.EnderecoBeneficiario.Logradouro,
                ["boleto.beneficiario.endereco.numero"] = _parametroBoleto.EnderecoBeneficiario.Numero,
                ["boleto.beneficiario.endereco.complemento"] = _parametroBoleto.EnderecoBeneficiario.Complemento ?? string.Empty,
                ["boleto.emissao"] = DateTime.Now.ToString("yyyy-MM-dd"),
                ["boleto.vencimento"] = boleto.DataVencimento.ToString("yyyy-MM-dd"),
                ["boleto.documento"] = $"CTR{boleto.IdentificadorContrato}",
                ["boleto.numero"] = numeroBoleto,
                ["boleto.titulo"] = "DM",
                ["boleto.valor"] = boleto.Valor.ToString("F").Replace(",", "."),
                ["boleto.pagador.nome"] = cliente.Nome,
                ["boleto.pagador.cprf"] = cliente.Cpf,
                ["boleto.pagador.endereco.cep"] = cliente.Endereco.Cep,
                ["boleto.pagador.endereco.uf"] = cliente.Endereco.Uf,
                ["boleto.pagador.endereco.localidade"] = cliente.Endereco.Municipio,
                ["boleto.pagador.endereco.bairro"] = cliente.Endereco.Bairro,
                ["boleto.pagador.endereco.logradouro"] = cliente.Endereco.Logradouro,
                ["boleto.pagador.endereco.numero"] = cliente.Endereco.Numero,
                ["boleto.pagador.endereco.complemento"] = cliente.Endereco.Complemento ?? string.Empty,
                ["boleto.instrucao"] = "Atenção! NÃO RECEBER ESTE BOLETO.",
                ["boleto.instrucao"] = "Este é apenas um teste utilizando a API Boleto Cloud",
                ["boleto.instrucao"] = "Mais info em http://boleto.cloud/app/dev/api"
            };

            return body;
        }

        private static string? ObterTokenDoHeader(HttpResponseMessage response)
        {
            if (response.Headers.Contains("Location"))
            {
                var location = response.Headers.GetValues("Location");

                if (location.Any())
                {
                    var token = location.First();

                    return token.Split('/').Last();
                }
            }

            return null;
        }
    }
}