using Newtonsoft.Json;
using System.Collections.Generic;

namespace eathappy.order.domain.Dtos.DynamicsPlatform
{
    public class Cr4b7Flinkcode
    {
        public string cr4b7_code { get; set; }
        public string cr4b7_flinkcodeid { get; set; }
    }

    public class StoreData
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }
        public string cr4b7_kostenstelle { get; set; }
        public string cr4b7_standortposid { get; set; }
        public Cr4b7Flinkcode cr4b7_flinkcode { get; set; }
    }

    public class StoreDetailsDto
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }
        [JsonProperty("value")]
        public List<StoreData> value { get; set; }
    }
}
