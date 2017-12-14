using System;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using Firebase.Auth;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;

namespace test
{
    class Program
    {
        public Section section = new Section();
        public Output_Neuron output_Neuron = new Output_Neuron();
        public Hidden_Neuron signoid1 = new Hidden_Neuron();
        public Hidden_Neuron signoid2 = new Hidden_Neuron();
        public Hidden_Neuron signoid3 = new Hidden_Neuron();
        private double howsgoing;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task pushFirebase_coef(List<double> output, List<double> hidden)
        {

            var auth = "nRj3htqLYTcSrq9pH5QZB4gAfLu1Cp1BU8Uvs5ra";
            var firebaseUrl = "https://flexware-soc.firebaseio.com";

            output_Neuron.threshold = output.ElementAt(0);
            output_Neuron.Node1 = output.ElementAt(1);
            output_Neuron.Node2 = output.ElementAt(2);
            output_Neuron.Node3 = output.ElementAt(3);

            signoid1.threshold = hidden.ElementAt(0);
            signoid1.time_elapse = hidden.ElementAt(1);
            signoid1.voltage = hidden.ElementAt(2);
            signoid1.current = hidden.ElementAt(3);
            signoid1.capacity = hidden.ElementAt(4);
            signoid1.internal_resistor = hidden.ElementAt(5);

            signoid2.threshold = hidden.ElementAt(6);
            signoid2.time_elapse = hidden.ElementAt(7);
            signoid2.voltage = hidden.ElementAt(8);
            signoid2.current = hidden.ElementAt(9);
            signoid2.capacity = hidden.ElementAt(10);
            signoid2.internal_resistor = hidden.ElementAt(11);

            signoid3.threshold = hidden.ElementAt(12);
            signoid3.time_elapse = hidden.ElementAt(13);
            signoid3.voltage = hidden.ElementAt(14);
            signoid3.current = hidden.ElementAt(15);
            signoid3.capacity = hidden.ElementAt(16);
            signoid3.internal_resistor = hidden.ElementAt(17);

            var fb = new FirebaseClient(firebaseUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(auth) });

            await fb.Child("attributes").Child("OUT").PutAsync(output_Neuron);
            await fb.Child("attributes").Child("Hidden_N1").PutAsync(signoid1);
            await fb.Child("attributes").Child("Hidden_N2").PutAsync(signoid2);
            await fb.Child("attributes").Child("Hidden_N3").PutAsync(signoid3);

            Console.WriteLine("PUSH_COEF_DONE");

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task pushFirebase_Data(string[] values)
        {

            var auth = "nRj3htqLYTcSrq9pH5QZB4gAfLu1Cp1BU8Uvs5ra";
            var firebaseUrl = "https://flexware-soc.firebaseio.com";
            /*
            double TIME = Convert.ToDouble(values[0]);
            double VOLTAGE = Convert.ToDouble(values[1]);
            double CURRENT = Convert.ToDouble(values[2]);
            double CAPACITY = Convert.ToDouble(values[3]);
            double INTERNAL_RESISTOR = Convert.ToDouble(values[4]);
            double SOC = Convert.ToDouble(values[5]);

            Inputs test = new Inputs();
            test.time = TIME;
            test.voltage = VOLTAGE;
            test.current = CURRENT;
            test.capacity = CAPACITY;
            test.internal_resistor = INTERNAL_RESISTOR;
            test.SOC = SOC;
            */

            Real_Data test = new Real_Data();
            test.time = Convert.ToDouble(values[0]);
            test.v1 = Convert.ToDouble(values[1]);
            test.current = Convert.ToDouble(values[2]);

            var fb = new FirebaseClient(firebaseUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(auth) });

            //await fb.Child("testing_data").PutAsync(test);
            var logs = await fb.Child("another_testing").PostAsync(test);

            Console.WriteLine("pushed");
            Console.WriteLine();
            Console.WriteLine();

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////


        public async Task getFirebase_Data()
        {
            var inputs = new Inputs();

            section.capacity = new List<double>();
            section.time = new List<double>();
            section.current = new List<double>();
            section.voltage = new List<double>();
            section.soc = new List<double>();
            section.internal_resistance = new List<double>();

            var auth = "nRj3htqLYTcSrq9pH5QZB4gAfLu1Cp1BU8Uvs5ra";
            var firebaseUrl = "https://flexware-soc.firebaseio.com";

            var fb = new FirebaseClient(firebaseUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(auth) });



            var observable = await fb.Child("this_one").OnceAsync<Inputs>();


            foreach (var datas in observable)
            {
                inputs.internal_resistor = datas.Object.internal_resistor;
                inputs.time = datas.Object.time;
                inputs.voltage = datas.Object.voltage;
                inputs.capacity = datas.Object.capacity;
                inputs.current = datas.Object.current;
                inputs.SOC = datas.Object.SOC;

                Add_To_List(inputs);
            }
            Console.WriteLine("DONE_GETFIRE");

            //   Method3();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Add_To_List(Inputs input)
        {
            section.capacity.Add(input.capacity);
            section.time.Add(input.time);
            section.voltage.Add(input.voltage);
            section.current.Add(input.current);
            section.internal_resistance.Add(input.internal_resistor);
            section.soc.Add(input.SOC);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async void getFirebase_Coef()
        {
            var auth = "nRj3htqLYTcSrq9pH5QZB4gAfLu1Cp1BU8Uvs5ra";
            var firebaseUrl = "https://flexware-soc.firebaseio.com";
            var fb = new FirebaseClient(firebaseUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(auth) });

            output_Neuron = await fb.Child("attributes").Child("OUT").OnceSingleAsync<Output_Neuron>();
            signoid1 = await fb.Child("attributes").Child("Hidden_N1").OnceSingleAsync<Hidden_Neuron>();
            signoid2 = await fb.Child("attributes").Child("Hidden_N2").OnceSingleAsync<Hidden_Neuron>();
            signoid3 = await fb.Child("attributes").Child("Hidden_N3").OnceSingleAsync<Hidden_Neuron>();

            Console.WriteLine("DONE_GETTING_COEF");

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Better_Read()
        {
            using (var reader = new StreamReader("cell_training_data_FINAL.csv"))
            {

                while (!reader.EndOfStream)
                {
                    bool run = true;
                    while (run)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        new Program().pushFirebase_Data(values).Wait();
                        //Console.WriteLine(values[0]);
                        run = false;
                    }
                    if (!run) Thread.Sleep(10); run = true;

                }
            }
            Console.WriteLine("DONE PUSHING");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        static void Main(string[] args)
        {
            Program pro = new Program();
            List<double> t1 = new List<double>();
            List<double> t2 = new List<double>();
           // pro.getFirebase_Coef();

            int x;
            do
            {
                x = Console.Read();
                char c = Convert.ToChar(x);


                if (c == 'c') pro.getFirebase_Data();
                if (c == 'w') pro.Write_ARFF();
                if (c == '1')
                {
                    t1 = ML.OUTPUTLAYER(ML.WEKA_GETMLP(ML.GET_INSTS()));
                    t2 = ML.HIDDENLAYER(ML.WEKA_GETMLP(ML.GET_INSTS()));
                    pro.pushFirebase_coef(t1, t2);

                }
                if (c == '5') Read();
                if (c == '2') pro.getFirebase_Coef();
                if (c == '7')
                {
                    //pro.Estimation(294.076116, 4.1, -7.49, 7.458977654, 4.009090909);
                }

            } while (true);


        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Read()
        {
            Thread readThread = new Thread(Read);
            Program pro = new Program();
            string message;
            pro.getFirebase_Coef();
            
            SerialPort _serialPort = new SerialPort("COM8", 9600, Parity.None, 8, StopBits.One);
            _serialPort.Open();
            double temp = 0;
            double time_interval = (10.14/3600);
            double ttemp = 0;
            while (true)
            {
                Thread.Sleep(2500);
                message = _serialPort.ReadLine();
                Console.WriteLine(message);

                string[] nums = message.Split(',');
                double a = Double.Parse(nums[0]);
                double b = Double.Parse(nums[1]);
                double c = Double.Parse(nums[2]);

                if (a < 10) ttemp = 0;


                temp = time_interval * c;
                ttemp += temp;
                pro.Estimation(a, b, c, 7.458977654, 4.009090909);
                Console.WriteLine("calculated one is : " + ((ttemp / 7.5) * 100 + 100));
                Console.WriteLine();

                pro.pushFirebase_Data(nums);
                
            }
            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Write_ARFF()
        {
            string filename;
            string[] atts = { "@attribute time_elapsed numeric", "@attribute voltage numeric",
                "@attribute current numeric", "@attribute capacity numeric",
                "@attribute internal_resistance numeric","@attribute state_of_charge numeric" };
            string data = "@data";

            while (true)
            {
                filename = Console.ReadLine();
                if (filename.Any(x => char.IsLetter(x))) break;
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename + ".arff"))
            {

                file.WriteLine("@relation " + filename);

                for (int i = 0; i < atts.Length; i++)
                {
                    file.WriteLine(atts[i]);
                }


                file.WriteLine(data);


                for (int i = 0; i < section.capacity.Count; i++)
                {
                    file.WriteLine(section.time.ElementAt(i) + "," +
                        section.voltage.ElementAt(i) + "," + section.current.ElementAt(i) + "," +
                        section.capacity.ElementAt(i) + "," + section.internal_resistance.ElementAt(i) + "," +
                        section.soc.ElementAt(i)
                        );
                }
            }
            Console.Write("DONE_WRITE");
        }
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*public void gotoEstimate()
        {
            string line;

            while (true)
            {
                line = Console.ReadLine();
                if (line.Any(x => char.IsNumber(x))) break;
            }
            string[] nums = line.Split(',');
            double a = Double.Parse(nums[0]);
            double b = Double.Parse(nums[1]);
            double c = Double.Parse(nums[2]);
            double d = Double.Parse(nums[3]);
            double e = Double.Parse(nums[4]);
            Estimation(a,b,c,d,e);
            
        }*/

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Estimation(double time, double voltage, double current, double capacity, double internal_resistance)
        {

            var norm_T = (((time - 0) * 2) / (3728.188 - 0) - 1);
            var norm_V = (((voltage - 3.07) * 2) / (4.21 - 3.07) - 1);
            var norm_C = (((capacity - 6.822) * 2) / (7.769 - 6.822) - 1);
            var norm_A = (((current - (-7.643)) * 2) / (0 - (-7.643)) - 1);
            var norm_R = (((internal_resistance - 1.946) * 2) / (65.416 - 1.946) - 1);

            double N1 = 0;
            double N2 = 0;
            double N3 = 0;
            double OUT = 0;

            N1 += signoid1.threshold;
            N1 += signoid1.time_elapse * norm_T;
            N1 += signoid1.voltage * norm_V;
            N1 += signoid1.current * norm_A;
            N1 += signoid1.capacity * norm_C;
            N1 += signoid1.internal_resistor * norm_R;

            N1 = 1 / (1 + Math.Exp(N1 * -1));

            N2 += signoid2.threshold;
            N2 += signoid2.time_elapse * norm_T;
            N2 += signoid2.voltage * norm_V;
            N2 += signoid2.current * norm_A;
            N2 += signoid2.capacity * norm_C;
            N2 += signoid2.internal_resistor * norm_R;

            N2 = 1 / (1 + Math.Exp(N2 * -1));


            N3 += signoid3.threshold;
            N3 += signoid3.time_elapse * norm_T;
            N3 += signoid3.voltage * norm_V;
            N3 += signoid3.current * norm_A;
            N3 += signoid3.capacity * norm_C;
            N3 += signoid3.internal_resistor * norm_R;

            N3 = 1 / (1 + Math.Exp(N3 * -1));

            OUT += output_Neuron.threshold;
            OUT += N1 * output_Neuron.Node1;
            OUT += N2 * output_Neuron.Node2;
            OUT += N3 * output_Neuron.Node3;

            Console.WriteLine("ESTIMATED : " + (OUT + 1) * 50);
            this.howsgoing = (OUT + 1) * 50;

        }
    }


}