using Core;
using LoggerHelperSpace;
using ShopsConfig;
using System;
using System.Diagnostics;
using System.Linq;
using WebApiCreator.CabinetApi;

namespace WebApiCreator
{
    public class Connector
    {
        LoggerHelper Logger { get; set; }
        public Connector(Root root, LoggerHelper logger)
        {            
            Database = root.Database;
            Login = root.Login;
            Password = root.Password;
            IntegratedSecurity = root.IntegratedSecurity;
            Server = root.Server;
            
            Logger = logger;

            SetUpApi();
        }
        private string Database { get; set; }
        private string Login { get; set; }
        private string Password { get; set; }
        private string Server { get; set; }
        private bool IntegratedSecurity { get; set; }
        PartnerCabinetApiClient Api { get; set; }

        private void SetUpApi()
        {
            try
            {
                //Logger.WriteInfo("Установка настроек апи сервиса");
                //var uri = new Uri(ApiUrl);
                //var address = new EndpointAddress(uri);
                Api = new PartnerCabinetApiClient(/*CreateHttpBinding(), address*/);
                Logger.WriteInfo("Установка настроек апи сервиса - done");
            }
            catch (Exception ex)
            {
                Logger.WriteError("Ошибка при конфигурации Апи сервиса ", ex);
            }
        }

        public RoboxEntities SetUpDataBase()
        {
            Logger.WriteInfo("Установка настроек соединения к БД");
            DbFabric fabric = new DbFabric
            {
                Initial = Database,
                Ip = Server,
                Password = Password,
                User = Login
            };
            RoboxEntities context = fabric.GetRoboxContext();
            Logger.WriteInfo("Установка настроек соединения к БД - done");
            return context;
        }
        public bool CheckDbConnection()
        {
            try
            {
                RoboxEntities context = SetUpDataBase();
                return context.Database.Exists();
            }
            catch (Exception ex)
            {
                Logger.WriteError("не удалось установить соединение с БД", ex);
                return false;
            }
        }
        public PartnerInfo CreatePartner(PartnerInfo partner)
        {
            CreatePartnerRequest request = new CreatePartnerRequest
            {
                PartnerIdentifier = partner.PartnerIdentifier,
                Login = partner.Login,
                PartnerRole = partner.PartnerRole,
                IsResident = partner.IsResident,
                Description = partner.Title,
                RecoveryQuestion = null,
                RecoveryAnswer = null,
                Email = partner.Email,
                FullName = null,
                Comment = null,
                IsDisabledManual = null,
                IsPermittedUsePromoCode = false,
                PromoCode = null,
                NeedSendMail = true
            };

            CheckPartnerIdentifierExistsRequest existsRequest = new CheckPartnerIdentifierExistsRequest
            {
                PartnerIdentifier = partner.PartnerIdentifier
            };

            CheckPartnerIdentifierExistsResponse existsResponse = Api.CheckPartnerIdentifierExists(existsRequest);
            if (existsResponse.Exists)
            {
                Logger.WriteError($"Пользователь с данным идентификатором {partner.PartnerIdentifier} уже существует");
                throw new Exception($"Пользователь с данным идентификатором {partner.PartnerIdentifier} уже существует");
            }

            CreatePartnerResponse response = Api.CreatePartner(request);

            if (response.Result.ResultCode == PartnerApiServiceErrorCode.Ok)
            {
                partner.PartnerId = response.Id;
                partner.WorkerId = response.WorkerId;
            }
            if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
            {
                Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                Environment.Exit(11);
            }
            return partner;
        }

        public PartnerInfo CreateOutAccount(PartnerInfo partner)
        {
            CreateOutAccountRequest request;
            if (partner.MerchantIP)
            {
                request = new CreateOutAccountRequest()
                {
                    OutAccountType = OutAccountType.RIB,
                    Account = partner.Phone,
                    DisplayName = "RIB",
                    PartnerId = (Guid)partner.PartnerId,
                    WorkerId = (Guid)partner.WorkerId                    
                };
            }
            else
            {
                request = new CreateOutAccountRequest()
                {
                    OutAccountType = partner.OutAccountType,
                    Account = partner.Account,
                    BIK = partner.BIK,
                    DisplayName = "Bank",
                    PartnerId = (Guid)partner.PartnerId,
                    WorkerId = (Guid)partner.WorkerId,
                    CountryCode = "RU"                    
                };
            }
            
            CreateOutAccountResponse response = Api.CreateOutAccount(request);
            if (response.Result.ResultCode == PartnerApiServiceErrorCode.Ok)
            {
                partner.OutAccountId = response.OutAccountId;
            }
            if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
            {
                Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                Environment.Exit(11);
            }
            return partner;
        }

        public PartnerInfo PartnerContactSave(PartnerInfo partner)
        {
            PartnerContactSaveRequest request = new PartnerContactSaveRequest()
            {
                ContactId = null,
                WorkerId = partner.WorkerId,
                UserId = partner.AutoId, // Пользователь "робомаркет-авто"
                PartnerId = partner.PartnerId,
                IsActive = true,
                FullName = partner.SignerInfo.FullName,
                Position = null,
                Email1 = partner.Email,
                Email2 = partner.Email,
                Phone1 = partner.Phone,
                Phone2 = partner.Phone,
                ICQ = null,
                Skype = null,
                Address = null,
                Comment = null,

                FullNameGenitif = partner.SignerInfo.FullNameGenitif,
                ConfirmDocGenitif = "",
                ConfirmDocProperties = "",
            };

            PartnerContactSaveResponse response = Api.PartnerContactSave(request);
            partner.ContactId = response.ContactId;
            if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
            {
                Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                Environment.Exit(11);
            }
            return partner;
        }

        public PartnerInfo PartnerContactSaveSigner(PartnerInfo partner)
        {
            PartnerContactSaveRequest request = new PartnerContactSaveRequest()
            {
                ContactId = null,
                WorkerId = null,
                UserId = partner.WorkerId,
                PartnerId = partner.PartnerId,
                IsActive = true,
                FullName = partner.SignerInfo.FullName,
                Position = partner.SignerInfo.Position,
                Email1 = partner.Email,
                Email2 = partner.Email,
                Phone1 = partner.Phone,
                Phone2 = partner.Phone,
                ICQ = null,
                Skype = null,
                Address = null,
                Comment = null,

                FullNameGenitif = partner.SignerInfo.FullNameGenitif,
                ConfirmDocGenitif = partner.SignerInfo.DocumentNameGenitif,
                ConfirmDocProperties = null,
            };

            PartnerContactSaveResponse response = Api.PartnerContactSave(request);
            if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
            {
                Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                Environment.Exit(11);
            }
            return partner;
        }

        public PartnerInfo LegalProfileGroupSave(PartnerInfo partner)
        {
            if(partner.MerchantIP)
            {
                return partner;
            }
            LegalProfileGroupSaveRequest request = new LegalProfileGroupSaveRequest()
            {
                PartnerId = partner.PartnerId.Value,
                WorkerId = partner.WorkerId.Value,

                AddressJur = partner.AddressJur,
                AddressFact = partner.AddressJur,
                Contacts = new Contacts
                {
                    Email = partner.Email,
                    Phone = partner.Phone,
                    Fax = null
                },
                EgrRecordDate = partner.EgrRecordDate.Value,
                INN = partner.INN,
                OGRN = partner.OGRN,
                OrganizationType = partner.OrganzitaionType,

                KPP = partner.KPP,
                SignerInfo = partner.SignerInfo,
                CountryCode = "RU",
                IndividualInfo = new IndividualInfo()
                {
                    Birth = DateTime.Now,
                    FullName = Helpers.GetRandomString(),
                    OgrnCertificate = new OgrnCertificate
                    {
                        IssueDate = DateTime.Now,
                        Number = Helpers.GetRandomNumber(),
                        Series = Helpers.GetRandomNumber()
                    },
                    BirthPlace = Helpers.GetRandomString(),
                    Passport = new Passport
                    {
                        Series = Helpers.GetRandomNumber(4),
                        Number = Helpers.GetRandomNumber(6),
                        IssueDate = DateTime.Now,
                        IssuedBy = Helpers.GetRandomString(),
                        SubdivisionCode = Helpers.GetRandomNumber(3) + "-" + Helpers.GetRandomNumber(3)
                    },
                    RegistrationAddress = Helpers.GetRandomString(),
                }
                
            };

            LegalProfileGroupSaveResponse groupSaveResponse = Api.LegalProfileGroupSave(request);
            if (groupSaveResponse.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
            {
                Logger.WriteError($"resultCode {groupSaveResponse.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");

                Environment.Exit(11);
            }

            return partner;
        }

        public PartnerInfo CreateAllShops(PartnerInfo partner)
        {
            for (int i = 0; i < partner.Shops.Count; i++)
            {
                CreateShopRequest request;
                if(partner.MerchantIP)
                {
                    request = new CreateShopRequest()
                    {
                        PartnerId = partner.PartnerId.Value,
                        Name = partner.Shops.ElementAt(i).Name,
                        Url = partner.Shops.ElementAt(i).SiteUrl,
                        PartnerRole = partner.PartnerRole,
                        CurrentAccountId = partner.OutAccountId.Value,
                        NotificationContactId = partner.ContactId.Value,
                        SupportContactId = partner.ContactId.Value,
                        CatalogSections = null,
                        GoodsDescription = "",
                        Culture = "RU",
                        Identifier = partner.Shops.ElementAt(i).Identifier,
                        ShopOwner = ShopOwner.PaySend,
                        AdminLogin = "Robomarket",
                        JurType = "none",
                    };
                }
                else
                {
                    request = new CreateShopRequest()
                    {
                        PartnerId = partner.PartnerId.Value,
                        Name = partner.Shops.ElementAt(i).Name,
                        Url = partner.Shops.ElementAt(i).SiteUrl,
                        PartnerRole = partner.PartnerRole,
                        CurrentAccountId = partner.OutAccountId.Value,
                        NotificationContactId = partner.ContactId.Value,
                        SupportContactId = partner.ContactId.Value,
                        CatalogSections = null,
                        GoodsDescription = "",
                        Culture = "RU",
                        Identifier = partner.Shops.ElementAt(i).Identifier,
                        ShopOwner = partner.Shops.ElementAt(i).Owner,
                        AdminLogin = "Robomarket",
                        JurType = "ЮЛ",
                    };
                }                

                CreateShopResponse response = Api.CreateShop(request);

                if (response.Result.ResultCode == PartnerApiServiceErrorCode.Ok)
                {
                    partner.MerchantId = response.MrhId;
                    partner.ShopIds.Add(response.ShopId);
                }
                if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
                {
                    Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                    Environment.Exit(11);
                }

            }

            return partner;
        }

        public PartnerInfo ProfileGroupSave(PartnerInfo partner)
        {
            for (int i = 0; i < partner.Shops.Count; i++)
            {
                CommonProfileGroupSaveRequest request = new CommonProfileGroupSaveRequest()
                {
                    AddressData = partner.AddressJur,
                    CatalogItem = 103,
                    ExternalAccountId = partner.OutAccountId.Value,
                    GoodsDescription = null,
                    WorkerId = partner.WorkerId.Value,
                    ShopId = partner.ShopIds.ElementAt(i).Value,
                    ShopUrl = partner.Shops.ElementAt(i).SiteUrl,
                    NotificationContactId = partner.ContactId.Value,
                    SupportContactId = partner.ContactId.Value,
                };

                CommonProfileGroupSaveResponse profileGroupSaveResponse = Api.CommonProfileGroupSave(request);
                if (profileGroupSaveResponse.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
                {
                    Logger.WriteError($"resultCode {profileGroupSaveResponse.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                    Environment.Exit(11);
                }
            }

            return partner;
        }

        public PartnerInfo SettingProfileGroupSave(PartnerInfo partner)
        {
            for (int i = 0; i < partner.Shops.Count; i++)
            {
                SettingsProfileGroupSaveRequest request = new SettingsProfileGroupSaveRequest()
                {
                    ShopId = partner.ShopIds.ElementAt(i).Value,
                    ShopUrl = partner.Shops.ElementAt(i).SiteUrl,
                    TechSettings = new TechSettings
                    {
                        HashAlgorithm = partner.Shops.ElementAt(i).HashMethod,
                        Password1 = partner.Shops.ElementAt(i).Password1,
                        Password2 = partner.Shops.ElementAt(i).Password2,
                        ResultUrl = partner.Shops.ElementAt(i).ResultUrl,
                        ResultMethod = partner.Shops.ElementAt(i).ResultMethod,
                        SuccessUrl = partner.Shops.ElementAt(i).SuccessUrl,
                        SuccessUrlMethod = partner.Shops.ElementAt(i).SuccessUrlMethod,
                        FailUrl = partner.Shops.ElementAt(i).FailUrl,
                        FailUrlMethod = partner.Shops.ElementAt(i).FailUrlMethod,
                        TestHashAlgorithm = partner.Shops.ElementAt(i).HashMethod,
                        TestPassword1 = partner.Shops.ElementAt(i).TestPassword1,
                        TestPassword2 = partner.Shops.ElementAt(i).TestPassword2,
                    },
                    UserId = partner.AutoId.Value, //RobomarketAuto
                };
                SettingsProfileGroupSaveResponse response = Api.SettingsProfileGroupSave(request);
                if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
                {
                    Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                    Environment.Exit(11);
                }

            }

            return partner;
        }

        public PartnerInfo ClientSettingsProfileGroupSave(PartnerInfo partner)
        {
            ClientSettingsProfileGroupSaveRequest request = new ClientSettingsProfileGroupSaveRequest()
            {
                PartnerId = partner.PartnerId.Value,
                WorkerId = partner.WorkerId.Value,
                Value = "atolAgent",
                Display = "Робочеки",
            };
            ClientSettingsProfileGroupSaveResponse response = Api.ClientSettingsProfileGroupSave(request);
            if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
            {
                Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                Environment.Exit(11);
            }
            return partner;
        }

        public PartnerInfo PrepareActivation(PartnerInfo partner, string RoleLabel)
        {
            for (int i = 0; i < partner.Shops.Count; i++)
            {
                PrepareActivationRequestRequest request = new PrepareActivationRequestRequest()
                {
                    PartnerId = partner.PartnerId.Value,
                    LangLetters = "ru",
                    RoleLabel = RoleLabel,
                    ShopName = partner.Title,
                    MerchantId = partner.MerchantId.Value,
                    ShopId = partner.ShopIds.ElementAt(i),
                };
                PrepareActivationRequestResponse response = Api.PrepareActivationRequest(request);
                if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
                {
                    Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                    Environment.Exit(11);
                }
            }

            return partner;
        }

        public PartnerInfo CreateActivation(PartnerInfo partner)
        {
            for (int i = 0; i < partner.Shops.Count; i++)
            {
                CreateActivationRequestRequest request = new CreateActivationRequestRequest()
                {
                    PartnerId = partner.PartnerId.Value,
                    MrhId = partner.MerchantId,
                    Role = partner.PartnerRole,
                    Subject = "Заявка на активацию магазина",
                    Body = "Это автоматически сгенерированная заявка",
                    UserId = partner.WorkerId,

                    ShopId = partner.ShopIds.ElementAt(i) ?? new Guid(),
                    ShopTitle = partner.Title
                };
                CreateActivationRequestResponse response = Api.CreateActivationRequest(request);
                if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
                {
                    Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                    Environment.Exit(11);
                }
            }

            return partner;
        }

        public PartnerInfo StartActivationRobomarket(PartnerInfo partner)
        {
            for (int i = 0; i < partner.Shops.Count; i++)
            {
                StartActivationRobomarketServiceRequest request = new StartActivationRobomarketServiceRequest()
                {
                    AdminLogin = "Robomarket",
                    PartnerId = partner.PartnerId.Value,
                    ShopId = partner.ShopIds.ElementAt(i),
                };

                StartActivationRobomarketServiceResponse response = Api.StartActivationRobomarketService(request);
                if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
                {
                    Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                    Environment.Exit(11);
                }
            }

            return partner;
        }

        public PartnerInfo RobomarketProfileGroupSave(PartnerInfo partner)
        {
            for (int i = 0; i < partner.Shops.Count; i++)
            {
                RobomarketProfileGroupSaveRequest request = new RobomarketProfileGroupSaveRequest()
                {
                    ShopId = partner.ShopIds.ElementAt(i).Value,
                    RobomarketShopId = partner.RobomarketShopId.ToString(),
                    UserId = partner.AutoId.Value,
                };

                RobomarketProfileGroupSaveResponse response = Api.RobomarketProfileGroupSave(request);
                if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
                {
                    Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                    Environment.Exit(11);
                }
            }

            return partner;
        }

        public PartnerInfo AddPartnerToRole(PartnerInfo partner)
        {
            AddPartnerToRoleRequest request = new AddPartnerToRoleRequest()
            {
                PartnerId = partner.PartnerId.Value,
                Role = PartnerRoleToAdd.Robomarket,
            };
            AddPartnerToRoleResponse response = Api.AddPartnerToRole(request);
            if (response.Result.ResultCode != PartnerApiServiceErrorCode.Ok)
            {
                Logger.WriteError($"resultCode {response.Result.ResultCode} + method {new StackTrace(false).GetFrame(0).GetMethod().Name}");
                Environment.Exit(11);
            }
            return partner;
        }
    }
}
