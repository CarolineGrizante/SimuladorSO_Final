namespace SimuladorSOLogica.Memoria
{
    public class EntradaTLB
    {
        public int PID { get; set; }
        public int NumeroPagina { get; set; }
        public int NumeroMoldura { get; set; }
        public bool Valida { get; set; }
        public long TempoUltimoAcesso { get; set; }

        public EntradaTLB()
        {
            PID = -1;
            NumeroPagina = -1;
            NumeroMoldura = -1;
            Valida = false;
            TempoUltimoAcesso = 0;
        }

        public void Atualizar(int pid, int numeroPagina, int numeroMoldura, long tempo)
        {
            PID = pid;
            NumeroPagina = numeroPagina;
            NumeroMoldura = numeroMoldura;
            Valida = true;
            TempoUltimoAcesso = tempo;
        }

        public void Invalidar()
        {
            Valida = false;
        }

        public override string ToString()
        {
            if (Valida)
                return $"TLB: PID={PID}, Página={NumeroPagina} -> Moldura={NumeroMoldura}";
            else
                return "TLB: Inválida";
        }
    }
}