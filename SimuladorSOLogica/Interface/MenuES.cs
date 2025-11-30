using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Interface
{
    public class MenuES
    {
        private Kernel _kernel;

        public MenuES(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("------------------- DISPOSITIVOS I/O -------------------");
            Console.WriteLine("1) Listar dispositivos (bloco / caractere)");
            Console.WriteLine("2) Criar requisição bloqueante");
            Console.WriteLine("3) Criar requisição não bloqueante");
            Console.WriteLine("4) Processar 1 tick de I/O");
            Console.WriteLine("5) Ver filas de dispositivos");
            Console.WriteLine("6) Ver interrupções geradas");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("---------------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        public void ProcessarOpcao(int opcao)
        {
            try
            {
                switch (opcao)
                {
                    case 1:
                        ListarDispositivos();
                        break;
                    case 2:
                        CriarRequisicaoBloqueante();
                        break;
                    case 3:
                        CriarRequisicaoNaoBloqueante();
                        break;
                    case 4:
                        ProcessarTick();
                        break;
                    case 5:
                        VerFilas();
                        break;
                    case 6:
                        VerInterrupcoes();
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

        private void ListarDispositivos()
        {
            Console.WriteLine(_kernel.GerenciadorES.ObterResumoDispositivos());
        }

        private void CriarRequisicaoBloqueante()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            Console.Write("Digite o nome do dispositivo (DISCO, TECLADO, IMPRESSORA, REDE): ");
            string dispositivo = Console.ReadLine();

            Console.Write("Digite o tempo de operação: ");
            int tempo = int.Parse(Console.ReadLine());

            _kernel.GerenciadorES.CriarRequisicao(pid, dispositivo, tempo, true);
            Console.WriteLine("Requisição bloqueante criada");
        }

        private void CriarRequisicaoNaoBloqueante()
        {
            Console.Write("Digite o PID do processo: ");
            int pid = int.Parse(Console.ReadLine());

            Console.Write("Digite o nome do dispositivo (DISCO, TECLADO, IMPRESSORA, REDE): ");
            string dispositivo = Console.ReadLine();

            Console.Write("Digite o tempo de operação: ");
            int tempo = int.Parse(Console.ReadLine());

            _kernel.GerenciadorES.CriarRequisicao(pid, dispositivo, tempo, false);
            Console.WriteLine("Requisição não bloqueante criada");
        }

        private void ProcessarTick()
        {
            _kernel.GerenciadorES.ProcessarTick();
            Console.WriteLine("Tick de I/O processado");
        }

        private void VerFilas()
        {
            Console.WriteLine(_kernel.GerenciadorES.ObterFilasDispositivos());
        }

        private void VerInterrupcoes()
        {
            var interrupcoes = _kernel.GerenciadorES.ListarInterrupcoes();
            Console.WriteLine("===== INTERRUPÇÕES =====");
            foreach (var interrupcao in interrupcoes)
            {
                Console.WriteLine(interrupcao);
            }
            Console.WriteLine($"Total: {interrupcoes.Count} interrupções");
            Console.WriteLine("========================");
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
