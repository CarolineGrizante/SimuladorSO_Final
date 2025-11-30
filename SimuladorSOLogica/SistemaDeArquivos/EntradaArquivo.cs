namespace SimuladorSOLogica.SistemaDeArquivos
{
    public class EntradaArquivo
    {
        public string Nome { get; set; }
        public int NumeroINode { get; set; }
        public bool EhDiretorio { get; set; }

        public EntradaArquivo(string nome, int numeroINode, bool ehDiretorio)
        {
            Nome = nome;
            NumeroINode = numeroINode;
            EhDiretorio = ehDiretorio;
        }

        public override string ToString()
        {
            string tipo = EhDiretorio ? "<DIR>" : "<ARQ>";
            return $"{tipo} {Nome} (INode: {NumeroINode})";
        }
    }
}