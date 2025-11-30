namespace SimuladorSOLogica.Processos
{
    public class PCB
    {
        public int PID { get; set; }
        public EstadoProcesso Estado { get; set; }
        public int Prioridade { get; set; }
        public int ContadorDePrograma { get; set; }
        public Dictionary<string, int> Registradores { get; set; }

        // Informações de tempo
        public long TempoChegada { get; set; }
        public long TempoInicio { get; set; }
        public long TempoFinalizacao { get; set; }
        public long TempoTotalCPU { get; set; }
        public long TempoEspera { get; set; }

        // Memória
        public int TamanhoMemoria { get; set; }
        public int EnderecoBase { get; set; }
        public int EnderecoLimite { get; set; }

        // Arquivos abertos
        public List<string> ArquivosAbertos { get; set; }

        // Threads do processo
        public List<int> ThreadsIDs { get; set; }

        public PCB(int pid, int prioridade)
        {
            PID = pid;
            Estado = EstadoProcesso.Novo;
            Prioridade = prioridade;
            ContadorDePrograma = 0;
            Registradores = new Dictionary<string, int>();

            TempoChegada = 0;
            TempoInicio = -1;
            TempoFinalizacao = -1;
            TempoTotalCPU = 0;
            TempoEspera = 0;

            TamanhoMemoria = 0;
            EnderecoBase = 0;
            EnderecoLimite = 0;

            ArquivosAbertos = new List<string>();
            ThreadsIDs = new List<int>();
        }

        public override string ToString()
        {
            return $"PID: {PID}, Estado: {Estado}, Prioridade: {Prioridade}, " +
                   $"Tempo CPU: {TempoTotalCPU}, Threads: {ThreadsIDs.Count}";
        }

        public string ObterDetalhesCompletos()
        {
            string detalhes = $"===== PCB - Processo {PID} =====\n";
            detalhes += $"Estado: {Estado}\n";
            detalhes += $"Prioridade: {Prioridade}\n";
            detalhes += $"Contador de Programa: {ContadorDePrograma}\n";
            detalhes += $"Tempo de Chegada: {TempoChegada}\n";
            detalhes += $"Tempo de Início: {TempoInicio}\n";
            detalhes += $"Tempo de Finalização: {TempoFinalizacao}\n";
            detalhes += $"Tempo Total de CPU: {TempoTotalCPU}\n";
            detalhes += $"Tempo de Espera: {TempoEspera}\n";
            detalhes += $"Tamanho de Memória: {TamanhoMemoria} bytes\n";
            detalhes += $"Endereço Base: {EnderecoBase}\n";
            detalhes += $"Endereço Limite: {EnderecoLimite}\n";
            detalhes += $"Arquivos Abertos: {ArquivosAbertos.Count}\n";
            detalhes += $"Threads: {ThreadsIDs.Count}\n";
            detalhes += "================================\n";
            return detalhes;
        }
    }
}