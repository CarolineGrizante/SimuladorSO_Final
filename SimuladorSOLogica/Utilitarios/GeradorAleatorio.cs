namespace SimuladorSOLogica.Utilitarios
{
    public class GeradorAleatorio
    {
        private Random _random;

        public GeradorAleatorio()
        {
            _random = new Random();
        }

        public GeradorAleatorio(int semente)
        {
            _random = new Random(semente);
        }

        public void DefinirSemente(int semente)
        {
            _random = new Random(semente);
        }

        public int GerarInteiro(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public double GerarDouble()
        {
            return _random.NextDouble();
        }

        public bool GerarBooleano()
        {
            return _random.Next(2) == 1;
        }

        public int GerarPrioridade()
        {
            return GerarInteiro(1, 5);
        }

        public int GerarTempoCPU()
        {
            return GerarInteiro(5, 50);
        }

        public int GerarTempoIO()
        {
            return GerarInteiro(10, 100);
        }
    }
}