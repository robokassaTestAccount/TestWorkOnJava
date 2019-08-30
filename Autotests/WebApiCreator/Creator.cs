using Core;
using LoggerHelperSpace;
using System.Collections.Generic;
using System.IO;
using static Core.HelperStructs;

namespace WebApiCreator
{
    public class Creator
    {
        public LoggerHelper Logger { get; set; }
        public string XmlPath { get; set; }
        public AssertHelper AssertHelper { get; set; }
        public List<PartnerInfo> PartnersChanged { get; set; }
        private XmlInfo Info { get; set; }
        private Connector Connector { get; set; }
        public LoginData User { get; set; }

        public Creator()
        {
            PartnersChanged = new List<PartnerInfo>();
        }

        public void CheckAccess()
        {
            CheckFileExists();
            Info = new XmlInfo(XmlPath, Logger);
            Connector = Info.GetConnector();
            CheckDbConnection();
            Info.FillPartnerInfo();
            User = Info.User;
            CheckExceptionCount();
            Actions.Connector = Connector;
            Actions.Info = Info;
            Actions.Logger = Logger;
            CheckCurrencies();
        }

        private void CheckFileExists()
        {
            if(!File.Exists(XmlPath))
            {
                AssertHelper.AssertIsFail("Отсутвует файл shops.xml");
            }
        }
        private void CheckDbConnection()
        {
            if (!Connector.CheckDbConnection())
            {
                AssertHelper.AssertIsFail("Соединение к базе недоступно");                
            }
        }
        private void CheckExceptionCount()
        {
            if (Info.ExceptionCount > 0)
            {
                AssertHelper.AssertIsFail("Количество ошибок больше 0");
            }
        }
        private void CheckCurrencies()
        {
            int checkResult = Actions.CheckCurrencies();
            if (checkResult != 0)
            {
                AssertHelper.AssertIsFail("количество валют не равно 0");
            }
        }

        public void CreateAllPartners()
        {
            foreach (PartnerInfo partner in Info.Partners)
            {
                Actions.Connector = Connector;
                PartnerInfo tempPartner = Actions.CreateByApi(partner);
                PartnersChanged.Add(tempPartner);
            }
            Actions.ChangePassword(PartnersChanged);
            Actions.UpdateDocs(PartnersChanged);
            Actions.UpdateShopsReqs(PartnersChanged);
            Actions.UpdateCurrencies(PartnersChanged);
        }
    }
}
