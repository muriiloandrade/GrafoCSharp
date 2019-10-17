using System;

namespace GraphApp
{
    class Vertice
    {
        internal int grau { get; set; }
        internal string nomeVertice { get; set; }
        internal Guid guidCode { get; set; }

        public Vertice(string nome)
        {
            this.nomeVertice = nome;
            this.guidCode = Guid.NewGuid();
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Vertice);
        }

        internal bool Equals(Vertice outroVertice)
        {
            if (outroVertice == null)
            {
                Console.WriteLine("Não comparou com outro vértice!");
                return false;
            }

            return this.nomeVertice.Equals(outroVertice.nomeVertice);
        }

        public override int GetHashCode()
        {
            var hashCode = -802182721;
            hashCode = hashCode * -1521134295 + nomeVertice.GetHashCode();
            hashCode = hashCode * -1521134295 + guidCode.GetHashCode();
            hashCode = hashCode * -1521134295 + grau.GetHashCode();
            return hashCode;
        }
    }
}
