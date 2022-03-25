using System.Collections.Generic;
using Mediatek86.modele;
using Mediatek86.metier;
using Mediatek86.vue;
using System;

namespace Mediatek86.controleur
{
    internal class Controle
    {
        private readonly List<Livre> lesLivres;
        private readonly List<Dvd> lesDvd;
        private readonly List<Revue> lesRevues;
        private readonly List<Categorie> lesRayons;
        private readonly List<Categorie> lesPublics;
        private readonly List<Categorie> lesGenres;

        /// <summary>
        /// Ouverture de la fenêtre
        /// </summary>
        public Controle()
        {
            lesLivres = Dao.GetAllLivres();
            lesDvd = Dao.GetAllDvd();
            lesRevues = Dao.GetAllRevues();
            lesGenres = Dao.GetAllGenres();
            lesRayons = Dao.GetAllRayons();
            lesPublics = Dao.GetAllPublics();
            FrmMediatek frmMediatek = new FrmMediatek(this);
            frmMediatek.ShowDialog();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Collection d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return lesGenres;
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Collection d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return lesLivres;
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Collection d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return lesDvd;
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Collection d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return lesRevues;
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return lesRayons;
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return lesPublics;
        }

        /// <summary>
        /// getter sur les suivis
        /// </summary>
        /// <returns>Collection d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            return Dao.GetAllSuivis();
        }

        /// <summary>
        /// getter sur les commandes de livre et de dvd
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns>Collection d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandesLivreDvd(string idDocument)
        {
            return Dao.GetCommandesLivreDvd(idDocument);
        }

        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <returns>Collection d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return Dao.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return Dao.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Crée une commande dans la bdd
        /// </summary>
        /// <param name="commande">L'objet CommandeDocument concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommande(CommandeDocument commande)
        {
            return Dao.CreerCommande(commande);
        }

        /// <summary>
        /// Met à jour le suivi d'une commande dans la bdd
        /// </summary>
        /// <param name="commande">L'objet CommandeDocument concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        internal bool UpdateCommande(CommandeDocument commande)
        {
            return Dao.UpdateCommande(commande);
        }

        /// <summary>
        /// Supprime une commande dans la bdd
        /// </summary>
        /// <param name="commande">L'objet CommandeDocument concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        internal bool DeleteCommande(CommandeDocument commande)
        {
            return Dao.DeleteCommande(commande);
        }
    }

}

