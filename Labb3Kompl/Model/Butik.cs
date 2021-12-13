using System.Collections.Generic;

namespace Labb3Kompl.Model
{
    class Butik
    {
        public string Name { get; set; }
        public User Admin { get; set; }

        public Dictionary<Produkt, int> Lager;
    }
}
