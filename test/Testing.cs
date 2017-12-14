using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class Inputs
    {
        public double time { get; set; }
        public double voltage { get; set; }
        public double current { get; set; }
        public double capacity { get; set; }
        public double internal_resistor { get; set; }
        public double SOC { get; set; }

    }

    public class Hidden_Neuron
    {
        public double time_elapse { get; set; }
        public double voltage { get; set; }
        public double current { get; set; }
        public double capacity { get; set; }
        public double internal_resistor { get; set; }
        public double threshold { get; set; }
    }
    public class Output_Neuron
    {
        public double threshold { get; set; }
        public double Node1 { get; set; }
        public double Node2 { get; set; }
        public double Node3 { get; set; }
    }
    public class Real_Data
    {
        public double time { get; set; }
        public double v1 { get; set; }
        public double current { get; set; }
    }
    public class Section
    {
        public List<double> voltage { get; set; }
        public List<double> time { get; set; }
        public List<double> current { get; set; }
        public List<double> capacity { get; set; }
        public List<double> internal_resistance { get; set; }
        public List<double> soc { get; set; }
    }
}
