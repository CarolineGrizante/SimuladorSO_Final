namespace SimuladorSOLogica.Threads
{
    public class ThreadSimulada
    {
        public TCB TCB { get; private set; }
        public int TID => TCB.TID;
        public int PID => TCB.PID;
        public EstadoThread Estado => TCB.Estado;

        public ThreadSimulada(int tid, int pid, int prioridade, long tempoChegada)
        {
            TCB = new TCB(tid, pid, prioridade);
            TCB.TempoChegada = tempoChegada;
        }

        public void MudarEstado(EstadoThread novoEstado)
        {
            TCB.Estado = novoEstado;
        }

        public void IncrementarTempoCPU()
        {
            TCB.TempoTotalCPU++;
        }

        public void IncrementarTempoEspera()
        {
            TCB.TempoEspera++;
        }

        public void DefinirTempoInicio(long tempo)
        {
            if (TCB.TempoInicio == -1)
                TCB.TempoInicio = tempo;
        }

        public void DefinirTempoFinalizacao(long tempo)
        {
            TCB.TempoFinalizacao = tempo;
        }

        public override string ToString()
        {
            return TCB.ToString();
        }
    }
}
