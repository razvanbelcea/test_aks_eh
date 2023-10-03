using Newtonsoft.Json;
using System.Collections.Generic;

namespace eathappy.order.domain.Dtos.DynamicsPlatform
{
    public class Cr4b7Produktionsshop
    {
        public string cr4b7_bezeichnung { get; set; }
        public string cr4b7_standortposid { get; set; }
    }

    public class Cr4b7Truhe
    {
        public string cr4b7_bezeichnung { get; set; }
        public string cr4b7_kostenstelle { get; set; }
        public string cr4b7_standortposid { get; set; }
    }

    public class StoreServedHubDto
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }
        [JsonProperty("value")]
        public List<HubData> value { get; set; }
    }

    public class HubData
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }
        public string cr4b7_mappingproduktionsshoptruheid { get; set; }
        public Cr4b7Produktionsshop cr4b7_Produktionsshop { get; set; }
        public Cr4b7Truhe cr4b7_Truhe { get; set; }
    }

}
