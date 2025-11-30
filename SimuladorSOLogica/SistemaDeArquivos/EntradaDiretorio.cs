namespace SimuladorSOLogica.SistemaDeArquivos
{
    public class EntradaDiretorio
    {
        public int NumeroINode { get; set; }
        public string Nome { get; set; }
        public int INodePai { get; set; }
        public List<EntradaArquivo> Entradas { get; set; }

        public EntradaDiretorio(int numeroINode, string nome, int iNodePai)
        {
            NumeroINode = numeroINode;
            Nome = nome;
            INodePai = iNodePai;
            Entradas = new List<EntradaArquivo>();
        }

        public void AdicionarEntrada(EntradaArquivo entrada)
        {
            if (!Entradas.Any(e => e.Nome == entrada.Nome))
            {
                Entradas.Add(entrada);
            }
        }

        public void RemoverEntrada(string nome)
        {
            Entradas.RemoveAll(e => e.Nome == nome);
        }

        public EntradaArquivo BuscarEntrada(string nome)
        {
            return Entradas.FirstOrDefault(e => e.Nome == nome);
        }

        public List<EntradaArquivo> ListarEntradas()
        {
            return new List<EntradaArquivo>(Entradas);
        }

        public override string ToString()
        {
            return $"Diretório: {Nome} (INode: {NumeroINode}, {Entradas.Count} entradas)";
        }
    }
}
