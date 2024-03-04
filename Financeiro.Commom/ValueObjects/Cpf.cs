using System.Text.RegularExpressions;

namespace Financeiro.Commom.ValueObjects
{
    public class Cpf : IComparable<Cpf>
    {
        private readonly string _numero = null!;

        public string Numero => _numero;

        public string Formatado => !EhValido ? throw new InvalidOperationException("O CPF nao é valido") : $"{_numero[..3]}.{_numero[3..3]}.{_numero[6..3]}-{_numero[9..2]}";

        public Cpf(string? numero)
        {
            if (numero == null)
            {
                _numero = string.Empty;
                return;
            }

            _numero = Regex.Replace(numero, @"[^0-9]", "");
        }

        public bool EhValido => ValidarCPF();

        private bool ValidarCPF()
        {
            if (_numero.Length != 11)
                return false;

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(_numero[i].ToString()) * (10 - i);

            int primeiroDigito = (soma % 11) < 2 ? 0 : 11 - (soma % 11);

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(_numero[i].ToString()) * (11 - i);

            int segundoDigito = (soma % 11) < 2 ? 0 : 11 - (soma % 11);

            return _numero.EndsWith(primeiroDigito.ToString() + segundoDigito.ToString());
        }

        public override string ToString()
        {
            return Numero;
        }

        public int CompareTo(Cpf? other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            return other.Numero.CompareTo(Numero);
        }

        public static implicit operator Cpf(string? numero) => new Cpf(numero);
    }
}