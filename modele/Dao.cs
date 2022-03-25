using Mediatek86.metier;
using System.Collections.Generic;
using Mediatek86.bdd;
using System;
using System.Windows.Forms;

namespace Mediatek86.modele
{
    public static class Dao
    {

        private static readonly string server = "localhost";
        private static readonly string userid = "root";
        private static readonly string password = "";
        private static readonly string database = "mediatek86";
        private static readonly string connectionString = "server="+server+";user id="+userid+";password="+password+";database="+database+";SslMode=none";

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public static List<Categorie> GetAllGenres()
        {
            List<Categorie> lesGenres = new List<Categorie>();
            string req = "Select * from genre order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Genre genre = new Genre((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesGenres.Add(genre);
            }
            curs.Close();
            return lesGenres;
        }

        /// <summary>
        /// Retourne tous les abonnements qui arrivent à leur terme
        /// </summary>
        /// <returns>Collection d'objets FinAbonnement</returns>
        public static List<FinAbonnement> GetAllFinAbonnement()
        {
            List<FinAbonnement> lesFinAbonnements = new List<FinAbonnement>();
            string req = "Call finAbonnement()";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                FinAbonnement finAbonnement = new FinAbonnement((string)curs.Field("titre"), (DateTime)curs.Field("dateFinAbonnement"));
                lesFinAbonnements.Add(finAbonnement);
            }
            curs.Close();
            return lesFinAbonnements;
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public static List<Categorie> GetAllRayons()
        {
            List<Categorie> lesRayons = new List<Categorie>();
            string req = "Select * from rayon order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Rayon rayon = new Rayon((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesRayons.Add(rayon);
            }
            curs.Close();
            return lesRayons;
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public static List<Categorie> GetAllPublics()
        {
            List<Categorie> lesPublics = new List<Categorie>();
            string req = "Select * from public order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Public lePublic = new Public((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesPublics.Add(lePublic);
            }
            curs.Close();
            return lesPublics;
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public static List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = new List<Livre>();
            string req = "Select l.id, l.ISBN, l.auteur, d.titre, d.image, l.collection, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from livre l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                string isbn = (string)curs.Field("ISBN");
                string auteur = (string)curs.Field("auteur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string collection = (string)curs.Field("collection");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idgenre, genre, 
                    idpublic, lepublic, idrayon, rayon);
                lesLivres.Add(livre);
            }
            curs.Close();

            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public static List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = new List<Dvd>();
            string req = "Select l.id, l.duree, l.realisateur, d.titre, d.image, l.synopsis, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from dvd l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                int duree = (int)curs.Field("duree");
                string realisateur = (string)curs.Field("realisateur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string synopsis = (string)curs.Field("synopsis");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon);
                lesDvd.Add(dvd);
            }
            curs.Close();

            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public static List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = new List<Revue>();
            string req = "Select l.id, l.empruntable, l.periodicite, d.titre, d.image, l.delaiMiseADispo, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from revue l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                bool empruntable = (bool)curs.Field("empruntable");
                string periodicite = (string)curs.Field("periodicite");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                int delaiMiseADispo = (int)curs.Field("delaimiseadispo");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Revue revue = new Revue(id, titre, image, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon, empruntable, periodicite, delaiMiseADispo);
                lesRevues.Add(revue);
            }
            curs.Close();

            return lesRevues;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <returns>Liste d'objets Exemplaire</returns>
        public static List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
            string req = "Select e.id, e.numero, e.dateAchat, e.photo, e.idEtat ";
            req += "from exemplaire e join document d on e.id=d.id ";
            req += "where e.id = @id ";
            req += "order by e.dateAchat DESC";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);

            while (curs.Read())
            {
                string idDocuement = (string)curs.Field("id");
                int numero = (int)curs.Field("numero");
                DateTime dateAchat = (DateTime)curs.Field("dateAchat");
                string photo = (string)curs.Field("photo");
                string idEtat = (string)curs.Field("idEtat");
                Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocuement);
                lesExemplaires.Add(exemplaire);
            }
            curs.Close();

            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public static bool CreerExemplaire(Exemplaire exemplaire)
        {
            try
            {
                string req = "insert into exemplaire values (@idDocument,@numero,@dateAchat,@photo,@idEtat)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idDocument", exemplaire.IdDocument},
                    { "@numero", exemplaire.Numero},
                    { "@dateAchat", exemplaire.DateAchat},
                    { "@photo", exemplaire.Photo},
                    { "@idEtat",exemplaire.IdEtat}
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }catch{
                return false;
            }
        }

        /// <summary>
        /// ecriture d'une commande de livre ou de dvd en base de données
        /// </summary>
        /// <param name="commande"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public static bool CreerCommandeLivreDvd(CommandeDocument commande)
        {
            try
            {
                string req2 = "insert into commande values (@id, @dateCommande, @montant, @idSuivi)";
                Dictionary<string, object> parameters2 = new Dictionary<string, object>
                {
                    { "@id", commande.Id},
                    { "@dateCommande", commande.DateCommande},
                    { "@montant", commande.Montant},
                    { "@idSuivi", commande.IdSuivi}
                };
                BddMySql curs2 = BddMySql.GetInstance(connectionString);
                curs2.ReqUpdate(req2, parameters2);
                curs2.Close();

                string req1 = "insert into commandedocument values ((select id from commande where id = @id), @nbExemplaire, @idLivreDvd)";
                Dictionary<string, object> parameters1 = new Dictionary<string, object>
                {
                    { "@id", commande.Id},
                    { "@nbExemplaire", commande.NbExemplaire},
                    { "@idLivreDvd", commande.IdLivreDvd}
                };
                BddMySql curs1 = BddMySql.GetInstance(connectionString);
                curs1.ReqUpdate(req1, parameters1);
                curs1.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ecriture d'une commande de revue en base de données
        /// </summary>
        /// <param name="commande"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public static bool CreerCommandeRevue(Abonnement commande)
        {
            try
            {
                string req2 = "insert into commande values (@id, @dateCommande, @montant, @idSuivi)";
                Dictionary<string, object> parameters2 = new Dictionary<string, object>
                {
                    { "@id", commande.Id},
                    { "@dateCommande", commande.DateCommande},
                    { "@montant", commande.Montant},
                    { "@idSuivi", null}
                };
                BddMySql curs2 = BddMySql.GetInstance(connectionString);
                curs2.ReqUpdate(req2, parameters2);
                curs2.Close();

                string req1 = "insert into abonnement values ((select id from commande where id = @id), @dateFinAbonnement, @idRevue)";
                Dictionary<string, object> parameters1 = new Dictionary<string, object>
                {
                    { "@id", commande.Id},
                    { "@dateFinAbonnement", commande.DateFinAbonnement},
                    { "@idRevue", commande.IdRevue}
                };
                BddMySql curs1 = BddMySql.GetInstance(connectionString);
                curs1.ReqUpdate(req1, parameters1);
                curs1.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// mise à jour d'une commande en base de données
        /// </summary>
        /// <param name="commande"></param>
        /// <returns>true si l'update a pu se faire</returns>
        public static bool UpdateCommande(CommandeDocument commande)
        {
            try
            {
                string req = "update commande set idSuivi = @idSuivi where id = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idSuivi", commande.IdSuivi},
                    { "@id", commande.Id}
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// supprime une commande de livre ou de dvd en base de données
        /// </summary>
        /// <param name="commande"></param>
        /// <returns>true si la suppression a pu se faire</returns>
        public static bool DeleteCommandeLivreDvd(CommandeDocument commande)
        {
            try
            {
                string req1 = "delete from commandedocument where id = @id";
                Dictionary<string, object> parameters1 = new Dictionary<string, object>
                {
                    { "@id", commande.Id}
                };
                BddMySql curs1 = BddMySql.GetInstance(connectionString);
                curs1.ReqUpdate(req1, parameters1);
                curs1.Close();

                string req2 = "delete from commande where id = @id";
                Dictionary<string, object> parameters2 = new Dictionary<string, object>
                {
                    { "@id", commande.Id}
                };
                BddMySql curs2 = BddMySql.GetInstance(connectionString);
                curs2.ReqUpdate(req2, parameters2);
                curs2.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// supprime une commande de revue en base de données
        /// </summary>
        /// <param name="commande"></param>
        /// <returns>true si la suppression a pu se faire</returns>
        public static bool DeleteCommandeRevue(Abonnement commande)
        {
            try
            {
                string req1 = "delete from abonnement where id = @id";
                Dictionary<string, object> parameters1 = new Dictionary<string, object>
                {
                    { "@id", commande.Id}
                };
                BddMySql curs1 = BddMySql.GetInstance(connectionString);
                curs1.ReqUpdate(req1, parameters1);
                curs1.Close();

                string req2 = "delete from commande where id = @id";
                Dictionary<string, object> parameters2 = new Dictionary<string, object>
                {
                    { "@id", commande.Id}
                };
                BddMySql curs2 = BddMySql.GetInstance(connectionString);
                curs2.ReqUpdate(req2, parameters2);
                curs2.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retourne tous les suivis à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public static List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = new List<Suivi>();
            string req = "Select * from suivi order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Suivi suivi = new Suivi((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesSuivis.Add(suivi);
            }
            curs.Close();
            return lesSuivis;
        }

        /// <summary>
        /// Retourne toutes les commandes de livre ou de DVD à partir de la BDD
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns>Collection d'objets CommandeDocument</returns>
        public static List<CommandeDocument> GetCommandesLivreDvd(string idDocument)
        {
            List<CommandeDocument> lesCommandes = new List<CommandeDocument>();
            string req = "Select d.id, c.dateCommande as 'date commande', c.montant, c.idSuivi, s.libelle as suivi, ";
            req += "d.nbExemplaire as 'nombre exemplaire', d.idLivreDvd ";
            req += "from commandedocument d join commande c on d.id=c.id ";
            req += "join suivi s on s.id=c.idSuivi ";
            req += "where d.idLivreDvd = @id ";
            req += "order by 'date commande' DESC ";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                DateTime dateCommande = (DateTime)curs.Field("date commande");
                double montant = (double)curs.Field("montant");
                string idSuivi = (string)curs.Field("idSuivi");
                string suivi = (string)curs.Field("suivi");
                int nbExemplaire = (int)curs.Field("nombre exemplaire");
                string idLivreDvd = (string)curs.Field("idLivreDvd");
                CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, idSuivi, suivi, nbExemplaire, idLivreDvd);
                lesCommandes.Add(commandeDocument);
            }
            curs.Close();

            return lesCommandes;
        }

        /// <summary>
        /// Retourne toutes les commandes de revue à partir de la BDD
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns>Collection d'objets Abonnement</returns>
        public static List<Abonnement> GetCommandesRevues(string idDocument)
        {
            List<Abonnement> lesCommandes = new List<Abonnement>();
            string req = "Select a.id, c.dateCommande as 'date commande', c.montant, ";
            req += "a.dateFinAbonnement as 'date fin abonnement', a.idRevue ";
            req += "from abonnement a join commande c on a.id=c.id ";
            req += "where a.idRevue = @id ";
            req += "order by 'date commande' DESC ";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                DateTime dateCommande = (DateTime)curs.Field("date commande");
                double montant = (double)curs.Field("montant");
                DateTime dateFinAbonnement = (DateTime)curs.Field("date fin abonnement");
                string idRevue = (string)curs.Field("idRevue");
                Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
                lesCommandes.Add(abonnement);
            }
            curs.Close();

            return lesCommandes;
        }
    }
}
