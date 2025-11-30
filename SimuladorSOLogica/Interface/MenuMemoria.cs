using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Interface
{
    public class MenuMemoria
    {
        private Kernel _kernel;

        public MenuMemoria(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("------------------ MEMÓRIA ----------------------");
            Console.WriteLine("1) Mostrar tabela de páginas / segmentos");
            Console.WriteLine("2) Alocar memória para processo");
            Console.WriteLine("3) Liberar memória");
            Console.WriteLine("4) Mostrar mapa de molduras");
            Console.WriteLine("5) Ativar/Desativar TLB simulada");
            Console.WriteLine("6) Ver estatísticas de faltas de página");
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
                        MostrarTabelaPaginas();
                        break;
                    case 2:
                        AlocarMemoria();
                        break;
                    case 3:
                        LiberarMemoria();
                        break;
                    case 4:
                        MostrarMapaMolduras();
                        break;
                    case 5:
                        ToggleTLB();
                        break;
                    case 6:
                        VerEstatisticas();
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

        private void MostrarTabelaPaginas()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            var tabela = _kernel.GerenciadorDeMemoria.ObterTabelaPaginas(pid);
            Console.WriteLine(tabela.ObterResumo());
        }

        private void AlocarMemoria()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            Console.Write("Digite o tamanho em bytes: ");
            int tamanho = int.Parse(Console.ReadLine());

            _kernel.GerenciadorDeMemoria.AlocarMemoria(pid, tamanho);
            Console.WriteLine($"Memória alocada para processo {pid}");
        }

        private void LiberarMemoria()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            _kernel.GerenciadorDeMemoria.LiberarMemoria(pid);
            Console.WriteLine($"Memória liberada para processo {pid}");
        }

        private void MostrarMapaMolduras()
        {
            var tabela = _kernel.GerenciadorDeMemoria.ObterTabelaMolduras();
            if (tabela != null)
            {
                Console.WriteLine(tabela.ObterMapaMolduras());
            }
            else
            {
                Console.WriteLine("Memória não inicializada");
            }
        }

        private void ToggleTLB()
        {
            if (_kernel.Configuracoes.UsarTLB)
            {
                _kernel.GerenciadorDeMemoria.DesativarTLB();
                Console.WriteLine("TLB desativada");
            }
            else
            {
                Console.Write("Digite o tamanho da TLB: ");
                int tamanho = int.Parse(Console.ReadLine());
                _kernel.GerenciadorDeMemoria.AtivarTLB(tamanho);
                Console.WriteLine($"TLB ativada com {tamanho} entradas");
            }
        }

        private void VerEstatisticas()
        {
            Console.WriteLine(_kernel.GerenciadorDeMemoria.ObterEstatisticas());
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