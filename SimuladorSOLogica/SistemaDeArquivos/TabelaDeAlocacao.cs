namespace SimuladorSOLogica.SistemaDeArquivos
{
    public class TabelaDeAlocacao
    {
        private Dictionary<int, bool> _blocos; // true = ocupado, false = livre
        private int _totalBlocos;

        public TabelaDeAlocacao(int totalBlocos)
        {
            _totalBlocos = totalBlocos;
            _blocos = new Dictionary<int, bool>();

            for (int i = 0; i < totalBlocos; i++)
            {
                _blocos[i] = false;
            }
        }

        public int AlocarBloco()
        {
            for (int i = 0; i < _totalBlocos; i++)
            {
                if (!_blocos[i])
                {
                    _blocos[i] = true;
                    return i;
                }
            }
            return -1; // Sem blocos livres
        }

        public List<int> AlocarBlocos(int quantidade)
        {
            List<int> blocosAlocados = new List<int>();

            for (int i = 0; i < _totalBlocos && blocosAlocados.Count < quantidade; i++)
            {
                if (!_blocos[i])
                {
                    _blocos[i] = true;
                    blocosAlocados.Add(i);
                }
            }

            return blocosAlocados;
        }

        public void LiberarBloco(int numeroBloco)
        {
            if (_blocos.ContainsKey(numeroBloco))
            {
                _blocos[numeroBloco] = false;
            }
        }

        public void LiberarBlocos(int[] blocos)
        {
            foreach (int bloco in blocos)
            {
                LiberarBloco(bloco);
            }
        }

        public int ContarBlocosLivres()
        {
            return _blocos.Values.Count(b => !b);
        }

        public int ContarBlocosOcupados()
        {
            return _blocos.Values.Count(b => b);
        }

        public string ObterResumo()
        {
            return $"Blocos: Total={_totalBlocos}, Livres={ContarBlocosLivres()}, Ocupados={ContarBlocosOcupados()}";
        }
    }
}
