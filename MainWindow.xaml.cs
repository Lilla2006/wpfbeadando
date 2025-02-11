using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Filmek
        {
            public string eredetiCim { get; set; }
            public string magyarCim { get; set; }
            public DateTime bemutato { get; set; }
            public string forgalmazo { get; set; }
            public int bevetel { get; set; }
            public int latogato { get; set; }

            public Filmek(string sor)
            {
                string[] t = sor.Split(';');
                eredetiCim = t[0];
                magyarCim = t[1];
                bemutato = Convert.ToDateTime(t[2]);
                forgalmazo = t[3];
                bevetel = int.Parse(t[4]);
                latogato = int.Parse(t[5]);
            }
        }
        List<Filmek> adat = new List<Filmek>();
        List<int> keresesiadat = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            foreach (var i in File.ReadAllLines("beadando.txt").Skip(1))
            {
                adat.Add(new Filmek(i));
            }
        }

        private void betolt_Click(object sender, RoutedEventArgs e)
        {
            lista.Items.Clear();
            foreach (var i in adat)
            {
                lista.Items.Add(i.eredetiCim + " " + i.magyarCim + " " + i.bemutato + " " + i.forgalmazo + " " + i.bevetel + " " + i.latogato);
            }
        }

        private void atlag_Click(object sender, RoutedEventArgs e)
        {
            lista.Items.Clear();
            double atlagb = adat.Average(i => i.bevetel);
            lista.Items.Add("A filmek átlag bevétele: " + Math.Round(atlagb,2));
        }

        private void legtobb_Click(object sender, RoutedEventArgs e)
        {
            lista.Items.Clear();
            var legtobblat = adat.Max(i => i.latogato);
            var latogatok = adat.Where(i => i.latogato == legtobblat).Select(i => i).ToList();
            foreach (var i in latogatok)
            {
                lista.Items.Add("A legtöbbet látogatott film adatai: \n" + i.eredetiCim + "\n" + i.magyarCim + "\n" + i.bemutato + "\n" + i.forgalmazo + "\n" + i.bevetel + "\n" + i.latogato);
            }
        }

        private void legkorabb_Click(object sender, RoutedEventArgs e)
        {
            lista.Items.Clear();
            var minev = adat.Min(i => i.bemutato);
            var legkorabbev = adat.Where(i => i.bemutato == minev).Select(i => i).ToList();
            foreach (var i in legkorabbev)
            {
                lista.Items.Add("A legkorábban bemutatot film adatai: \n" + i.eredetiCim + "\n" + i.magyarCim + "\n" + i.bemutato + "\n" + i.forgalmazo + "\n" + i.bevetel + "\n" + i.latogato);
            }
        }

        private void statisztika_Click(object sender, RoutedEventArgs e)
        {
            lista.Items.Clear();
            Dictionary<string, int> stat = new Dictionary<string, int>();
            foreach (var i in adat)
            {
                if (stat.ContainsKey(i.forgalmazo))
                {
                    stat[i.forgalmazo]++;
                }
                else
                {
                    stat.Add(i.forgalmazo, 1);
                }
            }
            foreach (var i in stat)
            {
                lista.Items.Add(i.Key + " : " + i.Value + " db");
            }
        }

        private void keres_Click(object sender, RoutedEventArgs e)
        {
            lista.Items.Clear();
            
            int kereses = Convert.ToInt32(szoveg.Text);

            bool letezo = false;
            if (adat != null && keresesiadat != null)
            {
                foreach (var i in adat)
                {
                    foreach (var j in keresesiadat)
                    {
                        if (j > kereses)
                        {
                            lista.Items.Add($"{i.eredetiCim}\n{i.magyarCim}\n{i.bemutato}\n{i.forgalmazo}\n{i.bevetel}\n{i.latogato}");
                            letezo = true;
                            
                        }
                    }
                }
            }

            if (!letezo)
            {
                MessageBox.Show("Nincs ilyen film az adatbázisban!");
            }

        }
    }
}