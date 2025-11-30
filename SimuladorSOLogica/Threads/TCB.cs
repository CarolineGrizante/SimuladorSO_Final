namespace SimuladorSOLogica.Threads
{
    public class TCB
    {
        public int TID { get; set; }
        public int PID { get; set; }
        public EstadoThread Estado { get; set; }
        public int Prioridade { get; set; }
        public int ContadorDePrograma { get; set; }
        public Dictionary<string, int> Registradores { get; set; }

        // Informações de tempo
        public long TempoChegada { get; set; }
        public long TempoInicio { get; set; }
        public long TempoFinalizacao { get; set; }
        public long TempoTotalCPU { get; set; }
        public long TempoEspera { get; set; }

        // Pilha da thread
        public int PonteiroBase { get; set; }
        public int PonteiroTopo { get; set; }

        public TCB(int tid, int pid, int prioridade)
        {
            TID = tid;
            PID = pid;
            Estado = EstadoThread.Nova;
            Prioridade = prioridade;
            ContadorDePrograma = 0;
            Registradores = new Dictionary<string, int>();

            TempoChegada = 0;
            TempoInicio = -1;
            TempoFinalizacao = -1;
            TempoTotalCPU = 0;
            TempoEspera = 0;

            PonteiroBase = 0;
            PonteiroTopo = 0;
        }

        public override string ToString()
        {
            return $"TID: {TID}, PID: {PID}, Estado: {Estado}, Prioridade: {Prioridade}, " +
                   $"Tempo CPU: {TempoTotalCPU}";
        }

        public string ObterDetalhesCompletos()
        {
            string detalhes = $"===== TCB - Thread {TID} =====\n";
            detalhes += $"PID do Processo: {PID}\n";
            detalhes += $"Estado: {Estado}\n";
            detalhes += $"Prioridade: {Prioridade}\n";
            detalhes += $"Contador de Programa: {ContadorDePrograma}\n";
            detalhes += $"Tempo de Chegada: {TempoChegada}\n";
            detalhes += $"Tempo de Início: {TempoInicio}\n";
            detalhes += $"Tempo de Finalização: {TempoFinalizacao}\n";
            detalhes += $"Tempo Total de CPU: {TempoTotalCPU}\n";
            detalhes += $"Tempo de Espera: {TempoEspera}\n";
            detalhes += $"Ponteiro Base: {PonteiroBase}\n";
            detalhes += $"Ponteiro Topo: {PonteiroTopo}\n";
            detalhes += "================================\n";
            return detalhes;
        }
    }
}
