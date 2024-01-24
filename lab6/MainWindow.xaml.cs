using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace lab6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Hive hive;
        Random random;
        DispatcherTimer timer;
        int iteration = 0;
        bool started = false;
        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 1, 0) };
            timer.Tick += new EventHandler(Timer_Tick);
            drawEllipse(new Point(0, 0), 10, 4);
        }
        double square_summ(Point position)
        {
            return -((position.X * position.X) + (position.Y * position.Y));
        }

        double Restrigin_function(Point position)
        {
            return -(((position.X * position.X) - 10 * Math.Cos(2 * Math.PI * position.X)) + ((position.Y * position.Y) - 10 * Math.Cos(2 * Math.PI * position.Y)));
        }

        public void drawScouts()
        {
            foreach (Scout scout in hive.Scouts)
            {
                drawEllipse(scout.position, 5, 0);
            }
        }

        public void drawAreas()
        {
            foreach (WorkArea area in hive.WorkAreas)
            {
                foreach (Point p in area.points)
                {
                    drawEllipse(p, hive.radius, 2);
                }
            }
        }
        public void drawWorkers()
        {
            if (iteration == 0)
            {
                foreach (Bee scout in hive.Workers)
                {
                    drawEllipse(scout.position, 5, 1);
                }
            }
            else
            {
                for(int i = 0; i<hive.Workers.Count; i++)
                {
                    if(i==0)
                    {
                        drawEllipse(hive.Workers[i].position, 8, 3);
                    }
                    else
                    {
                        drawEllipse(hive.Workers[i].position, 5, 1);
                    }
                }
            }
        }

        public void drawScene()
        {
            scene.Children.Clear();
            drawEllipse(new Point(0, 0), 10, 4);
            drawScouts();
            drawAreas();
            drawWorkers();

        }

        void drawEllipse(Point p, double r, int type)
        {
            Ellipse el = new Ellipse();
            SolidColorBrush brush = new SolidColorBrush();

            switch (type)
            {
                case 0: brush.Color = Color.FromArgb(255, 0, 0, 255); break;
                case 1: brush.Color = Color.FromArgb(255, 255, 255, 0); break;
                case 2: brush.Color = Color.FromArgb(0, 0, 0, 0); break;
                case 3: brush.Color = Color.FromArgb(255, 255, 0, 255); break;
                case 4: brush.Color = Color.FromArgb(255, 255, 0, 0); break;
            }
            el.Fill = brush;
            el.StrokeThickness = 1;
            el.Stroke = Brushes.Black;



            el.Width = r;
            el.Height = r;

            el.RenderTransform = new TranslateTransform((p.X + scene.Width / 2) - r / 2, (p.Y + scene.Height / 2) - r / 2);

            scene.Children.Add(el);
        }

        private void Start_B_Click(object sender, RoutedEventArgs e)
        {
            if (!started)
            {
                Start_B.Content = "Undo something";
                hive = new Hive(20, 10, 50, Restrigin_function, scene.Height, scene.Width, random);
                hive.createScouts();
                hive.CreateWorkers();
                timer.Start();
            }
            else
            {
                Start_B.Content = "Do something";
                timer.Stop();
                iteration = 0;
            }


        }

        private void Start_B_Copy_Click(object sender, RoutedEventArgs e)
        {
            drawEllipse(new Point(double.Parse(tb1.Text), double.Parse(tb2.Text)), 40, 0);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            hive.placeScouts();
            hive.rearangeAreas();
            hive.placeWorkers();
            hive.findBestFitness();
            iteration++;
            best_f.Content = "Best fitness: " + hive.best_fitness + '\n' + "Iteration: " + iteration + '\n' + "Position:  " + Math.Round(hive.Workers[0].position.X,2) + " : " + Math.Round(hive.Workers[0].position.Y,2);
            drawScene();
        }
    }
}
