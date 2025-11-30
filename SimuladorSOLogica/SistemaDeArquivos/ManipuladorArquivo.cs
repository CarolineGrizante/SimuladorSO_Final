namespace SimuladorSOLogica.SistemaDeArquivos
{
    public class ManipuladorArquivo
    {
        public int ManipuladorID { get; set; }
        public int NumeroINode { get; set; }
        public int PID { get; set; }
        public string Caminho { get; set; }
        public int PosicaoAtual { get; set; }
        public string Modo { get; set; } // "r", "w", "rw"

        public ManipuladorArquivo(int id, int inode, int pid, string caminho, string modo)
        {
            ManipuladorID = id;
            NumeroINode = inode;
            PID = pid;
            Caminho = caminho;
            PosicaoAtual = 0;
            Modo = modo;
        }

        public override string ToString()
        {
            return $"Manipulador {ManipuladorID}: {Caminho} (INode={NumeroINode}, PID={PID}, Modo={Modo})";
        }
    }
}