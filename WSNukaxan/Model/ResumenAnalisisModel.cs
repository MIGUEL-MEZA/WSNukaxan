using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WSNukaxan.Model
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ResumenAnalisisModel
    {
        public string CodProducto { get; set; }
        public string NomProducto { get; set; }
        public string Analisis { get; set; }
        public string Tecnica { get; set; }
        public string Total { get; set; }
        public string Promedio { get; set; }
        public string Desviacion { get; set; }
        public string Covarianza { get; set; }
        public string Minimo { get; set; }
        public string Maximo { get; set; }
        public string Especificacion { get; set; }
        public string Cantidad { get; set; }
    }

}
