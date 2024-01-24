using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab6
{
    public class WorkArea
    {
        public List<Point> points;
        public int radius;
        public double fitness;

        public WorkArea(List<Point> points, int radius, double fitness)
        {
            this.points = points;
            this.radius = radius;
            this.fitness = fitness;
        }

    }
}
