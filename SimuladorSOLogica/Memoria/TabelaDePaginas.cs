namespace SimuladorSOLogica.Memoria
{
    public class TabelaDePaginas
    {
        public int PID { get; private set; }
        private Dictionary<int, Pagina> _paginas;

        public TabelaDePaginas(int pid)
        {
            PID = pid;
            _paginas = new Dictionary<int, Pagina>();
        }

        public void AdicionarPagina(Pagina pagina)
        {
            _paginas[pagina.NumeroPagina] = pagina;
        }

        public Pagina ObterPagina(int numeroPagina)
        {
            if (_paginas.ContainsKey(numeroPagina))
                return _paginas[numeroPagina];

            // Criar página sob demanda
            Pagina novaPagina = new Pagina(numeroPagina, PID);
            _paginas[numeroPagina] = novaPagina;
            return novaPagina;
        }

        public bool PaginaPresente(int numeroPagina)
        {
            return _paginas.ContainsKey(numeroPagina) && _paginas[numeroPagina].Presente;
        }

        public List<Pagina> ObterTodasPaginas()
        {
            return _paginas.Values.ToList();
        }

        public int ContarPaginas()
        {
            return _paginas.Count;
        }

        public int ContarPaginasPresentes()
        {
            return _paginas.Values.Count(p => p.Presente);
        }

        public void LimparPaginas()
        {
            _paginas.Clear();
        }

        public string ObterResumo()
        {
            string resumo = $"===== TABELA DE PÁGINAS - PID {PID} =====\n";
            foreach (var pagina in _paginas.Values.OrderBy(p => p.NumeroPagina))
            {
                resumo += $"{pagina}\n";
            }
            resumo += $"Total: {_paginas.Count} páginas ({ContarPaginasPresentes()} presentes)\n";
            resumo += "==========================================\n";
            return resumo;
        }
    }
}