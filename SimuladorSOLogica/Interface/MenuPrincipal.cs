using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Interface
{
    public class MenuPrincipal
    {
        private Kernel _kernel;

        public MenuPrincipal(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("=============== SISTEMA OPERACIONAL – SIMULADOR ===============");
            Console.WriteLine("1) Gerenciar Processos");
            Console.WriteLine("2) Gerenciar Threads");
            Console.WriteLine("3) Escalonador de CPU");
            Console.WriteLine("4) Gerenciamento de Memória");
            Console.WriteLine("5) Entrada e Saída (I/O)");
            Console.WriteLine("6) Sistema de Arquivos");
            Console.WriteLine("7) Estatísticas e Métricas");
            Console.WriteLine("8) Configurações do Simulador");
            Console.WriteLine("0) Sair");
            Console.WriteLine("===============================================================");
            Console.Write("Escolha uma opção: ");
        }

        public void ProcessarOpcao(int opcao)
        {
            switch (opcao)
            {
                case 1:
                    new MenuProcessos(_kernel).Executar();
                    break;
                case 2:
                    new MenuThreads(_kernel).Executar();
                    break;
                case 3:
                    new MenuEscalonamento(_kernel).Executar();
                    break;
                case 4:
                    new MenuMemoria(_kernel).Executar();
                    break;
                case 5:
                    new MenuES(_kernel).Executar();
                    break;
                case 6:
                    new MenuArquivo(_kernel).Executar();
                    break;
                case 7:
                    new MenuMetricas(_kernel).Executar();
                    break;
                case 8:
                    new MenuConfiguracoes(_kernel).Executar();
                    break;
                case 0:
                    Console.WriteLine("Encerrando simulador...");
                    _kernel.Desligar();
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
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
            } while (opcao != 0 && _kernel.EstaExecutando());
        }
    }
}