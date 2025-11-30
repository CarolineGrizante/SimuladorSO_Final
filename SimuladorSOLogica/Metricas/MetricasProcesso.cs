namespace SimuladorSOLogica.Metricas
{
    public class MetricasProcesso
    {
        public int PID { get; set; }
        public long TempoRetorno { get; set; }
        public long TempoEspera { get; set; }
        public long TempoResposta { get; set; }
        public long TempoCPU { get; set; }
        public long TempoChegada { get; set; }
        public long TempoFinalizacao { get; set; }

        public MetricasProcesso(int pid)
        {
            PID = pid;
            TempoRetorno = 0;
            TempoEspera = 0;
            TempoResposta = 0;
            TempoCPU = 0;
            TempoChegada = 0;
            TempoFinalizacao = 0;
        }

        public void CalcularMetricas(long tempoChegada, long tempoInicio, long tempoFim, long tempoCPU, long tempoEspera)
        {
            TempoChegada = tempoChegada;
            TempoFinalizacao = tempoFim;
            TempoCPU = tempoCPU;
            TempoEspera = tempoEspera;
            TempoResposta = tempoInicio - tempoChegada;
            TempoRetorno = tempoFim - tempoChegada;
        }

        public override string ToString()
        {
            return $"PID {PID}: Retorno={TempoRetorno}, Espera={TempoEspera}, " +
                   $"Resposta={TempoResposta}, CPU={TempoCPU}";
        }
    }
}