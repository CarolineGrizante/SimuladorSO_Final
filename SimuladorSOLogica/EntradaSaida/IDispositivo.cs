namespace SimuladorSOLogica.EntradaSaida
{
    public interface IDispositivo
    {
        string Nome { get; }
        string Tipo { get; }
        bool EstaOcupado { get; }
        int TempoOperacao { get; }
        void IniciarOperacao(RequisicaoES requisicao);
        void ProcessarTick();
        bool OperacaoConcluida();
        RequisicaoES ObterRequisicaoAtual();
    }
}