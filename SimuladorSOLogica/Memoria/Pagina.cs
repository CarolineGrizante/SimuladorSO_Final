namespace SimuladorSOLogica.Memoria
{
    public class Pagina
    {
        public int NumeroPagina { get; set; }
        public int PID { get; set; }
        public bool Presente { get; set; }
        public int NumeroMoldura { get; set; }
        public bool Modificada { get; set; }
        public bool Referenciada { get; set; }
        public long TempoUltimoAcesso { get; set; }

        public Pagina(int numeroPagina, int pid)
        {
            NumeroPagina = numeroPagina;
            PID = pid;
            Presente = false;
            NumeroMoldura = -1;
            Modificada = false;
            Referenciada = false;
            TempoUltimoAcesso = 0;
        }

        public override string ToString()
        {
            return $"Página {NumeroPagina} (PID={PID}): Presente={Presente}, " +
                   $"Moldura={NumeroMoldura}, Modificada={Modificada}";
        }
    }
}