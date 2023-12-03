using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab6ai
{
    public class CSwarm
    {
        Random rnd;

        List<CParticle> particles;

        public double c1;
        public double c2;
        public double w;

        public Vector global_best;
        public double gb_fitness;

        Vector upper_left;
        Vector bottom_right;

        public delegate double f_function(Vector position);

        f_function fitness_function;

        public CSwarm(int particle_count, Vector upper_left, Vector bottom_right, double max_velocity, f_function fitness_function)
        {
            this.fitness_function = fitness_function;

            rnd = new Random();

            this.upper_left = upper_left;
            this.bottom_right = bottom_right;

            particles = new List<CParticle>();

            for(int i=0; i<particle_count; i++)
            {
                double x = rnd.NextDouble() * (bottom_right.X - upper_left.X) + upper_left.X;
                double y = rnd.NextDouble() * (bottom_right.Y - upper_left.Y) + upper_left.Y;
                Vector position = new Vector(x, y);

                Vector velocity = (randomDir() * (rnd.NextDouble() * max_velocity));

                CParticle particle = new CParticle(position, velocity);
                particle.pb_fitness = fitness_function(particle.position);
                particles.Add(particle);

            }

            gb_fitness = double.NegativeInfinity;
            foreach(CParticle particle in particles)
            {
                if (particle.pb_fitness > gb_fitness)
                { 
                    gb_fitness = particle.pb_fitness;
                    global_best = particle.position;
                }
            }

            c1 = c2 = 0.1;
            w = 0.8;
        }

        Vector randomDir()
        {
            Vector res = new Vector();

            double r = rnd.NextDouble();

            res.X = Math.Cos(Math.PI * 2 * r);
            res.Y = Math.Sin(Math.PI * 2 * r);

            return res;
        }

        void borders_controll(CParticle particle)
        {
            if (particle.position.X < upper_left.X || particle.position.X > bottom_right.X)
                particle.velocity.X = 0;
            if(particle.position.Y < upper_left.Y || particle.position.Y > bottom_right.Y)
                particle.velocity.Y = 0;
        }

        public void update_particles()
        {
            foreach(CParticle particle in particles)
            {
                double r1 = rnd.NextDouble();
                double r2 = rnd.NextDouble();

                particle.update_position(w, c1, c2, r1, r2, global_best);
                borders_controll (particle);
            }

            foreach(CParticle particle in particles)
            {
                double fitness = fitness_function(particle.position);
                if(fitness>particle.pb_fitness)
                {
                    particle.pb_fitness = fitness;
                    particle.personal_best = particle.position;

                    if(fitness>gb_fitness)
                    {
                        gb_fitness = fitness;
                        global_best = particle.position;
                    }
                }
            }

        }

        public List<COutputData> get_particles_data()
        {
            List<COutputData> particles_data = new List<COutputData>();

            foreach(CParticle particle in particles)
            {
                particles_data.Add(particle.get_data());
            }
            return particles_data;
        }


    }
}
