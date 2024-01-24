using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace lab6ai
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CSwarm swarm;
        Vector target;
        int iteration;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 150) };
            timer.Tick += new EventHandler(Timer_Tick);
        }

        double square_summ(Vector position)
        {
            return -((position.X * position.X) + (position.Y * position.Y));
        }

        double Restrigin_function(Vector position)
        {
            return -(((position.X * position.X) - 10 * Math.Cos(2 * Math.PI * position.X)) + ((position.Y * position.Y) - 10 * Math.Cos(2 * Math.PI * position.Y)));
        }

        void drawEllipse(Vector p, double r, int type)
        {
            Ellipse el = new Ellipse();
            SolidColorBrush brush = new SolidColorBrush();

            if (type == 0) brush.Color = Color.FromArgb(255, 0, 0, 255);
            else
            {
                if (type == 1) brush.Color = Color.FromArgb(255, 255, 0, 0);
                else brush.Color = Color.FromArgb(255, 255, 255, 0);
            }
            el.Fill = brush;
            el.StrokeThickness = 1;
            el.Stroke = Brushes.Black;
            el.Width = r;
            el.Height = r;
            el.RenderTransform = new TranslateTransform((p.X+scene.Width/2) - r/2, (p.Y + scene.Height/2) - r/2);
            scene.Children.Add(el);
        }

        void drawLine(Vector p1, Vector p2, double r)
        {
            Line line = new Line();
            line.Stroke = Brushes.Black;
            line.X1 = (p1.X) + scene.Width / 2;
            line.X2 = (p2.X) + scene.Width / 2;
            line.Y1 = (p1.Y) + scene.Height / 2;
            line.Y2 = (p2.Y) + scene.Height / 2;
            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.StrokeThickness = 1;
            scene.Children.Add(line);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            iteration++;
            swarm.update_particles();

            List<COutputData> particles = swarm.get_particles_data();
            draw_scene(particles);

            if (Vector.Subtract(target, swarm.global_best).Length <0.01)
                timer.Stop();
        }

        void draw_scene(List<COutputData> particles)
        {
            scene.Children.Clear();

            drawEllipse(target, 50, 0);

            foreach(COutputData particle in particles)
            {
                drawEllipse(particle.position, 10, 2);
                drawLine(particle.position, particle.position + particle.velocity, 10);
            }

            drawEllipse(swarm.global_best, 10, 1);

            LB_best.Content = swarm.global_best.X.ToString("0:00") + " : " + swarm.global_best.Y.ToString("0:00");
            LB_iteration.Content = "Iteration: " + iteration.ToString();
        }

        private void Start_B_Click(object sender, RoutedEventArgs e)
        {
            iteration = 0;

            swarm = new CSwarm(30, new Vector(-scene.Width / 2, -scene.Height / 2), new Vector(scene.Width / 2, scene.Height / 2), 30, Restrigin_function);
            swarm.w = 0.8;
            swarm.c1 = 0.2;
            swarm.c2 = 0.2;

            timer.Start();
        }
    }
}
