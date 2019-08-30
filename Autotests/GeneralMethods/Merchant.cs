using System;

namespace GeneralMethods
{
    public class Merchant
    {
        public string merchantId { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public string operation { get; set; }
        public string sno { get; set; }
        public Item[] items { get; set; }
        public Payments[] payments { get; set; }
        public Vats[] vats { get; set; }
        public double total { get; set; }
        public Client client { get; set; }
    }
    public class MerchantCorrection : Merchant
    {
        public string merchantId { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public string operation { get; set; }
        public string sno { get; set; }
        public CorrectionInfo correction_info { get; set; }
        public Item[] items { get; set; }
        public Payments[] payments { get; set; }
        public Vats[] vats { get; set; }
        public double total { get; set; }
        public Client client { get; set; }
    }
    public struct CorrectionInfo
    {
        public string type { get; set; }
        public DateTime base_date { get; set; }
        public string base_number { get; set; }
        public string base_name { get; set; }
    }
    public struct Client
    {
        public string email { get; set; }
    }
    public struct Payments
    {
        public int type { get; set; }
        public double sum { get; set; }
    }

    public struct Vats
    {
        public string type { get; set; }
        public decimal sum { get; set; }
    }

    public struct Item
    {
        public string name { get; set; }
        public double quantity { get; set; }
        public decimal sum { get; set; }
        public string tax { get; set; }
        public string payment_method { get; set; }
        public string payment_object { get; set; }
    }

    public struct RequestToAtol
    {
        public string external_id { get; set; }
        public Receipt receipt { get; set; }
        public Service service { get; set; }
        public string timestamp { get; set; }
    }
    public struct Service
    {
        public string callback_url { get; set; }
    }

    public struct Receipt
    {
        public Vats[] vats { get; set; }
        public Client client { get; set; }
        public Company company { get; set; }
        public AtolItem[] items { get; set; }
        public Payments[] payments { get; set; }
        public double total { get; set; }
    }

    public struct Company
    {
        public string sno { get; set; }
        public string inn { get; set; }
        public string email { get; set; }
        public string payment_address { get; set; }
    }

    public struct AtolItem
    {
        public string name { get; set; }
        public double price { get; set; }
        public double quantity { get; set; }
        public double sum { get; set; }
        public string payment_method { get; set; }
        public string payment_object { get; set; }
        public Vats vat { get; set; }
    }

    public struct ResponseFromAtol
    {
        public string callback_url { get; set; }
        public string daemon_code { get; set; }
        public string device_code { get; set; }
        public string warnings { get; set; }
        public string error { get; set; }
        public string external_id { get; set; }
        public string group_code { get; set; }
        public Payload payload { get; set; }
        public string status { get; set; }
        public string uuid { get; set; }
        public string timestamp { get; set; }
    }

    public struct Payload
    {
        public string ecr_registration_number { get; set; }
        public string fiscal_document_attribute { get; set; }
        public string fiscal_document_number { get; set; }
        public string fiscal_receipt_number { get; set; }
        public string fn_number { get; set; }
        public string fns_site { get; set; }
        public string receipt_datetime { get; set; }
        public string shift_number { get; set; }
        public string total { get; set; }
    }

    public struct DbResponseFromAtol
    {
        public string FiscalType { get; set; }
        public double total { get; set; }
        public Client client { get; set; }
        public Payments[] payments { get; set; }
        public Vats[] vats { get; set; }
        public string url { get; set; }
        public string FnNumber { get; set; }
        public string FiscalDocumentNumber { get; set; }
        public string FiscalDocumentAttribute { get; set; }
        public string FiscalDate { get; set; }
    }

    public struct ErrorTypes
    {
        public bool InvalidMerchId { get; set; }
        public bool InvalidTaxs { get; set; }
        public bool InvalidTotal { get; set; }
        public bool SameID { get; set; }
        public bool NotFiscalUser { get; set; }
        public bool EmptyClient { get; set; }
    }
}