namespace SimuladorSOLogica.EntradaSaida
{
    public class Interrupcao
    {
        public int InterrupcaoID { get; set; }
        public string NomeDispositivo { get; set; }
        public int PID { get; set; }
        public long TempoGeracao { get; set; }
        public string Mensagem { get; set; }
        public bool Processada { get; set; }

        public Interrupcao(int id, string dispositivo, int pid, long tempo, string mensagem)
        {
            InterrupcaoID = id;
            NomeDispositivo = dispositivo;
            PID = pid;
            TempoGeracao = tempo;
            Mensagem = mensagem;
            Processada = false;
        }

        public override string ToString()
        {
            return $"Interrupção {InterrupcaoID}: Dispositivo={NomeDispositivo}, PID={PID}, " +
                   $"Tempo={TempoGeracao}, Mensagem={Mensagem}, Processada={Processada}";
        }
    }
}