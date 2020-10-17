using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcoStand.Models {
    /// <summary>
    /// Classe que representa um utilizador do sistema
    /// </summary>
    public class Utilizadores {


        public Utilizadores() {
            //Instanciar o Icollection de Artigos
            ListaArtigos = new HashSet<Artigos>();
            //Instanciar o Icollection de Favoritos
            ListaFavoritos = new HashSet<Favoritos>();
            }
        /// <summary>
        /// Chave primaria do utilizador
        /// </summary>
        [Key]
        public int IdUtilizador { get; set; }

        [Display(Name = "Nome do Utilizador")]
        [Required(ErrorMessage = "Preenchimento obrigatório.")]
        [StringLength(75, ErrorMessage = "O {0} deverá conter {1} caracteres no máximo.")]
        [RegularExpression("[A-ZÁÉÍÓÚ][a-záéíóúàèÌòùãõîôûâç]+(( | e | de | do | das | da | dos |-|')[A-ZÁÉÍÓÚ][a-zzáéíóúàèÌòùãõîôûâç]+)*",
             ErrorMessage = "P.f verifique o {0} introduzido. Este deve ser composto por primeiro e ultimo nome. Cada palavra deve começar com Maiúscula. Não são aceites caracteres numéricos.")]
        public string Nome { get; set; }

        /// <summary>
        /// Username do utilizador
        /// </summary>
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// Localidade do Utilizador 
        /// </summary>
        [Required(ErrorMessage = "Preenchimento obrigatório.")]
        [StringLength(30, ErrorMessage = "A {0} deverá conter {1} caracteres no máximo.")]
        [RegularExpression("[A-ZÁÉÍÓÚ][a-záéíóúàèÌòùãõîôûâç]+(( | e | de | do | das | da | dos |-|')[A-ZÁÉÍÓÚ][a-zzáéíóúàèÌòùãõîôûâç]+)*",
            ErrorMessage = "A {0} só pode conter letras. Cada palavra deve começar com Maiúscula.")]
        public string Localidade { get; set; }

        /// <summary>
        /// Sexo do Utilizador
        /// </summary>
        [Required]
        [StringLength(9)]
        [RegularExpression("Masculino|Feminino", ErrorMessage = "Introduzir: Masculino ou Feminino")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }


        /// <summary>
        /// Data Nascimento
        /// </summary>
        [Required(ErrorMessage = "Preenchimento obrigatório.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNasc { get; set; }

        /// <summary>
        /// Lista de artigos de um utilizador
        /// </summary>
        public virtual ICollection<Artigos> ListaArtigos { get; set; }

        /// <summary>
        /// Lista de Favoritos de um Utilizador
        /// </summary>
        public virtual ICollection<Favoritos> ListaFavoritos { get; set; }

        // Chave estrangeira que fará a ligação das tabelas entre os utilizadores do Identity e um User
        [Required]
        public string UserFK { get; set; }


    }
    }
