using System.Collections.Generic;

namespace Labb3Kompl.Model
{
    class User
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public Dictionary<Produkt, int> Kundkorg;
    }
}
