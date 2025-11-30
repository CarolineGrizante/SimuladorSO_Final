using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Escalonamento
{
    public class FCFS : IAlgoritmoEscalonamento
    {
        public string Nome => "FCFS (First Come First Served)";
        public bool EhPreemptivo => false;

        public Processo SelecionarProximoProcesso(FilaProntos filaProntos)
        {
            return filaProntos.RemoverPrimeiro();
        }

        public void ConfigurarQuantum(int quantum)
        {
            // FCFS não usa quantum
        }
    }
}