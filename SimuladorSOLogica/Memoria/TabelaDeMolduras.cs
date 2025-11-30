namespace SimuladorSOLogica.Memoria
{
    public class TabelaDeMolduras
    {
        private Dictionary<int, Moldura> _molduras;
        private int _numeroMolduras;

        public TabelaDeMolduras(int numeroMolduras)
        {
            _numeroMolduras = numeroMolduras;
            _molduras = new Dictionary<int, Moldura>();

            for (int i = 0; i < numeroMolduras; i++)
            {
                _molduras[i] = new Moldura(i);
            }
        }

        public Moldura ObterMoldura(int numeroMoldura)
        {
            if (!_molduras.ContainsKey(numeroMoldura))
                throw new ArgumentException($"Moldura {numeroMoldura} não existe.");

            return _molduras[numeroMoldura];
        }

        public Moldura AlocarMolduraLivre(int pid, int numeroPagina, long tempo)
        {
            Moldura molduraLivre = _molduras.Values.FirstOrDefault(m => !m.Ocupada);

            if (molduraLivre != null)
            {
                molduraLivre.Alocar(pid, numeroPagina, tempo);
            }

            return molduraLivre;
        }

        public void LiberarMoldura(int numeroMoldura)
        {
            if (_molduras.ContainsKey(numeroMoldura))
            {
                _molduras[numeroMoldura].Liberar();
            }
        }

        public void LiberarMoldurasPorProcesso(int pid)
        {
            foreach (var moldura in _molduras.Values.Where(m => m.PID == pid))
            {
                moldura.Liberar();
            }
        }

        public List<Moldura> ObterTodasMolduras()
        {
            return _molduras.Values.OrderBy(m => m.NumeroMoldura).ToList();
        }

        public int ContarMoldurasLivres()
        {
            return _molduras.Values.Count(m => !m.Ocupada);
        }

        public int ContarMoldurasOcupadas()
        {
            return _molduras.Values.Count(m => m.Ocupada);
        }

        public string ObterMapaMolduras()
        {
            string mapa = "===== MAPA DE MOLDURAS =====\n";
            foreach (var moldura in _molduras.Values.OrderBy(m => m.NumeroMoldura))
            {
                mapa += $"{moldura}\n";
            }
            mapa += $"Livres: {ContarMoldurasLivres()}, Ocupadas: {ContarMoldurasOcupadas()}\n";
            mapa += "============================\n";
            return mapa;
        }
    }
}