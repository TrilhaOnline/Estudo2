using DC.Resources.Utils.Converter;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DC.Application.Console
{
    /// <summary>
    /// Classe console responsável por chamada de APIs de criação de Usuário e Geração de Token para o usuário criado
    /// Auto: Marcelo Branco
    /// </summary>
    class Program
    {
        public static HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {            
            #region Passo variáveis simulando um cadastro de usuário na base
            var input = new CustomerRequestDTO
            {
                email = "teste@teste.com", //Email 
                codeIn = "teste@123", //Senha
                repeatCodeIn = "teste@123",//Repete Senha
                nickName = "Joca"
            };

            #endregion

            #region Crio uma criptografia dos dados sensíveis a serem transitado pela API (Senha e Repetição de Senha)
            var request = new HttpRequestMessage();
            input.codeIn = EncodeDecode.CrashIn(input.codeIn);
            input.repeatCodeIn = EncodeDecode.CrashIn(input.repeatCodeIn);
            #endregion

            #region Crio o usuário na base
            var resultCreateUser = CallCreateUser(input);
            #endregion

            #region Crio token de acesso ao Sistema para o usuário anterior
            if (resultCreateUser != null)
            {
                if (resultCreateUser.FirstOrDefault().hasSuccess == true && resultCreateUser.FirstOrDefault().logged == true)
                {
                    #region Crio o Token de acesso ao sistema
                    var resultCreatToken = CallCreateToken(new AuthenticationRequestDTO
                    {
                        email = input.email,
                        nameSystem = "ExemploNomeAplicacao",
                        sKey = resultCreateUser.FirstOrDefault().guid,
                        userName = input.nickName
                    });
                    #endregion                    
                }
            }
            #endregion
        }

        /// <summary>
        /// Método responsável pela chamada de criação do token de autenticação
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static AuthenticationResponseDTO[] CallCreateToken(AuthenticationRequestDTO data)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var result = new AuthenticationResponseDTO[] { };
            if (client.BaseAddress == null)
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["api-base"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = client.PostAsync("api/security/accesstoken", content).Result;
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<AuthenticationResponseDTO[]>(response.Content.ReadAsStringAsync().Result);
            return result;
        }

        /// <summary>
        /// Método responsável pela chamada de serviço para criação do usuário na base
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CustomerResponseDTO[] CallCreateUser(CustomerRequestDTO data)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var result = new CustomerResponseDTO[] { };
            if (client.BaseAddress == null)
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["api-base"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = client.PostAsync("api/security/createuser", content).Result;
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
                result = JsonConvert.DeserializeObject<CustomerResponseDTO[]>(response.Content.ReadAsStringAsync().Result);
            return result;
        }
    }

    /// <summary>
    /// Entidade responsável pelo input de dados para cadastro de usuário
    /// </summary>
    public class CustomerRequestDTO
    {
        public string customerCode { get; set; }
        public string ip { get; set; }
        public string nickName { get; set; }
        public string email { get; set; }
        public string codeIn { get; set; }
        public string nameSystem { get; set; }
        public string repeatCodeIn { get; set; }
        public string name { get; set; }
        public string descriptionTitleOne { get; set; }
        public string descriptionSubTitleOne { get; set; }
        public string idCustomer { get; set; }
        public string url { get; set; }
    }

    /// <summary>
    /// Entidade responsável pelo output do cadastro do usuário na base
    /// </summary>
    public class CustomerResponseDTO 
    {
        public CustomerResponseDTO()
        {
            this.logged = false;
            this.hasSuccess = false;
            this.hasExist = false;
        }
        public string nickName { get; set; }
        public string url { get; set; }
        public string guid { get; set; }
        public string message { get; set; }
        public bool logged { get; set; }
        public bool hasSuccess { get; set; }
        public bool hasExist { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }

    /// <summary>
    /// Entidade responsável pelo output de dados para retorno do token de autenticação
    /// </summary>
    public class AuthenticationResponseDTO
    {
        public AuthenticationResponseDTO() 
        {
            this.hasAccess = false;
        }
        public string guid { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public bool generateToken { get; set; }
        public bool hasAccess { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string url { get; set; }
        public string haskKeyUser { get; set; }
        public DateTime processDate { get; set; }
        public string nameSystem { get; set; }
        public DateTimeOffset? expires { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }

    /// <summary>
    /// Entidade responsável pelo input de dados de autenticação
    /// </summary>
    public class AuthenticationRequestDTO
    {
        public string userName { get; set; }
        public string codeIn { get; set; }
        public string nameSystem { get; set; }
        public string email { get; set; }
        public string sKey { get; set; }
    }
}
