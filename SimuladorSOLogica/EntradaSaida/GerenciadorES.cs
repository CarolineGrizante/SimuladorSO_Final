using SimuladorSOLogica.Nucleo;
using SimuladorSOLogica.Utilitarios;
using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.EntradaSaida
{
    public class GerenciadorES
    {
        private Kernel _kernel;
        private Dictionary<string, IDispositivo> _dispositivos;
        private Dictionary<string, Queue<RequisicaoES>> _filasPorDispositivo;
        private List<Interrupcao> _interrupcoes;
        private GeradorIDs _geradorIDs;

        public GerenciadorES(Kernel kernel)
        {
            _kernel = kernel;
            _dispositivos = new Dictionary<string, IDispositivo>();
            _filasPorDispositivo = new Dictionary<string, Queue<RequisicaoES>>();
            _interrupcoes = new List<Interrupcao>();
            _geradorIDs = new GeradorIDs();

            // Criar dispositivos padrão
            CriarDispositivo("DISCO", "Bloco", kernel.Configuracoes.TempoDisco);
            CriarDispositivo("TECLADO", "Caractere", kernel.Configuracoes.TempoTeclado);
            CriarDispositivo("IMPRESSORA", "Caractere", kernel.Configuracoes.TempoImpressora);
            CriarDispositivo("REDE", "Bloco", kernel.Configuracoes.TempoRede);
        }

        public void CriarDispositivo(string nome, string tipo, int tempoOperacao)
        {
            IDispositivo dispositivo;

            if (tipo.ToUpper() == "BLOCO")
            {
                dispositivo = new DispositivoDeBloco(nome, tempoOperacao);
            }
            else if (tipo.ToUpper() == "CARACTERE")
            {
                dispositivo = new DispositivoDeCaractere(nome, tempoOperacao);
            }
            else
            {
                throw new ArgumentException($"Tipo de dispositivo desconhecido: {tipo}");
            }

            _dispositivos[nome.ToUpper()] = dispositivo;
            _filasPorDispositivo[nome.ToUpper()] = new Queue<RequisicaoES>();

            _kernel.RegistradorDeEventos.Registrar($"Dispositivo criado: {nome} ({tipo})");
        }

        public void CriarRequisicao(int pid, string nomeDispositivo, int tempo, bool bloqueante)
        {
            nomeDispositivo = nomeDispositivo.ToUpper();

            if (!_dispositivos.ContainsKey(nomeDispositivo))
                throw new ArgumentException($"Dispositivo não encontrado: {nomeDispositivo}");

            int reqID = _geradorIDs.GerarProximoID();
            RequisicaoES requisicao = new RequisicaoES(reqID, pid, nomeDispositivo, tempo, bloqueante);
            requisicao.TempoInicio = _kernel.Relogio.TempoAtual;

            _filasPorDispositivo[nomeDispositivo].Enqueue(requisicao);

            if (bloqueante)
            {
                _kernel.GerenciadorDeProcessos.MudarEstadoProcesso(pid, EstadoProcesso.Bloqueado);
            }

            _kernel.RegistradorDeEventos.Registrar($"Requisição I/O criada: {requisicao}");
        }

        public void ProcessarTick()
        {
            foreach (var nomeDispositivo in _dispositivos.Keys.ToList())
            {
                IDispositivo dispositivo = _dispositivos[nomeDispositivo];
                Queue<RequisicaoES> fila = _filasPorDispositivo[nomeDispositivo];

                // Se dispositivo está livre e há requisições na fila, iniciar próxima
                if (!dispositivo.EstaOcupado && fila.Count > 0)
                {
                    RequisicaoES requisicao = fila.Dequeue();
                    dispositivo.IniciarOperacao(requisicao);
                    _kernel.RegistradorDeEventos.Registrar($"Iniciando I/O: {requisicao}");
                }

                // Processar tick do dispositivo
                if (dispositivo.EstaOcupado)
                {
                    dispositivo.ProcessarTick();

                    // Verificar se operação foi concluída
                    if (dispositivo.OperacaoConcluida())
                    {
                        RequisicaoES requisicao = dispositivo.ObterRequisicaoAtual();
                        requisicao.TempoFim = _kernel.Relogio.TempoAtual;

                        // Gerar interrupção
                        GerarInterrupcao(nomeDispositivo, requisicao.PID, "Operação de I/O concluída");

                        // Se era bloqueante, desbloquear processo
                        if (requisicao.Bloqueante)
                        {
                            _kernel.GerenciadorDeProcessos.MudarEstadoProcesso(
                                requisicao.PID, EstadoProcesso.Pronto);
                            _kernel.Escalonador.AdicionarProcessoPronto(
                                _kernel.GerenciadorDeProcessos.ObterProcesso(requisicao.PID));
                        }

                        _kernel.RegistradorDeEventos.Registrar($"I/O concluído: {requisicao}");

                        // Limpar requisição do dispositivo
                        if (dispositivo is DispositivoDeBloco bloco)
                            bloco.LimparRequisicao();
                        else if (dispositivo is DispositivoDeCaractere caractere)
                            caractere.LimparRequisicao();
                    }
                }
            }
        }

        private void GerarInterrupcao(string dispositivo, int pid, string mensagem)
        {
            int intID = _geradorIDs.GerarProximoID();
            Interrupcao interrupcao = new Interrupcao(
                intID, dispositivo, pid, _kernel.Relogio.TempoAtual, mensagem);

            _interrupcoes.Add(interrupcao);
            _kernel.RegistradorDeEventos.Registrar($"Interrupção gerada: {interrupcao}");
        }

        public List<IDispositivo> ListarDispositivos()
        {
            return _dispositivos.Values.ToList();
        }

        public List<Interrupcao> ListarInterrupcoes()
        {
            return new List<Interrupcao>(_interrupcoes);
        }

        public string ObterFilasDispositivos()
        {
            string resumo = "===== FILAS DE DISPOSITIVOS =====\n";
            foreach (var kvp in _filasPorDispositivo)
            {
                resumo += $"{kvp.Key}: {kvp.Value.Count} requisições\n";
                foreach (var req in kvp.Value)
                {
                    resumo += $"  - {req}\n";
                }
            }
            resumo += "=================================\n";
            return resumo;
        }

        public string ObterResumoDispositivos()
        {
            string resumo = "===== DISPOSITIVOS =====\n";
            foreach (var dispositivo in _dispositivos.Values)
            {
                resumo += $"{dispositivo}\n";
            }
            resumo += "========================\n";
            return resumo;
        }
    }
}