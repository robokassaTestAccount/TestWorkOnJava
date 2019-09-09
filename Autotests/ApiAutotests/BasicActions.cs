using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ApiAutotests
{
    public class BasicActions : SuiteBase
    {
        public string GetSeason(int numberOfMonth)
        {
            if (numberOfMonth < 1 || numberOfMonth > 12)
            {
                return "Некорректное значение";
            }
            SetSeassons();
            return Seasons[numberOfMonth];          
        }

        private Dictionary<int, string> Seasons { get; set; }
        private void SetSeassons()
        {
            Seasons = new Dictionary<int, string>(12)
            {
                { 1, "Зима" },
                { 2, "Зима" },
                { 3, "Весна" },
                { 4, "Весна" },
                { 5, "Весна" },
                { 6, "Лето" },
                { 7, "Лето" },
                { 8, "Лето" },
                { 9, "Осень" },
                { 10, "Осень" },
                { 11, "Осень" },
                { 12, "Зима" },
            };
        }

        public HelperStruct.ResponceInfo StandartizeInfo(HelperStruct.RequestInfo info)
        {
            StandartizationApi standartization = new StandartizationApi(info);
            return standartization.SendReq();
        }

        public string SendErrorReq(HelperStruct.RequestInfo info)
        {
            StandartizationApi standartization = new StandartizationApi(info);
            return standartization.SendErrorReq();
        }
    }
}
