using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphApp
{
    class Grafo
    {
        internal string nomeGrafo { get; set; }
        internal bool ponderado { get; set; }
        internal bool dirigido { get; set; }
        internal Guid guidCode { get; set; }
        internal int ite { get; set; }
        internal int comp { get; set; }
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
            this.arestas.ForEach(a => g.arestas.Add(a));
            this.vertices.ForEach(v => g.vertices.Add(v));

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

        private int buscaEmProfundidade(int index)
        {
            // Busca em profundidade iterativa usando pilha
            int cont = 0;
            bool[] visited = new bool[this.vertices.Count];
            Stack<int> stack = new Stack<int>();

            stack.Push(index);

            while (stack.Count > 0)
            {
                index = stack.Peek();
                stack.Pop();
                if (visited[index] == false)
                {
                    visited[index] = true;
                    cont += 1;
                }

                var i = linkedlistVertices[index].GetEnumerator();

                while (i.MoveNext())
                {
                    Vertice v = i.Current;
                    if (!visited[this.vertices.IndexOf(v)])
                    {
                        stack.Push(this.vertices.IndexOf(v));
                    }
                }
            }
            return cont;
        }

        private void floyd(int[,] matrix)
        {
            for (int k = 0; k < this.vertices.Count; k++)
            {
                for (int i = 0; i < this.vertices.Count; i++)
                {
                    for (int j = 0; j < this.vertices.Count; j++)
                    {
                        this.ite++;
                        if (matrix[i, k] + matrix[k, j] < matrix[i, j])
                        {
                            matrix[i, j] = matrix[i, k] + matrix[k, j];
                        }
                        this.comp++;
                    }
                }
            }
        }

        private void warshall(int[,] matrix)
        {
            for (int k = 0; k < this.vertices.Count; k++)
            {
                for (int i = 0; i < this.vertices.Count; i++)
                {
                    for (int j = 0; j < this.vertices.Count; j++)
                    {
                        matrix[i, j] = Math.Max(matrix[i, j], (Math.Min(matrix[i, k], matrix[k, j])));
                    }
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
                    if (!g.linkedlistVertices[i].Contains(v) && this.arestas.Any(a => a.vInicial.Equals(this.vertices[i]) && a.vFinal.Equals(v)))
                        g.linkedlistVertices[i].AddLast(v);
                }
            }
        }

        private void fillListaDeAdjacenciasNaoDirigido(Grafo g)
        {
            for (int i = 0; i < g.linkedlistVertices.Length; i++)
            {
                foreach (Vertice v in this.getVerticesAdjacentes(this.vertices[i]))
                {
                    if (!g.linkedlistVertices[i].Contains(v) &&
                        this.arestas.Any(a => (a.vInicial.Equals(this.vertices[i]) || a.vFinal.Equals(this.vertices[i]))
                                                            && (a.vFinal.Equals(v) || a.vInicial.Equals(v))))
                    {
                        g.linkedlistVertices[i].AddLast(v);
                    }
                }
            }
        }

        internal void dijkstra(int indiceVerticeInicial, int indiceVerticeFinal)
        {
            int nVertices = this.vertices.Count;
            this.ite = 0;
            this.comp = 0;
            int[,] matrix = new int[nVertices, nVertices];

            //Preenche a matriz com os pesos das arestas
            for (int i = 0; i < nVertices; i++)
            {
                for (int j = 0; j < nVertices; j++)
                {
                    matrix[i, j] = int.MaxValue;

                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice))
                    {
                        matrix[i, j] = this.getArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice).peso;
                    }
                }
            }

            // Vértices que já foram percorridos
            List<Vertice> IN = new List<Vertice>
            {
                this.vertices[indiceVerticeInicial]
            };

            // Vetor de índice de vértices, distância até o inicial usando vértices de IN
            int[] d = new int[nVertices];

            // Vetor de índice de vértices, contém o indice do vértice anterior no caminho mínimo
            int[] s = new int[nVertices];

            d[indiceVerticeInicial] = 0;

            // Para todo vértice z não pertencente a IN faça
            foreach (var z in this.vertices.Where(v => IN.All(inV => inV.nomeVertice != v.nomeVertice)))
            {
                this.ite++;
                d[this.vertices.IndexOf(z)] = matrix[indiceVerticeInicial, this.vertices.IndexOf(z)];
                s[this.vertices.IndexOf(z)] = indiceVerticeInicial;
            }

            // Enquanto o vértice final não pertencer a IN
            while (!IN.Contains(this.vertices[indiceVerticeFinal]))
            {
                //pesos = contém os pesos até os vértices que não estão em IN
                List<int> pesos = new List<int>();
                foreach (var z in this.vertices.Where(v => !IN.Contains(v)))
                {
                    this.ite++;
                    if (this.arestas.Exists(a => (a.vInicial.Equals(IN.Last()) &&
                                                  a.vFinal.Equals(z)) ||
                                                 (a.vFinal.Equals(IN.Last()) &&
                                                  a.vInicial.Equals(z))))
                    {
                        pesos.Add(this.arestas.First(a => (a.vInicial.Equals(IN.Last()) &&
                                                           a.vFinal.Equals(z)) ||
                                                          (a.vFinal.Equals(IN.Last()) &&
                                                           a.vInicial.Equals(z))).peso);
                    }
                    this.comp++;
                }

                Vertice p;
                if (IN.Contains(this.arestas.First(a => (a.vInicial.nomeVertice.Equals(IN.Last().nomeVertice) || a.vFinal.nomeVertice.Equals(IN.Last().nomeVertice) && !IN.Contains(a.vInicial)) && a.peso == pesos.Min()).vFinal))
                {
                    p = this.arestas.First(a => (a.vInicial.nomeVertice.Equals(IN.Last().nomeVertice) || a.vFinal.nomeVertice.Equals(IN.Last().nomeVertice) && !IN.Contains(a.vInicial)) && a.peso == pesos.Min()).vInicial;
                }
                else
                {
                    p = this.arestas.First(a => (a.vInicial.nomeVertice.Equals(IN.Last().nomeVertice) || a.vFinal.nomeVertice.Equals(IN.Last().nomeVertice) && !IN.Contains(a.vInicial)) && a.peso == pesos.Min()).vFinal;
                }
                this.comp++;

                //IN = IN U {p}
                IN.Add(p);

                var indiceDoVerticeP = this.vertices.IndexOf(p);
                foreach (var z in this.vertices.Where(v => IN.All(inV => inV.nomeVertice != v.nomeVertice)))
                {
                    this.ite++;
                    var indiceDoVerticeZ = this.vertices.IndexOf(z);
                    var distanciaAnterior = d[indiceDoVerticeZ];
                    d[indiceDoVerticeZ] = Math.Min(distanciaAnterior, d[indiceDoVerticeP] + matrix[indiceDoVerticeP, indiceDoVerticeZ]);
                    this.comp++;
                    if (d[indiceDoVerticeZ] != distanciaAnterior)
                    {
                        s[indiceDoVerticeZ] = indiceDoVerticeP;
                    }
                    this.comp++;
                }
            }

            Console.Write("Caminho: " + this.vertices[indiceVerticeFinal].nomeVertice);
            var foo = indiceVerticeFinal;
            do
            {
                Console.Write(this.vertices[s[foo]].nomeVertice);
                foo = s[foo];
            } while (foo != indiceVerticeInicial);
            Console.WriteLine($"\nA distância mínima é: {d[indiceVerticeFinal]}\nNúmero de comparações: {comp}\nNúmero de iterações: {ite}");
        }

        internal void bellmanFord(int indiceVerticeInicial)
        {
            this.comp = 0; this.ite = 0;
            int nVertices = this.vertices.Count;
            int[,] matrix = new int[nVertices, nVertices];

            //Preenche a matriz com os pesos das arestas
            for (int i = 0; i < nVertices; i++)
            {
                for (int j = 0; j < nVertices; j++)
                {
                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice))
                    {
                        matrix[i, j] = this.getArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice).peso;
                    }
                }
            }

            int[] menoresDistancias = new int[nVertices];

            bool[] adicionado = new bool[nVertices];

            for (int i = 0; i < nVertices; i++)
            {
                this.ite++;
                menoresDistancias[i] = int.MaxValue;
                adicionado[i] = false;
            }

            menoresDistancias[indiceVerticeInicial] = 0;

            int[] pais = new int[nVertices];

            pais[indiceVerticeInicial] = -1;

            for (int j = 1; j < nVertices; j++)
            {
                int verticeMaisProximo = -1;
                int menorDistancia = int.MaxValue;
                for (int i = 0; i < nVertices; i++)
                {
                    this.ite++;
                    if (!adicionado[i] && menoresDistancias[i] < menorDistancia)
                    {
                        verticeMaisProximo = i;
                        menorDistancia = menoresDistancias[i];
                    }
                    this.comp++;
                }

                adicionado[verticeMaisProximo] = true;

                for (int i = 0; i < nVertices; i++)
                {
                    this.ite++;
                    int pesoAresta = matrix[verticeMaisProximo, i];

                    if (pesoAresta > 0 && ((menorDistancia + pesoAresta) < menoresDistancias[i]))
                    {
                        pais[i] = verticeMaisProximo;
                        menoresDistancias[i] = menorDistancia + pesoAresta;
                    }
                    this.comp++;
                }
            }
            showSolucaoBellmanFord(indiceVerticeInicial, menoresDistancias, pais);
        }

        internal int numberDeComponentesConectados()
        {
            this.newListaDeAdjacencias(this);
            this.fillListaDeAdjacenciasNaoDirigido(this);

            bool[] visited = new bool[this.vertices.Count];

            int nComponents = 0;

            for (int i = 0; i < this.vertices.Count; i++)
            {
                if (visited[i] == false)
                {
                    buscaEmProfundidade(i, visited);
                    nComponents += 1;
                }
            }

            return nComponents;
        }

        internal int numberDeVerticesMaiorComponenteConectados()
        {
            this.newListaDeAdjacencias(this);
            this.fillListaDeAdjacenciasNaoDirigido(this);

            List<int> qtdVertices = new List<int>();

            for (int i = 0; i < this.vertices.Count; i++)
            {
                qtdVertices.Add(buscaEmProfundidade(i));
            }

            return qtdVertices.Max();
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
            return (this.dirigido ? isFortementeConexo() : isConexo()) && (getTodosOsGraus().Count(grau => grau % 2 != 0) == 0 || getTodosOsGraus().Count(grau => grau % 2 != 0) == 2);
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
            try
            {
                return this.arestas.First(a => a.nomeAresta.Equals(nomeAresta, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        internal bool existsArestaEntreVertices(string nomeV1, string nomeV2)
        {
            Vertice v1 = this.getVerticePorNome(nomeV1);
            Vertice v2 = this.getVerticePorNome(nomeV2);

            return this.arestas.Exists(a => a.vInicial.Equals(v1) && a.vFinal.Equals(v2));
        }

        internal int howManyArestasEntreVertices(string nomeV1, string nomeV2)
        {
            Vertice v1 = this.getVerticePorNome(nomeV1);
            Vertice v2 = this.getVerticePorNome(nomeV2);

            return this.arestas.Count(a => a.vInicial.Equals(v1) && a.vFinal.Equals(v2));
        }

        internal Aresta getArestaEntreVertices(string nomeV1, string nomeV2)
        {
            Vertice v1 = this.getVerticePorNome(nomeV1);
            Vertice v2 = this.getVerticePorNome(nomeV2);

            return this.arestas.First(a => a.vInicial.Equals(v1) && a.vFinal.Equals(v2));
        }
        #endregion Arestas

        #region Matriz
        internal void showMatrizDeAcessibilidade()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];

            Console.WriteLine("\n--------------- Matriz de Acessibilidade ---------------\n");
            Console.Write("   ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + "  "));
            Console.Write("\n");

            for (int i = 0; i < this.vertices.Count; i++)
            {
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice))
                    {
                        matrix[i, j] = 1;
                    }
                }
            }

            warshall(matrix);

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "| ");
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    Console.Write(matrix[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }

        internal void showMatrizDeCaminhosMínimos()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];
            this.ite = 0; this.comp = 0;
            Console.WriteLine("\n--------------- Matriz de Caminhos Mínimos ---------------\n");
            Console.Write("    ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + "   "));
            Console.Write("\n");

            // Preenchimento da matriz
            for (int i = 0; i < this.vertices.Count; i++)
            {
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 0;
                    }
                    else
                    {
                        if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice))
                        {
                            matrix[i, j] = this.getArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice).peso;
                        }
                        else
                        {
                            matrix[i, j] = 999;
                        }
                    }
                }
            }

            floyd(matrix);

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (matrix[i, j] < 10)
                    {
                        Console.Write("  " + matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] >= 10)
                    {
                        Console.Write(matrix[i, j] + " ");
                    }
                    else if (matrix[i, j] > 100)
                    {
                        Console.Write(matrix[i, j]);
                    }
                }

                Console.WriteLine();
            }
            Console.WriteLine($"\nNúmero de comparações: {this.comp}\nNúmero de iterações: {this.ite}");
        }

        internal void showMatrizAdjacenteDirigido()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];

            Console.WriteLine("\n--------------- Matriz de Adjacência ---------------\n");
            Console.Write("    ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + "   "));
            Console.Write("\n");

            for (int i = 0; i < this.vertices.Count; i++)
            {
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.howManyArestasEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice) > 0)
                    {
                        matrix[i, j] = this.howManyArestasEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice);
                    }
                }
            }

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
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
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.howManyArestasEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice) > 0)
                    {
                        matrix[i, j] = this.howManyArestasEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice);
                        matrix[j, i] = this.howManyArestasEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice);
                    }
                }
            }

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
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
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.howManyArestasEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice) > 0)
                    {
                        matrix[i, j] = this.getArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice).peso;
                    }
                }
            }

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
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
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.howManyArestasEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice) > 0)
                    {
                        matrix[i, j] = this.getArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice).peso;
                        matrix[j, i] = this.getArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice).peso;
                    }
                }
            }

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "|");
                for (int j = 0; j < this.vertices.Count; j++)
                {
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

        private void showSolucaoBellmanFord(int source, int[] pesosTotais, int[] pais)
        {
            Console.Write("Vértice\t Distância\tCaminho");

            for (int i = 0; i < this.vertices.Count(); i++)
            {
                Console.Write("\n" + this.vertices[source].nomeVertice + " -> ");
                Console.Write(this.vertices[i].nomeVertice + " \t ");
                Console.Write(pesosTotais[i] + "\t\t");
                printCaminho(i, pais);
            }
            Console.WriteLine($"\nNúmero de comparações: {this.comp}\nNúmero de iterações: {this.ite}");
        }

        private void printCaminho(int verticeAtual, int[] pais)
        {
            if (verticeAtual == -1)
            {
                return;
            }
            printCaminho(pais[verticeAtual], pais);
            Console.Write(this.vertices[verticeAtual].nomeVertice + " ");
        }
        #endregion MatrizPonderada
        #endregion Matriz
    }
}
