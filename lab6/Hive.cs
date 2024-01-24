using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace lab6
{
    public class Hive
    {
        public List<Bee> Workers;
        public List<Scout> Scouts;
        public List<WorkArea> WorkAreas;

        Random random;

        public int bees;
        public int scouts;
        public int radius;

        public double best_fitness = double.MaxValue;

        public double H;
        public double W;

        public delegate double f_function(Point position);

        f_function fitness_function;

        public Hive(int bees, int scouts, int radius, f_function function, double H, double W, Random random)
        {
            Workers = new List<Bee>();
            Scouts = new List<Scout>();
            WorkAreas = new List<WorkArea>();
            this.bees = bees;
            this.scouts = scouts;
            this.radius = radius;
            this.H = H;
            this.W = W;
            this.random = random;
            this.fitness_function = function;
        }

        public void createScouts()
        {
            for(int i = 0; i<scouts; i++)
            {
                Scouts.Add(new Scout(new Point(0,0), radius));
            }
        }

        public void calcFitness(Bee bee)
        {
            bee.fitness = fitness_function(bee.position);
        }

        public void createWorkArea(Scout s)
        {
            List<Point> points = new List<Point>();
            points.Add(s.position);
            WorkAreas.Add(new WorkArea(points, s.radius, s.fitness));
        }

        public void placeScouts()
        {
            foreach(Scout scout in Scouts)
            {
                double x = GenerateRandomDouble(-W/2, W/2);
                double y = GenerateRandomDouble(-H/2, H/2);

                scout.setPosition(new Point(x, y));
                calcFitness(scout);
                createWorkArea(scout);
            }
        }

        public void rearangeAreas()
        {
            //CombineAreas();
            //shiftAreas();
            easyAreasCalc();
        }

        public void CombineAreas()
        {
            for (int i = 0; i < WorkAreas.Count; i++)
            {
                for (int j = 0; j < WorkAreas.Count; j++)
                {
                    if (WorkAreas[i].fitness > WorkAreas[j].fitness)
                    {
                        bool close = false;
                        if (j != i)
                        {
                            foreach (Point p in WorkAreas[i].points)
                            {
                                foreach (Point p1 in WorkAreas[j].points)
                                {
                                    if (CalculateDistance(p, p1) <= radius / 4)
                                    {
                                        close = true;
                                        break;
                                    }
                                }
                                if (close) break;
                            }
                        }

                        if (close)
                        {
                            rearangeAreas(WorkAreas[i], WorkAreas[j]);
                        }
                    }
                }
            }
        }

        public void shiftAreas()
        {
            for (int i = 0; i < WorkAreas.Count; i++)
            {
                for (int j = 0; j < WorkAreas.Count; j++)
                {
                    bool shift = false;
                    double dist = 0;
                    Point shiftpoint = new Point();

                    if (j != i)
                    {
                        foreach (Point p in WorkAreas[i].points)
                        {
                            foreach (Point p1 in WorkAreas[j].points)
                            {
                                if (areAreasIntersect(p,p1))
                                {
                                    shift = true;
                                    if(CalculateDistance(p,p1) > dist)
                                    {
                                        shiftpoint = p;
                                        dist = CalculateDistance(p, p1);
                                    }
                                }
                            }
                        }
                    }

                    if(shift)
                    {
                        for(int z = 0; z < WorkAreas[j].points.Count; z++)
                        {
                            WorkAreas[j].points[z] = ShiftPerpendicular(shiftpoint, WorkAreas[j].points[z], dist);
                        }
                    }

                }
            }
        }

        public void rearangeAreas(WorkArea a, WorkArea b)
        {
            a.points.AddRange(b.points);
            WorkAreas.Remove(b);
        }

        public double CalculateDistance(Point a,Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public bool areAreasIntersect(Point a, Point b)
        {
            double distanceBetweenCenters = CalculateDistance(a, b);

            if (distanceBetweenCenters <= 2 * radius) return true;
            else return false;
        }

        static Point ShiftPerpendicular(Point a, Point b, double distance)
        {
            // 1. Находим вектор между точками A и B
            double vectorX = b.X - a.X;
            double vectorY = b.Y - a.Y;

            // 2. Нормализуем вектор
            double length = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);
            double normalizedVectorX = vectorX / length;
            double normalizedVectorY = vectorY / length;

            // 3. Находим новые координаты точки, сдвинутой перпендикулярно на расстояние distance
            double newX = b.X + distance * normalizedVectorY;
            double newY = b.Y - distance * normalizedVectorX;

            return new Point(newX, newY);
        }

        public void CreateWorkers()
        {
            for(int i =0; i<bees; i++)
            {
                Workers.Add(new Bee(new Point(0,0)));
            }
        }

        public double GenerateRandomDouble(double min, double max)
        {
            return min + (max - min) * random.NextDouble();
        }

        public void placeWorkers()
        {
            WorkAreas.Sort((x, y) => y.fitness.CompareTo(x.fitness));

            if(WorkAreas.Count > 3)
            {
                while(WorkAreas.Count > 3)
                {
                    WorkAreas.Remove(WorkAreas.Last());
                }
            }


            int half = bees / 2;

            for(int i = 0; i< half; i++)
            {
                if(best_fitness != double.MaxValue)
                {
                    if (i != 0)
                    {
                        int index = random.Next(WorkAreas[0].points.Count);
                        Workers[i].position = GenerateRandomPointInCircle(WorkAreas[0].points[index]);
                        calcFitness(Workers[i]);
                    }
                    
                }
                else
                {
                    int index = random.Next(WorkAreas[0].points.Count);
                    Workers[i].position = GenerateRandomPointInCircle(WorkAreas[0].points[index]);
                    calcFitness(Workers[i]);
                }

                        
            }

            for(int i=1;i<3;i++)
            {
                for (int j = (half + half / 2 * (i - 1)); j < bees; j++)
                {
                    if (j != half + half / 2 * (i - 1) && best_fitness != double.MaxValue)
                    {
                        int index = random.Next(WorkAreas[i].points.Count);
                        Workers[j].position = GenerateRandomPointInCircle(WorkAreas[i].points[index]);
                        calcFitness(Workers[j]);
                    }
                    else
                    {
                        int index = random.Next(WorkAreas[i].points.Count);
                        Workers[j].position = GenerateRandomPointInCircle(WorkAreas[i].points[index]);
                        calcFitness(Workers[j]);
                    }
                }
            }

            Workers.Sort((x, y) => y.fitness.CompareTo(x.fitness));
        }

        public Point GenerateRandomPointInCircle(Point p)
        {
            bool t = false;
            double x = -1;
            double y = -1;
            while (!t)
            {
                double angle = 2 * Math.PI * random.NextDouble();
                double distance = (radius * Math.Sqrt(random.NextDouble())) / 2;

                x = p.X + distance * Math.Cos(angle);
                y = p.Y + distance * Math.Sin(angle);

                if((x > -W/2 && x < W) && (y > -H/2 && y < H))
                {
                    t = true;
                }
            }

            return new Point(x, y);
        }

        public void easyAreasCalc()
        {
            for (int i = 0; i < WorkAreas.Count; i++)
            {
                WorkArea area1 = WorkAreas[i];

                for(int j = 0; j< WorkAreas.Count; j++)
                {
                    WorkArea area2 = WorkAreas[j];
                    if (area1 != area2) if (area1.fitness >= area2.fitness) if (areAreasIntersect(area1.points[0], area2.points[0])) WorkAreas.Remove(area2);
                }
            }


        }

        public void findBestFitness()
        {
            double best = best_fitness;

            foreach (Scout s in Scouts) if (best > s.fitness) best = s.fitness;
            foreach(Bee b in Workers) if (best > b.fitness) best = b.fitness;

            best_fitness = best;
        }


    }
}
