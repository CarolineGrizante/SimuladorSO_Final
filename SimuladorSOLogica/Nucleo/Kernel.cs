using SimuladorSOLogica.Processos;
using SimuladorSOLogica.Threads;
using SimuladorSOLogica.Escalonamento;
using SimuladorSOLogica.Memoria;
using SimuladorSOLogica.EntradaSaida;
using SimuladorSOLogica.Metricas;

namespace SimuladorSOLogica.Nucleo
{
    public class Kernel
    {
        public Relogio Relogio { get; private set; }
        public RegistradorDeEventos RegistradorDeEventos { get; private set; }
        public Configuracoes Configuracoes { get; private set; }
        public GerenciadorDeProcessos GerenciadorDeProcessos { get; private set; }
        public GerenciadorDeThreads GerenciadorDeThreads { get; private set; }
        public Escalonador Escalonador { get; private set; }
        public GerenciadorDeMemoria GerenciadorDeMemoria { get; private set; }
        public GerenciadorES GerenciadorES { get; private set; }
        public SistemaDeArquivos.SistemaDeArquivos SistemaDeArquivos { get; private set; }
        public GerenciadorDeMetricas GerenciadorDeMetricas { get; private set; }

        private bool _executando;

        public Kernel()
        {
            Relogio = new Relogio();
            RegistradorDeEventos = new RegistradorDeEventos();
            Configuracoes = new Configuracoes();

            GerenciadorDeProcessos = new GerenciadorDeProcessos(this);
            GerenciadorDeThreads = new GerenciadorDeThreads(this);
            GerenciadorDeMemoria = new GerenciadorDeMemoria(this);
            GerenciadorES = new GerenciadorES(this);
            SistemaDeArquivos = new SistemaDeArquivos.SistemaDeArquivos(this);
            Escalonador = new Escalonador(this);
            GerenciadorDeMetricas = new GerenciadorDeMetricas(this);

            _executando = false;

            RegistradorDeEventos.Registrar("Kernel inicializado com sucesso.");
        }

        public void Inicializar()
        {
            _executando = true;
            RegistradorDeEventos.Registrar("Sistema operacional iniciado.");
        }

        public void Desligar()
        {
            _executando = false;
            RegistradorDeEventos.Registrar("Sistema operacional desligado.");
        }

        public bool EstaExecutando()
        {
            return _executando;
        }

        public void ExecutarCiclo()
        {
            if (!_executando) return;

            Relogio.Tick();
            Escalonador.ExecutarCiclo();
            GerenciadorES.ProcessarTick();

            RegistradorDeEventos.Registrar($"Ciclo executado no tempo {Relogio.TempoAtual}");
        }

        public void ExecutarAteCompletar()
        {
            while (GerenciadorDeProcessos.ExistemProcessosAtivos())
            {
                ExecutarCiclo();
            }
            RegistradorDeEventos.Registrar("Todos os processos foram finalizados.");
        }
    }
}