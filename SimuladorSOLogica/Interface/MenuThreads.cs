using SimuladorSOLogica.Nucleo;
using SimuladorSOLogica.Threads;

namespace SimuladorSOLogica.Interface
{
    public class MenuThreads
    {
        private Kernel _kernel;

        public MenuThreads(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("------------- GERENCIAR THREADS ----------------");
            Console.WriteLine("1) Criar thread em um processo");
            Console.WriteLine("2) Listar threads");
            Console.WriteLine("3) Mudar estado da thread");
            Console.WriteLine("4) Remover thread");
            Console.WriteLine("5) Ver TCB completo");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("-------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        public void ProcessarOpcao(int opcao)
        {
            try
            {
                switch (opcao)
                {
                    case 1:
                        CriarThread();
                        break;
                    case 2:
                        ListarThreads();
                        break;
                    case 3:
                        MudarEstadoThread();
                        break;
                    case 4:
                        RemoverThread();
                        break;
                    case 5:
                        VerTCB();
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        private void CriarThread()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            Console.Write("Digite a prioridade da thread (0-5): ");
            int prioridade = int.Parse(Console.ReadLine());

            var thread = _kernel.GerenciadorDeThreads.CriarThread(pid, prioridade);
            Console.WriteLine($"Thread criada com TID={thread.TID} no processo PID={pid}");
        }

        private void ListarThreads()
        {
            Console.WriteLine(_kernel.GerenciadorDeThreads.ObterResumoThreads());
        }

        private void MudarEstadoThread()
        {
            Console.Write("Digite o TID da thread: ");
            int tid = int.Parse(Console.ReadLine());

            Console.WriteLine("Estados disponíveis:");
            Console.WriteLine("1) Pronta");
            Console.WriteLine("2) Bloqueada");
            Console.WriteLine("3) Finalizada");
            Console.Write("Escolha o novo estado: ");
            int estado = int.Parse(Console.ReadLine());

            EstadoThread novoEstado = EstadoThread.Pronta;
            switch (estado)
            {
                case 1: novoEstado = EstadoThread.Pronta; break;
                case 2: novoEstado = EstadoThread.Bloqueada; break;
                case 3: novoEstado = EstadoThread.Finalizada; break;
            }

            _kernel.GerenciadorDeThreads.MudarEstadoThread(tid, novoEstado);
            Console.WriteLine($"Estado da thread {tid} alterado para {novoEstado}");
        }

        private void RemoverThread()
        {
            Console.Write("Digite o TID da thread: ");
            int tid = int.Parse(Console.ReadLine());

            _kernel.GerenciadorDeThreads.RemoverThread(tid);
            Console.WriteLine($"Thread {tid} removida");
        }

        private void VerTCB()
        {
            Console.Write("Digite o TID da thread: ");
            int tid = int.Parse(Console.ReadLine());

            var thread = _kernel.GerenciadorDeThreads.ObterThread(tid);
            Console.WriteLine(thread.TCB.ObterDetalhesCompletos());
        }

        public void Executar()
        {
            int opcao;
            do
            {
                Exibir();
                string entrada = Console.ReadLine();
                if (int.TryParse(entrada, out opcao))
                {
                    ProcessarOpcao(opcao);
                }
                else
                {
                    Console.WriteLine("Entrada inválida!");
                }
            } while (opcao != 0);
        }
    }
}