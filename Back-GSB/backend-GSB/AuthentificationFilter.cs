using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;

namespace backend_GSB
{
    public class AuthentificationFilter : ActionFilterAttribute
    {

        // définition d'une varibale qui stock le token que je vais utiliser pour pouvoir sécuriser mes api
        private string ApiKeyToCheck = "123456789";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            bool validKey = false;

            IEnumerable<string> requestHeader;

            var checkApiExist = actionContext.Request.Headers.TryGetValues("token", out requestHeader);

            // je suis OK, le token récupéré depuis le header est authentique
            if (checkApiExist == true)
            {
                if (requestHeader.FirstOrDefault().Equals(ApiKeyToCheck))
                    validKey = true;
            }

            // si le contraire, le token n'est authentique, j'affiche un message accès non autorisé
            if (!validKey)
            {
                
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);

            }
            }
    }
}