namespace GraphApp.App
{
    public class GrafoJSON
    {
        public string nomeGrafo { get; set; }
        public bool ponderado { get; set; }
        public bool dirigido { get; set; }
        public Vertice[] vertices { get; set; }
        public Aresta[] arestas { get; set; }
    }

    public class Vertice
    {
        public object guidCode { get; set; }
        public string nomeVertice { get; set; }
    }

    public class Aresta
    {
        public string vInicial { get; set; }
        public string vFinal { get; set; }
        public string nomeAresta { get; set; }
        public int peso { get; set; }
        public object guidCode { get; set; }
    }

}
