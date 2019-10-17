using System;
using System.Collections.Generic;

namespace GraphApp
{
    class Aresta
    {
        internal int peso;
        internal Vertice vInicial;
        internal Vertice vFinal;
        internal Guid guidCode { get; set; }
        internal string nomeAresta { get; set; }

        public Aresta(Vertice inicial, Vertice final, String nome)
        {
            this.vInicial = inicial;
            this.vFinal = final;
            this.nomeAresta = nome;
            this.guidCode = Guid.NewGuid();
        }

        public Aresta(Vertice inicial, Vertice final, String nome, int peso)
        {
            this.vInicial = inicial;
            this.vFinal = final;
            this.nomeAresta = nome;
            this.peso = peso;
            this.guidCode = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Aresta);
        }

        internal bool Equals(Aresta outraAresta)
        {
            if (outraAresta == null)
            {
                Console.WriteLine("Não comparou com outra aresta!");
                return false;
            }

            return Object.Equals(this.vInicial, outraAresta.vInicial) &&
                   Object.Equals(this.vFinal, outraAresta.vFinal) &&
                   Object.Equals(this.nomeAresta, outraAresta.nomeAresta) &&
                   Object.Equals(this.peso, outraAresta.peso) &&
                   Object.Equals(this.guidCode, outraAresta.guidCode);
        }

        public override int GetHashCode()
        {
            var hashCode = 471315560;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertice>.Default.GetHashCode(vInicial);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertice>.Default.GetHashCode(vFinal);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nomeAresta);
            hashCode = hashCode * -1521134295 + EqualityComparer<int>.Default.GetHashCode(peso);
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(guidCode);
            return hashCode;
        }
    }
}
