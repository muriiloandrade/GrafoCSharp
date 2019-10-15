﻿using Newtonsoft.Json;
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
        static void Main()
        {
            Grafo g = addGrafo();

            while (true)
            {
                Console.Write("Escolha uma opção: \n" +
                    "1 - Add Vértice\n" +
                    "2 - Remover Vértice\n" +
                    "3 - Add Aresta\n" +
                    "4 - Remover Aresta\n" +
                    "5 - Matriz\n" +
                    "6 - Exibir vértices adjacentes\n" +
                    "Escolha uma opção: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Digite o nome do vértice a ser adicionado: ");
                        g.addVertice(new Vertice(Console.ReadLine()));
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "2":
                        Console.Write("Digite o nome do vértice a ser excluído: ");
                        g.removeVertice(g.getVerticePorNome(Console.ReadLine()));
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "3":
                        Console.Write("Digite o nome do vértice de origem: ");
                        Vertice vInicial = g.getVerticePorNome(Console.ReadLine());
                        Console.Write("Digite o nome do vértice de destino: ");
                        Vertice vFinal = g.getVerticePorNome(Console.ReadLine());
                        Console.Write("Digite um nome para a aresta: ");
                        g.addAresta(new Aresta(vInicial, vFinal, Console.ReadLine()));
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "4":
                        Console.Write("Digite o nome da aresta a ser excluída: ");
                        g.removeAresta(g.getArestaPorNome(Console.ReadLine()));
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "5":
                        if (g.dirigido)
                        {
                            g.showMatrizAdjacenteDirigido();
                        }
                        else
                        {
                            g.showMatrizAdjacenteNaoDirigido();
                        }
                        Console.WriteLine();
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "6":
                        Console.Write("Digite o nome do vértice que deseja conhecer os adjacentes: ");
                        List<Vertice> vAdj = null;
                        if (g.dirigido)
                        {
                            vAdj = g.getVerticesAdjacentes(g.getVerticePorNome(Console.ReadLine()));
                        }
                        else
                        {
                            vAdj = g.getVerticesAdjacentesNaoPonderadoNaoDirigido(g.getVerticePorNome(Console.ReadLine()));
                        }
                        Console.Write("Vértices adjacentes: ");
                        vAdj.ForEach(v => Console.Write(" " + v.nomeVertice));
                        Console.WriteLine();
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        Console.Write("Opção inválida!\nDigite uma opção válida: ");
                        break;
                }
            }
        }

        private static Grafo GraphFromJson(string filepath)
        {
            using (StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding("ISO-8859-1")))
            {
                var json = sr.ReadToEnd();
                var grafo = JsonConvert.DeserializeObject<App.GrafoJSON>(json);
                return new Grafo(grafo, grafo.ponderado, grafo.dirigido);
            }
        }

        private static Grafo addGrafo()
        {
            bool ponderado = false, dirigido = false;
            Grafo grafo = null;
            Console.Write("Grafo ponderado [S/N]? ");
            var weighted = Console.ReadKey();
            Console.Clear();

            if (weighted.Key.Equals(ConsoleKey.S))
            {
                ponderado = true;
            }

            Console.Write("Grafo dirigido [S/N]? ");
            var directed = Console.ReadKey();
            Console.Clear();

            if (directed.Key.Equals(ConsoleKey.S))
            {
                dirigido = true;
            }

            Console.Write("Importar de um arquivo [S/N]? ");
            var import = Console.ReadKey();
            Console.Clear();

            if (import.Key.Equals(ConsoleKey.S))
            {
                if (weighted.Key.Equals(ConsoleKey.S) &&
                   directed.Key.Equals(ConsoleKey.S))
                {
                    // Ler do arquivo de grafo ponderado e dirigido
                    grafo = GraphFromJson(@"~\..\..\..\GraphsToImport\GrafoPonderadoDirigido.json");
                }
                else if (weighted.Key.Equals(ConsoleKey.S) &&
                         directed.Key.Equals(ConsoleKey.N))
                {
                    // Ler do arquivo de grafo ponderado e não dirigido
                    grafo = GraphFromJson(@"~\..\..\..\GraphsToImport\GrafoPonderadoNaoDirigido.json");
                }
                else if (weighted.Key.Equals(ConsoleKey.N) &&
                         directed.Key.Equals(ConsoleKey.S))
                {
                    // Ler do arquivo de grafo não ponderado e dirigido
                    grafo = GraphFromJson(@"~\..\..\..\GraphsToImport\GrafoNaoPonderadoDirigido.json");
                }
                else if (weighted.Key.Equals(ConsoleKey.N) &&
                         directed.Key.Equals(ConsoleKey.N))
                {
                    // Ler do arquivo de grafo não ponderado e não dirigido
                    grafo = GraphFromJson(@"~\..\..\..\GraphsToImport\GrafoNaoPonderadoNaoDirigido.json");
                }
            }
            else if (import.Key.Equals(ConsoleKey.N))
            {
                Console.Write("Digite um nome para o grafo: ");
                grafo = new Grafo(Console.ReadLine(), ponderado, dirigido);
            }

            return grafo;
        }
    }
}