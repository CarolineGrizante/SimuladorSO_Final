using SimuladorSOLogica.Processos;

namespace SimuladorSOLogica.Escalonamento
{
    public class FilaProntos
    {
        private List<Processo> _fila;

        public FilaProntos()
        {
            _fila = new List<Processo>();
        }

        public void Adicionar(Processo processo)
        {
            if (!_fila.Contains(processo))
            {
                _fila.Add(processo);
            }
        }

        public void Remover(Processo processo)
        {
            _fila.Remove(processo);
        }

        public Processo RemoverPrimeiro()
        {
            if (_fila.Count == 0)
                return null;

            Processo processo = _fila[0];
            _fila.RemoveAt(0);
            return processo;
        }

        public Processo ObterPrimeiro()
        {
            return _fila.Count > 0 ? _fila[0] : null;
        }

        public List<Processo> ObterTodos()
        {
            return new List<Processo>(_fila);
        }

        public List<Processo> ObterPorPrioridade()
        {
            return _fila.OrderByDescending(p => p.PCB.Prioridade).ToList();
        }

        public int Tamanho()
        {
            return _fila.Count;
        }

        public bool EstaVazia()
        {
            return _fila.Count == 0;
        }

        public void Limpar()
        {
            _fila.Clear();
        }

        public string ObterResumo()
        {
            string resumo = "===== FILA DE PRONTOS =====\n";
            for (int i = 0; i < _fila.Count; i++)
            {
                resumo += $"{i + 1}. {_fila[i]}\n";
            }
            resumo += $"Total: {_fila.Count} processos\n";
            resumo += "===========================\n";
            return resumo;
        }
    }
}