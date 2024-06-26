﻿namespace Financeiro.Aula.Domain.Configurations
{
    public class RabbitMqConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public RabbitMqQueuesConfiguration Queues { get; set; } = null!;
        //public string Username { get; set; }
        //public string Password { get; set; }
    }

    public class RabbitMqQueuesConfiguration
    {
        public string RegistrarBoleto { get; set; } = string.Empty;
        public string BoletoRegistrado { get; set; } = string.Empty;
        public string DeadLetterQueue { get; set; } = string.Empty;
        public string DeadeLetterExchange { get; set; } = string.Empty;
    }
}