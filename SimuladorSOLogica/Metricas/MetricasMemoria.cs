namespace SimuladorSOLogica.Metricas
{
    public class MetricasMemoria
    {
        public int TotalFaltasPagina { get; set; }
        public int TotalAcessos { get; set; }
        public double TaxaFaltasPagina { get; set; }
        public int AcertosTLB { get; set; }
        public int FalhasTLB { get; set; }
        public double TaxaAcertoTLB { get; set; }
        public int MoldurasLivres { get; set; }
        public int MoldurasOcupadas { get; set; }

        public MetricasMemoria()
        {
            TotalFaltasPagina = 0;
            TotalAcessos = 0;
            TaxaFaltasPagina = 0.0;
            AcertosTLB = 0;
            FalhasTLB = 0;
            TaxaAcertoTLB = 0.0;
            MoldurasLivres = 0;
            MoldurasOcupadas = 0;
        }

        public void CalcularTaxas()
        {
            TaxaFaltasPagina = TotalAcessos > 0 ? (double)TotalFaltasPagina / TotalAcessos : 0.0;
            int totalAcessosTLB = AcertosTLB + FalhasTLB;
            TaxaAcertoTLB = totalAcessosTLB > 0 ? (double)AcertosTLB / totalAcessosTLB : 0.0;
        }

        public override string ToString()
        {
            return $"Memória: Faltas de Página={TotalFaltasPagina} ({TaxaFaltasPagina:P2}), " +
                   $"TLB Acertos={AcertosTLB} ({TaxaAcertoTLB:P2})";
        }
    }
}