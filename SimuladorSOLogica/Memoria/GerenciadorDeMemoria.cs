using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Memoria
{
    public class GerenciadorDeMemoria
    {
        private Kernel _kernel;
        private Dictionary<int, TabelaDePaginas> _tabelasPorProcesso;
        private TabelaDeMolduras _tabelaMolduras;
        private TLB _tlb;
        private int _faltasDePagina;
        private PoliticaAlocacao _politica;

        public int FaltasDePagina => _faltasDePagina;

        public GerenciadorDeMemoria(Kernel kernel)
        {
            _kernel = kernel;
            _tabelasPorProcesso = new Dictionary<int, TabelaDePaginas>();
            _tabelaMolduras = null;
            _tlb = null;
            _faltasDePagina = 0;
            _politica = PoliticaAlocacao.FirstFit;
        }

        public void InicializarMolduras(int numeroMolduras)
        {
            _tabelaMolduras = new TabelaDeMolduras(numeroMolduras);
            _kernel.RegistradorDeEventos.Registrar($"Memória inicializada com {numeroMolduras} molduras");
        }

        public void AtivarTLB(int tamanho)
        {
            _tlb = new TLB(tamanho);
            _kernel.Configuracoes.UsarTLB = true;
            _kernel.RegistradorDeEventos.Registrar($"TLB ativada com {tamanho} entradas");
        }

        public void DesativarTLB()
        {
            _tlb = null;
            _kernel.Configuracoes.UsarTLB = false;
            _kernel.RegistradorDeEventos.Registrar("TLB desativada");
        }

        public void AlocarMemoria(int pid, int tamanhoBytes)
        {
            if (_tabelaMolduras == null)
                InicializarMolduras(_kernel.Configuracoes.NumeroMolduras);

            if (!_tabelasPorProcesso.ContainsKey(pid))
            {
                _tabelasPorProcesso[pid] = new TabelaDePaginas(pid);
            }

            int tamanhoPagina = _kernel.Configuracoes.TamanhoPagina;
            int numeroPaginas = (int)Math.Ceiling((double)tamanhoBytes / tamanhoPagina);

            _kernel.RegistradorDeEventos.Registrar(
                $"Alocando {tamanhoBytes} bytes ({numeroPaginas} páginas) para processo {pid}");

            for (int i = 0; i < numeroPaginas; i++)
            {
                Pagina pagina = new Pagina(i, pid);
                _tabelasPorProcesso[pid].AdicionarPagina(pagina);
            }
        }

        public void LiberarMemoria(int pid)
        {
            if (_tabelaMolduras != null)
            {
                _tabelaMolduras.LiberarMoldurasPorProcesso(pid);
            }

            if (_tlb != null)
            {
                _tlb.InvalidarPorProcesso(pid);
            }

            _tabelasPorProcesso.Remove(pid);

            _kernel.RegistradorDeEventos.Registrar($"Memória liberada para processo {pid}");
        }

        public void AcessarPagina(int pid, int enderecoVirtual)
        {
            if (_tabelaMolduras == null)
                throw new InvalidOperationException("Memória não inicializada");

            if (!_tabelasPorProcesso.ContainsKey(pid))
                throw new ArgumentException($"Processo {pid} não possui tabela de páginas");

            int tamanhoPagina = _kernel.Configuracoes.TamanhoPagina;
            int numeroPagina = enderecoVirtual / tamanhoPagina;

            // Verificar TLB primeiro
            if (_tlb != null)
            {
                int? molduraTLB = _tlb.Buscar(pid, numeroPagina);
                if (molduraTLB.HasValue)
                {
                    _kernel.RegistradorDeEventos.Registrar($"TLB Hit: PID={pid}, Página={numeroPagina}");
                    return;
                }
            }

            // Buscar na tabela de páginas
            TabelaDePaginas tabela = _tabelasPorProcesso[pid];
            Pagina pagina = tabela.ObterPagina(numeroPagina);

            if (!pagina.Presente)
            {
                // Falta de página
                _faltasDePagina++;
                _kernel.RegistradorDeEventos.Registrar($"Falta de página: PID={pid}, Página={numeroPagina}");

                // Alocar moldura
                Moldura moldura = _tabelaMolduras.AlocarMolduraLivre(pid, numeroPagina, _kernel.Relogio.TempoAtual);

                if (moldura == null)
                {
                    throw new InvalidOperationException("Memória física esgotada - substituição de página não implementada");
                }

                pagina.Presente = true;
                pagina.NumeroMoldura = moldura.NumeroMoldura;
                pagina.TempoUltimoAcesso = _kernel.Relogio.TempoAtual;

                // Adicionar à TLB
                if (_tlb != null)
                {
                    _tlb.Adicionar(pid, numeroPagina, moldura.NumeroMoldura, _kernel.Relogio.TempoAtual);
                }
            }
            else
            {
                // Página já está presente
                pagina.TempoUltimoAcesso = _kernel.Relogio.TempoAtual;
                pagina.Referenciada = true;
            }
        }

        public TabelaDePaginas ObterTabelaPaginas(int pid)
        {
            if (!_tabelasPorProcesso.ContainsKey(pid))
                throw new ArgumentException($"Processo {pid} não possui tabela de páginas");

            return _tabelasPorProcesso[pid];
        }

        public TabelaDeMolduras ObterTabelaMolduras()
        {
            return _tabelaMolduras;
        }

        public TLB ObterTLB()
        {
            return _tlb;
        }

        public string ObterEstatisticas()
        {
            string stats = "===== ESTATÍSTICAS DE MEMÓRIA =====\n";
            stats += $"Faltas de página: {_faltasDePagina}\n";

            if (_tabelaMolduras != null)
            {
                stats += $"Molduras livres: {_tabelaMolduras.ContarMoldurasLivres()}\n";
                stats += $"Molduras ocupadas: {_tabelaMolduras.ContarMoldurasOcupadas()}\n";
            }

            if (_tlb != null)
            {
                stats += _tlb.ObterEstatisticas() + "\n";
            }

            stats += "====================================\n";
            return stats;
        }
    }
}