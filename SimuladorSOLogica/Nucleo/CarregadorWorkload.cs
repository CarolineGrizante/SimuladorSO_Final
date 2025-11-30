namespace SimuladorSOLogica.Nucleo
{
    public class CarregadorWorkload
    {
        private Kernel _kernel;
        private Dictionary<string, int> _processosPorNome;

        public CarregadorWorkload(Kernel kernel)
        {
            _kernel = kernel;
            _processosPorNome = new Dictionary<string, int>();
        }

        public void CarregarArquivo(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException($"Arquivo não encontrado: {caminhoArquivo}");

            string[] linhas = File.ReadAllLines(caminhoArquivo);

            foreach (string linha in linhas)
            {
                ProcessarLinha(linha.Trim());
            }

            _kernel.RegistradorDeEventos.Registrar($"Workload carregado de: {caminhoArquivo}");
        }

        private void ProcessarLinha(string linha)
        {
            // Ignorar linhas vazias e comentários
            if (string.IsNullOrWhiteSpace(linha) || linha.StartsWith("#"))
                return;

            string[] partes = linha.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 0) return;

            string comando = partes[0].ToUpper();

            try
            {
                switch (comando)
                {
                    case "SET_SEED":
                        _kernel.Configuracoes.DefinirSemente(int.Parse(partes[1]));
                        break;

                    case "SET_QUANTUM":
                        _kernel.Configuracoes.Quantum = int.Parse(partes[1]);
                        break;

                    case "SET_ESCALONADOR":
                        _kernel.Escalonador.TrocarAlgoritmo(partes[1]);
                        break;

                    case "SET_TAMANHO_PAGINA":
                        _kernel.Configuracoes.TamanhoPagina = int.Parse(partes[1]);
                        break;

                    case "SET_FRAMES":
                        _kernel.Configuracoes.NumeroMolduras = int.Parse(partes[1]);
                        _kernel.GerenciadorDeMemoria.InicializarMolduras(_kernel.Configuracoes.NumeroMolduras);
                        break;

                    case "CRIAR_PROCESSO":
                        string nomeProcesso = partes[1];
                        int prioridade = int.Parse(partes[2]);
                        var processo = _kernel.GerenciadorDeProcessos.CriarProcesso(prioridade);
                        _processosPorNome[nomeProcesso] = processo.PID;
                        break;

                    case "CRIAR_THREAD":
                        int pidThread = _processosPorNome[partes[1]];
                        _kernel.GerenciadorDeThreads.CriarThread(pidThread);
                        break;

                    case "MEM_ALOCAR":
                        int pidMem = _processosPorNome[partes[1]];
                        int tamanho = int.Parse(partes[2]);
                        _kernel.GerenciadorDeMemoria.AlocarMemoria(pidMem, tamanho);
                        break;

                    case "MEM_ACESSO":
                        int pidAcesso = _processosPorNome[partes[1]];
                        int endereco = Convert.ToInt32(partes[2], 16);
                        _kernel.GerenciadorDeMemoria.AcessarPagina(pidAcesso, endereco);
                        break;

                    case "CPU_TICK":
                        int numTicks = int.Parse(partes[1]);
                        for (int i = 0; i < numTicks; i++)
                            _kernel.ExecutarCiclo();
                        break;

                    case "IO_REQ":
                        int pidIO = _processosPorNome[partes[1]];
                        string dispositivo = partes[2];
                        int tempo = int.Parse(partes[3]);
                        _kernel.GerenciadorES.CriarRequisicao(pidIO, dispositivo, tempo, true);
                        break;

                    case "IO_TICK":
                        int ticksIO = int.Parse(partes[1]);
                        for (int i = 0; i < ticksIO; i++)
                            _kernel.GerenciadorES.ProcessarTick();
                        break;

                    case "ARQ_CRIAR":
                        _kernel.SistemaDeArquivos.CriarArquivo(partes[1]);
                        break;

                    case "ARQ_ESCREVER":
                        int pidEscrita = _processosPorNome[partes[1]];
                        string caminhoEscrita = partes[2];
                        int tamanhoEscrita = int.Parse(partes[3]);
                        _kernel.SistemaDeArquivos.EscreverArquivo(caminhoEscrita, new byte[tamanhoEscrita]);
                        break;

                    case "ARQ_LER":
                        int pidLeitura = _processosPorNome[partes[1]];
                        string caminhoLeitura = partes[2];
                        int tamanhoLeitura = int.Parse(partes[3]);
                        _kernel.SistemaDeArquivos.LerArquivo(caminhoLeitura, tamanhoLeitura);
                        break;

                    case "ARQ_ABRIR":
                        int pidAbrir = _processosPorNome[partes[1]];
                        _kernel.SistemaDeArquivos.AbrirArquivo(partes[2]);
                        break;

                    case "ARQ_FECHAR":
                        int pidFechar = _processosPorNome[partes[1]];
                        _kernel.SistemaDeArquivos.FecharArquivo(partes[2]);
                        break;

                    case "ARQ_APAGAR":
                        _kernel.SistemaDeArquivos.ApagarArquivo(partes[1]);
                        break;

                    case "FINALIZAR":
                        int pidFinalizar = _processosPorNome[partes[1]];
                        _kernel.GerenciadorDeProcessos.FinalizarProcesso(pidFinalizar);
                        break;

                    case "GERAR_METRICAS":
                        _kernel.GerenciadorDeMetricas.GerarRelatorioCompleto();
                        break;

                    default:
                        _kernel.RegistradorDeEventos.Registrar($"Comando desconhecido: {comando}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _kernel.RegistradorDeEventos.Registrar($"Erro ao processar comando '{linha}': {ex.Message}");
            }
        }
    }
}