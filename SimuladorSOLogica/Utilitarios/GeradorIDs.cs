namespace SimuladorSOLogica.Utilitarios
{
    public class GeradorIDs
    {
        private int _proximoID;

        public GeradorIDs()
        {
            _proximoID = 0;
        }

        public int GerarProximoID()
        {
            return _proximoID++;
        }

        public void Resetar()
        {
            _proximoID = 0;
        }

        public int ObterProximoID()
        {
            return _proximoID;
        }
    }
}