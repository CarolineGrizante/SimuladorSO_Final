using SimuladorSOLogica.Nucleo;
using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Escalonamento
{
    public class Escalonador
    {
        private Kernel _kernel;
        private IAlgoritmoEscalonamento _algoritmo;
        private FilaProntos _filaProntos;
        private TrocaDeContexto _trocaDeContexto;
        private Processo _processoAtual;
        private int _ticksProcessoAtual;

        public Escalonador(Kernel kernel)
        {
            _kernel = kernel;
            _filaProntos = new FilaProntos();
            _trocaDeContexto = new TrocaDeContexto(kernel);
            _algoritmo = new FCFS(); // Algoritmo padrão
            _processoAtual = null;
            _ticksProcessoAtual = 0;
        }

        public void TrocarAlgoritmo(string nomeAlgoritmo)
        {
            switch (nomeAlgoritmo.ToUpper())
            {
                case "FCFS":
                    _algoritmo = new FCFS();
                    break;
                case "RR":
                case "ROUNDROBIN":
                case "ROUND_ROBIN":
                    _algoritmo = new RoundRobin(_kernel.Configuracoes.Quantum);
                    break;
                case "PRIORIDADE_PREEMPTIVO":
                case "PRIORIDADE":
                    _algoritmo = new PrioridadePreemptivo();
                    break;
                case "PRIORIDADE_NAO_PREEMPTIVO":
                    _algoritmo = new PrioridadeNaoPreemptivo();
                    break;
                default:
                    throw new ArgumentException($"Algoritmo desconhecido: {nomeAlgoritmo}");
            }

            _kernel.RegistradorDeEventos.Registrar($"Algoritmo de escalonamento alterado para: {_algoritmo.Nome}");
        }

        public void ConfigurarQuantum(int quantum)
        {
            _kernel.Configuracoes.Quantum = quantum;
            _algoritmo.ConfigurarQuantum(quantum);
            _kernel.RegistradorDeEventos.Registrar($"Quantum configurado para: {quantum}");
        }

        public void AdicionarProcessoPronto(Processo processo)
        {
            processo.MudarEstado(EstadoProcesso.Pronto);
            _filaProntos.Adicionar(processo);
            _kernel.RegistradorDeEventos.Registrar($"Processo {processo.PID} adicionado à fila de prontos");
        }

        public void ExecutarCiclo()
        {
            // Se não há processo atual, selecionar o próximo
            if (_processoAtual == null || _processoAtual.Estado != EstadoProcesso.Executando)
            {
                SelecionarProximoProcesso();
            }

            // Se ainda não há processo, não há nada para executar
            if (_processoAtual == null)
                return;

            // Executar o processo atual
            _processoAtual.IncrementarTempoCPU();
            _processoAtual.DefinirTempoInicio(_kernel.Relogio.TempoAtual);
            _ticksProcessoAtual++;

            // Verificar se deve fazer preempção (Round Robin)
            if (_algoritmo is RoundRobin rr)
            {
                rr.DecrementarQuantum();
                if (rr.QuantumExpirou())
                {
                    // Retornar processo para a fila de prontos
                    _processoAtual.MudarEstado(EstadoProcesso.Pronto);
                    _filaProntos.Adicionar(_processoAtual);
                    _kernel.RegistradorDeEventos.Registrar($"Quantum expirado para processo {_processoAtual.PID}");
                    SelecionarProximoProcesso();
                }
            }

            // Incrementar tempo de espera para processos na fila
            foreach (var processo in _filaProntos.ObterTodos())
            {
                processo.IncrementarTempoEspera();
            }
        }

        private void SelecionarProximoProcesso()
        {
            Processo processoAnterior = _processoAtual;

            _processoAtual = _algoritmo.SelecionarProximoProcesso(_filaProntos);

            if (_processoAtual != null)
            {
                _processoAtual.MudarEstado(EstadoProcesso.Executando);
                _ticksProcessoAtual = 0;

                if (processoAnterior != _processoAtual)
                {
                    _trocaDeContexto.RealizarTroca(processoAnterior, _processoAtual);
                }
            }
        }

        public Processo ObterProcessoAtual()
        {
            return _processoAtual;
        }

        public FilaProntos ObterFilaProntos()
        {
            return _filaProntos;
        }

        public TrocaDeContexto ObterTrocaDeContexto()
        {
            return _trocaDeContexto;
        }

        public string ObterNomeAlgoritmo()
        {
            return _algoritmo.Nome;
        }

        public string ObterResumo()
        {
            string resumo = "===== ESCALONADOR =====\n";
            resumo += $"Algoritmo: {_algoritmo.Nome}\n";
            resumo += $"Processo atual: {_processoAtual?.PID ?? -1}\n";
            resumo += $"Ticks do processo atual: {_ticksProcessoAtual}\n";
            resumo += $"Processos na fila: {_filaProntos.Tamanho()}\n";
            resumo += _trocaDeContexto.ObterEstatisticas() + "\n";
            resumo += "=======================\n";
            return resumo;
        }
    }
}