using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Escalonamento
{
    public interface IAlgoritmoEscalonamento
    {
        string Nome { get; }
        Processo SelecionarProximoProcesso(FilaProntos filaProntos);
        bool EhPreemptivo { get; }
        void ConfigurarQuantum(int quantum);
    }
}