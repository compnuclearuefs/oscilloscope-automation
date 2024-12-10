using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TekVISANet;

namespace comunicacaoOciloscopio.classes
{
    // CLASSE PÚBLICA TEKVISA
    public class TekVISA
    {
        // ATRIBUTO DA CLASSE
        private VISA connector;

        // CONSTRUTOR PARA INICIALIZAR O OBJETO TEKVISA
        public TekVISA()
        {
            this.connector = new VISA();
        }

        // MÉTODO PARA OBTER OS RECURSOS CONECTADOS
        public List<string> GetResources()
        {
            ArrayList _resources = new ArrayList();
            List<string> resources = new List<string>();

            this.connector.FindResources("?*", out _resources);
            foreach (string r in _resources)
                resources.Add(r);

            return resources;
        }

        // MÉTODO PARA CONVERTER LIST<STRING> PARA ARRAY
        public string[] GetArrayResources()
        {
            List<string> resources = GetResources();
            return resources.ToArray();
        }

        // MÉTODO PARA OBTER O ID DO DISPOSITIVO
        public string GetID()
        {
            string response;

            this.connector.Write("*IDN?");
            bool status = this.connector.Read(out response);
            if (status) return response;

            throw new Exception("Não foi possível realizar a leitura do ID");
        }

        // MÉTODO PARA CONECTAR AO OSCILOSCÓPIO
        public bool Connect(string resourceId)
        {
            bool connected = this.connector.Open(resourceId);
            if (!connected)
                throw new Exception("Não conectou com o osciloscópio");

            return true;
        }

        // MÉTODO PARA CONFIGURAR O CANAL
        public void Configure(OscilloscopeConfigs oscilloscopeConfigs)
        {
            //
            //this.connector.Write("HEADER OFF;:DATA:SOU CH1;:DATA:ENCDG SRPbinary;:DATA:WIDTH 2;:DATA:START 1;:DATA:STOP");

            /*
            this.connector.Write($"HEADER OFF;:DATA:SOU {channel};:DATA:ENCDG SRPbinary;:DATA:WIDTH 2;:DATA:START 1;:DATA:STOP");
            this.connector.Write("ACQUIRE:STOPAFTER SEQUENCE;MODE SAMPLE;");
            this.connector.Write($"TRIG:MAI:MOD NORMAL;TYPE EDGE;LEVEL -8.0E-1;VIDEO:SOURCE{channel};:TRIG:MAI:EDGE:SLOPE FALL;COUP DC;");
            this.connector.Write($"{channel}:SCA 2;POS 1;:HOR:SCA 5.0E-9;POS 0.0E0");
            */
            this.connector.Write(oscilloscopeConfigs.DataFormatSet);
            this.connector.Write(oscilloscopeConfigs.AcquireSet);
            this.connector.Write(oscilloscopeConfigs.TriggerSet);
            this.connector.Write(oscilloscopeConfigs.VisualizationSet);

            /*
            this.connector.Write("HEADER OFF");
            //Setting the oscilloscope for save data
            this.connector.Write(String.Format("DATA:SOU {0}", channel));

            //Setting the wave length that will be stored
            this.connector.Write("DATA:ENCDG SRPbinary");
            this.connector.Write("DATA:WIDTH 2");
            this.connector.Write("DATA:START 1");
            this.connector.Write("DATA:STOP 2500");
            */
        }

        // MÉTODO PARA DEFINIR O CANAL ATUAL
        public void SetChannel(string channel)
        {
            this.connector.Write($"DATA:SOURCE {channel}");
            this.connector.Write($"MEASU:IMM:SOURCE {channel}");
        }

        public void Write(string command)
        {
            this.connector.Write(command);
        }

        public string Query(string command)
        {
            string response;
            this.connector.Query(command, out response);
            return response;
        }


        // MÉTODO PARA CONFIGURAR MEDIÇÕES
        public void SetMeasurement(List<string> measurements, string channel)
        {
            for (int i = 0; i < measurements.Count; i++)
            {
                //if (i > 0) queryCommand += ";:";
                this.connector.Write($"MEASUREMENT:MEAS{i + 1}:SOURCE {channel}");
                this.connector.Write($"MEASUREMENT:MEAS{i + 1}:TYPE {measurements[i]}");
            }
        }

        // MÉTODO PARA CAPTURAR A FORMA DE ONDA
        public string CaptureWaveform(string channel)
        {
            string response;
            this.connector.Write($"DATA:SOURCE {channel};:CURVE?");
            bool status = this.connector.Read(out response);
            if (status) return response;

            throw new Exception("Não foi possível capturar a forma de onda.");
        }

        // MÉTODO PARA FAZER ESPERAR DADO
        public bool WaitData()
        {
            /*
            string stateTrigger = "";
            this.connector.Query("TRIGger:STATE?", out stateTrigger);
            return stateTrigger.Equals("TRIGGER") || stateTrigger.Equals("SAVE");
            */
            string stateTrigger = "";
            this.connector.Query("*OPC?", out stateTrigger);
            return stateTrigger.Equals("1");
            //*/
        }

        // MÉTODO PARA COMEÇAR MAEPAMENTO DE EVENTOS
        public void Run()
        {
            this.connector.Write("ACQUIRE:STATE ON;");
        }
        
        public List<string> GetMeasurementsIMM(List<string> masurements)
        {
            string meas;
            List<string> all = new List<string> { };
            for (int i = 0; i < masurements.Count(); i++)
            {
                this.connector.Write($"MEASU:IMM:TYPE {masurements[i]}");
                meas = this.Query("MEASU:IMM:VAL?");
                this.connector.Query("*ESR?", out string esr);
                if (esr != "16")
                {
                    all.Add(meas);
                }
            }
            return all;
        }

        public List<string> GetData()
        {
            string all;
            List<string> data = new List<string>();
            bool status;
            status = this.connector.Query($"MEASU:MEAS1:VAL?;:MEASU:MEAS2:VAL?;:MEASU:MEAS3:VAL?;:MEASU:MEAS4:VAL?", out all); 
            if (!status)
                Console.WriteLine("Erro ao recuperar dados");

            data = all.Split(';').ToList();
            /*for (int i = 0; i <= 4; i++)
            {
                status = this.connector.Query($"MEASU:MEAS{i+1}:VAL?", out all);//;:MEASU:MEAS5:VAL?
                //status = this.connector.Read(out all);
                if (!status)
                    Console.WriteLine("Erro ao recuperar dados");
                data.Add(all);
            }*/
            //this.connector.Write("MEASU:MEAS1:VAL?");//;:MEASU:MEAS5:VAL?
            //bool status = this.connector.Read(out all);
            /*
            if (!status)
                Console.WriteLine("Erro ao recuperar dados");
            return all.Split(';').ToList();
            */
            return data;

        }

        public List<string> GetTriggerConf()
        {
            string val;
            List<string> trig = new List<string>();
            bool status;
            status = this.connector.Query($"TRIG:MAI:MOD?;TYPE?;LEVEL?;VIDEO:SOURCE?;:TRIG:MAI:EDGE:SLOPE?;COUP?;", out val);
            if (!status)
                Console.WriteLine("Erro ao recuperar trigger");

            trig = val.Split(';').ToList();
            return trig;

        }

        public List<string> GetVisualizationConf()
        {
            string val;
            List<string> vis;
            bool status;
            status = this.connector.Query($"CH1:SCA?;POS?;:HOR:SCA?;POS?", out val);
            if (!status)
                Console.WriteLine("Erro ao recuperar visual");

            vis = val.Split(';').ToList();
            return vis;

        }
    }

}
