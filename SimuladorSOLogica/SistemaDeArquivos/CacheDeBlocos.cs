namespace SimuladorSOLogica.SistemaDeArquivos
{
    public class CacheDeBlocos
    {
        private Dictionary<int, byte[]> _cache;
        private int _tamanhoMaximo;
        private Dictionary<int, long> _temposAcesso;

        public CacheDeBlocos(int tamanhoMaximo)
        {
            _tamanhoMaximo = tamanhoMaximo;
            _cache = new Dictionary<int, byte[]>();
            _temposAcesso = new Dictionary<int, long>();
        }

        public byte[] Buscar(int numeroBloco)
        {
            if (_cache.ContainsKey(numeroBloco))
            {
                _temposAcesso[numeroBloco] = System.DateTime.Now.Ticks;
                return _cache[numeroBloco];
            }
            return null;
        }

        public void Adicionar(int numeroBloco, byte[] dados)
        {
            if (_cache.Count >= _tamanhoMaximo)
            {
                // Remover bloco menos recentemente usado (LRU)
                int blocoMaisAntigo = _temposAcesso.OrderBy(kvp => kvp.Value).First().Key;
                _cache.Remove(blocoMaisAntigo);
                _temposAcesso.Remove(blocoMaisAntigo);
            }

            _cache[numeroBloco] = dados;
            _temposAcesso[numeroBloco] = System.DateTime.Now.Ticks;
        }

        public void Remover(int numeroBloco)
        {
            _cache.Remove(numeroBloco);
            _temposAcesso.Remove(numeroBloco);
        }

        public void Limpar()
        {
            _cache.Clear();
            _temposAcesso.Clear();
        }

        public int TamanhoAtual()
        {
            return _cache.Count;
        }

        public string ObterResumo()
        {
            return $"Cache de Blocos: {_cache.Count}/{_tamanhoMaximo} blocos em cache";
        }
    }
}