using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab6
{
    public class Bee
    {
        public double fitness;
        public Point position;

        public Bee(Point position) 
        {
            this.position = position;
        }

        public void setPosition(Point p)
        {
            this.position = p;
        }
    }
}
