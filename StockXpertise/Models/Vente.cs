﻿namespace StockXpertise.Models
{
    public class Vente : BaseModel<Vente>
    {
        public override string TableName => "vente";

        public int id_vente { get; set; }
        public int id_produit { get; set; }
        public int prix_ventes { get; set; }
        public DateTime date_vente { get; set; }
         public string facture { get; set; }
         public int id_mouvement { get; set; }
    }

}
