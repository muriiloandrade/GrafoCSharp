using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp
{
    class Aresta
    {
        Vertice vInicial;
        Vertice vFinal;
        private Guid guidCode { get; set; }
        private string nomeAresta { get; set; }

        Aresta(Vertice inicial, Vertice final, String nome)
        {
            this.vInicial = inicial;
            this.vFinal = final;
            this.nomeAresta = nome;
            this.guidCode = new Guid();
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Aresta);
        }

        public bool Equals(Aresta outraAresta)
        {
            if (outraAresta == null)
            {
                Console.WriteLine("Não comparou com outra aresta!");
                return false;
            }

            return Object.Equals(this.vInicial, outraAresta.vInicial) &&
                   Object.Equals(this.vFinal, outraAresta.vFinal) &&
                   Object.Equals(this.nomeAresta, outraAresta.nomeAresta) &&
                   Object.Equals(this.guidCode, outraAresta.guidCode);
        }

        public override int GetHashCode()
        {
            var hashCode = 471315560;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertice>.Default.GetHashCode(vInicial);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertice>.Default.GetHashCode(vFinal);
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(guidCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nomeAresta);
            return hashCode;
        }
    }
}
