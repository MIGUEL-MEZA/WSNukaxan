using Newtonsoft.Json;

namespace WSNukaxan.Model
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ResumenProductoModel
    {
        public string TipoProducto { get; set; }
        public string CodProducto { get; set; }
        public string NomProducto { get; set; }
        public string Plan { get; set; }
        public string Especificacion { get; set; }
        public string Cantidad { get; set; }
    }
}
