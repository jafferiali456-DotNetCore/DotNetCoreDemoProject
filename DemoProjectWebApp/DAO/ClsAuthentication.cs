using Microsoft.AspNetCore.Mvc;
using DemoProjectWebApp.Models;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoProjectWebApp.DAO
{
    public class ClsAuthentication
    {

        private const string SecretKey = "Mazen_Key_123_456_789_0000000000";
        private const string Issuer = "MazenWebApp";
        private const string Audience = "MazenMicroservice";
        private const string username = "mazen.1";
        public string GenerateToken()
        {
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> getAuthenticationToken(string endpoint)
        {
            try
            {
                AuthRequest req = new AuthRequest();
                req.Username = "mazen.1";
                req.Password = "1234";

                AuthResponse res = new AuthResponse();

                using (var client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

                    using (var Response = await client.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string authResponse = await Response.Content.ReadAsStringAsync();
                            res = JsonConvert.DeserializeObject<AuthResponse>(authResponse);
                        }
                    }
                }

                return res.Token;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        public async Task<string> APIMethodPost(string endpoint, string token, Object obj, string Method, string jwtToken = null)
        {
            try
            {

                #region test code
                using (var client = new HttpClient())
                {
                    string con;
                    if (!string.IsNullOrEmpty(jwtToken))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                    }
                    var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

                    //List<object> items = new List<object>();
                    //items.Add(obj);
                    //string con = JsonConvert.SerializeObject(items);
                    //// con="[ {\"id\": 0,\"refText1\": \"hello wordl123\",    \"refText2\": \"string\",    \"refText3\": \"string\",    \"refText4\": \"string\",    \"refText5\": \"string\",    \"refText6\": \"string\",    \"refText7\": \"string\",    \"refText8\": \"string\",    \"refText9\": \"string\",    \"refText10\": \"string\",    \"refText11\": \"string\",    \"refText12\": \"string\",    \"refText13\": \"string\",    \"refText14\": \"string\",    \"refText15\": \"string\",    \"refText16\": \"string\",    \"refText17\": \"string\",    \"refText18\": \"string\",    \"refText19\": \"string\",    \"refText20\": \"string\",    \"refText21\": \"string\",    \"refText22\": \"string\",    \"refText23\": \"string\",    \"refText24\": \"string\",    \"refText25\": \"string\",    \"refText26\": \"string\",    \"refText27\": \"string\",    \"refText28\": \"string\",    \"refText29\": \"string\",    \"refText30\": \"string\",    \"refText31\": \"string\",    \"refText32\": \"string\",    \"refText33\": \"string\",    \"refText34\": \"string\",    \"refText35\": \"string\",    \"refText36\": \"string\",    \"refText37\": \"string\",    \"refText38\": \"string\",    \"refText39\": \"string\",    \"refText40\": \"string\",    \"refText41\": \"string\",    \"refText42\": \"string\",    \"refText43\": \"string\",    \"refText44\": \"string\",    \"refText45\": \"string\",    \"refText46\": \"string\",    \"refText47\": \"string\",    \"refText48\": \"string\",    \"refText49\": \"string\",    \"refText50\": \"string\",    \"refInt1\": 0,    \"refInt2\": 0,    \"refInt3\": 0,    \"refInt4\": 0,    \"refInt5\": 0,    \"refInt6\": 0,    \"refInt7\": 0,    \"refInt8\": 0,    \"refInt9\": 0,    \"refInt10\": 0,    \"refInt11\": 0,    \"refInt12\": 0,    \"refInt13\": 0,    \"refInt14\": 0,    \"refInt15\": 0,    \"refInt16\": 0,    \"refInt17\": 0,    \"refInt18\": 0,    \"refInt19\": 0,    \"refInt20\": 0,    \"refInt21\": 0,    \"refInt22\": 0,    \"refInt23\": 0,    \"refInt24\": 0,    \"refInt25\": 0,    \"refDate1\": \"2024-12-09T07:24:44.459Z\",    \"refDate2\": \"2024-12-09T07:24:44.459Z\",    \"refDate3\": \"2024-12-09T07:24:44.459Z\",    \"refDate4\": \"2024-12-09T07:24:44.459Z\",    \"refDate5\": \"2024-12-09T07:24:44.459Z\",    \"refDate6\": \"2024-12-09T07:24:44.459Z\",    \"refDate7\": \"2024-12-09T07:24:44.459Z\",    \"refDate8\": \"2024-12-09T07:24:44.459Z\",    \"refDate9\": \"2024-12-09T07:24:44.459Z\",    \"refDate10\": \"2024-12-09T07:24:44.459Z\",    \"refDate11\": \"2024-12-09T07:24:44.459Z\",    \"refDate12\": \"2024-12-09T07:24:44.459Z\",    \"refDate13\": \"2024-12-09T07:24:44.459Z\",    \"refDate14\": \"2024-12-09T07:24:44.459Z\",    \"refDate15\": \"2024-12-09T07:24:44.459Z\",    \"refDate16\": \"2024-12-09T07:24:44.459Z\",    \"refDate17\": \"2024-12-09T07:24:44.459Z\",    \"refDate18\": \"2024-12-09T07:24:44.459Z\",    \"refDate19\": \"2024-12-09T07:24:44.459Z\",    \"refDate20\": \"2024-12-09T07:24:44.459Z\",    \"refDate21\": \"2024-12-09T07:24:44.459Z\",    \"refDate22\": \"2024-12-09T07:24:44.459Z\",    \"refDate23\": \"2024-12-09T07:24:44.459Z\",    \"refDate24\": \"2024-12-09T07:24:44.459Z\",    \"refDate25\": \"2024-12-09T07:24:44.459Z\",    \"refDbl1\": 0,    \"refDbl2\": 0,    \"refDbl3\": 0,    \"refDbl4\": 0,    \"refDbl5\": 0,    \"refDbl6\": 0,    \"refDbl7\": 0,    \"refDbl8\": 0,    \"refDbl9\": 0,    \"refDbl10\": 0,    \"refDbl11\": 0,    \"refDbl12\": 0,    \"refDbl13\": 0,    \"refDbl14\": 0,    \"refDbl15\": 0,    \"refDbl16\": 0,    \"refDbl17\": 0,    \"refDbl18\": 0,    \"refDbl19\": 0,    \"refDbl20\": 0,    \"refDbl21\": 0,    \"refDbl22\": 0,    \"refDbl23\": 0,    \"refDbl24\": 0,    \"refDbl25\": 0,    \"rec_Rec_Modified_on\": \"2024-12-09T07:24:44.459Z\",    \"rec_time_stamp\": \"2024-12-09T07:24:44.459Z\"  }]";

                    if (obj is System.Collections.IEnumerable && !(obj is string))
                    {
                        con = JsonConvert.SerializeObject(obj);
                    }
                    else
                    {
                        // obj is a single object, so wrap it in a list
                        List<object> items = new List<object>();
                        items.Add(obj);
                        con = JsonConvert.SerializeObject(items);
                    }
                    var content = new StringContent(con, null, "application/json");

                    request.Content = content;
                    if (Method == "PUT")
                    {
                        request.Method = HttpMethod.Put;
                    }
                    else if (Method == "POST")
                    {
                        request.Method = HttpMethod.Post;
                    }
                    else if (Method == "Delete")
                    {
                        request.Method = HttpMethod.Delete;
                    }
                    var response = await client.SendAsync(request);
                    //.WaitAsync(TimeSpan.FromMinutes(timesp));
                    //                 //  if (messagecode == 57)
                    response.EnsureSuccessStatusCode();
                    var res = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)// response.StatusCode == System.Net.HttpStatusCode.OK)

                    {
                        return "Success";
                    }
                    else return response.StatusCode + "|" + response.ReasonPhrase;


                    //if (Method == "PUT")
                    //{
                    //    using (var Response = await client.PutAsJsonAsync(endpoint, obj))
                    //    {
                    //        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    //        {
                    //            //string temp = JsonConvert.SerializeObject(obj);
                    //            return "Success";
                    //        }
                    //        else
                    //        {
                    //            return Response.StatusCode + "|" + Response.ReasonPhrase;
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //}
                }


                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> APIMethodPostReturnResponse(string endpoint, string token, Object obj, string Method,string jwtToken=null)
        {
            try
            {

                #region test code
                using (var client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(jwtToken))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                    }
                    var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                    //List<object> items = new List<object>();
                    //items.Add(obj);
                    string con = JsonConvert.SerializeObject(obj);
                    // con="[ {\"id\": 0,\"refText1\": \"hello wordl123\",    \"refText2\": \"string\",    \"refText3\": \"string\",    \"refText4\": \"string\",    \"refText5\": \"string\",    \"refText6\": \"string\",    \"refText7\": \"string\",    \"refText8\": \"string\",    \"refText9\": \"string\",    \"refText10\": \"string\",    \"refText11\": \"string\",    \"refText12\": \"string\",    \"refText13\": \"string\",    \"refText14\": \"string\",    \"refText15\": \"string\",    \"refText16\": \"string\",    \"refText17\": \"string\",    \"refText18\": \"string\",    \"refText19\": \"string\",    \"refText20\": \"string\",    \"refText21\": \"string\",    \"refText22\": \"string\",    \"refText23\": \"string\",    \"refText24\": \"string\",    \"refText25\": \"string\",    \"refText26\": \"string\",    \"refText27\": \"string\",    \"refText28\": \"string\",    \"refText29\": \"string\",    \"refText30\": \"string\",    \"refText31\": \"string\",    \"refText32\": \"string\",    \"refText33\": \"string\",    \"refText34\": \"string\",    \"refText35\": \"string\",    \"refText36\": \"string\",    \"refText37\": \"string\",    \"refText38\": \"string\",    \"refText39\": \"string\",    \"refText40\": \"string\",    \"refText41\": \"string\",    \"refText42\": \"string\",    \"refText43\": \"string\",    \"refText44\": \"string\",    \"refText45\": \"string\",    \"refText46\": \"string\",    \"refText47\": \"string\",    \"refText48\": \"string\",    \"refText49\": \"string\",    \"refText50\": \"string\",    \"refInt1\": 0,    \"refInt2\": 0,    \"refInt3\": 0,    \"refInt4\": 0,    \"refInt5\": 0,    \"refInt6\": 0,    \"refInt7\": 0,    \"refInt8\": 0,    \"refInt9\": 0,    \"refInt10\": 0,    \"refInt11\": 0,    \"refInt12\": 0,    \"refInt13\": 0,    \"refInt14\": 0,    \"refInt15\": 0,    \"refInt16\": 0,    \"refInt17\": 0,    \"refInt18\": 0,    \"refInt19\": 0,    \"refInt20\": 0,    \"refInt21\": 0,    \"refInt22\": 0,    \"refInt23\": 0,    \"refInt24\": 0,    \"refInt25\": 0,    \"refDate1\": \"2024-12-09T07:24:44.459Z\",    \"refDate2\": \"2024-12-09T07:24:44.459Z\",    \"refDate3\": \"2024-12-09T07:24:44.459Z\",    \"refDate4\": \"2024-12-09T07:24:44.459Z\",    \"refDate5\": \"2024-12-09T07:24:44.459Z\",    \"refDate6\": \"2024-12-09T07:24:44.459Z\",    \"refDate7\": \"2024-12-09T07:24:44.459Z\",    \"refDate8\": \"2024-12-09T07:24:44.459Z\",    \"refDate9\": \"2024-12-09T07:24:44.459Z\",    \"refDate10\": \"2024-12-09T07:24:44.459Z\",    \"refDate11\": \"2024-12-09T07:24:44.459Z\",    \"refDate12\": \"2024-12-09T07:24:44.459Z\",    \"refDate13\": \"2024-12-09T07:24:44.459Z\",    \"refDate14\": \"2024-12-09T07:24:44.459Z\",    \"refDate15\": \"2024-12-09T07:24:44.459Z\",    \"refDate16\": \"2024-12-09T07:24:44.459Z\",    \"refDate17\": \"2024-12-09T07:24:44.459Z\",    \"refDate18\": \"2024-12-09T07:24:44.459Z\",    \"refDate19\": \"2024-12-09T07:24:44.459Z\",    \"refDate20\": \"2024-12-09T07:24:44.459Z\",    \"refDate21\": \"2024-12-09T07:24:44.459Z\",    \"refDate22\": \"2024-12-09T07:24:44.459Z\",    \"refDate23\": \"2024-12-09T07:24:44.459Z\",    \"refDate24\": \"2024-12-09T07:24:44.459Z\",    \"refDate25\": \"2024-12-09T07:24:44.459Z\",    \"refDbl1\": 0,    \"refDbl2\": 0,    \"refDbl3\": 0,    \"refDbl4\": 0,    \"refDbl5\": 0,    \"refDbl6\": 0,    \"refDbl7\": 0,    \"refDbl8\": 0,    \"refDbl9\": 0,    \"refDbl10\": 0,    \"refDbl11\": 0,    \"refDbl12\": 0,    \"refDbl13\": 0,    \"refDbl14\": 0,    \"refDbl15\": 0,    \"refDbl16\": 0,    \"refDbl17\": 0,    \"refDbl18\": 0,    \"refDbl19\": 0,    \"refDbl20\": 0,    \"refDbl21\": 0,    \"refDbl22\": 0,    \"refDbl23\": 0,    \"refDbl24\": 0,    \"refDbl25\": 0,    \"rec_Rec_Modified_on\": \"2024-12-09T07:24:44.459Z\",    \"rec_time_stamp\": \"2024-12-09T07:24:44.459Z\"  }]";


                    var content = new StringContent(con, null, "application/json");

                    request.Content = content;
                    if (Method == "PUT")
                    {
                        request.Method = HttpMethod.Put;
                    }
                    else if (Method == "POST")
                    {
                        request.Method = HttpMethod.Post;
                    }
                    else if (Method == "Delete")
                    {
                        request.Method = HttpMethod.Delete;
                    }
                    var response = await client.SendAsync(request);
                    //.WaitAsync(TimeSpan.FromMinutes(timesp));
                    //                 //  if (messagecode == 57)
                    response.EnsureSuccessStatusCode();
                    return response;
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
