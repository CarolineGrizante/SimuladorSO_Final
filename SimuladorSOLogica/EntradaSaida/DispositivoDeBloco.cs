namespace SimuladorSOLogica.EntradaSaida
{
    public class DispositivoDeBloco : IDispositivo
    {
        public string Nome { get; private set; }
        public string Tipo => "Bloco";
        public bool EstaOcupado => _requisicaoAtual != null;
        public int TempoOperacao { get; private set; }

        private RequisicaoES _requisicaoAtual;

        public DispositivoDeBloco(string nome, int tempoOperacao)
        {
            Nome = nome;
            TempoOperacao = tempoOperacao;
            _requisicaoAtual = null;
        }

        public void IniciarOperacao(RequisicaoES requisicao)
        {
            _requisicaoAtual = requisicao;
        }

        public void ProcessarTick()
        {
            if (_requisicaoAtual != null)
            {
                _requisicaoAtual.ProcessarTick();
            }
        }

        public bool OperacaoConcluida()
        {
            return _requisicaoAtual != null && _requisicaoAtual.Concluida;
        }

        public RequisicaoES ObterRequisicaoAtual()
        {
            return _requisicaoAtual;
        }

        public void LimparRequisicao()
        {
            _requisicaoAtual = null;
        }

        public override string ToString()
        {
            return $"Dispositivo de Bloco: {Nome}, Tempo: {TempoOperacao}, Ocupado: {EstaOcupado}";
        }
    }
}