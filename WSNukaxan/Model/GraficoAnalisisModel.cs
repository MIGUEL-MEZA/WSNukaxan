using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace WSNukaxan.Model
{
    

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GraficoAnalisisModel
    {
        public string Crono { get; set; }
        public string CodProduto { get; set; }
        public string NomProducto { get; set; }
        public string NomAnalisis { get; set; }
        public string CodAnalisis { get; set; }
        public string Fecha { get; set; }
        public string Real { get; set; }
        public string Esperado { get; set; }
        public string Minimo { get; set; }
        public string Maximo { get; set; }
        public string MinProcess { get; set; }
        public string MaxProcess { get; set; }
        public string CodCliente { get; set; }
        public string ChoixValeur { get; set; }
        public string CodNutriment { get; set; }
        public string NomNutriment { get; set; }
    }

}
