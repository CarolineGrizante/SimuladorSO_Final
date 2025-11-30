using System.Windows;
using Microsoft.Win32;
using SimuladorSOLogica.Nucleo;

namespace SimuladorSOInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Kernel _kernel;

        public MainWindow()
        {
            InitializeComponent();
            InicializarSimulador();
        }

        private void InicializarSimulador()
        {
            _kernel = new Kernel();
            _kernel.Inicializar();
            AtualizarLog("Simulador inicializado com sucesso.");
            AtualizarInterface();
        }

        private void AtualizarLog(string mensagem)
        {
            txtLog.Text += $"[{DateTime.Now:HH:mm:ss}] {mensagem}\n";
            txtLog.ScrollToEnd();
        }

        private void AtualizarInterface()
        {
            AtualizarProcessos();
            AtualizarEscalonador();
            AtualizarMemoria();
            AtualizarDispositivos();
            AtualizarArquivos();
        }

        private void AtualizarProcessos()
        {
            dgProcessos.ItemsSource = null;
            dgProcessos.ItemsSource = _kernel.GerenciadorDeProcessos.ListarProcessos();
        }

        private void AtualizarEscalonador()
        {
            txtEscalonadorInfo.Text = $"Algoritmo: {_kernel.Escalonador.ObterNomeAlgoritmo()}";

            var processoAtual = _kernel.Escalonador.ObterProcessoAtual();
            txtProcessoAtual.Text = processoAtual != null
                ? $"Processo Atual: PID {processoAtual.PID}"
                : "Processo Atual: Nenhum";

            var trocas = _kernel.Escalonador.ObterTrocaDeContexto();
            txtTrocasContexto.Text = $"Trocas de Contexto: {trocas.ContagemTrocas}";

            lstFilaProntos.Items.Clear();
            foreach (var processo in _kernel.Escalonador.ObterFilaProntos().ObterTodos())
            {
                lstFilaProntos.Items.Add(processo.ToString());
            }
        }

        private void AtualizarMemoria()
        {
            txtMemoriaInfo.Text = $"Molduras: {_kernel.Configuracoes.NumeroMolduras}, " +
                                  $"Tamanho Página: {_kernel.Configuracoes.TamanhoPagina} bytes";

            txtFaltasPagina.Text = $"Faltas de Página: {_kernel.GerenciadorDeMemoria.FaltasDePagina}";

            var tlb = _kernel.GerenciadorDeMemoria.ObterTLB();
            txtTLBInfo.Text = tlb != null
                ? $"TLB: Ativa - {tlb.ObterEstatisticas()}"
                : "TLB: Desativada";

            var tabelaMolduras = _kernel.GerenciadorDeMemoria.ObterTabelaMolduras();
            if (tabelaMolduras != null)
            {
                dgMolduras.ItemsSource = null;
                dgMolduras.ItemsSource = tabelaMolduras.ObterTodasMolduras();
            }
        }

        private void AtualizarDispositivos()
        {
            dgDispositivos.ItemsSource = null;
            dgDispositivos.ItemsSource = _kernel.GerenciadorES.ListarDispositivos();

            lstInterrupcoes.Items.Clear();
            foreach (var interrupcao in _kernel.GerenciadorES.ListarInterrupcoes())
            {
                lstInterrupcoes.Items.Add(interrupcao.ToString());
            }
        }

        private void AtualizarArquivos()
        {
            lstArquivos.Items.Clear();
            foreach (var entrada in _kernel.SistemaDeArquivos.ListarDiretorioAtual())
            {
                lstArquivos.Items.Add(entrada.ToString());
            }
        }

        // Event Handlers - Arquivo
        private void CarregarWorkload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Arquivos de Texto (*.txt)|*.txt|Todos os Arquivos (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CarregadorWorkload carregador = new CarregadorWorkload(_kernel);
                    carregador.CarregarArquivo(dialog.FileName);
                    AtualizarLog($"Workload carregado: {dialog.FileName}");
                    AtualizarInterface();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar workload: {ex.Message}", "Erro",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportarLog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Arquivos de Texto (*.txt)|*.txt";
            dialog.FileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _kernel.RegistradorDeEventos.ExportarParaArquivo(dialog.FileName);
                    AtualizarLog($"Log exportado: {dialog.FileName}");
                    MessageBox.Show("Log exportado com sucesso!", "Sucesso",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao exportar log: {ex.Message}", "Erro",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Event Handlers - Simulação
        private void IniciarSimulador_Click(object sender, RoutedEventArgs e)
        {
            InicializarSimulador();
        }

        private void ExecutarCiclo_Click(object sender, RoutedEventArgs e)
        {
            _kernel.ExecutarCiclo();
            AtualizarLog($"Ciclo executado - Tempo: {_kernel.Relogio.TempoAtual}");
            AtualizarInterface();
        }

        private void ExecutarAteCompletar_Click(object sender, RoutedEventArgs e)
        {
            _kernel.ExecutarAteCompletar();
            AtualizarLog("Execução completa - Todos os processos finalizados");
            AtualizarInterface();
        }

        private void Resetar_Click(object sender, RoutedEventArgs e)
        {
            InicializarSimulador();
            txtLog.Clear();
            AtualizarLog("Simulador resetado");
        }

        // Event Handlers - Configurações
        private void ConfigurarEscalonador_Click(object sender, RoutedEventArgs e)
        {
            // Diálogo simples para escolher algoritmo
            var resultado = MessageBox.Show(
                "Escolha o algoritmo:\n\n1 - FCFS\n2 - Round Robin\n3 - Prioridade Preemptivo\n4 - Prioridade Não Preemptivo",
                "Configurar Escalonador", MessageBoxButton.OK);

            // Aqui você pode implementar um diálogo customizado
        }

        private void ConfigurarQuantum_Click(object sender, RoutedEventArgs e)
        {
            // Diálogo para configurar quantum
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite o valor do quantum:", "Configurar Quantum", "4");

            if (int.TryParse(input, out int quantum))
            {
                _kernel.Escalonador.ConfigurarQuantum(quantum);
                AtualizarLog($"Quantum configurado para: {quantum}");
                AtualizarInterface();
            }
        }

        private void ConfigurarMemoria_Click(object sender, RoutedEventArgs e)
        {
            // Diálogo para configurar memória
            MessageBox.Show("Configuração de memória", "Configurar");
        }

        // Event Handlers - Visualizar
        private void VisualizarProcessos_Click(object sender, RoutedEventArgs e)
        {
            AtualizarProcessos();
            AtualizarLog("Visualização de processos atualizada");
        }

        private void VisualizarThreads_Click(object sender, RoutedEventArgs e)
        {
            AtualizarLog("Visualização de threads");
        }

        private void VisualizarMemoria_Click(object sender, RoutedEventArgs e)
        {
            AtualizarMemoria();
            AtualizarLog("Visualização de memória atualizada");
        }

        private void VisualizarIO_Click(object sender, RoutedEventArgs e)
        {
            AtualizarDispositivos();
            AtualizarLog("Visualização de I/O atualizada");
        }

        private void VisualizarArquivos_Click(object sender, RoutedEventArgs e)
        {
            AtualizarArquivos();
            AtualizarLog("Visualização de arquivos atualizada");
        }

        // Event Handlers - Métricas
        private void GerarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            string relatorio = _kernel.GerenciadorDeMetricas.GerarRelatorioCompleto();

            RelatorioWindow relatorioWindow = new RelatorioWindow(relatorio);
            relatorioWindow.ShowDialog();

            AtualizarLog("Relatório de métricas gerado");
        }

        private void VerEstatisticas_Click(object sender, RoutedEventArgs e)
        {
            string stats = _kernel.GerenciadorDeMetricas.ObterResumoMetricas();
            MessageBox.Show(stats, "Estatísticas", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Event Handlers - Processos
        private void CriarProcesso_Click(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite a prioridade do processo (0-5):", "Criar Processo", "3");

            if (int.TryParse(input, out int prioridade))
            {
                var processo = _kernel.GerenciadorDeProcessos.CriarProcesso(prioridade);
                _kernel.Escalonador.AdicionarProcessoPronto(processo);
                AtualizarLog($"Processo criado: PID={processo.PID}");
                AtualizarProcessos();
            }
        }

        private void FinalizarProcesso_Click(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite o PID do processo:", "Finalizar Processo", "");

            if (int.TryParse(input, out int pid))
            {
                _kernel.GerenciadorDeProcessos.FinalizarProcesso(pid);
                AtualizarLog($"Processo finalizado: PID={pid}");
                AtualizarProcessos();
            }
        }

        private void AtualizarProcessos_Click(object sender, RoutedEventArgs e)
        {
            AtualizarProcessos();
        }

        // Event Handlers - Sistema de Arquivos
        private void CriarArquivo_Click(object sender, RoutedEventArgs e)
        {
            string caminho = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite o caminho do arquivo:", "Criar Arquivo", "/arquivo.txt");

            if (!string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaDeArquivos.CriarArquivo(caminho);
                AtualizarLog($"Arquivo criado: {caminho}");
                AtualizarArquivos();
            }
        }

        private void CriarDiretorio_Click(object sender, RoutedEventArgs e)
        {
            string caminho = Microsoft.VisualBasic.Interaction.InputBox(
                "Digite o caminho do diretório:", "Criar Diretório", "/pasta");

            if (!string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaDeArquivos.CriarDiretorio(caminho);
                AtualizarLog($"Diretório criado: {caminho}");
                AtualizarArquivos();
            }
        }

        private void AtualizarArquivos_Click(object sender, RoutedEventArgs e)
        {
            AtualizarArquivos();
        }

        // Event Handlers - Log
        private void LimparLog_Click(object sender, RoutedEventArgs e)
        {
            txtLog.Clear();
            _kernel.RegistradorDeEventos.LimparEventos();
        }
    }
}