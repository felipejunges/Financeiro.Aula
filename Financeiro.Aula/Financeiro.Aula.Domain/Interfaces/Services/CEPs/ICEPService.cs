using Financeiro.Common.ValueObjects;

namespace Financeiro.Aula.Domain.Interfaces.Services.CEPs
{
    public interface ICepService
    {
        Task<Endereco?> BuscarCEP(string cep);
    }
}