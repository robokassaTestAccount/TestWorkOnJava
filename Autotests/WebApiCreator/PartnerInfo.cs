using System;
using System.Collections.Generic;
using WebApiCreator.CabinetApi;

namespace WebApiCreator
{
    public class PartnerInfo
    {
        public bool MerchantIP { get; set; }
        public string Title { get; set; } //partner.Name
        public string Phone { get; set; }
        public string Email { get; set; }
        public Guid? AutoId { get; set; }
        public string PartnerIdentifier { get; set; }
        public JurType OrganzitaionType { get; set; } //Legal - юрлицо, физик - любое другое
        public OutAccountType OutAccountType { get; set; } //у юриков RIB, у физиков QIWI
        public PartnerRole PartnerRole { get; set; } //MerchantBN - у юриков, MerchantOff - у физиков
        public AddressData AddressJur { get; set; }
        public SignerInfo SignerInfo { get; set; } //генерить
        public string Account { get; set; }
        public string BIK { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string OGRN { get; set; }
        public DateTime? EgrRecordDate { get; set; } //генерить
        public Guid? WorkerId { get; set; }
        public Guid? ContactId { get; set; }
        public Guid? OutAccountId { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? RobomarketShopId { get; set; }
        public int? MerchantId { get; set; }
        public bool IsResident { get; set; } //true
        public bool NeedSendMail { get; set; } //true
        public string Login { get; set; }
        public List<Shop> Shops { get; set; }
        public List<Guid?> ShopIds { get; set; }
    }
}
