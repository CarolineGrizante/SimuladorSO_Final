using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Escalonamento
{
    public class RoundRobin : IAlgoritmoEscalonamento
    {
        public string Nome => "Round Robin";
        public bool EhPreemptivo => true;
        public int Quantum { get; private set; }
        public int TempoRestante { get; set; }

        public RoundRobin(int quantum = 4)
        {
            Quantum = quantum;
            TempoRestante = quantum;
        }

        public Processo SelecionarProximoProcesso(FilaProntos filaProntos)
        {
            TempoRestante = Quantum;
            return filaProntos.RemoverPrimeiro();
        }

        public void ConfigurarQuantum(int quantum)
        {
            Quantum = quantum;
            TempoRestante = quantum;
        }

        public bool QuantumExpirou()
        {
            return TempoRestante <= 0;
        }

        public void DecrementarQuantum()
        {
            TempoRestante--;
        }

        public void ResetarQuantum()
        {
            TempoRestante = Quantum;
        }
    }
}
