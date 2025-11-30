using SimuladorSOLogica.Processos;
using SimuladorSOLogica.Nucleo;

namespace SimuladorSOLogica.Escalonamento
{
    public class TrocaDeContexto
    {
        private Kernel _kernel;
        private int _contagemTrocas;
        private long _sobrecargaTotal;
        private const int CUSTO_TROCA = 1; // Custo em ticks de uma troca de contexto

        public int ContagemTrocas => _contagemTrocas;
        public long SobrecargaTotal => _sobrecargaTotal;

        public TrocaDeContexto(Kernel kernel)
        {
            _kernel = kernel;
            _contagemTrocas = 0;
            _sobrecargaTotal = 0;
        }

        public void RealizarTroca(Processo processoAnterior, Processo proximoProcesso)
        {
            if (processoAnterior != null)
            {
                SalvarContexto(processoAnterior);
            }

            if (proximoProcesso != null)
            {
                RestaurarContexto(proximoProcesso);
            }

            _contagemTrocas++;
            _sobrecargaTotal += CUSTO_TROCA;

            _kernel.RegistradorDeEventos.Registrar(
                $"Troca de contexto: {processoAnterior?.PID ?? -1} -> {proximoProcesso?.PID ?? -1}");
        }

        private void SalvarContexto(Processo processo)
        {
            // Simula salvamento do contexto do processo
            // Em um sistema real, salvaria registradores, PC, etc.
            _kernel.RegistradorDeEventos.Registrar($"Contexto salvo: PID={processo.PID}");
        }

        private void RestaurarContexto(Processo processo)
        {
            // Simula restauração do contexto do processo
            // Em um sistema real, restauraria registradores, PC, etc.
            _kernel.RegistradorDeEventos.Registrar($"Contexto restaurado: PID={processo.PID}");
        }

        public void ResetarEstatisticas()
        {
            _contagemTrocas = 0;
            _sobrecargaTotal = 0;
        }

        public string ObterEstatisticas()
        {
            return $"Trocas de contexto: {_contagemTrocas}, Sobrecarga total: {_sobrecargaTotal} ticks";
        }
    }
}