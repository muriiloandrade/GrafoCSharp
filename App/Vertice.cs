using System;
using System.Collections.Generic;

namespace GraphApp
{
    class Vertice
    {
        private string nomeVertice { get; set; }
        private Guid guidCode { get; set; }

        Vertice(string nome)
        {
            this.nomeVertice = nome;
            this.guidCode = new Guid();
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Vertice);
        }

        public bool Equals(Vertice outroVertice)
        {
            if (outroVertice == null)
            {
                Console.WriteLine("Não comparou com outro vértice!");
                return false;
            }

            return this.guidCode.Equals(outroVertice.guidCode) &&
                   this.nomeVertice.Equals(outroVertice.nomeVertice);
        }

        public override int GetHashCode()
        {
            var hashCode = -802182721;
            hashCode = hashCode * -1521134295 + nomeVertice.GetHashCode();
            hashCode = hashCode * -1521134295 + guidCode.GetHashCode();
            return hashCode;
        }
    }
}
