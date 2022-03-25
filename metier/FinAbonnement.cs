using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class FinAbonnement
    {
        private readonly string titre;
        private readonly DateTime dateFinAbonnement;

        public FinAbonnement(string titre, DateTime dateFinAbonnement)
        {
            this.titre = titre;
            this.dateFinAbonnement = dateFinAbonnement;
        }

        public string Titre { get => titre; }
        public DateTime DateFinAbonnement { get => dateFinAbonnement; }
    }
}
