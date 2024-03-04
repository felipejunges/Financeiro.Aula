using Financeiro.Common.ValueObjects;

namespace Financeiro.Common.Events
{
    public class GerarBoletoEvent
    {
        public Guid TokenRetorno { get; private set; }

        public string IdentificadorContrato { get; private set; }

        public DateTime DataVencimento { get; private set; }

        public double Valor { get; private set; }

        public GerarBoletoCliente Cliente { get; private set; }

        public GerarBoletoEvent(Guid tokenRetorno, string identificadorContrato, DateTime dataVencimento, double valor, GerarBoletoCliente cliente)
        {
            TokenRetorno = tokenRetorno;
            IdentificadorContrato = identificadorContrato;
            DataVencimento = dataVencimento;
            Valor = valor;
            Cliente = cliente;
        }
    }

    public record GerarBoletoCliente
    {
        public string Nome { get; private set; }

        public string Cpf { get; private set; }

        public Endereco Endereco { get; private set; }

        public GerarBoletoCliente(string nome, string cpf, Endereco endereco)
        {
            Nome = nome;
            Cpf = cpf;
            Endereco = endereco;
        }
    }
}