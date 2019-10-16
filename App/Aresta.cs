using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp
{
    class Aresta
    {
        // TODO: Inserir um novo atributo "Peso" e
        // Gerar um novo override em Equals para comparar os Pesos também
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
