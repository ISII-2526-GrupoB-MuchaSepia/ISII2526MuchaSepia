using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger)
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


        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {

            foreach (string roleName in roles)
            {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es", "Avda. España 2, Albacete");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("gregorio@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("2", "Gregorio", "Diaz Descalzo", "gregorio@uclm.es", "Avda. España 25, Ciudad Real");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "APassword1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null)
            {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es", "Avda. España 75, London");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();

                }
            }

        }
<<<<<<< Updated upstream
=======

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




>>>>>>> Stashed changes
    }
}


    
