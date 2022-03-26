using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class Utilisateur
    {

        private readonly string id;
        private readonly string login;
        private readonly string pwd;
        private readonly string idService;

        public Utilisateur(string id, string login, string pwd, string idService)
        {
            this.id = id;
            this.login = login;
            this.pwd = pwd;
            this.idService = idService;
        }

        public string Id { get => id; }
        public string Login { get => login; }
        public string Pwd { get => pwd; }
        public string IdService { get => idService; }
    }
}
