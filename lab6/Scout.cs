using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab6
{
    public class Scout : Bee
    {
        public int radius;

        public Scout(Point position, int radius) : base(position)
        {
            this.radius = radius;
        }
    }
}
