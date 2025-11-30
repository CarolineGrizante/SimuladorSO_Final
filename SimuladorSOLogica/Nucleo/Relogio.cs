namespace SimuladorSOLogica.Nucleo
{
    public class Relogio
    {
        public long TempoAtual { get; private set; }

        public Relogio()
        {
            TempoAtual = 0;
        }

        public void Tick()
        {
            TempoAtual++;
        }

        public void Resetar()
        {
            TempoAtual = 0;
        }

        public void AvancarTempo(long ticks)
        {
            if (ticks < 0)
                throw new ArgumentException("Número de ticks deve ser positivo.");

            TempoAtual += ticks;
        }
    }
}