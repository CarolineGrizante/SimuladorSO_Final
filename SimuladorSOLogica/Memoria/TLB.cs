namespace SimuladorSOLogica.Memoria
{
    public class TLB
    {
        private List<EntradaTLB> _entradas;
        private int _tamanho;
        private int _acertos;
        private int _falhas;

        public int Acertos => _acertos;
        public int Falhas => _falhas;
        public double TaxaAcerto => (_acertos + _falhas) > 0 ? (double)_acertos / (_acertos + _falhas) : 0;

        public TLB(int tamanho)
        {
            _tamanho = tamanho;
            _entradas = new List<EntradaTLB>();

            for (int i = 0; i < tamanho; i++)
            {
                _entradas.Add(new EntradaTLB());
            }

            _acertos = 0;
            _falhas = 0;
        }

        public int? Buscar(int pid, int numeroPagina)
        {
            EntradaTLB entrada = _entradas.FirstOrDefault(e =>
                e.Valida && e.PID == pid && e.NumeroPagina == numeroPagina);

            if (entrada != null)
            {
                _acertos++;
                return entrada.NumeroMoldura;
            }
            else
            {
                _falhas++;
                return null;
            }
        }

        public void Adicionar(int pid, int numeroPagina, int numeroMoldura, long tempo)
        {
            // Procurar entrada inválida
            EntradaTLB entradaLivre = _entradas.FirstOrDefault(e => !e.Valida);

            if (entradaLivre != null)
            {
                entradaLivre.Atualizar(pid, numeroPagina, numeroMoldura, tempo);
            }
            else
            {
                // Substituir entrada mais antiga (LRU)
                EntradaTLB entradaMaisAntiga = _entradas.OrderBy(e => e.TempoUltimoAcesso).First();
                entradaMaisAntiga.Atualizar(pid, numeroPagina, numeroMoldura, tempo);
            }
        }

        public void InvalidarPorProcesso(int pid)
        {
            foreach (var entrada in _entradas.Where(e => e.PID == pid))
            {
                entrada.Invalidar();
            }
        }

        public void LimparTLB()
        {
            foreach (var entrada in _entradas)
            {
                entrada.Invalidar();
            }
            _acertos = 0;
            _falhas = 0;
        }

        public string ObterEstatisticas()
        {
            return $"TLB - Acertos: {_acertos}, Falhas: {_falhas}, Taxa de acerto: {TaxaAcerto:P2}";
        }

        public string ObterResumo()
        {
            string resumo = "===== TLB =====\n";
            foreach (var entrada in _entradas)
            {
                resumo += $"{entrada}\n";
            }
            resumo += ObterEstatisticas() + "\n";
            resumo += "===============\n";
            return resumo;
        }
    }
}