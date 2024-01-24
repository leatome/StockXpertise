﻿using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise.Models
{
    public class Fournisseur : BaseModel<Fournisseur>
    {
        protected override string TableName => "fournisseur";

        public int id_fournisseur { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public int numero { get; set; }
        public string mail { get; set; }
        public string adresse { get; set; }
    }

}
