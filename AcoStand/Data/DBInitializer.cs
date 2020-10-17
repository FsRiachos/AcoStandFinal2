using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AcoStand.Data;
using AcoStand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcoStand.Data
{
    public class DbInitializer
    {

        /// <summary>
        /// método que cria os dados iniciais do sistema
        // / recebe a password das contas iniciais criadas pelo sistema
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="password"></param>
        /// <returns></returns>        

        public static async Task Initialize(IServiceProvider serviceProvider, string password)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

                //mail dos users a criar pelo sistema
                var users = new List<string>
                {
                    "a@a.a",
                    "b@b.b",
                    "c@c.c"
                    
                };

                //  Cria um role do tipo Admin   
                var roleExiste = await roleManager.RoleExistsAsync("Admin");
                // Caso este role ainda não exista,  cria-o
                if (!roleExiste)
                {
                    var role = new IdentityRole
                    {
                        Name = "Admin"
                    };
                    await roleManager.CreateAsync(role);
                }

                // Cria um role do tipo User
                roleExiste = await roleManager.RoleExistsAsync("User");
                // Caso este role ainda não exista,  cria-o
                if (!roleExiste)
                {
                    var role = new IdentityRole
                    {
                        Name = "User"
                    };
                    await roleManager.CreateAsync(role);
                }

                // Cria um utilizador do sistema e atribui-lhe um role
                string user1 = await CreateUser(serviceProvider, password, users[0], "Admin");
                string user2 = await CreateUser(serviceProvider, password, users[1], "User");
                string user3 = await CreateUser(serviceProvider, password, users[2], "User");


                // Coloca numa lista o GUID criado pelo sistema aquando a criação de um utilizador
                var userID = new List<string>
                {
                    user1,
                    user2,
                    user3
                };
                SeedDB(context, userID);
            }
        }

        /// <summary>
        /// Cria um utiilizador no sistema e atribui-lhe um role
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="password"></param>
        /// <param name="UserName"></param>
        /// <param name="role"></param>
        /// <returns>Id de um utilizador(GUID)</returns>
        private static async Task<string> CreateUser(IServiceProvider serviceProvider, string password, string UserName, string role)
        {
            // Gestor de utilizadores
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            // Verifica se um utilizador já existe com o username inserido
            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                // Caso  não exista é criado um novo
                user = new IdentityUser { UserName = UserName, Email = UserName };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, role);
            }
            return user.Id;
        }

        /// <summary>
        /// Seed da nossa Base de Dados
        /// </summary>
        /// <param name="context"></param>
        /// <param name="usersID"></param>
        public static void SeedDB(ApplicationDbContext context, List<string> usersID)
        {
            context.Database.EnsureCreated();


            // Verifica se existe algum utilizador na Base de Dados, caso exista, não "seeda" a Base de Dados
            if (context.Utilizadores.Any())
            {
                return;   // DB has been seeded
            }

            var utilizadores = new Utilizadores[]
            {
                new Utilizadores {Nome="Frederico", Username= "Fsr", Localidade="Golegã", Sexo="Masculino", DataNasc=new DateTime(1998, 6, 2),  UserFK=usersID[0]},
                new Utilizadores {Nome="Tiago Rodrigues", Username= "Tiago", Localidade= "Entroncamento", Sexo="Masculino", DataNasc=new DateTime(1998, 6, 2), UserFK=usersID[1]},
                new Utilizadores {Nome="Isso", Username= "Isso", Localidade= "Entroncamento", Sexo="Masculino", DataNasc=new DateTime(1998, 6, 2), UserFK=usersID[2]}
            };
            foreach (Utilizadores s in utilizadores)
            {
                context.Utilizadores.Add(s);
            }
            context.SaveChanges();

            var categorias = new Categorias[]
             {
                new Categorias {Designacao = "Carros" },
                new Categorias {Designacao = "Peças" }
             };


            foreach (Categorias f in categorias)
            {
                context.Categorias.Add(f);
            }
            context.SaveChanges();

            var artigos = new Artigos[]
             {
                new Artigos {Titulo = "Audi A4", Preco = "25,000", Descricao = "Carro como novo, inspeção em dia", Contacto = "a@aa.aa", Validado = true, DonoFK = 1, CategoriaFK = categorias[0].IdCategoria },
                new Artigos {Titulo = "BMW X5", Preco = "30,000", Descricao = "Carro como novo, inspeção em dia", Contacto = "a@aa.aa", Validado = true, DonoFK = 2, CategoriaFK = categorias[0].IdCategoria },
                new Artigos {Titulo = "Opel XXX", Preco = "10,000", Descricao = "Carro como novo, inspeção em dia", Contacto = "a@aa.aa", Validado = true, DonoFK = 1, CategoriaFK = categorias[0].IdCategoria },
                new Artigos {Titulo = "Opel YY", Preco = "13,000", Descricao = "Carro como novo, inspeção em dia", Contacto = "a@aa.aa", Validado = true, DonoFK = 2, CategoriaFK = categorias[0].IdCategoria },
                new Artigos {Titulo = "Ford", Preco = "8,000", Descricao = "Carro como novo, inspeção em dia", Contacto = "a@aa.aa", Validado = true, DonoFK = 1, CategoriaFK = categorias[0].IdCategoria},
                new Artigos {Titulo = "Nissan x", Preco = "27,000", Descricao = "Carro como novo, inspeção em dia", Contacto = "a@aa.aa", Validado = true, DonoFK = 1, CategoriaFK = categorias[0].IdCategoria}
             };
            foreach (Artigos p in artigos)
            {
                context.Artigos.Add(p);
            }
            context.SaveChanges();


            var recursos = new Recursos[]
            {
                new Recursos {Designacao = "Audi A4", Tipo = "imagem", ArtigoFK = artigos[0].IdArtigo },
                new Recursos {Designacao = "BMW X5", Tipo = "imagem", ArtigoFK = artigos[1].IdArtigo  },
                new Recursos {Designacao = "Opel XXX", Tipo = "imagem", ArtigoFK = artigos[2].IdArtigo  },
                new Recursos {Designacao = "Opel YY", Tipo = "imagem", ArtigoFK = artigos[3].IdArtigo  },
                new Recursos { Designacao = "Ford", Tipo = "imagem", ArtigoFK = artigos[4].IdArtigo  },
                new Recursos {Designacao = "Nissan x", Tipo = "imagem", ArtigoFK = artigos[5].IdArtigo  }
            };
            foreach (Recursos e in recursos)
            {
                context.Recursos.Add(e);
            }
            context.SaveChanges();
        }
    }
}