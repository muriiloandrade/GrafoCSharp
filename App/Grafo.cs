﻿using System;
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
        //internal Dictionary<Vertice, List<Aresta>> grafo { get; set; }


        public Grafo(string nome)
        {
            this.nomeGrafo = nome;
            this.guidCode = Guid.NewGuid();
        }

        public Grafo(string nome, bool weighted, bool directed)
        {
            this.nomeGrafo = nome;
            this.ponderado = weighted;
            this.dirigido = directed;
            this.guidCode = Guid.NewGuid();
            this.vertices = new List<Vertice>();
            this.arestas = new List<Aresta>();
            //this.grafo = new Dictionary<Vertice, List<Aresta>>();
        }

        public Grafo(App.GrafoJSON json, bool weighted, bool directed)
        {
            this.nomeGrafo = json.nomeGrafo;
            this.guidCode = Guid.NewGuid();
            this.ponderado = weighted;
            this.dirigido = directed;
            this.vertices = new List<Vertice>();
            this.arestas = new List<Aresta>();

            foreach (var v in json.vertices)
            {
                this.vertices.Add(new Vertice(v.nomeVertice));
            }

            foreach (var a in json.arestas)
            {
                this.arestas.Add(new Aresta(new Vertice(a.vInicial), new Vertice(a.vFinal), a.nomeAresta));
            }

            //this.grafo = new Dictionary<Vertice, List<Aresta>>();
        }

        #region Vertices
        internal void addVertice(Vertice vertice)
        {
            try
            {
                //if (this.grafo.ContainsKey(vertice))
                if (this.vertices.Exists(v => v.nomeVertice.Equals(vertice.nomeVertice, StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new Exception("O grafo já contém esse vértice!\nInsira outro!");
                }

                this.vertices.Add(vertice);
                //this.grafo.Add(vertice, new List<Aresta>());
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
                //if (!this.grafo.ContainsKey(vertice))
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
                #region lixo
                //this.getVerticesAdjacentes(vertice).Where(verticeAdjacente =>
                //{
                //    this.grafo.Values.Where(arestas => arestas.Where(v => v.vInicial.Equals(verticeAdjacente) ||
                //                                                      v.vFinal.Equals(verticeAdjacente)));
                //});

                //foreach (Vertice v in verticesAdjacentes)
                //{
                //var b = this.grafo.Values.AsEnumerable();
                //}

                //foreach (List<Aresta> arestas in this.grafo.Values)
                //{
                //    foreach (Aresta a in arestas)
                //    {
                //        foreach (Vertice v in verticesAdjacentes)
                //        {
                //            if (a.vInicial.Equals(v))
                //            {
                //                arestas.Remove(a);
                //            }

                //            if (a.vFinal.Equals(v))
                //            {
                //                arestas.Remove(a);
                //            }
                //        }
                //    }
                //}

                //this.grafo.Remove(vertice);
                #endregion lixo
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            #region lixo
            //foreach (var item in this.grafo)
            //{
            //    foreach (Aresta a in item.Value)
            //    {
            //        if (a.vInicial.Equals(vertice))
            //        {
            //            verticesAdjacentes.Add(a.vFinal);
            //        }

            //        if (a.vFinal.Equals(vertice))
            //        {
            //            verticesAdjacentes.Add(a.vInicial);
            //        }
            //    }
            //}
            #endregion lixo
        }

        internal List<Vertice> getVerticesAdjacentesNaoPonderadoNaoDirigido(Vertice vertice)
        {
            var verticesAdjacentes = new List<Vertice>();

            foreach (Aresta a in this.arestas)
            {
                if (a.vInicial.Equals(vertice) && !verticesAdjacentes.Contains(a.vFinal))
                {
                    verticesAdjacentes.Add(a.vFinal);
                }

                if (a.vFinal.Equals(vertice) && !verticesAdjacentes.Contains(a.vInicial))
                {
                    verticesAdjacentes.Add(a.vInicial);
                }
            }

            return verticesAdjacentes;
            #region lixo
            //foreach (var item in this.grafo)
            //{
            //    foreach (Aresta a in item.Value)
            //    {
            //        if (a.vInicial.Equals(vertice))
            //        {
            //            verticesAdjacentes.Add(a.vFinal);
            //        }

            //        if (a.vFinal.Equals(vertice))
            //        {
            //            verticesAdjacentes.Add(a.vInicial);
            //        }
            //    }
            //}
            #endregion lixo
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

            #region lixo
            //foreach (Vertice v in this.vertices)
            //{
            //    if (v.nomeVertice.Equals(nomeVertice, StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        return v;
            //    }
            //}
            #endregion lixo
        }

        #endregion Vertices

        #region Arestas
        internal void addAresta(Aresta aresta)
        {
            if (this.isArestaValid(aresta))
            {
                this.arestas.Add(aresta);
            }

            #region lixo
            //var vInicial = this.grafo.Keys.Where(v => v.Equals(aresta.vInicial));
            //var vFinal = this.grafo.Keys.Where(v => v.Equals(aresta.vFinal));
            #endregion lixo
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

        private bool existsArestaEntreVertices(string nomeV1, string nomeV2)
        {
            Vertice v1 = this.getVerticePorNome(nomeV1);
            Vertice v2 = this.getVerticePorNome(nomeV2);

            return this.arestas.Exists(a => a.vInicial.Equals(v1) && a.vFinal.Equals(v2));
        }
        #endregion Arestas

        #region Matriz
        internal void showMatrizAdjacenteDirigido()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];

            Console.WriteLine("\n--------------- Matriz de Adjacência ---------------\n");
            Console.Write("   ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + " "));
            Console.Write("\n");

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "| ");
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice))
                    {
                        matrix[i, j] = 1;
                    }

                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        internal void showMatrizAdjacenteNaoDirigido()
        {
            int[,] matrix = new int[this.vertices.Count, this.vertices.Count];

            Console.WriteLine("\n--------------- Matriz de Adjacência ---------------\n");
            Console.Write("   ");
            this.vertices.ForEach(v => Console.Write(v.nomeVertice + " "));
            Console.Write("\n");

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Console.Write(this.vertices[i].nomeVertice + "| ");
                for (int j = 0; j < this.vertices.Count; j++)
                {
                    if (this.existsArestaEntreVertices(this.vertices[i].nomeVertice, this.vertices[j].nomeVertice))
                    {
                        matrix[i, j] = 1;
                        matrix[j, i] = 1;
                    }

                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        #endregion Matriz
    }
}
