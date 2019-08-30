using System.Collections.Generic;

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
    }
}
