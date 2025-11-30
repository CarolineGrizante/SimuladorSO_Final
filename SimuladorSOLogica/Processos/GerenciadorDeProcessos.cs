using SimuladorSOLogica.Nucleo;
using SimuladorSOLogica.Utilitarios;

namespace SimuladorSOLogica.Processos
{
    public class GerenciadorDeProcessos
    {
        private Kernel _kernel;
        private Dictionary<int, Processo> _processos;
        private GeradorIDs _geradorIDs;

        public GerenciadorDeProcessos(Kernel kernel)
        {
            _kernel = kernel;
            _processos = new Dictionary<int, Processo>();
            _geradorIDs = new GeradorIDs();
        }

        public Processo CriarProcesso(int prioridade = 0)
        {
            int pid = _geradorIDs.GerarProximoID();
            long tempoChegada = _kernel.Relogio.TempoAtual;

            Processo processo = new Processo(pid, prioridade, tempoChegada);
            processo.MudarEstado(EstadoProcesso.Novo);

            _processos[pid] = processo;

            _kernel.RegistradorDeEventos.Registrar($"Processo criado: PID={pid}, Prioridade={prioridade}");

            return processo;
        }

        public void RemoverProcesso(int pid)
        {
            if (!_processos.ContainsKey(pid))
                throw new ArgumentException($"Processo {pid} não existe.");

            _processos.Remove(pid);
            _kernel.RegistradorDeEventos.Registrar($"Processo removido: PID={pid}");
        }

        public void FinalizarProcesso(int pid)
        {
            if (!_processos.ContainsKey(pid))
                throw new ArgumentException($"Processo {pid} não existe.");

            Processo processo = _processos[pid];
            processo.MudarEstado(EstadoProcesso.Finalizado);
            processo.DefinirTempoFinalizacao(_kernel.Relogio.TempoAtual);

            _kernel.RegistradorDeEventos.Registrar($"Processo finalizado: PID={pid}");
        }

        public void MudarEstadoProcesso(int pid, EstadoProcesso novoEstado)
        {
            if (!_processos.ContainsKey(pid))
                throw new ArgumentException($"Processo {pid} não existe.");

            Processo processo = _processos[pid];
            EstadoProcesso estadoAnterior = processo.Estado;
            processo.MudarEstado(novoEstado);

            _kernel.RegistradorDeEventos.Registrar(
                $"Processo {pid}: {estadoAnterior} -> {novoEstado}");
        }

        public Processo ObterProcesso(int pid)
        {
            if (!_processos.ContainsKey(pid))
                throw new ArgumentException($"Processo {pid} não existe.");

            return _processos[pid];
        }

        public List<Processo> ListarProcessos()
        {
            return _processos.Values.ToList();
        }

        public List<Processo> ListarProcessosPorEstado(EstadoProcesso estado)
        {
            return _processos.Values
                .Where(p => p.Estado == estado)
                .ToList();
        }

        public bool ExistemProcessosAtivos()
        {
            return _processos.Values.Any(p =>
                p.Estado != EstadoProcesso.Finalizado);
        }

        public int ContarProcessos()
        {
            return _processos.Count;
        }

        public string ObterResumoProcessos()
        {
            string resumo = "===== PROCESSOS =====\n";
            foreach (var processo in _processos.Values.OrderBy(p => p.PID))
            {
                resumo += $"{processo}\n";
            }
            resumo += $"Total: {_processos.Count} processos\n";
            resumo += "=====================\n";
            return resumo;
        }
    }
}