using System.Text.RegularExpressions;

namespace Financeiro.Common.ValueObjects
{
    public class Cep : IComparable<Cep>
    {
        private readonly string _numero = null!;

        public string Numero => _numero;

        public string Formatado => !EhValido ? throw new InvalidOperationException("O CEP nao é valido") : $"{_numero[..5]}-{_numero[5..3]}";

        public Cep(string? numero)
        {
            if (numero == null)
            {
                _numero = string.Empty;
                return;
            }

            _numero = Regex.Replace(numero, @"[^0-9]", "");
        }

        public bool EhValido => ValidarCEP();

        private bool ValidarCEP()
        {
            Regex regex = new Regex(@"^\d{8}$");
            return regex.IsMatch(_numero);
        }

        public override string ToString()
        {
            return Numero;
        }

        public int CompareTo(Cep? other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            return other.Numero.CompareTo(Numero);
        }

        public static implicit operator Cep(string? numero) => new Cep(numero);
    }
}
