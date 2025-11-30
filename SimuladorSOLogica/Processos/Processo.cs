namespace SimuladorSOLogica.Processos
{
    public class Processo
    {
        public PCB PCB { get; private set; }
        public int PID => PCB.PID;
        public EstadoProcesso Estado => PCB.Estado;

        public Processo(int pid, int prioridade, long tempoChegada)
        {
            PCB = new PCB(pid, prioridade);
            PCB.TempoChegada = tempoChegada;
        }

        public void MudarEstado(EstadoProcesso novoEstado)
        {
            PCB.Estado = novoEstado;
        }

        public void IncrementarTempoCPU()
        {
            PCB.TempoTotalCPU++;
        }

        public void IncrementarTempoEspera()
        {
            PCB.TempoEspera++;
        }

        public void DefinirTempoInicio(long tempo)
        {
            if (PCB.TempoInicio == -1)
                PCB.TempoInicio = tempo;
        }

        public void DefinirTempoFinalizacao(long tempo)
        {
            PCB.TempoFinalizacao = tempo;
        }

        public void AbrirArquivo(string caminhoArquivo)
        {
            if (!PCB.ArquivosAbertos.Contains(caminhoArquivo))
                PCB.ArquivosAbertos.Add(caminhoArquivo);
        }

        public void FecharArquivo(string caminhoArquivo)
        {
            PCB.ArquivosAbertos.Remove(caminhoArquivo);
        }

        public void AdicionarThread(int threadID)
        {
            if (!PCB.ThreadsIDs.Contains(threadID))
                PCB.ThreadsIDs.Add(threadID);
        }

        public void RemoverThread(int threadID)
        {
            PCB.ThreadsIDs.Remove(threadID);
        }

        public override string ToString()
        {
            return PCB.ToString();
        }
    }
}