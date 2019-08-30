using Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralMethods
{
    public class FiscalMethods
    {
        public AssertHelper AssertHelper { get; set; }
        public DbFabric DbFabric { get; set; }
        public FiscalMethods()
        {
        }
        public RequestToAtol DeserializeFiscalData(string fiscalData)
        {
            return JsonConvert.DeserializeObject<RequestToAtol>(fiscalData);
        }
        public ResponseFromAtol DeserializeAtolAnswer(string answer)
        {
            return JsonConvert.DeserializeObject<ResponseFromAtol>(answer);
        }
        public HelperStructs.ApiAnswerValue DeserilizaAnswer(string answer)
        {
            return JsonConvert.DeserializeObject<HelperStructs.ApiAnswerValue>(answer);
        }
        public string SerializeToJson(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public HelperStructs.PostRespLogin DeserializeLoginAnswer(string answer)
        {
            return JsonConvert.DeserializeObject<HelperStructs.PostRespLogin>(answer);
        }
        public HelperStructs.ErrorResponces DeserializeInvalidLoginAnswer(string answer)
        {
            return JsonConvert.DeserializeObject<HelperStructs.ErrorResponces>(answer);
        }

        public HelperStructs.PaymentPageRoboxContext DeserializePaymnetAnswer(string answer)
        {
            return JsonConvert.DeserializeObject<HelperStructs.PaymentPageRoboxContext>(answer);
        }
        public HelperStructs.PaymentResponce DeserializePaymentResponce(string answer)
        {
            return JsonConvert.DeserializeObject<HelperStructs.PaymentResponce>(answer);
        }
        public HelperStructs.OpState DeserializeOpStateResponce(string answer)
        {
            return JsonConvert.DeserializeObject<HelperStructs.OpState>(answer);
        }
        public RequestToAtol GetRequestToAtol(int opId)
        {
            RequestToAtol merchantFromFiscal;
            using (RoboxEntities context = DbFabric.GetRoboxContext())
            {
                bool exists = false;
                int waitTime = 0;
                while (!exists)
                {
                    exists = context.C_NotificationQueryLog.Where(x => x.Request.Contains(opId.ToString())).FirstOrDefault() != null;
                    Thread.Sleep(1000);
                    waitTime += 1000;
                    if (waitTime > 30000)
                    {
                        AssertHelper.AssertIsFail("Запрос не появился в базе за 30 секунд");
                        break;
                    }
                }
                C_NotificationQueryLog fiscalData = context.C_NotificationQueryLog.Where(
                                    x => !string.IsNullOrEmpty(x.Request)).ToList().LastOrDefault();

                merchantFromFiscal = DeserializeFiscalData(fiscalData.Request);
            }
            return merchantFromFiscal;
        }
        public int GetQueryIdToAtol(int opId)
        {
            using (RoboxEntities context = DbFabric.GetRoboxContext())
            {
                bool exists = false;
                int waitTime = 0;
                while (!exists)
                {
                    exists = context.C_NotificationQueryLog.Where(x => x.Request.Contains(opId.ToString())).FirstOrDefault() != null;
                    Thread.Sleep(1000);
                    waitTime += 1000;
                    if (waitTime > 30000)
                    {
                        AssertHelper.AssertIsFail("Запрос не появился в базе за 30 секунд");
                        break;
                    }
                }
                C_NotificationQueryLog fiscalData = context.C_NotificationQueryLog.Where(
                                    x => !string.IsNullOrEmpty(x.Request)).ToList().LastOrDefault();

                return fiscalData.QueryID;
            }
        }

        public ResponseFromAtol AssertOKResponse(int QuerryId)
        {
            using (RoboxEntities context = DbFabric.GetRoboxContext())
            {
                bool exists = false;
                int waitTime = 0;
                while (!exists)
                {
                    exists = context.C_NotificationQueryLog.Where(x => x.QueryID == QuerryId).FirstOrDefault() != null;
                    Thread.Sleep(1000);
                    waitTime += 1000;
                    if (waitTime > 60000)
                    {
                        AssertHelper.AssertIsFail("Запрос не появился в базе за 60 секунд");
                        break;
                    }
                }
                C_NotificationQueryLog fiscalData = context.C_NotificationQueryLog.Where(
                                                    x => x.QueryID == QuerryId).ToList().LastOrDefault();
                string response = fiscalData.Response;
                ResponseFromAtol fromAtol = DeserializeAtolAnswer(response);
                AssertHelper.AssertIsTrueAndAccumulate(string.IsNullOrEmpty(fromAtol.error), 
                                                       $"Проверка отсутвия ошибки в ответе атол" +
                                                       $" {fromAtol.error}");
                AssertHelper.AssertIsTrueAndAccumulate(fromAtol.status == "done",
                                                       $"Проверка статуса в ответе атол" +
                                                       $" {fromAtol.status}");
                return fromAtol;
            }
        }
        public DbResponseFromAtol GetResponseFromAtol(int opId)
        {
            using (RoboxEntities context = DbFabric.GetRoboxContext())
            {
                string fiscalData = context.C_MrhOperation_Receipt.Where(x => 
                                    x.OpID == opId).FirstOrDefault().FiscalData;
                return DeserializeDbAtolResp(fiscalData);
            }
        }

        public void GetBalance()
        {
            Guid shopGuid = GetShopId();
            using (AccountingEntities context = DbFabric.GetAccountingContext())
            {           
                var id = from acc in context.Account
                         join at in context.AccountTag on acc.id equals at.accountId
                         join tt in context.TagType on at.tagTypeId equals tt.id
                         where tt.label == "ShopGuid"
                         join atv in context.AccountTagValue on at.id equals atv.id
                         where atv.value == shopGuid.ToString()
                         join at2 in context.AccountTag on acc.id equals at2.accountId
                         join tt2 in context.TagType on at2.tagTypeId equals tt2.id
                         where tt2.label == "ContractSign"
                         join tv2 in context.TagValue on at2.valueId equals tv2.id
                         join own in context.Owners on tv2.value equals own.contractSign
                         where acc.name.Substring(0,3).EndsWith("3")
                         select new
                         {
                             accid = acc.id
                         };
                int some = id.FirstOrDefault().accid;
            }
        }

        private Guid GetShopId()
        {
            using (RoboxEntities context = DbFabric.GetRoboxContext())
            {
                return context.C_pc_PartnerShop.Where(x => x.identifier == "0e0l79d3n3").FirstOrDefault().id;
            }
        }

        public DbResponseFromAtol DeserializeDbAtolResp(string resp)
        {
            return JsonConvert.DeserializeObject<DbResponseFromAtol>(resp);
        }
        public int GetOpId(string opKey)
        {
            using (RoboxEntities context = DbFabric.GetRoboxContext())
            {
                Guid guid = Guid.Parse(opKey.Substring(0, 36));
                C_op result = context.C_op.Where(x => x.guid == guid).FirstOrDefault();
                return result.id;
            }
        }
    }
}
