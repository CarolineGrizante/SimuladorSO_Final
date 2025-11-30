using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Interface
{
    public class MenuMetricas
    {
        private Kernel _kernel;

        public MenuMetricas(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Exibir()
        {
            Console.WriteLine("------------------ ESTATÍSTICAS ----------------------");
            Console.WriteLine("1) Tempo de retorno por processo");
            Console.WriteLine("2) Tempo de espera em pronto");
            Console.WriteLine("3) Tempo de resposta");
            Console.WriteLine("4) Utilização da CPU");
            Console.WriteLine("5) Utilização por dispositivo");
            Console.WriteLine("6) Throughput");
            Console.WriteLine("7) Número de trocas de contexto");
            Console.WriteLine("8) Sobrecarga total do escalonamento");
            Console.WriteLine("9) Exportar log completo");
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
                        Console.WriteLine($"Tempo médio de retorno: {_kernel.GerenciadorDeMetricas.CalcularTempoMedioRetorno():F2}");
                        break;
                    case 2:
                        Console.WriteLine($"Tempo médio de espera: {_kernel.GerenciadorDeMetricas.CalcularTempoMedioEspera():F2}");
                        break;
                    case 3:
                        Console.WriteLine($"Tempo médio de resposta: {_kernel.GerenciadorDeMetricas.CalcularTempoMedioResposta():F2}");
                        break;
                    case 4:
                        Console.WriteLine($"Utilização da CPU: {_kernel.GerenciadorDeMetricas.CalcularUtilizacaoCPU():P2}");
                        break;
                    case 5:
                        Console.WriteLine("Utilização por dispositivo: (não implementado)");
                        break;
                    case 6:
                        Console.WriteLine($"Throughput: {_kernel.GerenciadorDeMetricas.CalcularThroughput():F4} processos/tick");
                        break;
                    case 7:
                        Console.WriteLine($"Trocas de contexto: {_kernel.Escalonador.ObterTrocaDeContexto().ContagemTrocas}");
                        break;
                    case 8:
                        Console.WriteLine($"Sobrecarga total: {_kernel.Escalonador.ObterTrocaDeContexto().SobrecargaTotal} ticks");
                        break;
                    case 9:
                        ExportarLog();
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

        private void ExportarLog()
        {
            Console.Write("Digite o caminho do arquivo: ");
            string caminho = Console.ReadLine();

            _kernel.RegistradorDeEventos.ExportarParaArquivo(caminho);
            Console.WriteLine($"Log exportado para: {caminho}");
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
