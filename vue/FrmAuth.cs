using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mediatek86.metier;
using Mediatek86.controleur;

namespace Mediatek86.vue
{
    public partial class FrmAuth : Form
    {
        private readonly Controle controle;

        internal FrmAuth(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
        }



        /// <summary>
        /// Gère les parties auxquelles l'utilisateur a accès en fonction
        /// du service auxquel il appartient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnexion_Click(object sender, EventArgs e)
        {
            FrmFinAbonnement frmFinAbonnement = new FrmFinAbonnement(controle);
            FrmMediatek frmMediatek = new FrmMediatek(controle);

            string login = txbLogin.Text;
            string pwd = txbPwd.Text;
            List<Utilisateur> lesUtilisateurs = controle.GetUtilisateur(login, pwd);

            if (lesUtilisateurs.Count() != 0)
            {
                foreach (Utilisateur utilisateur in lesUtilisateurs)
                {
                    if (utilisateur.IdService == "0" || utilisateur.IdService == "1")
                    {
                        this.Visible = false;
                        controle.Authentification(1);
                    }
                    else if (utilisateur.IdService == "2")
                    {
                        this.Visible = false;
                        controle.Authentification(2);
                    }
                    else
                    {
                        this.Visible = false;
                        MessageBox.Show("Vous n'avez pas les droits suffisants pour accéder à cette application");
                    }
                }
            }
            else
            {
                MessageBox.Show("Login ou mot de passe invalide", "Information");
            }
        }
    }
}
