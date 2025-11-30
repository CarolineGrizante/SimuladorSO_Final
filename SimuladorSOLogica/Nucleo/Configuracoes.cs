namespace SimuladorSOLogica.Nucleo
{
    public class Configuracoes
    {
        public int Quantum { get; set; }
        public int TamanhoPagina { get; set; }
        public int NumeroMolduras { get; set; }
        public int SementeDeterministica { get; set; }
        public bool UsarTLB { get; set; }
        public int TamanhoTLB { get; set; }

        // Tempos de dispositivos (em ticks)
        public int TempoDisco { get; set; }
        public int TempoTeclado { get; set; }
        public int TempoImpressora { get; set; }
        public int TempoRede { get; set; }

        public Configuracoes()
        {
            // Valores padrão
            Quantum = 4;
            TamanhoPagina = 1024;
            NumeroMolduras = 16;
            SementeDeterministica = 0;
            UsarTLB = false;
            TamanhoTLB = 8;

            TempoDisco = 30;
            TempoTeclado = 10;
            TempoImpressora = 40;
            TempoRede = 20;
        }

        public void DefinirSemente(int semente)
        {
            SementeDeterministica = semente;
        }

        public void ConfigurarMemoria(int tamanhoPagina, int numeroMolduras)
        {
            if (tamanhoPagina <= 0)
                throw new ArgumentException("Tamanho de página deve ser positivo.");
            if (numeroMolduras <= 0)
                throw new ArgumentException("Número de molduras deve ser positivo.");

            TamanhoPagina = tamanhoPagina;
            NumeroMolduras = numeroMolduras;
        }

        public void ConfigurarTempoDispositivo(string dispositivo, int tempo)
        {
            if (tempo < 0)
                throw new ArgumentException("Tempo deve ser não-negativo.");

            switch (dispositivo.ToUpper())
            {
                case "DISCO":
                    TempoDisco = tempo;
                    break;
                case "TECLADO":
                    TempoTeclado = tempo;
                    break;
                case "IMPRESSORA":
                    TempoImpressora = tempo;
                    break;
                case "REDE":
                    TempoRede = tempo;
                    break;
                default:
                    throw new ArgumentException($"Dispositivo desconhecido: {dispositivo}");
            }
        }
    }
}