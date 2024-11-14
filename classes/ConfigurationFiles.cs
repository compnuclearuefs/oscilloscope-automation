using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace comunicacaoOciloscopio.classes
{
    // CLASSE COM AS CONFIGS DO USUARIO
    public class UserConfigs
    {
        public string Directory { get; set; }
        public string Resource { get; set; }
        public string Channel { get; set; }
        public int MaxEvents { get; set; }
        public string TypeExperiment { get; set; }

        // Construtor sem parâmetros (necessário para desserialização)
        public UserConfigs() { }

        public UserConfigs(string directory, string resource, string channel, int maxEvents, string typeExperiment)
        {
            Directory = directory;
            Resource = resource;
            Channel = channel;
            MaxEvents = maxEvents;
            TypeExperiment = typeExperiment;
        }
    }

    public class OscilloscopeConfigs
    {
        public string DataFormatSet { get; set; }
        public string AcquireSet { get; set; }
        public string TriggerSet { get; set; }
        public string VisualizationSet { get; set; }

        // Construtor sem parâmetros (necessário para desserialização)
        public OscilloscopeConfigs() { }

        public OscilloscopeConfigs(string dataFormatSet, string acquireSet, string triggerSet, string visualizationSet)
        {
            DataFormatSet = dataFormatSet;
            AcquireSet = acquireSet;
            TriggerSet = triggerSet;
            VisualizationSet = visualizationSet;
        }
    }

    public class JsonStorageService<T>
    {
        public string FilePath { get; set; } // Agora FilePath tem get e set públicos

        public JsonStorageService(string filePath)
        {
            FilePath = filePath;
        }

        // Método para salvar o objeto em JSON
        public void Save(T obj)
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Dispose();
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(obj, options);

            File.WriteAllText(FilePath, jsonString);
        }
      
        // Método para carregar o objeto do JSON
        public T Load()
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Arquivo de configuração não encontrado.");
            }

            String jsonString = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}


