using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcoStand.Models {
    public class Favoritos {
        [Key]
        public int IdFavoritos { get; set; }

        /// <summary>
        /// Chave Forasteira que indica a os artigos que estao nos favoritos
        /// </summary>
        [ForeignKey(nameof(Artigo))]
        public int IdArtigo { get; set; }
        public virtual Artigos Artigo { get; set; }

        /// <summary>
        /// Chave Forasteira que indica a que utilizador pertence os fvoritos
        /// </summary>
        [ForeignKey(nameof(Utilizador))]
        public int IdUtlizador { get; set; }
        public virtual Utilizadores Utilizador { get; set; }
        }
    }
