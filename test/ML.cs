using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace test
{
    public class ML
    {

        public static weka.core.Instances GET_INSTS()
        {
         //   weka.core.converters.ConverterUtils.DataSource DATA = new weka.core.converters.ConverterUtils.DataSource();

            weka.core.Instances insts = new weka.core.Instances(new java.io.FileReader("testing.arff"));

            return insts;
        }

        public static string WEKA_GETMLP(weka.core.Instances insts)
        {
            string THEOUTPUT = " ";
            
            try
            {
                
                insts.setClassIndex(insts.numAttributes() - 1);
                weka.classifiers.functions.MultilayerPerceptron mlp = new weka.classifiers.functions.MultilayerPerceptron();

                //SETTING PARAMETERS
                weka.core.Utils.splitOptions("-L 0.3 -M 0.2 -N 500 -V 0 -S 0 -E 20 -H 3");
                mlp.buildClassifier(insts);


                THEOUTPUT = mlp.ToString();

                
            }
            catch (java.lang.Exception ex)
            {
                ex.printStackTrace();
            }

            return THEOUTPUT;

            //new Program().Method3


        }


        public static List<double> OUTPUTLAYER(string mlp)
        {
            List<string> outlayer = new List<string>();
            //List<string> hiddenlayer = new List<string>();
            
            string[] lines = mlp.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Linear"))
                {
                    for (int j = i+1; j < lines.Length; j++)
                    {
                        if (lines[j].Contains("Sigmoid")) break;
                        string[] words = lines[j].Split(' ');
                        //Console.WriteLine(words[4] + words[words.Length -1]);
                        if (words.Contains("Node") || words.Contains("Threshold")) outlayer.Add(words[words.Length-1]);

                    }
                }
                /*
                if (lines[i].Contains("Sigmoid"))
                {
                    for (int j = i+1; j < lines.Length; j++)
                    {
                        if (lines[j].Contains("Sigmoid")) break;
                        string[] words = lines[j].Split(' ');
                        
                        if (words.Contains("Attrib") || words.Contains("Threshold")) hiddenlayer.Add(words[words.Length - 1]);
                    }
                }*/
            }

            List<double> OUTLAYER = outlayer.Select(x => double.Parse(x)).ToList();
            // List<double> HIDDENLAYER = hiddenlayer.Select(x => double.Parse(x)).ToList();
            Console.WriteLine("DONE_OUT");
            return OUTLAYER;
        }

        public static List<double> HIDDENLAYER(string mlp)
        {

            List<string> hiddenlayer = new List<string>();


            string[] lines = mlp.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Sigmoid"))
                {
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        if (lines[j].Contains("Sigmoid")) break;
                        string[] words = lines[j].Split(' ');

                        if (words.Contains("Attrib") || words.Contains("Threshold")) hiddenlayer.Add(words[words.Length - 1]);
                    }
                }
            }

            List<double> HIDDENLAYER = hiddenlayer.Select(x => double.Parse(x)).ToList();
            Console.WriteLine("DONE_HIDDEN");
            return HIDDENLAYER;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Evaluation()
        {

            
        }


    }
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////
/* public static void Test()
 {
     Console.WriteLine("in test");
     try
     {
         weka.core.Instances insts = new weka.core.Instances(new java.io.FileReader("sample.arff"));
         weka.core.TestInstances test = new weka.core.TestInstances();
         weka.classifiers.meta.WeightedInstancesHandlerWrapper weights = new weka.classifiers.meta.WeightedInstancesHandlerWrapper();
         insts.setClassIndex(insts.numAttributes() - 1);
         weka.classifiers.functions.MultilayerPerceptron mlp = new weka.classifiers.functions.MultilayerPerceptron();

         //SETTING PARAMETERS
         weka.core.Utils.splitOptions("-L 0.3 -M 0.2 -N 500 -V 0 -S 0 -E 20 -H a");
         mlp.buildClassifier(insts);
         var testing = mlp;
         weka.classifiers.Evaluation eval = new weka.classifiers.Evaluation(insts);
         eval.evaluateModel(mlp, insts);



         Console.WriteLine(mlp);
     }
     catch(java.lang.Exception ex)
     {
         ex.printStackTrace();
     }

 }*/
