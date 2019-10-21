using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp
{
    class Grafo
    {
        internal string nomeGrafo { get; set; }
        internal bool ponderado { get; set; }
        internal bool dirigido { get; set; }
        internal Guid guidCode { get; set; }
        internal List<Vertice> vertices { get; set; }
        internal List<Aresta> arestas { get; set; }
        internal LinkedList<Vertice>[] linkedlistVertices { get; set; }

        public Grafo(string nome, bool weighted, bool directed)
        {
            this.nomeGrafo = nome;
            this.ponderado = weighted;
            this.dirigido = directed;
            this.guidCode = Guid.NewGuid();
            this.vertices = new List<Vertice>();
            this.arestas = new List<Aresta>();
        }

        public Grafo(App.GrafoJSON json, bool weighted, bool directed)
        {
            this.nomeGrafo = json.nomeGrafo;
            this.ponderado = weighted;
            this.dirigido = directed;
            this.guidCode = Guid.NewGuid();
            this.vertices = new List<Vertice>();
            this.arestas = new List<Aresta>();

            foreach (var v in json.vertices)
            {
                this.vertices.Add(new Vertice(v.nomeVertice));
            }

            if (weighted)
            {
                foreach (var a in json.arestas)
                {
                    this.arestas.Add(new Aresta(new Vertice(a.vInicial), new Vertice(a.vFinal), a.nomeAresta, a.peso));
                }
            }
            else
            {
                foreach (var a in json.arestas)
                {
                    this.arestas.Add(new Aresta(new Vertice(a.vInicial), new Vertice(a.vFinal), a.nomeAresta));
                }
            }
        }

        #region Grafo
        Grafo getGrafoInvertido()
        {
            Grafo g = new Grafo("Grafo invertido", false, this.dirigido);
            this.newListaDeAdjacencias(g);

            for (int v = 0; v < this.vertices.Count; v++)
            {
                var i = linkedlistVertices[v].GetEnumerator();
                while (i.MoveNext())
                {
                    g.linkedlistVertices[this.vertices.IndexOf(i.Current)].AddLast(this.vertices[v]);
                }
            }
            return g;
        }

        private void buscaEmProfundidade(int index, bool[] visited)
        {
            visited[index] = true;

            var i = linkedlistVertices[index].GetEnumerator();
            while (i.MoveNext())
            {
                int n = this.vertices.IndexOf(i.Current);
                if (!visited[n])
                {
                    this.buscaEmProfundidade(n, visited);
                }
            }
        }

        internal bool isConexo()
        {
            this.newListaDeAdjacencias(this);
            this.fillListaDeAdjacencias(this);

            bool[] visited = new bool[this.vertices.Count];

            int j;

            for (j = 0; j < this.vertices.Count; j++)
            {
                //Se alguma lista de adjacência não possuir itens, o grafo não é conexo
                if (linkedlistVertices[j].Count() == 0)
                    return false;

                //Acha um vértice com grau > 0
                if (linkedlistVertices[j].Count() > 0)
                    break;
            }

            this.buscaEmProfundidade(j, visited);

            for (j = 0; j < this.vertices.Count; j++)
            {
                if (visited[j] == false && linkedlistVertices[j].Count() > 0)
                    return false;
            }

            return true;
        }

        internal bool isFortementeConexo()
        {
            this.newListaDeAdjacencias(this);
            this.fillListaDeAdjacencias(this);

            bool[] visited = new bool[this.vertices.Count];

            this.buscaEmProfundidade(0, visited);

            //Se a busca não visitar todos os vértices, não é fortemente conexo
            for (int i = 0; i < this.vertices.Count; i++)
            {
                if (visited[i] == false)
                    return false;
            }

            //Cria um grafo invertido
            Grafo grafoInvertido = getGrafoInvertido();
            
            //Reseta o array de visitados para a busca em profundidade do grafo invertido
            for (int i = 0; i < this.vertices.Count; i++)
            {
                visited[i] = false;
            }

            grafoInvertido.buscaEmProfundidade(0, visited);

            //Se a busca não visitar todos os vértices, não é fortemente conexo
            for (int i = 0; i < this.vertices.Count; i++)
            {
                if (visited[i] == false)
                    return false;
            }

            return true;
        }

        private void newListaDeAdjacencias(Grafo g)
        {
            //Instancia uma nova lista encadeada para a lista encadeada de cada vértice do grafo
            g.linkedlistVertices = new LinkedList<Vertice>[this.vertices.Count];

            for (int i = 0; i < this.vertices.Count; i++)
            {
                g.linkedlistVertices[i] = new LinkedList<Vertice>();
            }
        }

        private void fillListaDeAdjacencias(Grafo g)
        {
            for (int i = 0; i < g.linkedlistVertices.Length; i++)
            {
                foreach (Vertice v in this.getVerticesAdjacentes(this.vertices[i]))
                {
                    if (!g.linkedlistVertices[i].Contains(v))
                        g.linkedlistVertices[i].AddLast(v);
                }
            }
        }
        #endregion Grafo

        #region Vertices
        internal void addVertice(Vertice vertice)
        {
            try
            {
                if (this.vertices.Exists(v => v.nomeVertice.Equals(vertice.nomeVertice, StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new Exception("O grafo já contém esse vértice, insira outro!");
                }

                this.vertices.Add(vertice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal void removeVertice(Vertice vertice)
        {
            try
            {
                if (!this.vertices.Contains(vertice))
                {
                    throw new Exception("O grafo não contém este vértice!");
                }

                foreach (Vertice v in this.getVerticesAdjacentes(vertice))
                {
                    Aresta[] arestas = new Aresta[this.arestas.Count];
                    this.arestas.CopyTo(arestas);
                    foreach (Aresta a in arestas)
                    {
                        if (a.vInicial.Equals(v) && a.vFinal.Equals(vertice))
                        {
                            this.arestas.Remove(a);
                        }

                        if (a.vFinal.Equals(v) && a.vInicial.Equals(vertice))
                        {
                            this.arestas.Remove(a);
                        }
                    }
                }

                this.vertices.Remove(vertice);
                Console.WriteLine("Vértice " + vertice.nomeVertice + " excluído!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal bool existsCaminhoDeEuler()
        {
            return isConexo() && (getTodosOsGraus().Count(grau => grau % 2 != 0) == 0 || getTodosOsGraus().Count(grau => grau % 2 != 0) == 2);
        }

        internal string getGrauMinMedMax()
        {
            List<int> graus = getTodosOsGraus();

            return "Grau mínimo: " + graus.Min() + "\n" +
                   "Grau médio: " + graus.Average() + "\n" +
                   "Grau máximo: " + graus.Max();
        }

        private List<int> getTodosOsGraus()
        {
            List<int> graus = new List<int>();
            foreach (Vertice vertice in this.vertices)
            {
                graus.Add(getGrauVertice(vertice));
            }

            return graus;
        }

        internal int getGrauVertice(Vertice vertice)
        {
            int grau = 0;
            foreach (Aresta a in this.arestas)
            {
                if (a.vInicial.Equals(vertice))
                {
                    grau++;
                }

                if (a.vFinal.Equals(vertice))
                {
                    grau++;
                }
            }
            return grau;
        }

        internal List<Vertice> getVerticesAdjacentes(Vertice vertice)
        {
            var verticesAdjacentes = new List<Vertice>();

            foreach (Aresta a in this.arestas)
            {
                if (a.vInicial.Equals(vertice))
                {
                    verticesAdjacentes.Add(a.vFinal);
                }

                if (a.vFinal.Equals(vertice))
                {
                    verticesAdjacentes.Add(a.vInicial);
                }
            }

            return verticesAdjacentes;
        }

        internal Vertice getVerticePorNome(String nomeVertice)
        {
            try
            {
                var vertice = this.vertices.FirstOrDefault(v => v.nomeVertice.Equals(nomeVertice, StringComparison.CurrentCultureIgnoreCase));
                return vertice;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion Vertices

        #region Arestas
        internal void addAresta(Aresta aresta)
        {
            if (this.isArestaValid(aresta))
            {
                this.arestas.Add(aresta);
            }
        }

        internal void removeAresta(Aresta aresta)
        {
            if (this.isArestaValid(aresta))
            {
                this.arestas.Remove(aresta);
            }
        }

        private bool isArestaValid(Aresta aresta)
        {
            try
            {
                if (!this.vertices.Contains(aresta.vInicial) && !this.vertices.Contains(aresta.vFinal))
                {
                    throw new Exception("Vértices inicial e final não existem no grafo!");
                }
                else if (!this.vertices.Contains(aresta.vInicial))
                {
                    throw new Exception("Vértice inicial não existe no grafo!");
                }
                else if (!this.vertices.Contains(aresta.vFinal))
                {
                    throw new Exception("Vértice final não existe no grafo!");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        internal Aresta getArestaPorNome(string nomeAresta)
        {
            return this.arestas.First(a => a.nomeAresta.Equals(nomeAresta, StringComparison.CurrentCultureIgnoreCase));
        }

        internal int existsArestaEntreVertices(string nomeV1, string nomeV2)
        {
            Vertice v1 = this.getVerticePorNome(nomeV1);
            Vertice v2 = this.getVerticePorNome(nomeV2);

            return this.arestas.Count(a => a.vInicial.Equals(v1) && a.vFinal.Equals(v2));
        }

        internal Aresta existsArestaEntreVertices(string nomeV1, string nomeV2, bool weighted)
        {
            Vertice v1 = this.getVerticePorNome(nomeV1);
            Vertice v2 = this.getVerticePorNome(nomeV2);

            return this.arestas.First(a => a.vInicial.Equals(v1) && a.vFinal.Equals(v2));
        }
        #endregion Arestas

        #region Matriz
        internal void showMatrizAdjacenteDirigido()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];

            Console.WriteLine("\n--------------- Matriz de Adjacência ---------------\n");
            Console.Write("    ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + "   "));
            Console.Write("\n");

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice) > 0)
                    {
                        matrix[i, j] = this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice);
                    }

                    if (matrix[i, j] < 10)
                    {
                        Console.Write("  " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] >= 10)
                    {
                        Console.Write(" " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] > 100)
                    {
                        Console.Write(" " + matrix[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }

        internal void showMatrizAdjacenteNaoDirigido()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];

            Console.WriteLine("\n--------------- Matriz de Adjacência ---------------\n");
            Console.Write("    ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + "   "));
            Console.Write("\n");

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice) > 0)
                    {
                        matrix[i, j] = this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice);
                        matrix[j, i] = this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice);
                    }

                    if (matrix[i, j] < 10)
                    {
                        Console.Write("  " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] >= 10)
                    {
                        Console.Write(" " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] > 100)
                    {
                        Console.Write(" " + matrix[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }

        #region MatrizPonderada
        internal void showMatrizAdjacentePonderadoDirigido()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];

            Console.WriteLine("\n--------------- Matriz de Adjacência ---------------\n");
            Console.Write("    ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + "   "));
            Console.Write("\n");

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice) > 0)
                    {
                        matrix[i, j] = this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice, this.ponderado).peso;
                    }

                    if (matrix[i, j] < 10)
                    {
                        Console.Write("  " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] >= 10)
                    {
                        Console.Write(" " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] > 100)
                    {
                        Console.Write(" " + matrix[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }

        internal void showMatrizAdjacentePonderadoNaoDirigido()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];

            Console.WriteLine("\n--------------- Matriz de Adjacência ---------------\n");
            Console.Write("    ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + "   "));
            Console.Write("\n");

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice) > 0)
                    {
                        matrix[i, j] = this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice, this.ponderado).peso;
                        matrix[j, i] = this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice, this.ponderado).peso;
                    }

                    if (matrix[i, j] < 10)
                    {
                        Console.Write("  " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] >= 10)
                    {
                        Console.Write(" " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] > 100)
                    {
                        Console.Write(" " + matrix[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }
        #endregion MatrizPonderada
        #endregion Matriz
    }
}
