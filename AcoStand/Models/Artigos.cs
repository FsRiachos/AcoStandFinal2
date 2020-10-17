using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcoStand.Models {
    public class Artigos {
        public Artigos() {
            ListaRecursos = new HashSet<Recursos>();
            ListaFavUtilizador = new HashSet<Favoritos>();
            }

        /// <summary>
        /// Chave Primária do Artigo
        /// </summary>
        [Key] 
        public int IdArtigo { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "O preenchimento do {0} é obrigatório.")]
        [StringLength(75, ErrorMessage = "O {0} deverá ter, no máximo, {1} caracteres.")]
        public string Titulo { get; set; }

        [Display(Name = "Preço")]
        [Required(ErrorMessage = "O preenchimento do {0} é obrigatório.")]
        [RegularExpression("^([0-9]{0,5}((,)?[0-9]{0,3}))$", ErrorMessage = "Este campo apenas poderá conter números.")]
        public string Preco { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O preenchimento da {0} é obrigatório.")]
        [StringLength(500, ErrorMessage = "O {0} deverá ter, no máximo, {1} caracteres.")]
        public string Descricao { get; set; }

        [Display(Name = "Contacto")]
        [Required(ErrorMessage = "O preenchimento do {0} é obrigatório.")]
        public string Contacto { get; set; }

        /// <summary>
        /// Artigo será validado por um administrador da página
        /// </summary>
        public bool Validado { get; set; }

        /// <summary>
        /// Chave Forasteira que indica a que dono pertence este artigo
        /// </summary>
        [Display(Name = "Dono")]
        [ForeignKey(nameof(Dono))]
        public int DonoFK { get; set; }
        public virtual Utilizadores Dono { get; set; }

        /// <summary>
        /// Chave Forasteira que indica a que categoria pertence o artigo
        /// </summary>
        [Display(Name = "Categoria")]
        [ForeignKey(nameof(Categoria))]
        public int CategoriaFK { get; set; }
        public virtual Categorias Categoria { get; set; }

        /// <summary>
        /// Lista de recursos de um artigo
        /// </summary>
        public virtual ICollection<Recursos> ListaRecursos { get; set; }

        /// <summary>
        /// Lista de favoritos entre User e artigo (M-N)????
        /// </summary>
        public virtual ICollection<Favoritos> ListaFavUtilizador { get; set; }

        }
   }
