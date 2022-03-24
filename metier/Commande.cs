using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class Commande
    {
        public Commande(string id, DateTime dateCommande, double montant, string idSuivi, string suivi)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
            this.IdSuivi = idSuivi;
            this.Suivi = suivi;
        }

        public string Id { get; set; }
        public DateTime DateCommande { get; set; }
        public double Montant { get; set; }
        public string IdSuivi { get; set; }
        public string Suivi { get; set; }
    }
}
