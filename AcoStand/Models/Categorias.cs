using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AcoStand.Models {
    public class Categorias {
        public Categorias() {

            //Instanciar o Icollection de Artigos
            ListaArtigos = new HashSet<Artigos>();
            }
        /// <summary>
        /// Chave Primaria da Categoria
        /// </summary>
        [Key]
        public int IdCategoria { get; set; }


        [Display(Name = "Categoria")]
        [Required]
        public string Designacao { get; set; }
       


    
    public virtual ICollection<Artigos> ListaArtigos { get; set; }
   
        } 
    }
