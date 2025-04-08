namespace WSNukaxan.Model
{

    public class EnvioCompletosModel
    {
        public string NumChrono { get; set; }
        public string Commentaire { get; set; }
        public string ReferenceExterne { get; set; }
        public string LibelleOrigine { get; set; }
        public string LibelleLivraison { get; set; }

        public string DateEchantillon { get; set; }
        public string DatePrelevement { get; set; }
        public string LibelleFournisseurClient { get; set; }
        public string CodeProduit { get; set; }
        public string LibelleProduit { get; set; }
        public string LibelleProprietaire { get; set; }

        public string LibelleClientFacture { get; set; }
        public string CodeClienteFacture { get; set; }
        public string Lote { get; set; }
        public string LiePlan { get; set; }
        public string TipoProducto { get; set; }

        public string FechaCaducidad { get; set; }
        public string DateElaboration { get; set; }

        public List<EnvioCompletoAnalisisModel> Analisis { get; set; } = new List<EnvioCompletoAnalisisModel>();
    }

}
