namespace SimuladorSOLogica.Metricas
{
    public class MetricasDispositivo
    {
        public string NomeDispositivo { get; set; }
        public int TotalRequisicoes { get; set; }
        public long TempoTotalOcupado { get; set; }
        public long TempoTotal { get; set; }
        public double TaxaUtilizacao { get; set; }

        public MetricasDispositivo(string nome)
        {
            NomeDispositivo = nome;
            TotalRequisicoes = 0;
            TempoTotalOcupado = 0;
            TempoTotal = 0;
            TaxaUtilizacao = 0.0;
        }

        public void CalcularUtilizacao(long tempoOcupado, long tempoTotal)
        {
            TempoTotalOcupado = tempoOcupado;
            TempoTotal = tempoTotal;
            TaxaUtilizacao = tempoTotal > 0 ? (double)tempoOcupado / tempoTotal : 0.0;
        }

        public override string ToString()
        {
            return $"{NomeDispositivo}: Requisições={TotalRequisicoes}, " +
                   $"Utilização={TaxaUtilizacao:P2}";
        }
    }
}