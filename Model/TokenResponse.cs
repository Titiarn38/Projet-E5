using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;



namespace Model
{
    //répnse du serveur syite à ma demande du token
    public class TokenResponse
    {
        //constructeur
        public TokenResponse()
        {
            this.Token = string.Empty;
            this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }



        public string Token { get; set; }
        public HttpResponseMessage responseMsg { get; set; }
    }
}