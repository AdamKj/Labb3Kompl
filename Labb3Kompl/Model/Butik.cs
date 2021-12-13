using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labb3Kompl.Model
{
    class Butik
    {
        public string Name { get; set; }
        public User Admin { get; set; }

        public Dictionary<Produkt, int> Lager;

        public bool LogInAdmin(User user)
        {
            return true;
        }

        public async Task CheckOutUser(User kund)
        {
            //Töm kundkorgen och gör en message.showbox när det är klart och stäng ner
        }
    }
}
