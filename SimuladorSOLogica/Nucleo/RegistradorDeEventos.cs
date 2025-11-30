using System.Text;

namespace SimuladorSOLogica.Nucleo
{
    public class RegistradorDeEventos
    {
        private List<string> _eventos;

        public RegistradorDeEventos()
        {
            _eventos = new List<string>();
        }

        public void Registrar(string mensagem)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string eventoCompleto = $"[{timestamp}] {mensagem}";
            _eventos.Add(eventoCompleto);
        }

        public List<string> ObterEventos()
        {
            return new List<string>(_eventos);
        }

        public void LimparEventos()
        {
            _eventos.Clear();
        }

        public void ExportarParaArquivo(string caminhoArquivo)
        {
            try
            {
                File.WriteAllLines(caminhoArquivo, _eventos, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao exportar log: {ex.Message}");
            }
        }

        public string ObterLogCompleto()
        {
            return string.Join(Environment.NewLine, _eventos);
        }
    }
}