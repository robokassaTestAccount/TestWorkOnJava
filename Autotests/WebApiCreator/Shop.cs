using System.Collections.Generic;
using WebApiCreator.CabinetApi;

namespace WebApiCreator
{
    public class Shop
    {
        public Robomarket Rbm { get; set; }
        public string Identifier { get; set; }
        public string FailUrl { get; set; }
        public HashAlgorithm HashMethod { get; set; }
        public string Name { get; set; }
        public ShopOwner Owner { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string ResultUrl { get; set; }
        public string SiteUrl { get; set; }
        public string SuccessUrl { get; set; }
        public int CatalogItem { get; set; }
        public string TestPassword1 { get; set; }
        public string TestPassword2 { get; set; }
        public ResultMethod ResultMethod { get; set; }
        public ResultMethod SuccessUrlMethod { get; set; }
        public ResultMethod FailUrlMethod { get; set; }
        public List<string> Currencies { get; set; }
    }
}
