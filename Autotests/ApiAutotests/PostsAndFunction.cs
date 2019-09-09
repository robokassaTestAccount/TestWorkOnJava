using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiAutotests
{
    [TestClass]
    public class PostsAndFunction : BasicActions
    {       
        [TestMethod]
        public void SeasonCheck()
        {
            for(int i = -2; i<20; i++)
            {
                Logger.WriteInfo(GetSeason(i));
            }            
        }      
        
        [TestMethod]
        public void PostCheck()
        {
            PostClient.Auth();
        }

        [TestMethod]
        public void NameCheck()
        {
            HelperStruct.RequestInfo request = new HelperStruct.RequestInfo()
            {
                ContentType = "application/json",
                AuthorizationToken = "Token 21e39bb408ecf9647ed89b86398684941455fd30",
                XSecret = "fc5e7048820531bdec6d761d5ecb12a6463d2a37",
                Name = "[\"Иванов сергей петровеч\"]",
                Method = "POST"
            };
            HelperStruct.ResponceInfo info = StandartizeInfo(request);

            AssertHelper.AssertIsTrueAndAccumulate(info.source == "Иванов сергей петровеч", 
                                                   $"Проверка соответствия source = {request.Name}" +
                                                   $" и ответа апи {info.source}");
            AssertHelper.AssertIsTrueAndAccumulate(info.result == "Иванов Сергей Петрович",
                                                   $"Проверка соответствия " +
                                                   $"ответа апи result {info.result}");
            AssertHelper.AssertIsTrueAndAccumulate(info.result_genitive == "Иванова Сергея Петровича",
                                                   $"Проверка соответствия " +
                                                   $"ответа апи result_genitive {info.result_genitive}");
            AssertHelper.AssertIsTrueAndAccumulate(info.result_dative == "Иванову Сергею Петровичу",
                                                   $"Проверка соответствия " +
                                                   $"ответа апи result_dative {info.result_dative}");
            AssertHelper.AssertIsTrueAndAccumulate(info.result_ablative == "Ивановым Сергеем Петровичем",
                                                   $"Проверка соответствия " +
                                                   $"ответа апи result_ablative {info.result_ablative}");
            AssertHelper.AssertIsTrueAndAccumulate(info.surname == "Иванов",
                                                   $"Проверка соответствия " +
                                                   $"ответа апи surname {info.surname}");
            AssertHelper.AssertIsTrueAndAccumulate(info.name == "Сергей",
                                                   $"Проверка соответствия " +
                                                   $"ответа апи name {info.name}");
            AssertHelper.AssertIsTrueAndAccumulate(info.patronymic == "Петрович",
                                                   $"Проверка соответствия " +
                                                   $"ответа апи patronymic {info.patronymic}");
        }

        [TestMethod]
        public void InvalidKeys()
        {
            HelperStruct.RequestInfo request = new HelperStruct.RequestInfo()
            {
                ContentType = "application/json",
                AuthorizationToken = "Token invalid",
                XSecret = "invalid",
                Name = "[\"Иванов сергей петровеч\"]",
                Method = "POST"
            };
            string error = SendErrorReq(request);
            AssertHelper.AssertIsTrueAndAccumulate(error == "401", 
                                                   $"Код ошибки {error} несанкционированный");
        }
        [TestMethod]
        public void InvalidRequest()
        {
            HelperStruct.RequestInfo request = new HelperStruct.RequestInfo()
            {
                ContentType = "application/json",
                AuthorizationToken = "Token invalid",
                XSecret = "invalid",
                Name = "[\"Иванов сергей петровеч\"]",
                Method = "gfdg"
            };
            string error = SendErrorReq(request);
            AssertHelper.AssertIsTrueAndAccumulate(error == "400",
                                                   $"Код ошибки {error} несанкционированный");
        }
        [TestMethod]
        public void InvalidMethod()
        {
            HelperStruct.RequestInfo request = new HelperStruct.RequestInfo()
            {
                ContentType = "application/json",
                AuthorizationToken = "Token 21e39bb408ecf9647ed89b86398684941455fd30",
                XSecret = "fc5e7048820531bdec6d761d5ecb12a6463d2a37",
                Name = "[\"Иванов сергей петровеч\"]",
                Method = "PUT"
            };
            string error = SendErrorReq(request);
            AssertHelper.AssertIsTrueAndAccumulate(error == "405",
                                                   $"Код ошибки {error} несанкционированный");
        }
    }
}
