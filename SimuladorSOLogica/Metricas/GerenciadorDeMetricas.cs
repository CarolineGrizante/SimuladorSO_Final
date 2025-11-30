using System.Text;
using SimuladorSOLogica.Nucleo;
using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Metricas
{
    /// <summary>
    /// Gerenciador de métricas e estatísticas do sistema.
    /// </summary>
    public class GerenciadorDeMetricas
    {
        private Kernel _kernel;
        private Dictionary<int, MetricasProcesso> _metricasProcessos;
        private Dictionary<string, MetricasDispositivo> _metricasDispositivos;
        private MetricasMemoria _metricasMemoria;

        public GerenciadorDeMetricas(Kernel kernel)
        {
            _kernel = kernel;
            _metricasProcessos = new Dictionary<int, MetricasProcesso>();
            _metricasDispositivos = new Dictionary<string, MetricasDispositivo>();
            _metricasMemoria = new MetricasMemoria();
        }

        public void ColetarMetricasProcesso(Processo processo)
        {
            MetricasProcesso metricas = new MetricasProcesso(processo.PID);
            metricas.CalcularMetricas(
                processo.PCB.TempoChegada,
                processo.PCB.TempoInicio,
                processo.PCB.TempoFinalizacao,
                processo.PCB.TempoTotalCPU,
                processo.PCB.TempoEspera
            );
            _metricasProcessos[processo.PID] = metricas;
        }

        public void ColetarMetricasMemoria()
        {
            _metricasMemoria.TotalFaltasPagina = _kernel.GerenciadorDeMemoria.FaltasDePagina;

            var tabelaMolduras = _kernel.GerenciadorDeMemoria.ObterTabelaMolduras();
            if (tabelaMolduras != null)
            {
                _metricasMemoria.MoldurasLivres = tabelaMolduras.ContarMoldurasLivres();
                _metricasMemoria.MoldurasOcupadas = tabelaMolduras.ContarMoldurasOcupadas();
            }

            var tlb = _kernel.GerenciadorDeMemoria.ObterTLB();
            if (tlb != null)
            {
                _metricasMemoria.AcertosTLB = tlb.Acertos;
                _metricasMemoria.FalhasTLB = tlb.Falhas;
            }

            _metricasMemoria.CalcularTaxas();
        }

        public double CalcularTempoMedioRetorno()
        {
            if (_metricasProcessos.Count == 0) return 0;
            return _metricasProcessos.Values.Average(m => m.TempoRetorno);
        }

        public double CalcularTempoMedioEspera()
        {
            if (_metricasProcessos.Count == 0) return 0;
            return _metricasProcessos.Values.Average(m => m.TempoEspera);
        }

        public double CalcularTempoMedioResposta()
        {
            if (_metricasProcessos.Count == 0) return 0;
            return _metricasProcessos.Values.Average(m => m.TempoResposta);
        }

        public double CalcularUtilizacaoCPU()
        {
            long tempoTotal = _kernel.Relogio.TempoAtual;
            if (tempoTotal == 0) return 0;

            long tempoCPUTotal = _metricasProcessos.Values.Sum(m => m.TempoCPU);
            return (double)tempoCPUTotal / tempoTotal;
        }

        public double CalcularThroughput()
        {
            long tempoTotal = _kernel.Relogio.TempoAtual;
            if (tempoTotal == 0) return 0;

            int processosFinalizados = _metricasProcessos.Count;
            return (double)processosFinalizados / tempoTotal;
        }

        public string GerarRelatorioCompleto()
        {
            // Coletar métricas de todos os processos
            foreach (var processo in _kernel.GerenciadorDeProcessos.ListarProcessos())
            {
                if (processo.Estado == EstadoProcesso.Finalizado)
                {
                    ColetarMetricasProcesso(processo);
                }
            }

            ColetarMetricasMemoria();

            StringBuilder relatorio = new StringBuilder();

            // Título
            relatorio.AppendLine("╔════════════════════════════════════════════════╗");
            relatorio.AppendLine("║        RELATÓRIO COMPLETO DE MÉTRICAS          ║");
            relatorio.AppendLine("╚════════════════════════════════════════════════╝");
            relatorio.AppendLine();

            // TEMPO DE RETORNO
            relatorio.AppendLine("===== TEMPO DE RETORNO POR PROCESSO =====");
            foreach (var metricas in _metricasProcessos.Values.OrderBy(m => m.PID))
            {
                relatorio.AppendLine($"P{metricas.PID}: {metricas.TempoRetorno} ticks");
            }
            relatorio.AppendLine();
            relatorio.AppendLine($"Média: {CalcularTempoMedioRetorno():F2} ticks");
            relatorio.AppendLine("=========================================");
            relatorio.AppendLine();

            // TEMPO DE ESPERA EM PRONTO
            relatorio.AppendLine("===== TEMPO DE ESPERA EM PRONTO =====");
            foreach (var metricas in _metricasProcessos.Values.OrderBy(m => m.PID))
            {
                relatorio.AppendLine($"P{metricas.PID}: {metricas.TempoEspera} ticks");
            }
            relatorio.AppendLine();
            relatorio.AppendLine($"Média: {CalcularTempoMedioEspera():F2} ticks");
            relatorio.AppendLine("=====================================");
            relatorio.AppendLine();

            // TEMPO DE RESPOSTA
            relatorio.AppendLine("===== TEMPO DE RESPOSTA =====");
            foreach (var metricas in _metricasProcessos.Values.OrderBy(m => m.PID))
            {
                relatorio.AppendLine($"P{metricas.PID}: {metricas.TempoResposta} ticks");
            }
            relatorio.AppendLine();
            relatorio.AppendLine($"Média: {CalcularTempoMedioResposta():F2} ticks");
            relatorio.AppendLine("=============================");
            relatorio.AppendLine();

            // UTILIZAÇÃO DA CPU
            relatorio.AppendLine("===== UTILIZAÇÃO DA CPU =====");
            long tempoTotal = _kernel.Relogio.TempoAtual;
            long tempoCPUTotal = _metricasProcessos.Values.Sum(m => m.TempoCPU);
            relatorio.AppendLine($"Tempo Total: {tempoTotal} ticks");
            relatorio.AppendLine($"Tempo Ocupado: {tempoCPUTotal} ticks");
            relatorio.AppendLine($"Utilização: {CalcularUtilizacaoCPU():P2}");
            relatorio.AppendLine("=============================");
            relatorio.AppendLine();

            // THROUGHPUT
            relatorio.AppendLine("===== THROUGHPUT =====");
            relatorio.AppendLine($"Processos Finalizados: {_metricasProcessos.Count}");
            relatorio.AppendLine($"Tempo Total: {tempoTotal} ticks");
            relatorio.AppendLine($"Throughput: {CalcularThroughput():F4} processos/tick");
            relatorio.AppendLine("======================");
            relatorio.AppendLine();

            // TROCAS DE CONTEXTO
            relatorio.AppendLine("===== TROCAS DE CONTEXTO =====");
            var trocas = _kernel.Escalonador.ObterTrocaDeContexto();
            relatorio.AppendLine($"Número de Trocas: {trocas.ContagemTrocas}");
            relatorio.AppendLine($"Sobrecarga Total: {trocas.SobrecargaTotal} ticks");
            relatorio.AppendLine("==============================");
            relatorio.AppendLine();

            // MÉTRICAS DE MEMÓRIA
            relatorio.AppendLine("===== MÉTRICAS DE MEMÓRIA =====");
            relatorio.AppendLine($"Faltas de Página: {_metricasMemoria.TotalFaltasPagina}");
            relatorio.AppendLine($"Hits TLB: {_metricasMemoria.AcertosTLB}");
            relatorio.AppendLine($"Misses TLB: {_metricasMemoria.FalhasTLB}");
            relatorio.AppendLine($"Taxa de Acerto TLB: {_metricasMemoria.TaxaAcertoTLB:P2}");
            relatorio.AppendLine("===============================");
            relatorio.AppendLine();

            string relatorioCompleto = relatorio.ToString();
            _kernel.RegistradorDeEventos.Registrar("Relatório de métricas gerado");
            return relatorioCompleto;
        }

        public string ObterResumoMetricas()
        {
            StringBuilder resumo = new StringBuilder();
            resumo.AppendLine("===== RESUMO DE MÉTRICAS =====");
            resumo.AppendLine($"Tempo médio de retorno: {CalcularTempoMedioRetorno():F2}");
            resumo.AppendLine($"Tempo médio de espera: {CalcularTempoMedioEspera():F2}");
            resumo.AppendLine($"Utilização da CPU: {CalcularUtilizacaoCPU():P2}");
            resumo.AppendLine($"Throughput: {CalcularThroughput():F4}");
            resumo.AppendLine("==============================");
            return resumo.ToString();
        }
    }
}