using System.Windows;

namespace SimuladorSOInterface
{
    /// <summary>
    /// Interaction logic for RelatorioWindow.xaml
    /// </summary>
    public partial class RelatorioWindow : Window
    {
        public RelatorioWindow(string relatorioTexto)
        {
            InitializeComponent();
            txtRelatorio.Text = relatorioTexto;
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}