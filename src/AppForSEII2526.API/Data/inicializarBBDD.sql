
INSERT INTO [dbo].[AspNetUsers]
(Id, Nombre, NombreUsuario, Apellido, Metodos_Pagos, Direccion, Pais, Conductor,
 UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
 PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
 TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES
('1', 'Lucas', 'lucas.mdn', 'Maldonado',
 '[]', 'C/ Costa Azul nº 14', 'España', 0,
 'lucas.mdn', 'LUCAS.MDN', 'lucas.mdn@gmail.com', 'LUCAS.MDN@GMAIL.COM',
 1, NULL, NULL, NULL, '678123456', 1, 0, NULL, 1, 0);


INSERT INTO [dbo].[AspNetUsers]
(Id, Nombre, NombreUsuario, Apellido, Metodos_Pagos, Direccion, Pais, Conductor,
 UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
 PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
 TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES
('2', 'Elena', 'elena.rtz', 'Ruiz',
 '[]', 'Av. Palmera nº 6', 'España', 1,
 'elena.rtz', 'ELENA.RTZ', 'elena.ruiz@hotmail.com', 'ELENA.RUIZ@HOTMAIL.COM',
 0, NULL, NULL, NULL, NULL, 0, 0, NULL, 1, 0);


INSERT INTO [dbo].[AspNetUsers]
(Id, Nombre, NombreUsuario, Apellido, Metodos_Pagos, Direccion, Pais, Conductor,
 UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
 PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
 TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES
('3', 'Sergio', 'sergio.gn', 'Giménez',
 '[]', 'C/ Prado Verde nº 21', 'España', 0,
 'sergio.gn', 'SERGIO.GN', 'sergio.gn@gmail.com', 'SERGIO.GN@GMAIL.COM',
 1, NULL, NULL, NULL, '612334567', 1, 0, NULL, 1, 0);


INSERT INTO [dbo].[AspNetUsers]
(Id, Nombre, NombreUsuario, Apellido, Metodos_Pagos, Direccion, Pais, Conductor,
 UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
 PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
 TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES
('4', 'Laura', 'laura.blc', 'Blanco',
 '[]', 'C/ Camino Alto nº 9', 'España', 1,
 'laura.blc', 'LAURA.BLC', 'laura.blc@outlook.com', 'LAURA.BLC@OUTLOOK.COM',
 0, NULL, NULL, NULL, NULL, 0, 0, NULL, 1, 0);


INSERT INTO [dbo].[AspNetUsers]
(Id, Nombre, NombreUsuario, Apellido, Metodos_Pagos, Direccion, Pais, Conductor,
 UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
 PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
 TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES
('5', 'Irene', 'irene.pns', 'Pons',
 '[]', 'Av. Las Flores nº 18', 'España', 0,
 'irene.pns', 'IRENE.PNS', 'irene.pns@gmail.com', 'IRENE.PNS@GMAIL.COM',
 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 1, 0);


INSERT INTO [dbo].[AspNetUsers]
(Id, Nombre, NombreUsuario, Apellido, Metodos_Pagos, Direccion, Pais, Conductor,
 UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
 PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
 TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES
('6', 'Marc', 'marc.fc', 'Ferrer',
 '[]', 'C/ Camino del Río nº 3', 'España', 1,
 'marc.fc', 'MARC.FC', 'marc.fc@gmail.com', 'MARC.FC@GMAIL.COM',
 0, NULL, NULL, NULL, '699112233', 1, 0, NULL, 1, 0);



SET IDENTITY_INSERT [dbo].[Modelos] ON;

INSERT INTO [dbo].[Modelos] ([Id], [Name]) VALUES (1, N'Mercedes Clase C');
INSERT INTO [dbo].[Modelos] ([Id], [Name]) VALUES (2, N'Volkswagen Golf');
INSERT INTO [dbo].[Modelos] ([Id], [Name]) VALUES (3, N'Ford Focus');
INSERT INTO [dbo].[Modelos] ([Id], [Name]) VALUES (4, N'Kia Sportage');
INSERT INTO [dbo].[Modelos] ([Id], [Name]) VALUES (5, N'Peugeot 308');
INSERT INTO [dbo].[Modelos] ([Id], [Name]) VALUES (6, N'Honda Civic');

SET IDENTITY_INSERT [dbo].[Modelos] OFF;


SET IDENTITY_INSERT [dbo].[Coches] ON;

INSERT INTO [dbo].[Coches]
([Id], [ClaseCoche], [Color], [Descripcion], [DesplazamientoMotor], [TipoCombustible],
 [TipoDeMantenimiento], [Fabricante], [PrecioCompra], [CantidadCompra], [CantidadAlquiler],
 [PrecioAlquiler], [TamanoLlanta], [ModeloId])
VALUES
(1, N'Berlina', N'Negro', N'Sedán premium de tamaño medio', N'180 CV', N'Diesel',
 0, N'Mercedes', 25000, 4, 2, 120, N'55.0', 1);

INSERT INTO [dbo].[Coches]
([Id], [ClaseCoche], [Color], [Descripcion], [DesplazamientoMotor], [TipoCombustible],
 [TipoDeMantenimiento], [Fabricante], [PrecioCompra], [CantidadCompra], [CantidadAlquiler],
 [PrecioAlquiler], [TamanoLlanta], [ModeloId])
VALUES
(2, N'Compacto', N'Azul', N'Compacto alemán muy vendido', N'115 CV', N'Gasolina',
 2, N'Volkswagen', 15000, 6, 3, 80, N'45.0', 2);

INSERT INTO [dbo].[Coches]
([Id], [ClaseCoche], [Color], [Descripcion], [DesplazamientoMotor], [TipoCombustible],
 [TipoDeMantenimiento], [Fabricante], [PrecioCompra], [CantidadCompra], [CantidadAlquiler],
 [PrecioAlquiler], [TamanoLlanta], [ModeloId])
VALUES
(3, N'Compacto', N'Rojo', N'Coche urbano económico', N'100 CV', N'Gasolina',
 0, N'Ford', 12000, 10, 5, 60, N'42.0', 3);

INSERT INTO [dbo].[Coches]
([Id], [ClaseCoche], [Color], [Descripcion], [DesplazamientoMotor], [TipoCombustible],
 [TipoDeMantenimiento], [Fabricante], [PrecioCompra], [CantidadCompra], [CantidadAlquiler],
 [PrecioAlquiler], [TamanoLlanta], [ModeloId])
VALUES
(4, N'SUV', N'Blanco', N'SUV compacto moderno', N'140 CV', N'Diesel',
 3, N'Kia', 23000, 3, 2, 110, N'55.5', 4);

INSERT INTO [dbo].[Coches]
([Id], [ClaseCoche], [Color], [Descripcion], [DesplazamientoMotor], [TipoCombustible],
 [TipoDeMantenimiento], [Fabricante], [PrecioCompra], [CantidadCompra], [CantidadAlquiler],
 [PrecioAlquiler], [TamanoLlanta], [ModeloId])
VALUES
(5, N'Compacto', N'Gris', N'Compacto francés del segmento C', N'130 CV', N'Gasolina',
 4, N'Peugeot', 14000, 7, 4, 75, N'44.0', 5);

INSERT INTO [dbo].[Coches]
([Id], [ClaseCoche], [Color], [Descripcion], [DesplazamientoMotor], [TipoCombustible],
 [TipoDeMantenimiento], [Fabricante], [PrecioCompra], [CantidadCompra], [CantidadAlquiler],
 [PrecioAlquiler], [TamanoLlanta], [ModeloId])
VALUES
(6, N'Compacto', N'Verde', N'Coche compacto japonés', N'125 CV', N'Gasolina',
 5, N'Honda', 16000, 5, 3, 90, N'46.0', 6);

SET IDENTITY_INSERT [dbo].[Coches] OFF;

SET IDENTITY_INSERT [dbo].[Alquileres] ON; INSERT INTO [dbo].[Alquileres] ([Id], [Total], [FechaAlquiler], [InicioAlquiler], [FinAlquiler], [ConcesionarioEntrega],  [MetodoPago], [ApplicationUserId]) VALUES (1, 240, '2025-01-05 10:30:00', '2025-01-05 10:30:00', '2025-01-07 18:00:00', 'C/ Toledo Nº 12', 0, N'1'); 
INSERT INTO [dbo].[Alquileres] ([Id], [Total], [FechaAlquiler], [InicioAlquiler], [FinAlquiler], [ConcesionarioEntrega], [MetodoPago], [ApplicationUserId]) VALUES (2, 160, '2025-02-12 14:15:00', '2025-02-12 14:30:00', '2025-02-14 19:00:00', 'Av. España Nº 44', 1, N'2');
INSERT INTO [dbo].[Alquileres] ([Id], [Total], [FechaAlquiler], [InicioAlquiler], [FinAlquiler], [ConcesionarioEntrega],  [MetodoPago], [ApplicationUserId]) VALUES (3, 60, '2025-03-01 09:00:00', '2025-03-01 09:00:00', '2025-03-02 20:00:00', 'C/ Ciudad Real Nº 27',  2, N'3'); 
INSERT INTO [dbo].[Alquileres] ([Id], [Total], [FechaAlquiler], [InicioAlquiler], [FinAlquiler], [ConcesionarioEntrega],  [MetodoPago], [ApplicationUserId]) VALUES (4, 330, '2025-04-10 11:25:00', '2025-04-10 11:30:00', '2025-04-13 18:45:00', 'C/ Alemania Nº 8',  0, N'4'); 
INSERT INTO [dbo].[Alquileres] ([Id], [Total], [FechaAlquiler], [InicioAlquiler], [FinAlquiler], [ConcesionarioEntrega],  [MetodoPago], [ApplicationUserId]) VALUES (5, 150, '2025-05-21 16:00:00', '2025-05-21 16:15:00', '2025-05-23 17:00:00', 'Av. Libertad Nº 6',  1, N'5'); 
INSERT INTO [dbo].[Alquileres] ([Id], [Total], [FechaAlquiler], [InicioAlquiler], [FinAlquiler], [ConcesionarioEntrega],  [MetodoPago], [ApplicationUserId]) VALUES (6, 180, '2025-06-02 08:45:00', '2025-06-02 09:00:00', '2025-06-04 20:30:00', 'C/ Alicante Nº 3',  2, N'6'); 
SET IDENTITY_INSERT [dbo].[Alquileres] OFF;
INSERT INTO [dbo].[AlquilerItem] (CocheId, AlquilerId, Cantidad) VALUES (1, 1, 1);
INSERT INTO [dbo].[AlquilerItem] (CocheId, AlquilerId, Cantidad) VALUES (2, 2, 1);
INSERT INTO [dbo].[AlquilerItem] (CocheId, AlquilerId, Cantidad) VALUES (3, 3, 1);
INSERT INTO [dbo].[AlquilerItem] (CocheId, AlquilerId, Cantidad) VALUES (4, 4, 1);
INSERT INTO [dbo].[AlquilerItem] (CocheId, AlquilerId, Cantidad) VALUES (5, 5, 1);
INSERT INTO [dbo].[AlquilerItem] (CocheId, AlquilerId, Cantidad) VALUES (6, 6, 1);


--BORRAR LOS DATOS DE LAS TABLAS

/*
DELETE FROM [dbo].[AlquilerItem];
DELETE FROM [dbo].[Alquileres];
DELETE FROM [dbo].[Coches];


DELETE FROM [dbo].[Modelos];


DELETE FROM [dbo].[AspNetUsers];


//prueba para el examen
{
  "total": 0,
  "nombreUsuario": "sergio.gn",
  "inicioAlquiler": "2025-11-20",
  "finAlquiler": "2025-11-22",
  "fechaAlquiler": "2025-11-19",
  "concesionarioEntrega": "C/ Prado Verde nº 21",
  "nombre": "Sergio",
  "apellido": "Giménez",
  "alquilerItems": [
    {
      "cantidad": 1,
      "modelo": "Honda Civic",
      "fabricante": "Honda",
      "precioAlquiler": 0
    }
  ],
  "metodoPago": "Visa"
}


DELETE FROM [dbo].[AlquilerItem]
WHERE AlquilerId = 11;
DELETE FROM [dbo].[Alquileres]
WHERE Id = 11;


Si queremos eliminar la última
migración:
Update-Database –Migration 0
 Remove-Migration
 Si quieres borrar más migraciones
ejecuta remove-migration las veces
que lo necesites.

Si quieres borrar tu base de datos:
 drop-database

Podemos ejecutar la siguiente
instrucción para crear una base de
datos vacía:
 Update-Database -Migration 0

Add-Migration CreateIdentitySchema
Update-Database



*/


