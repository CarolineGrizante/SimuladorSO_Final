using SimuladorSOLogica.Nucleo;
using SimuladorSOLogica.Utilitarios;

namespace SimuladorSOLogica.SistemaDeArquivos
{
    public class SistemaDeArquivos
    {
        private Kernel _kernel;
        private Dictionary<int, INode> _inodes;
        private Dictionary<int, EntradaDiretorio> _diretorios;
        private Dictionary<int, ManipuladorArquivo> _manipuladores;
        private TabelaDeAlocacao _tabelaAlocacao;
        private CacheDeBlocos _cache;
        private GeradorIDs _geradorINodes;
        private GeradorIDs _geradorManipuladores;
        private int _diretorioAtual;
        private const int TOTAL_BLOCOS = 1000;

        public SistemaDeArquivos(Kernel kernel)
        {
            _kernel = kernel;
            _inodes = new Dictionary<int, INode>();
            _diretorios = new Dictionary<int, EntradaDiretorio>();
            _manipuladores = new Dictionary<int, ManipuladorArquivo>();
            _tabelaAlocacao = new TabelaDeAlocacao(TOTAL_BLOCOS);
            _cache = new CacheDeBlocos(50);
            _geradorINodes = new GeradorIDs();
            _geradorManipuladores = new GeradorIDs();

            // Criar diretório raiz
            CriarDiretorioRaiz();
        }

        private void CriarDiretorioRaiz()
        {
            int inodeRaiz = _geradorINodes.GerarProximoID();
            INode inode = new INode(inodeRaiz, "/", true, 0);
            _inodes[inodeRaiz] = inode;

            EntradaDiretorio dirRaiz = new EntradaDiretorio(inodeRaiz, "/", -1);
            _diretorios[inodeRaiz] = dirRaiz;

            _diretorioAtual = inodeRaiz;

            _kernel.RegistradorDeEventos.Registrar("Sistema de arquivos inicializado com diretório raiz");
        }

        public void CriarArquivo(string caminho)
        {
            string[] partes = caminho.Split('/');
            string nomeArquivo = partes[partes.Length - 1];

            int inodeArquivo = _geradorINodes.GerarProximoID();
            INode inode = new INode(inodeArquivo, nomeArquivo, false, 0);
            _inodes[inodeArquivo] = inode;

            EntradaDiretorio dirAtual = _diretorios[_diretorioAtual];
            EntradaArquivo entrada = new EntradaArquivo(nomeArquivo, inodeArquivo, false);
            dirAtual.AdicionarEntrada(entrada);

            _kernel.RegistradorDeEventos.Registrar($"Arquivo criado: {caminho} (INode={inodeArquivo})");
        }

        public void CriarDiretorio(string caminho)
        {
            string[] partes = caminho.Split('/');
            string nomeDiretorio = partes[partes.Length - 1];

            int inodeDiretorio = _geradorINodes.GerarProximoID();
            INode inode = new INode(inodeDiretorio, nomeDiretorio, true, 0);
            _inodes[inodeDiretorio] = inode;

            EntradaDiretorio novoDir = new EntradaDiretorio(inodeDiretorio, nomeDiretorio, _diretorioAtual);
            _diretorios[inodeDiretorio] = novoDir;

            EntradaDiretorio dirAtual = _diretorios[_diretorioAtual];
            EntradaArquivo entrada = new EntradaArquivo(nomeDiretorio, inodeDiretorio, true);
            dirAtual.AdicionarEntrada(entrada);

            _kernel.RegistradorDeEventos.Registrar($"Diretório criado: {caminho} (INode={inodeDiretorio})");
        }

        public int AbrirArquivo(string caminho, string modo = "r")
        {
            // Buscar arquivo no diretório atual
            string[] partes = caminho.Split('/');
            string nomeArquivo = partes[partes.Length - 1];

            EntradaDiretorio dirAtual = _diretorios[_diretorioAtual];
            EntradaArquivo entrada = dirAtual.BuscarEntrada(nomeArquivo);

            if (entrada == null)
                throw new Exception($"Arquivo não encontrado: {caminho}");

            INode inode = _inodes[entrada.NumeroINode];
            inode.Aberto = true;
            inode.AtualizarDataAcesso();

            int manipuladorID = _geradorManipuladores.GerarProximoID();
            ManipuladorArquivo manipulador = new ManipuladorArquivo(
                manipuladorID, entrada.NumeroINode, 0, caminho, modo);

            _manipuladores[manipuladorID] = manipulador;

            _kernel.RegistradorDeEventos.Registrar($"Arquivo aberto: {caminho} (Manipulador={manipuladorID})");
            return manipuladorID;
        }

        public void FecharArquivo(string caminho)
        {
            ManipuladorArquivo manipulador = _manipuladores.Values
                .FirstOrDefault(m => m.Caminho == caminho);

            if (manipulador != null)
            {
                INode inode = _inodes[manipulador.NumeroINode];
                inode.Aberto = false;

                _manipuladores.Remove(manipulador.ManipuladorID);
                _kernel.RegistradorDeEventos.Registrar($"Arquivo fechado: {caminho}");
            }
        }

        public byte[] LerArquivo(string caminho, int tamanho)
        {
            _kernel.RegistradorDeEventos.Registrar($"Lendo arquivo: {caminho} ({tamanho} bytes)");
            return new byte[tamanho];
        }

        public void EscreverArquivo(string caminho, byte[] dados)
        {
            string[] partes = caminho.Split('/');
            string nomeArquivo = partes[partes.Length - 1];

            EntradaDiretorio dirAtual = _diretorios[_diretorioAtual];
            EntradaArquivo entrada = dirAtual.BuscarEntrada(nomeArquivo);

            if (entrada != null)
            {
                INode inode = _inodes[entrada.NumeroINode];
                inode.Tamanho = dados.Length;
                inode.AtualizarDataModificacao();

                // Alocar blocos necessários
                int blocosNecessarios = (int)Math.Ceiling((double)dados.Length / 512);
                List<int> blocos = _tabelaAlocacao.AlocarBlocos(blocosNecessarios);
                inode.BlocosAlocados = blocos.ToArray();

                _kernel.RegistradorDeEventos.Registrar(
                    $"Escrito em arquivo: {caminho} ({dados.Length} bytes, {blocosNecessarios} blocos)");
            }
        }

        public void ApagarArquivo(string caminho)
        {
            string[] partes = caminho.Split('/');
            string nomeArquivo = partes[partes.Length - 1];

            EntradaDiretorio dirAtual = _diretorios[_diretorioAtual];
            EntradaArquivo entrada = dirAtual.BuscarEntrada(nomeArquivo);

            if (entrada != null)
            {
                INode inode = _inodes[entrada.NumeroINode];

                // Liberar blocos
                _tabelaAlocacao.LiberarBlocos(inode.BlocosAlocados);

                // Remover do diretório
                dirAtual.RemoverEntrada(nomeArquivo);

                // Remover INode
                _inodes.Remove(entrada.NumeroINode);

                _kernel.RegistradorDeEventos.Registrar($"Arquivo apagado: {caminho}");
            }
        }

        public List<EntradaArquivo> ListarDiretorioAtual()
        {
            EntradaDiretorio dirAtual = _diretorios[_diretorioAtual];
            return dirAtual.ListarEntradas();
        }

        public void MudarDiretorio(string caminho)
        {
            if (caminho == "/")
            {
                _diretorioAtual = 0;
                _kernel.RegistradorDeEventos.Registrar("Mudado para diretório raiz");
                return;
            }

            EntradaDiretorio dirAtual = _diretorios[_diretorioAtual];
            EntradaArquivo entrada = dirAtual.BuscarEntrada(caminho);

            if (entrada != null && entrada.EhDiretorio)
            {
                _diretorioAtual = entrada.NumeroINode;
                _kernel.RegistradorDeEventos.Registrar($"Mudado para diretório: {caminho}");
            }
            else
            {
                throw new Exception($"Diretório não encontrado: {caminho}");
            }
        }

        public string ObterResumo()
        {
            string resumo = "===== SISTEMA DE ARQUIVOS =====\n";
            resumo += $"Total de INodes: {_inodes.Count}\n";
            resumo += $"Diretórios: {_diretorios.Count}\n";
            resumo += $"Arquivos abertos: {_manipuladores.Count}\n";
            resumo += _tabelaAlocacao.ObterResumo() + "\n";
            resumo += _cache.ObterResumo() + "\n";
            resumo += "===============================\n";
            return resumo;
        }
    }
}
