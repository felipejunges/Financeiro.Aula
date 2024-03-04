namespace Financeiro.Common.Events
{
    public class BoletoRegistradoEvent
    {
        public Guid Token { get; private set; }

        public string ChaveBoleto { get; private set; }

        public BoletoRegistradoEvent(Guid token, string chaveBoleto)
        {
            Token = token;
            ChaveBoleto = chaveBoleto;
        }
    }
}