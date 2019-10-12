using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp
{
    class MainGraph
    {
        static void Main(string[] args)
        {
            Grafo graph = addGrafo();
        }
        
        private static void LoadJson(string filepath)
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                var json = sr.ReadToEnd();
                //var items = JsonConvert.DeserializeObject<List<GraphSchema>>(json);

                //foreach (var item in items)
                //{

                //}
            }
        }

        private static Grafo addGrafo()
        {
            Console.Write("Grafo ponderado [S/N]? ");
            var weighted = Console.ReadKey();

            if (weighted.Key.Equals(ConsoleKey.S))
            {
                Console.Write("Grafo dirigido [S/N]? ");
                var directed = Console.ReadKey();

                if (directed.Key.Equals(ConsoleKey.S))
                {
                    Console.Write("Deseja importar um arquivo [S/N]? ");
                    var import = Console.ReadKey();

                    if (import.Key.Equals(ConsoleKey.S))
                    {
                        // Ler do arquivo de grafo ponderado e dirigido
                        LoadJson("../../GraphsToImport/GrafoPonderadoDirigido.json");
                    }
                    else if (import.Key.Equals(ConsoleKey.N))
                    {
                        Console.Write("Digite o nome do grafo: ");
                        return new Grafo(Console.ReadLine(), true, true);
                    }
                }
                else if (directed.Key.Equals(ConsoleKey.N))
                {
                    Console.Write("Deseja importar um arquivo [S/N]? ");
                    var import = Console.ReadKey();

                    if (import.Key.Equals(ConsoleKey.S))
                    {
                        // Ler do arquivo de grafo ponderado e não dirigido
                        LoadJson("../../GraphsToImport/GrafoPonderadoNaoDirigido.json");
                    }
                    else if (import.Key.Equals(ConsoleKey.N))
                    {
                        Console.Write("Digite o nome do grafo: ");
                        return new Grafo(Console.ReadLine(), true, false);
                    }
                }
            }
            else if (weighted.Key.Equals(ConsoleKey.N))
            {
                Console.Write("Grafo dirigido [S/N]? ");
                var directed = Console.ReadKey();

                if (directed.Key.Equals(ConsoleKey.S))
                {
                    Console.Write("Deseja importar um arquivo [S/N]? ");
                    var import = Console.ReadKey();

                    if (import.Key.Equals(ConsoleKey.S))
                    {
                        // Ler do arquivo de grafo não ponderado e dirigido
                        LoadJson("../../GraphsToImport/GrafoNaoPonderadoDirigido.json");
                    }
                    else if (import.Key.Equals(ConsoleKey.N))
                    {
                        Console.Write("Digite o nome do grafo: ");
                        return new Grafo(Console.ReadLine(), false, true);
                    }
                }
                else if (directed.Key.Equals(ConsoleKey.N))
                {
                    Console.Write("Deseja importar um arquivo [S/N]? ");
                    var import = Console.ReadKey();

                    if (import.Key.Equals(ConsoleKey.S))
                    {
                        // Ler do arquivo de grafo não ponderado e não dirigido
                        LoadJson("../../GraphsToImport/GrafoNaoPonderadoNaoDirigido.json");
                    }
                    else if (import.Key.Equals(ConsoleKey.N))
                    {
                        Console.Write("Digite o nome do grafo: ");
                        return new Grafo(Console.ReadLine(), false, false);
                    }
                }
            }
        }
    }
}
