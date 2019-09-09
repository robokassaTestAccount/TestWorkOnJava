namespace ApiAutotests
{
    public static class HelperStruct
    {
        public struct RequestInfo
        {
            public string ContentType { get; set; }
            public string AuthorizationToken { get; set; }
            public string XSecret { get; set; }
            public string Name { get; set; }
            public string Method { get; set; }
        }

        public struct ResponceInfo
        {
            public string source { get; set; }
            public string result { get; set; }
            public string result_genitive { get; set; }
            public string result_dative { get; set; }
            public string result_ablative { get; set; }
            public string surname { get; set; }
            public string name { get; set; }
            public string patronymic { get; set; }
            public string gender { get; set; }
            public int qc { get; set; }
        }
    }
}
