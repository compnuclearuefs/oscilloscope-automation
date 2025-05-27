using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace comunicacaoOciloscopio.classes
{
    // CLASSE COM OS DADOS OBTIDOS E MÉTODOS DE AUXÍLIO
    public class Data
    {

        public string RisePath { get; set; }
        public string AmplitudePath { get; set; }
        public string AmpXrisesPath { get; set; }
        public List<string> DataList { get; set; }
        public string Nwidth { get; set; }
        public string FallTime { get; set; }
        public string RiseTime { get; set; }
        public string Pkp { get; set; }


        // MÉTODO CONSTRUTOR
        public Data(string risePath,
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

    }

}
