using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcoStand.Models {
    public class Recursos {
        /// <summary>
        /// Chave Primária dos Recursos
        /// </summary>
        [Key] 
        public int IdRecursos { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório.")]
        public string Designacao { get; set; }

        [Required]
        public string Tipo { get; set; }

        /// <summary>
        /// Chave Forasteira que indica a que artigo pertencem os recursos
        /// </summary>
        [ForeignKey("Artigo")]
        public int ArtigoFK { get; set; }
        public virtual Artigos Artigo { get; set; }
        }
    }
