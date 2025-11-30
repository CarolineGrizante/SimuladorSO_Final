using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Interface
{
    public class MenuEscalonamento
    {
        private Kernel _kernel;

        public MenuEscalonamento(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("----------------- ESCALONADOR ------------------");
            Console.WriteLine("1) Trocar algoritmo de escalonamento");
            Console.WriteLine("    a) FCFS");
            Console.WriteLine("    b) Round Robin");
            Console.WriteLine("    c) Prioridades (preemptivo)");
            Console.WriteLine("    d) Prioridades (não preemptivo)");
            Console.WriteLine("2) Configurar quantum");
            Console.WriteLine("3) Executar 1 ciclo de CPU");
            Console.WriteLine("4) Executar até todos finalizarem");
            Console.WriteLine("5) Mostrar fila de prontos");
            Console.WriteLine("6) Ver contagem de trocas de contexto");
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
                        TrocarAlgoritmo();
                        break;
                    case 2:
                        ConfigurarQuantum();
                        break;
                    case 3:
                        ExecutarCiclo();
                        break;
                    case 4:
                        ExecutarAteCompletar();
                        break;
                    case 5:
                        MostrarFilaProntos();
                        break;
                    case 6:
                        VerTrocasContexto();
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

        private void TrocarAlgoritmo()
        {
            Console.WriteLine("Algoritmos disponíveis:");
            Console.WriteLine("1) FCFS");
            Console.WriteLine("2) Round Robin");
            Console.WriteLine("3) Prioridade (Preemptivo)");
            Console.WriteLine("4) Prioridade (Não Preemptivo)");
            Console.Write("Escolha o algoritmo: ");
            int escolha = int.Parse(Console.ReadLine());

            string algoritmo = "";
            switch (escolha)
            {
                case 1: algoritmo = "FCFS"; break;
                case 2: algoritmo = "RR"; break;
                case 3: algoritmo = "PRIORIDADE_PREEMPTIVO"; break;
                case 4: algoritmo = "PRIORIDADE_NAO_PREEMPTIVO"; break;
            }

            _kernel.Escalonador.TrocarAlgoritmo(algoritmo);
            Console.WriteLine($"Algoritmo alterado para: {_kernel.Escalonador.ObterNomeAlgoritmo()}");
        }

        private void ConfigurarQuantum()
        {
            Console.Write("Digite o valor do quantum: ");
            int quantum = int.Parse(Console.ReadLine());

            _kernel.Escalonador.ConfigurarQuantum(quantum);
            Console.WriteLine($"Quantum configurado para: {quantum}");
        }

        private void ExecutarCiclo()
        {
            _kernel.ExecutarCiclo();
            Console.WriteLine("Ciclo de CPU executado");
            Console.WriteLine(_kernel.Escalonador.ObterResumo());
        }

        private void ExecutarAteCompletar()
        {
            Console.WriteLine("Executando até todos os processos finalizarem...");
            _kernel.ExecutarAteCompletar();
            Console.WriteLine("Execução completa!");
        }

        private void MostrarFilaProntos()
        {
            var fila = _kernel.Escalonador.ObterFilaProntos();
            Console.WriteLine(fila.ObterResumo());
        }

        private void VerTrocasContexto()
        {
            var trocas = _kernel.Escalonador.ObterTrocaDeContexto();
            Console.WriteLine(trocas.ObterEstatisticas());
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