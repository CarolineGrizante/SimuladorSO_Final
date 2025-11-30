namespace SimuladorSOLogica.EntradaSaida
{
    public class RequisicaoES
    {
        public int RequisicaoID { get; set; }
        public int PID { get; set; }
        public string NomeDispositivo { get; set; }
        public int TempoRequerido { get; set; }
        public int TempoRestante { get; set; }
        public bool Bloqueante { get; set; }
        public long TempoInicio { get; set; }
        public long TempoFim { get; set; }
        public bool Concluida { get; set; }

        public RequisicaoES(int id, int pid, string dispositivo, int tempo, bool bloqueante)
        {
            RequisicaoID = id;
            PID = pid;
            NomeDispositivo = dispositivo;
            TempoRequerido = tempo;
            TempoRestante = tempo;
            Bloqueante = bloqueante;
            TempoInicio = 0;
            TempoFim = 0;
            Concluida = false;
        }

        public void ProcessarTick()
        {
            if (TempoRestante > 0)
            {
                TempoRestante--;
            }

            if (TempoRestante == 0)
            {
                Concluida = true;
            }
        }

        public override string ToString()
        {
            return $"Requisição {RequisicaoID}: PID={PID}, Dispositivo={NomeDispositivo}, " +
                   $"Tempo={TempoRequerido}, Restante={TempoRestante}, Bloqueante={Bloqueante}";
        }
    }
}