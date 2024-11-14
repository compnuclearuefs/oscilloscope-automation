using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace comunicacaoOciloscopio.classes
{
    // CLASSE COM OS DADOS OBTIDOS E MÉTODOS DE AUXÍLIO
    public class DataAcquisition
    {

        public string RisePath { get; set; }
        public string AmplitudePath { get; set; }
        public string AmpXrisesPath { get; set; }
        public List<string> Data { get; set; }
        public string Nwidth { get; set; }
        public string FallTime { get; set; }
        public string RiseTime { get; set; }
        public string Pkp { get; set; }


        // MÉTODO CONSTRUTOR
        public DataAcquisition(string risePath,
                               string amplitudePath,
                               string ampXrisesPath,
                               List<string> data,
                               string nWidht,
                               string fallTime,
                               string riseTime,
                               string pkp)
        {
            RisePath = risePath;
            AmplitudePath = amplitudePath;
            AmpXrisesPath = ampXrisesPath;
            Data = data;
            Nwidth = nWidht;
            FallTime = fallTime;
            RiseTime = riseTime;
            Pkp = pkp;

        }

        // ??? kkkkk
        public string ConvertExpoStr(string number)
        {
            string old_number = number;
            if (number.IndexOf("E-") != -1)
            {
                int end = number.IndexOf("E-");
                return number.Substring(0, end).Replace('.', ',');
                double number_converted = double.Parse(number.Substring(0, end).Replace('.', ','));

                //int expo = int.Parse(number[number.Length - 1].ToString());
                //number_converted = number_converted * Math.Pow(10, expo);
                //number = number_converted.ToString("F9");
            }
            else if (number.IndexOf("E") != -1)
            {
                int end = number.IndexOf("E");
                return number.Substring(0, end).Replace('.', ',');
                double number_converted = double.Parse(number.Substring(0, end).Replace('.', ','));
                int expo = int.Parse(number[number.Length - 1].ToString());
                number_converted = number_converted * Math.Pow(10, expo);
                number = number_converted.ToString("F9");
            }
            Console.WriteLine(old_number + " / " + number);
            return number;
        }

        /*
        static void iteratingAmpliRise(string aquisicaoRise, string aquisicaoAmplitude, string aquisicaoFallTime)
        {
            StringBuilder sb = new StringBuilder();
            string caminhoArquivo = "C:\\Users\\projetoMCA\\Downloads\\teste\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\netcoreapp3.1\\amplitude_dados_menoremaiorpaleta.txt";
            string arquivo = "ampXrise.txt";
            string arquivoRises = "C:\\Users\\projetoMCA\\Downloads\\teste\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\netcoreapp3.1\\rises.txt";
            string pathArq = System.IO.Path.Combine(folderPath, arquivo);


            // List<Aquisicao> listWithoutDuplicates = amplitudes.Distinct().ToList();
            StreamWriter file = new StreamWriter(ampXrisesPath, append: true);
            file.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",
                    aquisicaoRise.PadRight(10, ' '),
                    aquisicaoAmplitude.PadRight(10, ' '),
                    aquisicaoFallTime.PadRight(10, ' '),
                    DateTime.Now.ToString("dd/MM/yyyy").PadRight(10, ' '),
                    DateTime.Now.ToString("HH:mm").PadRight(10, ' '));
            file.Close();

        */

        /*
        public double CalculateJulianDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (month <= 2)
            {
                year--;
                month += 12;
            }

            // Cálculo da data juliana
            double jd = (int)(367 * year) - (int)(Math.Floor(7 * (year + Math.Floor((month + 9) / 12)) / 4)) + Math.Floor(275 * month / 9) + day + 1721013.5);

            // Adiciona a fração do dia
            jd += (date.Hour / 24.0) + (date.Minute / 1440.0) + (date.Second / 86400.0);

            return jd;
        }
        */
        public static double ConverterParaDataJuliana(DateTime dataHora)
        {
            // Definir o início da época juliana em 1 de janeiro de 4713 a.C.
            DateTime dataJulianaInicio = new DateTime(-4713, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            // Converter a diferença em dias, incluindo a fração do dia para a hora fornecida
            TimeSpan diferenca = dataHora.ToUniversalTime() - dataJulianaInicio;

            // Retornar o valor da data juliana
            return diferenca.TotalDays;
        }
    

    }

}
