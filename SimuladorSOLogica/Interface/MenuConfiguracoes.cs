using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Interface
{
    public class MenuConfiguracoes
    {
        private Kernel _kernel;

        public MenuConfiguracoes(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("------------------ CONFIGURAÇÕES ----------------------");
            Console.WriteLine("1) Definir semente determinística");
            Console.WriteLine("2) Configurar tamanho de página");
            Console.WriteLine("3) Configurar número de molduras");
            Console.WriteLine("4) Configurar tempos de dispositivos");
            Console.WriteLine("5) Carregar workload");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("------------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        public void ProcessarOpcao(int opcao)
        {
            try
            {
                switch (opcao)
                {
                    case 1:
                        DefinirSemente();
                        break;
                    case 2:
                        ConfigurarTamanhoPagina();
                        break;
                    case 3:
                        ConfigurarMolduras();
                        break;
                    case 4:
                        ConfigurarTemposDispositivos();
                        break;
                    case 5:
                        CarregarWorkload();
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

        private void DefinirSemente()
        {
            Console.Write("Digite a semente: ");
            int semente = int.Parse(Console.ReadLine());

            _kernel.Configuracoes.DefinirSemente(semente);
            Console.WriteLine($"Semente definida para: {semente}");
        }

        private void ConfigurarTamanhoPagina()
        {
            Console.Write("Digite o tamanho da página (bytes): ");
            int tamanho = int.Parse(Console.ReadLine());

            _kernel.Configuracoes.TamanhoPagina = tamanho;
            Console.WriteLine($"Tamanho de página configurado para: {tamanho} bytes");
        }

        private void ConfigurarMolduras()
        {
            Console.Write("Digite o número de molduras: ");
            int molduras = int.Parse(Console.ReadLine());

            _kernel.Configuracoes.NumeroMolduras = molduras;
            _kernel.GerenciadorDeMemoria.InicializarMolduras(molduras);
            Console.WriteLine($"Número de molduras configurado para: {molduras}");
        }

        private void ConfigurarTemposDispositivos()
        {
            Console.WriteLine("Dispositivos: DISCO, TECLADO, IMPRESSORA, REDE");
            Console.Write("Digite o nome do dispositivo: ");
            string dispositivo = Console.ReadLine();

            Console.Write("Digite o tempo de operação: ");
            int tempo = int.Parse(Console.ReadLine());

            _kernel.Configuracoes.ConfigurarTempoDispositivo(dispositivo, tempo);
            Console.WriteLine($"Tempo do dispositivo {dispositivo} configurado para: {tempo}");
        }

        private void CarregarWorkload()
        {
            Console.Write("Digite o caminho do arquivo de workload: ");
            string caminho = Console.ReadLine();

            CarregadorWorkload carregador = new CarregadorWorkload(_kernel);
            carregador.CarregarArquivo(caminho);
            Console.WriteLine("Workload carregado com sucesso!");
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
