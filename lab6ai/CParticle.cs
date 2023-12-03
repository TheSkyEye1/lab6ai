using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab6ai
{
    public class CParticle
    {
        public Vector position;
        public Vector velocity;
        public double pb_fitness;
        public Vector personal_best;

        public CParticle(Vector position, Vector velocity)
        {
            this.position = position;
            this.velocity = velocity;
            this.pb_fitness = double.NegativeInfinity;
            this.personal_best = position;
        }

        public void update_position(double w, double c1, double c2, double r1, double r2, Vector global_best)
        {
            velocity = (w * velocity) + (c1 * r1 * (personal_best - position)) + (c2 * r2 * (global_best - position));
            position += velocity;
        }

        public COutputData get_data()
        {
            return new COutputData(position, velocity);
        }
    }
}
