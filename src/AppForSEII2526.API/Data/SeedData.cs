using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger)
        {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try
            {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try
            {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }
            try
            {
                SeedModelosCoches(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Cars and Models in the Database.");
            }


            try
            {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "elena@uclm.es");
                SeedAlquiler(dbContext, user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding a Rental in the Database.");
            }


        }




        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    };

                    await roleManager.CreateAsync(role);
                }
            }
        }


        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es", "Avda. España 2, Albacete");
                user.UserName = "elena@uclm.es";
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
            }

            if (userManager.FindByNameAsync("gregorio@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("2", "Gregorio", "Diaz Descalzo", "gregorio@uclm.es", "Avda. España 25, Ciudad Real");
                user.UserName = "gregorio@uclm.es";
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "APassword1234%").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
            }

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es", "Avda. España 75, London");
                user.UserName = "peter@uclm.es";
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, roles[2]).Wait();
            }
        }


        public static void SeedModelosCoches(ApplicationDbContext dbcontext)
        {
            dbcontext.Database.EnsureCreated();



            string[] modelos =
            {
            "Volkswagen Golf",
            "Seat León",
            "Peugeot 308",
             "BMW Serie 3",
            "Mercedes Clase C",
            "Skoda Octavia"
                };


            foreach (var nombre in modelos)
            {
                if (!dbcontext.Modelos.Any(m => m.Name == nombre))
                {
                    dbcontext.Modelos.Add(new Modelo(nombre));
                }
            }

            dbcontext.SaveChanges();




            if (!dbcontext.Coches.Any(c => c.Modelo.Name == "Volkswagen Golf"))
            {
                var modelo = dbcontext.Modelos.First(m => m.Name == "Volkswagen Golf");

                var coche = new Coche(
                    claseCoche: "Compacto",
                    color: "Azul Marino",
                    descripcion: "Compacto europeo del segmento C",
                    desplazamientoMotor: "130 CV",
                    tipoCombustible: "Gasolina",
                    fabricante: "Volkswagen",
                    precioCompra: 16500m,
                    cantidadCompra: 4,
                    cantidadAlquiler: 2,
                    precioAlquiler: 55,
                    tamanoLlanta: "16 pulgadas",
                    modelo: modelo,
                    tiposdeMantenimiento: Coche.TipoMantenimiento.Frenos
                );

                dbcontext.Coches.Add(coche);
            }



            if (!dbcontext.Coches.Any(c => c.Modelo.Name == "Seat León"))
            {
                var modelo = dbcontext.Modelos.First(m => m.Name == "Seat León");

                var coche = new Coche(
                    claseCoche: "Compacto",
                    color: "Rojo",
                    descripcion: "Compacto español eficiente y moderno",
                    desplazamientoMotor: "115 CV",
                    tipoCombustible: "Gasolina",
                    fabricante: "SEAT",
                    precioCompra: 15000m,
                    cantidadCompra: 6,
                    cantidadAlquiler: 3,
                    precioAlquiler: 50,
                    tamanoLlanta: "16 pulgadas",
                    modelo: modelo,
                    tiposdeMantenimiento: Coche.TipoMantenimiento.Aceite
                );

                dbcontext.Coches.Add(coche);
            }

            dbcontext.SaveChanges();
        }

        public static void SeedAlquiler(ApplicationDbContext dbcontext, ApplicationUser user)
        {
            // Si no existe un alquiler con Id = 1, crearlo
            if (!dbcontext.Alquileres.Any(a => a.Id == 1))
            {
                // Creamos lista vacía que luego llenamos
                var items = new List<AlquilerItem>();

                // Creamos el alquiler
                var alquiler = new Alquiler(
                    nombre: "Juan",
                    apellido: "Pérez",
                    concesionarioEntrega: "C/Rosario 11",
                    fechaAlquiler: DateTime.Today,
                    metodoPago: MetodoPagoTipos.GooglePay,
                    inicioAlquiler: DateTime.Today.AddDays(1),
                    finAlquiler: DateTime.Today.AddDays(5),
                    alquilerItems: items,
                    applicationUser: user
                );

                // Obtener un coche existente para el alquiler 
                var coche = dbcontext.Coches.FirstOrDefault();
                if (coche != null)
                {
                    var alquilerItem = new AlquilerItem(coche, alquiler, cantidad: 1);
                    alquiler.AlquilerItems.Add(alquilerItem);
                }

                // Guardar
                dbcontext.Alquileres.Add(alquiler);
                dbcontext.SaveChanges();
            }

        }
    }
}


    
