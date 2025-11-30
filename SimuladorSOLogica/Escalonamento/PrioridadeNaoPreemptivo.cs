using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Escalonamento
{
    public class PrioridadeNaoPreemptivo : IAlgoritmoEscalonamento
    {
        public string Nome => "Prioridade (Não Preemptivo)";
        public bool EhPreemptivo => false;

        public Processo SelecionarProximoProcesso(FilaProntos filaProntos)
        {
            var processos = filaProntos.ObterPorPrioridade();
            if (processos.Count == 0)
                return null;

            Processo processoMaiorPrioridade = processos[0];
            filaProntos.Remover(processoMaiorPrioridade);
            return processoMaiorPrioridade;
        }

        public void ConfigurarQuantum(int quantum)
        {
            // Prioridade não usa quantum
        }
    }
}
