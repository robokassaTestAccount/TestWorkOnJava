using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiAutotests
{
    public class StandartizationApi
    {
        public string RequestPattern {  get; private set; }
        private WebRequest Request { get; set; }
        private byte[] Contenet { get; set; }

        public StandartizationApi(HelperStruct.RequestInfo info)
        {            
            RequestPattern = "https://dadata.ru/api/v2/clean/name";
            Request = WebRequest.Create(RequestPattern);
            Request.Method = info.Method;
            Request.ContentType = info.ContentType;
            Request.Headers.Add("Authorization", info.AuthorizationToken);
            Request.Headers.Add("X-Secret", info.XSecret);
            Request.ContentLength = Encoding.UTF8.GetBytes(info.Name).Length;
            Contenet = Encoding.UTF8.GetBytes(info.Name);
        }
        public HelperStruct.ResponceInfo SendReq()
        {
            HelperStruct.ResponceInfo info = new HelperStruct.ResponceInfo();
            using (Stream dataStream = Request.GetRequestStream())
            {
                dataStream.Write(Contenet, 0, Contenet.Length);
            }

            WebResponse response = Request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    info = DeserializeResponce(reader.ReadToEnd()).First();
                }
            }
            response.Close();
            return info;
        }

        public string SendErrorReq()
        {
            try
            {
                using (Stream dataStream = Request.GetRequestStream())
                {
                    dataStream.Write(Contenet, 0, Contenet.Length);
                }

                WebResponse response = Request.GetResponse();
                return "0";
            }
            catch(Exception ex)
            {
                string pattern = @"\d\d\d";
                return Regex.Match(ex.Message, pattern).Value;
            }
        }

        private List<HelperStruct.ResponceInfo> DeserializeResponce(string responce)
        {
            return JsonConvert.DeserializeObject<List<HelperStruct.ResponceInfo>>(responce);
        }
    }
}
