using SimuladorSOLogica.Nucleo;
using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Interface
{
    public class MenuProcessos
    {
        private Kernel _kernel;

        public MenuProcessos(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("------------- GERENCIAR PROCESSOS -------------");
            Console.WriteLine("1) Criar novo processo");
            Console.WriteLine("2) Listar processos (e estados)");
            Console.WriteLine("3) Mudar estado de processo (bloquear, prontificar, finalizar)");
            Console.WriteLine("4) Remover processo");
            Console.WriteLine("5) Ver PCB completo de um processo");
            Console.WriteLine("6) Abrir arquivo dentro de um processo");
            Console.WriteLine("7) Fechar arquivo dentro de um processo");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        public void ProcessarOpcao(int opcao)
        {
            try
            {
                switch (opcao)
                {
                    case 1:
                        CriarProcesso();
                        break;
                    case 2:
                        ListarProcessos();
                        break;
                    case 3:
                        MudarEstadoProcesso();
                        break;
                    case 4:
                        RemoverProcesso();
                        break;
                    case 5:
                        VerPCB();
                        break;
                    case 6:
                        AbrirArquivo();
                        break;
                    case 7:
                        FecharArquivo();
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

        private void CriarProcesso()
        {
            Console.Write("Digite a prioridade do processo (0-5): ");
            int prioridade = int.Parse(Console.ReadLine());

            var processo = _kernel.GerenciadorDeProcessos.CriarProcesso(prioridade);
            _kernel.Escalonador.AdicionarProcessoPronto(processo);

            Console.WriteLine($"Processo criado com PID={processo.PID}");
        }

        private void ListarProcessos()
        {
            Console.WriteLine(_kernel.GerenciadorDeProcessos.ObterResumoProcessos());
        }

        private void MudarEstadoProcesso()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            Console.WriteLine("Estados disponíveis:");
            Console.WriteLine("1) Pronto");
            Console.WriteLine("2) Bloqueado");
            Console.WriteLine("3) Finalizado");
            Console.Write("Escolha o novo estado: ");
            int estado = int.Parse(Console.ReadLine());

            EstadoProcesso novoEstado = EstadoProcesso.Pronto;
            switch (estado)
            {
                case 1: novoEstado = EstadoProcesso.Pronto; break;
                case 2: novoEstado = EstadoProcesso.Bloqueado; break;
                case 3: novoEstado = EstadoProcesso.Finalizado; break;
            }

            _kernel.GerenciadorDeProcessos.MudarEstadoProcesso(pid, novoEstado);
            Console.WriteLine($"Estado do processo {pid} alterado para {novoEstado}");
        }

        private void RemoverProcesso()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            _kernel.GerenciadorDeProcessos.RemoverProcesso(pid);
            Console.WriteLine($"Processo {pid} removido");
        }

        private void VerPCB()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            var processo = _kernel.GerenciadorDeProcessos.ObterProcesso(pid);
            Console.WriteLine(processo.PCB.ObterDetalhesCompletos());
        }

        private void AbrirArquivo()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            var processo = _kernel.GerenciadorDeProcessos.ObterProcesso(pid);
            processo.AbrirArquivo(caminho);

            Console.WriteLine($"Arquivo {caminho} aberto pelo processo {pid}");
        }

        private void FecharArquivo()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            var processo = _kernel.GerenciadorDeProcessos.ObterProcesso(pid);
            processo.FecharArquivo(caminho);

            Console.WriteLine($"Arquivo {caminho} fechado pelo processo {pid}");
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