using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp
{
    class Grafo
    {
        private string nomeGrafo { get; set; }
        private bool ponderado { get; set; }
        private bool dirigido { get; set; }
        private Guid guidCode { get; set; }
        private Dictionary<Vertice, List<Aresta>> grafo { get; set; }

        public Grafo(string nome)
        {
            this.nomeGrafo = nome;
            this.guidCode = new Guid();
        }

        public Grafo(string nome, bool weighted, bool directed)
        {
            this.nomeGrafo = nome;
            this.ponderado = weighted;
            this.dirigido = directed;
            this.grafo = new Dictionary<Vertice, List<Aresta>>();
        }

        void addVertice(Vertice vertice)
        {
            this.grafo.Add(vertice, new List<Aresta>());
        }

        void removeVertice(Vertice vertice)
        {
            if (!this.grafo.ContainsKey(vertice))
            {
                throw new Exception("O grafo não contém este vértice!");
            }

            this.grafo.Remove(vertice);
        }

        List<Vertice> getVerticesAdjacentes(Vertice vertice)
        {
            var verticesAdjacentes = new List<Vertice>();

            var arestas = this.grafo.GetEnumerator().Current.Value;

            //foreach (var item in arestas)
            //{
                
            //}

            return verticesAdjacentes;
        }
    }
}
