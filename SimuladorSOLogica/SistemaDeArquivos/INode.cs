namespace SimuladorSOLogica.SistemaDeArquivos
{
    public class INode
    {
        public int NumeroINode { get; set; }
        public string Nome { get; set; }
        public bool EhDiretorio { get; set; }
        public int Tamanho { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataModificacao { get; set; }
        public DateTime DataAcesso { get; set; }
        public int ProprietarioPID { get; set; }
        public int[] BlocosAlocados { get; set; }
        public bool Aberto { get; set; }

        public INode(int numero, string nome, bool ehDiretorio, int proprietario)
        {
            NumeroINode = numero;
            Nome = nome;
            EhDiretorio = ehDiretorio;
            Tamanho = 0;
            DataCriacao = DateTime.Now;
            DataModificacao = DateTime.Now;
            DataAcesso = DateTime.Now;
            ProprietarioPID = proprietario;
            BlocosAlocados = new int[0];
            Aberto = false;
        }

        public void AtualizarDataAcesso()
        {
            DataAcesso = DateTime.Now;
        }

        public void AtualizarDataModificacao()
        {
            DataModificacao = DateTime.Now;
        }

        public override string ToString()
        {
            string tipo = EhDiretorio ? "DIR" : "ARQ";
            return $"[{tipo}] {Nome} ({Tamanho} bytes) - INode: {NumeroINode}";
        }
    }
}