using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Interface
{
    public class MenuArquivo
    {
        private Kernel _kernel;

        public MenuArquivo(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("---------------- SISTEMA DE ARQUIVOS ----------------");
            Console.WriteLine("1) Listar diretório atual");
            Console.WriteLine("2) Criar arquivo");
            Console.WriteLine("3) Criar diretório");
            Console.WriteLine("4) Abrir arquivo");
            Console.WriteLine("5) Ler arquivo");
            Console.WriteLine("6) Escrever arquivo");
            Console.WriteLine("7) Fechar arquivo");
            Console.WriteLine("8) Apagar arquivo");
            Console.WriteLine("9) Mudar diretório");
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
                        ListarDiretorio();
                        break;
                    case 2:
                        CriarArquivo();
                        break;
                    case 3:
                        CriarDiretorio();
                        break;
                    case 4:
                        AbrirArquivo();
                        break;
                    case 5:
                        LerArquivo();
                        break;
                    case 6:
                        EscreverArquivo();
                        break;
                    case 7:
                        FecharArquivo();
                        break;
                    case 8:
                        ApagarArquivo();
                        break;
                    case 9:
                        MudarDiretorio();
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

        private void ListarDiretorio()
        {
            var entradas = _kernel.SistemaDeArquivos.ListarDiretorioAtual();
            Console.WriteLine("===== DIRETÓRIO ATUAL =====");
            foreach (var entrada in entradas)
            {
                Console.WriteLine(entrada);
            }
            Console.WriteLine($"Total: {entradas.Count} entradas");
            Console.WriteLine("===========================");
        }

        private void CriarArquivo()
        {
            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            _kernel.SistemaDeArquivos.CriarArquivo(caminho);
            Console.WriteLine($"Arquivo criado: {caminho}");
        }

        private void CriarDiretorio()
        {
            Console.Write("Digite o caminho do diretório: ");
            string caminho = Console.ReadLine();

            _kernel.SistemaDeArquivos.CriarDiretorio(caminho);
            Console.WriteLine($"Diretório criado: {caminho}");
        }

        private void AbrirArquivo()
        {
            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            Console.Write("Digite o modo (r/w/rw): ");
            string modo = Console.ReadLine();

            int manipulador = _kernel.SistemaDeArquivos.AbrirArquivo(caminho, modo);
            Console.WriteLine($"Arquivo aberto: {caminho} (Manipulador={manipulador})");
        }

        private void LerArquivo()
        {
            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            Console.Write("Digite o tamanho a ler: ");
            int tamanho = int.Parse(Console.ReadLine());

            byte[] dados = _kernel.SistemaDeArquivos.LerArquivo(caminho, tamanho);
            Console.WriteLine($"Lidos {dados.Length} bytes de {caminho}");
        }

        private void EscreverArquivo()
        {
            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            Console.Write("Digite o tamanho a escrever: ");
            int tamanho = int.Parse(Console.ReadLine());

            byte[] dados = new byte[tamanho];
            _kernel.SistemaDeArquivos.EscreverArquivo(caminho, dados);
            Console.WriteLine($"Escritos {tamanho} bytes em {caminho}");
        }

        private void FecharArquivo()
        {
            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            _kernel.SistemaDeArquivos.FecharArquivo(caminho);
            Console.WriteLine($"Arquivo fechado: {caminho}");
        }

        private void ApagarArquivo()
        {
            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            _kernel.SistemaDeArquivos.ApagarArquivo(caminho);
            Console.WriteLine($"Arquivo apagado: {caminho}");
        }

        private void MudarDiretorio()
        {
            Console.Write("Digite o caminho do diretório: ");
            string caminho = Console.ReadLine();

            _kernel.SistemaDeArquivos.MudarDiretorio(caminho);
            Console.WriteLine($"Diretório alterado para: {caminho}");
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
