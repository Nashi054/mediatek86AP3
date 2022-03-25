using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediatek86.metier;
using Mediatek86.controleur;
using System.Windows.Forms;

namespace Mediatek86.vue
{
    public partial class FrmFinAbonnement : Form
    {
        private readonly Controle controle;

        private readonly BindingSource bdgFinAbonnementListe = new BindingSource();
        private List<FinAbonnement> lesFinAbonnements = new List<FinAbonnement>();

        internal FrmFinAbonnement(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
        }

        /// <summary>
        /// Affiche la liste des abonnements qui arrivent à leur terme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmFinAbonnement_Load(object sender, EventArgs e)
        {
            lesFinAbonnements = controle.GetAllFinAbonnement();
            RemplirFinAbonnementListe(lesFinAbonnements);
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirFinAbonnementListe(List<FinAbonnement> finAbonnements)
        {
            bdgFinAbonnementListe.DataSource = finAbonnements;
            dgvFinAbonnement.DataSource = bdgFinAbonnementListe;
            dgvFinAbonnement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvFinAbonnement.Columns["titre"].DisplayIndex = 0;
            dgvFinAbonnement.Columns["dateFinAbonnement"].DisplayIndex = 1;
        }

        /// <summary>
        /// Ferme la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
