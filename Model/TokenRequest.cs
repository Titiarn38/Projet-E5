using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Model
{
    //requete du token, j'envoie au serveur un login/password
    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}