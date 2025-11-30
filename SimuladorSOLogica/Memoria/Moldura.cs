namespace SimuladorSOLogica.Memoria
{
    public class Moldura
    {
        public int NumeroMoldura { get; set; }
        public bool Ocupada { get; set; }
        public int PID { get; set; }
        public int NumeroPagina { get; set; }
        public long TempoAlocacao { get; set; }

        public Moldura(int numeroMoldura)
        {
            NumeroMoldura = numeroMoldura;
            Ocupada = false;
            PID = -1;
            NumeroPagina = -1;
            TempoAlocacao = 0;
        }

        public void Alocar(int pid, int numeroPagina, long tempo)
        {
            Ocupada = true;
            PID = pid;
            NumeroPagina = numeroPagina;
            TempoAlocacao = tempo;
        }

        public void Liberar()
        {
            Ocupada = false;
            PID = -1;
            NumeroPagina = -1;
            TempoAlocacao = 0;
        }

        public override string ToString()
        {
            if (Ocupada)
                return $"Moldura {NumeroMoldura}: PID={PID}, Página={NumeroPagina}";
            else
                return $"Moldura {NumeroMoldura}: Livre";
        }
    }
}