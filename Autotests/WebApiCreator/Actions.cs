using LoggerHelperSpace;
using ShopsConfig;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace WebApiCreator
{
    public static class Actions
    {
        public static Connector Connector { get; set; }
        public static XmlInfo Info { get; set; }
        public static LoggerHelper Logger { get; set; }
        public static PartnerInfo CreateByApi(PartnerInfo partner)
        {
            PartnerInfo tempPartner = partner;
            tempPartner = Connector.CreatePartner(tempPartner);
            tempPartner = Connector.CreateOutAccount(tempPartner);
            tempPartner = Connector.PartnerContactSave(tempPartner);
            tempPartner = Connector.PartnerContactSave(tempPartner);
            tempPartner = Connector.PartnerContactSaveSigner(tempPartner);
            tempPartner = Connector.LegalProfileGroupSave(tempPartner);
            tempPartner = Connector.CreateAllShops(tempPartner);
            tempPartner = Connector.ProfileGroupSave(tempPartner);
            tempPartner = Connector.SettingProfileGroupSave(tempPartner);
            tempPartner = Connector.ClientSettingsProfileGroupSave(tempPartner);
            tempPartner = Connector.PrepareActivation(tempPartner, "MerchantBN");
            tempPartner = Connector.CreateActivation(tempPartner);
            //tempPartner = Connector.StartActivationRobomarket(tempPartner);
            //tempPartner = Connector.RobomarketProfileGroupSave(tempPartner);
            //tempPartner = Connector.AddPartnerToRole(tempPartner);

            return tempPartner;
        }

        public static int CheckCurrencies()
        {
            try
            {
                Core.RoboxEntities context = Connector.SetUpDataBase();
                foreach (PartnerInfo partner in Info.Partners)
                {
                    foreach (Shop shop in partner.Shops)
                    {
                        if (shop.Currencies != null)
                        {
                            foreach (string curr in shop.Currencies)
                            {
                                Core.C_paysys_curr variable = context.C_paysys_curr.Where(x => x.curr == curr).FirstOrDefault();
                                if (variable == null)
                                {
                                    Logger.WriteError($"Не найдена валюта {curr}");
                                    return 7;
                                }
                            }
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logger.WriteError("Ошибка проверки существования валют", ex);
                return 6;
            }
        }

        public static int ChangePassword(List<PartnerInfo> partners)
        {
            try
            {
                Core.RoboxEntities context = Connector.SetUpDataBase();
                foreach (PartnerInfo partner in partners)
                {
                    context.C_pc_PartnerWorkerLogin.Where(x => x.partnerId == partner.PartnerId).FirstOrDefault().password = "Qwerty123456";
                    context.SaveChanges();
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logger.WriteError("Ошибка при обращении к БД ", ex);
                return 5;
            }
        }

        public static int UpdateDocs(List<PartnerInfo> partners)
        {
            try
            {
                Core.RoboxEntities context = Connector.SetUpDataBase();
                Guid adminId = context.C_Auth3.FirstOrDefault().SID;
                foreach (PartnerInfo partner in partners)
                {
                    List<Core.C_pc_PartnerDocument> partnerIds = context.C_pc_PartnerDocument.Where(x => x.partnerId == partner.PartnerId).ToList();
                    foreach (Core.C_pc_PartnerDocument doc in partnerIds)
                    {
                        context.f_pa_PartnerDocument_Update(doc.id, DateTimeOffset.Now, 20, null);
                    }
                    foreach (Guid? shopId in partner.ShopIds)
                    {
                        context.f_pa_ShopProfile_Save(shopId, 28830, DateTimeOffset.Now.ToString("o"), DateTimeOffset.Now.ToString("o"), null, adminId, null, null);
                    }
                    context.f_pc_ProfileValue_Save(null, partner.PartnerId, 1078, "1", "1", null, "autoadmin", DateTimeOffset.Now, null);
                }
                context.SaveChanges();
                return 0;
            }
            catch (Exception ex)
            {
                Logger.WriteError("Ошибка при обращении к БД ", ex);
                return 5;
            }
        }

        public static int UpdateShopsReqs(List<PartnerInfo> partners)
        {
            try
            {
                Core.RoboxEntities context = Connector.SetUpDataBase();
                Guid adminId = context.C_Auth3.FirstOrDefault().SID;
                foreach (PartnerInfo partner in partners)
                {
                    for (int i = 0; i < partner.Shops.Count; i++)
                    {
                        Guid reqId = context.C_pc_PartnerRequest.Where(x => x.partnerId == partner.PartnerId).FirstOrDefault().id;
                        context.f_pc_Shop_Update(partner.PartnerId, partner.ShopIds.ElementAt(i), "Active", null);
                        context.f_pa_PartnerRequest_Update(reqId, 92, adminId, "", false);
                    }
                }
                context.SaveChanges();
                return 0;
            }
            catch (Exception ex)
            {
                Logger.WriteError("Ошибка при обращении к БД ", ex);
                return 5;
            }
        }
        public static int UpdateCurrencies(List<PartnerInfo> partners)
        {
            try
            {
                Core.RoboxEntities context = Connector.SetUpDataBase();
                foreach (PartnerInfo partner in partners)
                {
                    for (int i = 0; i < partner.Shops.Count; i++)
                    {
                        foreach (string curr in partner.Shops.ElementAt(i).Currencies)
                        {
                            string ident = partner.Shops.ElementAt(i).Identifier;
                            Guid shopSid = context.C_pc_PartnerShop.Where(x => x.identifier == ident).FirstOrDefault().SID;
                            Guid sidCur = context.C_paysys_curr.Where(x => x.curr == curr).FirstOrDefault().SID;
                            Guid namespaces = new Guid("6C56D2FF-C3C3-452E-B278-4E8AB302D36A");
                            Guid right = new Guid("8363bf78-da69-47a9-ba25-3c97d653064d");
                            context.f_s_SetPermission(namespaces, sidCur, shopSid, right, true, "autoadmin");
                        }
                    }
                }
                context.SaveChanges();
                return 0;
            }
            catch (Exception ex)
            {
                Logger.WriteError("Ошибка при обращении к БД ", ex);
                return 5;
            }
        }

        public static void SetTechnicalShop(PartnerInfo partner, string path)
        {
            string ShopXmlPath = path;
            Root Batch;
            XmlSerializer serializer = new XmlSerializer(typeof(Root));

            using (XmlReader reader = XmlReader.Create(ShopXmlPath))
            {
                Batch = (Root)serializer.Deserialize(reader);
            }

            for (int i = 0; i < partner.Shops.Count; i++)
            {
                if (partner.Shops.ElementAt(i).Rbm.Enabled)
                {
                    Guid communityId = Guid.NewGuid();
                    SqlConnection cnn;
                    string connectionString = $"data source={Batch.Server};initial catalog={Batch.Database};user id={Batch.Login};password={Batch.Password}";
                    cnn = new SqlConnection(connectionString);
                    cnn.Open();
                    using (SqlCommand command = new SqlCommand($"insert into _pc_ShopCommunity(Id, Description, MasterShopId) values(\'{communityId}\', \'{partner.Shops.ElementAt(i).Identifier}\', \'{partner.ShopIds.ElementAt(i).Value}\');", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand($"insert into _pc_ShopCommunitySettings(CommunityID, TypeID, Value) values(\'{communityId}\', 1, CONVERT(bit, 0));", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand($"insert into _pc_ShopCommunitySettings(CommunityID, TypeID, Value) values(\'{communityId}\', 2, CONVERT(bit, 0));", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand($"insert into _pc_ShopCommunitySettings(CommunityID, TypeID, Value) values(\'{communityId}\', 3, CONVERT(uniqueidentifier, \'{partner.ShopIds.ElementAt(i).Value}\'));", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand($"insert into _pc_ShopCommunitySettings(CommunityID, TypeID, Value) values(\'{communityId}\', 4, CONVERT(bit, 1));", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand($"insert into _pc_ShopCommunitySettings(CommunityID, TypeID, Value) values(\'{communityId}\', 5, CONVERT(varchar(200), \'{partner.Shops.ElementAt(i).Rbm.ApproveUrl}\'));", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand($"insert into _pc_ShopCommunitySettings(CommunityID, TypeID, Value) values(\'{communityId}\', 6, CONVERT(varchar(200), \'{partner.Shops.ElementAt(i).Rbm.CompletedUrl}\'));", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }

                    Core.RoboxEntities context = Connector.SetUpDataBase();
                    Guid? shopId = partner.ShopIds.ElementAt(i);
                    int mrhId = context.C_pc_PartnerShop.Where(x => x.id == shopId).FirstOrDefault().mrhId;

                    using (SqlCommand command = new SqlCommand($"update _mrh set curr = \'SplitR\' where id = {mrhId};", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }

                    string obligayionId = context.C_mrh.Where(x => x.id == mrhId).FirstOrDefault().acc_dst;
                    using (SqlCommand command = new SqlCommand($"update _pc_PartnerAccount set currLabel = \'SplitR\' where obligationId = {obligayionId};", cnn))
                    {
                        int ret = command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
