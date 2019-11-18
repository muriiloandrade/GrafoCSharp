using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

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
                    "7 - Testar existência de aresta entre 2 vértices\n" +
                    "8 - Obter grau de um vértice\n" +
                    "9 - Obter grau mínimo, médio e máximo\n" +
                    "10 - É conexo?\n" +
                    "11 - Existe caminho de Euler?\n" +
                    "12 - Matriz de Acessibilidade\n" +
                    "13 - Bellman-Ford\n" +
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
                        int weight = 0;
                        if (g.ponderado)
                        {
                            Console.Write("Digite o peso da aresta: ");
                            string peso = Console.ReadLine();
                            weight = Convert.ToInt32(peso);
                        }
                        Console.Write("Digite um nome para a aresta: ");
                        if (g.ponderado)
                        {
                            g.addAresta(new Aresta(vInicial, vFinal, Console.ReadLine(), weight));
                        }
                        else
                        {
                            g.addAresta(new Aresta(vInicial, vFinal, Console.ReadLine()));
                        }

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
                        if (g.ponderado)
                        {
                            if (g.dirigido)
                            {
                                g.showMatrizAdjacentePonderadoDirigido();
                            }
                            else
                            {
                                g.showMatrizAdjacentePonderadoNaoDirigido();
                            }
                        }
                        else
                        {
                            if (g.dirigido)
                            {
                                g.showMatrizAdjacenteDirigido();
                            }
                            else
                            {
                                g.showMatrizAdjacenteNaoDirigido();
                            }
                        }
                        Console.WriteLine();
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "6":
                        Console.Write("Digite o nome do vértice que deseja conhecer os adjacentes: ");
                        List<Vertice> vAdj = null;
                        vAdj = g.getVerticesAdjacentes(g.getVerticePorNome(Console.ReadLine()));
                        Console.Write("Vértices adjacentes: ");
                        vAdj.ForEach(v => Console.Write(" " + v.nomeVertice));
                        Console.WriteLine();
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "7":
                        Console.Write("Digite o nome do vértice 1: ");
                        var v1 = g.getVerticePorNome(Console.ReadLine());
                        Console.Write("Digite o nome do vértice 2: ");
                        var v2 = g.getVerticePorNome(Console.ReadLine());

                        if (g.howManyArestasEntreVertices(v1.nomeVertice, v2.nomeVertice) >= 2)
                        {
                            Console.WriteLine($"Existem {g.howManyArestasEntreVertices(v1.nomeVertice, v2.nomeVertice)} arestas entre estes vértices!");
                        }
                        else if (g.howManyArestasEntreVertices(v1.nomeVertice, v2.nomeVertice) == 1)
                        {
                            Console.WriteLine($"Existe {g.howManyArestasEntreVertices(v1.nomeVertice, v2.nomeVertice)} aresta entre estes vértices!");
                        }
                        else
                        {
                            Console.WriteLine($"Não existem aresta entre estes vértices!");
                        }
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "8":
                        Console.Write("Digite o nome do vértice: ");
                        Vertice ver = g.getVerticePorNome(Console.ReadLine());
                        Console.WriteLine($"O grau do vértice é: {g.getGrauVertice(ver)}");
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "9":
                        Console.WriteLine(g.getGrauMinMedMax());
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "10":
                        if (g.dirigido)
                        {
                            Console.WriteLine("O grafo " + (g.isFortementeConexo() ? "é " : "não é ") + "conexo!");
                        }
                        else
                        {
                            Console.WriteLine("O grafo " + (g.isConexo() ? "é " : "não é ") + "conexo!");
                        }
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "11":
                        Console.WriteLine("O grafo " + (g.existsCaminhoDeEuler() ? "possui " : "não possui ") + "caminho de Euler!");
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "12":
                        g.showMatrizDeAcessibilidade();
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "13":
                        Console.Write("Digite o nome do vértice 1: ");
                        var s = g.getVerticePorNome(Console.ReadLine());
                        g.bellmanFord(g.vertices.IndexOf(s));
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        Console.Write("Opção inválida!\n");
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
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
