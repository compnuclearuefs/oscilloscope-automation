using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace comunicacaoOciloscopio.classes
{
    public class FileManager
    {
        // BEGINNING directory SET
        public string directory { get; set; } // Agora FilePath tem get e set públicos

        public FileManager(string in_directory)
        {
            directory = in_directory;
        }
        // END directory SET

        //public const string directory = "C:/Users/projetoMCA/Desktop/kauaWorkspace/projetoComunicacao/EXPERIMENTOS_TESTE/";
        public const string arqName = "data.txt";
        public static string filePath { get; set; }

        public void createFile(String pathName)
        {
            try
            {
                String folderPath = directory + pathName;
                // Verifica se o diretório existe, caso contrário, cria
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                filePath = Path.Combine(folderPath, arqName);
                // Verificar se o arquivo existe, caso contrário, criar
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();

                    string[] header = { "Acquisition time", "Neg Width", "Fall time", "Rise time", "Pk-Pk"};
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        foreach (string elemento in header)
                        {
                            // Escrevendo o elemento formatado no arquivo
                            sw.Write(elemento.PadRight(12) + "\t"); // Usando + para concatenar
                        }
                        sw.WriteLine(); // Quebrar linha após cada linha da tabela
                    }
                }
                Console.WriteLine($"Arquivo criado: {filePath}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }
        }


        static string timezone = "E. South America Standard Time"; // Define o fuso horário na zona GMT-3 Brasil/Brasília --> "E. South America Standard Time"
        public static void update(DataAcquisition data)
        {
            DateTime currentTime = DateTime.UtcNow;
            double julianDate = Tools.ConvertDateToJulian(currentTime);
            //String horaAtual = currentTime.ToString("HH:mm:ss");
            //String dataAtual = currentTime.ToString("dd/MM/yyyy");

            Console.WriteLine("Tempo: " + Tools.ConvertUtcToTimeZone(currentTime, timezone));
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.Write(($"{Math.Round(Tools.applyJulianOffset(julianDate), 6)}").PadRight(12) + "\t");
                // Iterando sobre cada elemento da lista 'data.Data'
                foreach (string elemento in data.Data)
                {
                    // Escrevendo o elemento formatado no arquivo
                    sw.Write(elemento.PadRight(12) + "\t"); // Usando + para concatenar
                }
                sw.WriteLine(); // Quebrar linha após cada linha da tabela

            }
        }
    }
}
