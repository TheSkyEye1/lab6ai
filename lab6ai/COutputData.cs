using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab6ai
{
    public class COutputData
    {
        public Vector position;
        public Vector velocity;

        public COutputData(Vector position, Vector velocity) 
        {
            this.position = position;
            this.velocity = velocity;  
        }

    }
}
