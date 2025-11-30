using SimuladorSOLogica.Nucleo;
using SimuladorSOLogica.Utilitarios;
using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Threads
{
    public class GerenciadorDeThreads
    {
        private Kernel _kernel;
        private Dictionary<int, ThreadSimulada> _threads;
        private GeradorIDs _geradorIDs;

        public GerenciadorDeThreads(Kernel kernel)
        {
            _kernel = kernel;
            _threads = new Dictionary<int, ThreadSimulada>();
            _geradorIDs = new GeradorIDs();
        }

        public ThreadSimulada CriarThread(int pid, int prioridade = 0)
        {
            // Verificar se o processo existe
            Processo processo = _kernel.GerenciadorDeProcessos.ObterProcesso(pid);

            int tid = _geradorIDs.GerarProximoID();
            long tempoChegada = _kernel.Relogio.TempoAtual;

            ThreadSimulada thread = new ThreadSimulada(tid, pid, prioridade, tempoChegada);
            thread.MudarEstado(EstadoThread.Nova);

            _threads[tid] = thread;
            processo.AdicionarThread(tid);

            _kernel.RegistradorDeEventos.Registrar(
                $"Thread criada: TID={tid}, PID={pid}, Prioridade={prioridade}");

            return thread;
        }

        public void RemoverThread(int tid)
        {
            if (!_threads.ContainsKey(tid))
                throw new ArgumentException($"Thread {tid} não existe.");

            ThreadSimulada thread = _threads[tid];
            Processo processo = _kernel.GerenciadorDeProcessos.ObterProcesso(thread.PID);
            processo.RemoverThread(tid);

            _threads.Remove(tid);
            _kernel.RegistradorDeEventos.Registrar($"Thread removida: TID={tid}");
        }

        public void FinalizarThread(int tid)
        {
            if (!_threads.ContainsKey(tid))
                throw new ArgumentException($"Thread {tid} não existe.");

            ThreadSimulada thread = _threads[tid];
            thread.MudarEstado(EstadoThread.Finalizada);
            thread.DefinirTempoFinalizacao(_kernel.Relogio.TempoAtual);

            _kernel.RegistradorDeEventos.Registrar($"Thread finalizada: TID={tid}");
        }

        public void MudarEstadoThread(int tid, EstadoThread novoEstado)
        {
            if (!_threads.ContainsKey(tid))
                throw new ArgumentException($"Thread {tid} não existe.");

            ThreadSimulada thread = _threads[tid];
            EstadoThread estadoAnterior = thread.Estado;
            thread.MudarEstado(novoEstado);

            _kernel.RegistradorDeEventos.Registrar(
                $"Thread {tid}: {estadoAnterior} -> {novoEstado}");
        }

        public ThreadSimulada ObterThread(int tid)
        {
            if (!_threads.ContainsKey(tid))
                throw new ArgumentException($"Thread {tid} não existe.");

            return _threads[tid];
        }

        public List<ThreadSimulada> ListarThreads()
        {
            return _threads.Values.ToList();
        }

        public List<ThreadSimulada> ListarThreadsPorProcesso(int pid)
        {
            return _threads.Values
                .Where(t => t.PID == pid)
                .ToList();
        }

        public List<ThreadSimulada> ListarThreadsPorEstado(EstadoThread estado)
        {
            return _threads.Values
                .Where(t => t.Estado == estado)
                .ToList();
        }

        public int ContarThreads()
        {
            return _threads.Count;
        }

        public string ObterResumoThreads()
        {
            string resumo = "===== THREADS =====\n";
            foreach (var thread in _threads.Values.OrderBy(t => t.TID))
            {
                resumo += $"{thread}\n";
            }
            resumo += $"Total: {_threads.Count} threads\n";
            resumo += "===================\n";
            return resumo;
        }
    }
}
