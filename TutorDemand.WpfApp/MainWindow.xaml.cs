using System.Windows;
using TutorDemand.WpfApp.UI;

namespace TutorDemand.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_wSubject_Click(object sender, RoutedEventArgs e)
        {
            var p = new wSubject();
            p.Owner = this;
            p.Show();
        }

        private void Open_wTutor_Click(object sender, RoutedEventArgs e)
        {
            var p = new wTutor();
            p.Owner = this;
            p.Show();
        }

        private void Open_wTeachingSchedule_Click(object sender, RoutedEventArgs e)
        {
            var p = new WTeachingSchedule();
            p.Owner = this;
            p.Show();
        }
    }
}