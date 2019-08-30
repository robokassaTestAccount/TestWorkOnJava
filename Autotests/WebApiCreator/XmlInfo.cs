using LoggerHelperSpace;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using ShopsConfig;
using WebApiCreator.CabinetApi;
using System.Linq;
using static Core.HelperStructs;

namespace WebApiCreator
{
    public class XmlInfo
    {
        LoggerHelper Logger { get; set; }

        public int ExceptionCount { get; set; }
        public Guid AutoId { get; set; }

        public List<PartnerInfo> Partners { get; set; }
        public XmlInfo(string ShopXmlPath, LoggerHelper logger)
        {
            this.ShopXmlPath = ShopXmlPath;
            Logger = logger;
            ExceptionCount = 0;
        }

        public LoginData User { get; set; }
        private string ShopXmlPath { get; set; }
        private Root Batch { get; set; }
        public Connector GetConnector()
        {
            try
            {
                Logger.WriteInfo("Открытие файла сериализации");
                XmlSerializer serializer = new XmlSerializer(typeof(Root));

                using (XmlReader reader = XmlReader.Create(ShopXmlPath))
                {
                    Batch = (Root)serializer.Deserialize(reader);
                }                
            }
            catch (Exception e)
            {
                ExceptionCount += 1;
                Logger.WriteError("Ошибка при открытии файла сериализации", e);
                return null;
            }


            AutoId = new Guid(Batch.AutoId);
            return new Connector(Batch, Logger);
        }

        public void FillPartnerInfo()
        {
            ChangeUniqueData();
            Partners = new List<PartnerInfo>();
            foreach (LegalPartner partner in Batch.Partners)
            {
                SignerInfo signer = new SignerInfo
                {
                    FullName = $"Иванов Иван Иваныч",
                    FullNameGenitif = "Иванова Ивана Иваныча",
                    DocumentNameGenitif = "Устава",
                    Position = "Генсек цк кпсс"
                };
                User = new LoginData
                {
                    Id = partner.Identifier,
                    Login = partner.Login,
                    Pass = partner.Password
                };
                PartnerInfo partnerInfo = new PartnerInfo
                {
                    AutoId = AutoId,
                    BIK = partner.Info.BIK,
                    Account = partner.Info.Account,
                    Email = partner.Info.EMail,
                    INN = partner.Info.INN,
                    KPP = partner.Info.KPP,
                    OGRN = partner.Info.OGRN,
                    Phone = partner.Info.Phone,
                    PartnerIdentifier = partner.Identifier,
                    OrganzitaionType = JurType.Legal,
                    Login = partner.Login,
                    Title = partner.Name,
                    OutAccountType = OutAccountType.RIB,
                    PartnerRole = PartnerRole.MerchantBN,
                    SignerInfo = signer,
                    EgrRecordDate = new DateTime(2018, 1, 10),
                    IsResident = true,
                    NeedSendMail = true,
                    MerchantIP = partner.MerchantTypeIP
                };
                partnerInfo.Shops = new List<Shop>();
                partnerInfo.ShopIds = new List<Guid?>();

                foreach (ShopsShop shop in partner.Shops)
                {
                    AddressData address = new AddressData
                    {
                        City = "Москва",
                        Code = "123456",
                        Country = "Россия",
                        Flat = Helpers.GetRandomNumber(2),
                        House = Helpers.GetRandomNumber(2),
                        Index = Helpers.GetRandomNumber(2),
                        Korpus = Helpers.GetRandomNumber(2),
                        Office = Helpers.GetRandomNumber(2),
                        Region = Helpers.GetRandomString(),
                        Street = Helpers.GetRandomString(),
                        Stroenie = Helpers.GetRandomNumber(2),
                        Subject = Helpers.GetRandomString()
                    };
                    partnerInfo.AddressJur = address;

                    Robomarket robomarket = new Robomarket();
                    if (shop.Robomarket != null)
                    {
                        robomarket.ApproveUrl = shop.Robomarket.ApproveUrl;
                        robomarket.CompletedUrl = shop.Robomarket.CompletedUrl;
                        robomarket.Enabled = shop.Robomarket.Enabled;
                    }

                    Shop tempShop = new Shop
                    {
                        CatalogItem = 100,
                        FailUrl = shop.FailUrl,
                        FailUrlMethod = ParceMethod(shop.FailMethod),
                        HashMethod = HashAlgorithm.MD5,
                        Identifier = shop.Identifier,
                        Name = shop.Name,
                        Owner = ParceOwner(shop.Owner),
                        Password1 = shop.Password1,
                        Password2 = shop.Password2,
                        ResultMethod = ParceMethod(shop.ResultMethod),
                        ResultUrl = shop.ResultUrl,
                        SiteUrl = shop.SiteUrl,
                        SuccessUrl = shop.SuccessUrl,
                        SuccessUrlMethod = ParceMethod(shop.SuccessMethod),
                        TestPassword1 = Helpers.GetRandomString(),
                        TestPassword2 = Helpers.GetRandomString(),
                        Currencies = new List<string>(),
                        Rbm = robomarket
                    };

                    if (shop.Currencies != null)
                    {
                        foreach (string curr in shop.Currencies)
                        {
                            tempShop.Currencies.Add(curr);
                        }
                    }
                    partnerInfo.Shops.Add(tempShop);
                }
                Partners.Add(partnerInfo);
            }
        }

        private void ChangeUniqueData()
        {
            for(int i=0;i<Batch.Partners.Count(); i++)
            {
                Batch.Partners.ElementAt(i).Identifier = Helpers.GetRandomString();
                Batch.Partners.ElementAt(i).Name = Helpers.GetRandomString();
                for (int j=0;j<Batch.Partners.ElementAt(i).Shops.Count();j++)
                {
                    Batch.Partners.ElementAt(i).Shops.ElementAt(j).Name = Helpers.GetRandomString()+$"number{j}";
                    Batch.Partners.ElementAt(i).Shops.ElementAt(j).Identifier = Helpers.GetRandomString();
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            using (XmlWriter writer = XmlWriter.Create(ShopXmlPath))
            {
                serializer.Serialize(writer, Batch);
            }
        }

        private ResultMethod ParceMethod(string method)
        {
            try
            {
                method = method.ToLower().Trim();

                switch (method)
                {
                    case "get":
                        {
                            return ResultMethod.Get;
                        }
                    case "post":
                        {
                            return ResultMethod.Post;
                        }
                    case "email":
                        {
                            return ResultMethod.Email;
                        }
                    default:
                        {
                            throw new Exception("Не удалось распознать метод GET\\POST\\EMAIL");
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionCount += 1;
                Logger.WriteError("Ошибка в распознавании метода", ex);
                return ResultMethod.Get;
            }
        }

        private ShopOwner ParceOwner(string owner)
        {
            try
            {
                owner = owner.ToLower().Trim();
                switch (owner)
                {
                    case "ocean":
                        {
                            return ShopOwner.Ocean;
                        }
                    case "paysend":
                        {
                            return ShopOwner.PaySend;
                        }
                    case "platron":
                        {
                            return ShopOwner.Platron;
                        }
                    case "qiwi":
                        {
                            return ShopOwner.Qiwi;
                        }
                    case "rib":
                        {
                            return ShopOwner.RIB;
                        }
                    case "robox":
                        {
                            return ShopOwner.Robox;
                        }
                    default:
                        {
                            throw new Exception("Не удалось распознать owner-a");
                        }
                }
            }
            catch (Exception ex)
            {
                ExceptionCount += 1;
                Logger.WriteError("Ошибка в распознавании owner-a", ex);
                return ShopOwner.Unknown;
            }
        }

    }
}
