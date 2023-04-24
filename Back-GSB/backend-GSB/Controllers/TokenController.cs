using Model;
using ORM_GSB;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;



namespace Back_GSB.Controllers
{
    public class TokenController : ApiController
    {



        private GSB_Data_Model db = new GSB_Data_Model();



        //api/Token
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] TokenRequest login)
        {
            User user = db.Users.Where(U => U.Pseudo.Equals(login.Username) && U.MotDePasse.Equals(login.Password)).FirstOrDefault();



            var loginResponse = new TokenResponse { };
            TokenRequest loginrequest = new TokenRequest { };
            loginrequest.Username = login.Username;
            loginrequest.Password = login.Password;



            IHttpActionResult response;
            bool isUsernamePasswordValid = false;



            if (login != null && user != null)
                isUsernamePasswordValid = (loginrequest.Username == user.Pseudo && loginrequest.Password == user.MotDePasse) ? true : false;

            if (isUsernamePasswordValid)
            {
                string token = createToken(loginrequest.Username);



                StoreToken(token);
                return Ok<string>(token);
            }
            else
            {
                loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                response = ResponseMessage(loginResponse.responseMsg);
                return response;
            }
        }




        private string createToken(string username)
        {
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddMinutes(10);



            var tokenHandler = new JwtSecurityTokenHandler();



            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });



            const string sec = "LeSoleilEtLaMereQueDemandeLePeuple";
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);




            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "https://localhost:44369", audience: "https://localhost:44369",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);



            return tokenString;
        }





        private void StoreToken(string token)
        {
            string fileName = @"C:\Temp\token.txt";




            // Check if file already exists. If yes, delete it.     
            if (File.Exists(fileName))
                File.Delete(fileName);



            // Create a new file     
            using (FileStream fs = File.Create(fileName))
            {



                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes(token);
                fs.Write(title, 0, title.Length);



            }

        }









    }
}