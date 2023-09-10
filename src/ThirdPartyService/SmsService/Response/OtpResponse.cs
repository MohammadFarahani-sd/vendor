using Newtonsoft.Json;

namespace SmsService.Response
{
    public class Entry
    {
        [JsonProperty("messageid")]
        public int MessageId { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("Status")]
        public int Status { get; set; }
        
        [JsonProperty("statustext")]
        public string StatusText { get; set; }
        
        [JsonProperty("sender")]
        public string Sender { get; set; }
        
        [JsonProperty("Receptor")]
        public string Receptor { get; set; }
        
        [JsonProperty("Date")]
        public int Date { get; set; }
        
        [JsonProperty("cost")]
        public int Cost { get; set; }
    }

    public class ReturnData
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class OtpResponse
    {
        [JsonProperty("return")]
        public ReturnData ReturnData { get; set; }
        
        [JsonProperty("entries")]
        public List<Entry> Entries { get; set; }
    }
}
