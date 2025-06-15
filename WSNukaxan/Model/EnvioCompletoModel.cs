using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WSNukaxan.Model
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]

    public class EnvioCompletoModel
    {
        public string LienJournal { get; set; }
        public string ValidationState { get; set; }
        public string LibelleMethode { get; set; }
        public string ChoixValeur { get; set; }
        public string ValeurMesure { get; set; }
        public string CodeUnite { get; set; }
        public string ValeurTheorique { get; set; }
        public string SeuilMini { get; set; }
        public string SeuilMaxi { get; set; }
        public string AnalyseEffectue { get; set; }
        public string DateRealisation { get; set; }
        public string LibelleLabo { get; set; }
        public string Methode { get; set; }
        public string RefMethode { get; set; }
        public string ValComment { get; set; }
        public string Nuevo { get; set; }
        public string NumChrono { get; set; }
        public string Commentaire { get; set; }
        public string RemisPar { get; set; }
        public string ReferenceExterne { get; set; }
        public string LienJournalEchant { get; set; }
        public string LibelleOrigine { get; set; }

        public string DateEchantillon { get; set; }
        public string DatePrelevement { get; set; }
        public string LibelleFournisseurClient { get; set; }
        public string CodeProduit { get; set; }
        public string LibelleProduit { get; set; }
        public string LibelleProprietaire { get; set; }

        public string IsSend { get; set; }
        public string Destinatarios { get; set; }

        public string LibelleClientFacture { get; set; }
        public string Direccion1 { get; set; }
        public string Direccion2 { get; set; }
        public string Direccion3 { get; set; }

        public string CodeClienteFacture { get; set; }
        public string CodeMethode { get; set; }
        public string Lote { get; set; }
        public string LiePlan { get; set; }
        public string TipoProducto { get; set; }

        public string FechaCaducidad { get; set; }
        public string DateElaboration { get; set; }
        public string Nutriment { get; set; }
        public string CodeFamilia { get; set; }
        public string Especificacion { get; set; }
    }
}
