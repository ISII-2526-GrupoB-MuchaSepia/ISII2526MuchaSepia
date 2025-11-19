--DELETE FROM dbo.Modelos WHERE Id IN (1,2,3,4,5,6);
--DELETE FROM dbo.Reseñas WHERE Id IN (1,2,3,4,5,6);

-- -----------------
-- Modelos
SET IDENTITY_INSERT [dbo].[Modelos] ON;

INSERT INTO [dbo].[Modelos] ([Id], [Name]) VALUES 
(1, N'Mercedes Clase C'),
(2, N'Volkswagen Golf'),
(3, N'Ford Focus'),
(4, N'Kia Sportage'),
(5, N'Peugeot 308'),
(6, N'Honda Civic');

SET IDENTITY_INSERT [dbo].[Modelos] OFF;

-- -----------------
-- Coches
SET IDENTITY_INSERT [dbo].[Coches] ON;

INSERT INTO [dbo].[Coches] (
    [Id], [ClaseCoche],        [Color], [Descripcion],                      [DesplazamientoMotor],
    [TipoCombustible], [TipoDeMantenimiento], [Fabricante],
    [PrecioCompra], [CantidadCompra], [CantidadAlquiler], [PrecioAlquiler],
    [TamanoLlanta], [ModeloId]
) VALUES
(1, N'Berlina',              N'Negro',  N'Sedán premium de tamaño medio',   N'180 CV',
 N'Diesel', 0, N'Mercedes',
 25000, 4, 2, 120,
 N'55.0', 1),

(2, N'Compacto Urbano',      N'Azul',   N'Compacto alemán muy vendido',     N'115 CV',
 N'Gasolina', 2, N'Volkswagen',
 15000, 6, 3, 80,
 N'45.0', 2),

(3, N'Compacto Ciudad',      N'Rojo',   N'Coche urbano económico',          N'100 CV',
 N'Gasolina', 0, N'Ford',
 12000, 10, 5, 60,
 N'42.0', 3),

(4, N'SUV',                  N'Blanco', N'SUV compacto moderno',            N'140 CV',
 N'Diesel', 3, N'Kia',
 23000, 3, 2, 110,
 N'55.5', 4),

(5, N'Compacto Familiar',    N'Gris',   N'Compacto francés del segmento C', N'130 CV',
 N'Gasolina', 4, N'Peugeot',
 14000, 7, 4, 75,
 N'44.0', 5),

(6, N'Compacto Deportivo',   N'Verde',  N'Coche compacto japonés',          N'125 CV',
 N'Gasolina', 5, N'Honda',
 16000, 5, 3, 90,
 N'46.0', 6);

SET IDENTITY_INSERT [dbo].[Coches] OFF;

-- -----------------
-- Compras

DECLARE @CompraLucas     INT;
DECLARE @CompraElena     INT;
DECLARE @CompraSergio    INT;
DECLARE @CompraLaura     INT;
DECLARE @CompraIrene     INT;
DECLARE @CompraMarc      INT;

-- Compra 1: Lucas (usuario 1) compra un Mercedes Clase C
INSERT INTO [dbo].[Compras] (
    [Nombre], [Apellido], [ConcesionarioEntrega],
    [PrecioCompra], [FechaCompra], [ApplicationUserId], [MetodoPago]
) VALUES (
    N'Lucas',  N'Maldonado', N'Concesionario Central - C/ Gran Vía 10',
    25000.00, '2025-01-10 11:00:00', N'1', 0
);
SET @CompraLucas = SCOPE_IDENTITY();

-- Compra 2: Elena (usuario 2) compra un Volkswagen Golf
INSERT INTO [dbo].[Compras] (
    [Nombre], [Apellido], [ConcesionarioEntrega],
    [PrecioCompra], [FechaCompra], [ApplicationUserId], [MetodoPago]
) VALUES (
    N'Elena',  N'Ruiz',      N'Concesionario Norte - Av. Palmera 6',
    15000.00, '2025-02-20 16:30:00', N'2', 1
);
SET @CompraElena = SCOPE_IDENTITY();

-- Compra 3: Sergio (usuario 3) compra un Ford Focus y un Peugeot 308
INSERT INTO [dbo].[Compras] (
    [Nombre], [Apellido], [ConcesionarioEntrega],
    [PrecioCompra], [FechaCompra], [ApplicationUserId], [MetodoPago]
) VALUES (
    N'Sergio', N'Giménez',   N'Concesionario Sur - C/ Prado Verde 21',
    26000.00, '2025-03-15 10:15:00', N'3', 2
);
SET @CompraSergio = SCOPE_IDENTITY();

-- Compra 4: Laura (usuario 4) compra un Kia Sportage
INSERT INTO [dbo].[Compras] (
    [Nombre], [Apellido], [ConcesionarioEntrega],
    [PrecioCompra], [FechaCompra], [ApplicationUserId], [MetodoPago]
) VALUES (
    N'Laura',  N'Blanco',    N'Concesionario Este - C/ Camino Alto 9',
    23000.00, '2025-04-22 12:45:00', N'4', 0
);
SET @CompraLaura = SCOPE_IDENTITY();

-- Compra 5: Irene (usuario 5) compra dos Honda Civic
INSERT INTO [dbo].[Compras] (
    [Nombre], [Apellido], [ConcesionarioEntrega],
    [PrecioCompra], [FechaCompra], [ApplicationUserId], [MetodoPago]
) VALUES (
    N'Irene',  N'Pons',      N'Concesionario Oeste - Av. Las Flores 18',
    32000.00, '2025-05-30 09:30:00', N'5', 1
);
SET @CompraIrene = SCOPE_IDENTITY();

-- Compra 6: Marc (usuario 6) compra un Volkswagen Golf y un Honda Civic
INSERT INTO [dbo].[Compras] (
    [Nombre], [Apellido], [ConcesionarioEntrega],
    [PrecioCompra], [FechaCompra], [ApplicationUserId], [MetodoPago]
) VALUES (
    N'Marc',   N'Ferrer',    N'Concesionario Río - C/ Camino del Río 3',
    31000.00, '2025-06-10 17:20:00', N'6', 2
);
SET @CompraMarc = SCOPE_IDENTITY();

-- -----------------
-- ComprarItem (compra de coches)
INSERT INTO [dbo].[ComprarItem] ([CocheId], [ComprarId], [Precio], [Cantidad]) VALUES
    -- Compra 1: Lucas compra 1 Mercedes Clase C
    (1, @CompraLucas, 25000.00, 1.00),

    -- Compra 2: Elena compra 1 Volkswagen Golf
    (2, @CompraElena, 15000.00, 1.00),

    -- Compra 3: Sergio compra 1 Ford Focus y 1 Peugeot 308
    (3, @CompraSergio, 12000.00, 1.00),
    (5, @CompraSergio, 14000.00, 1.00),

    -- Compra 4: Laura compra 1 Kia Sportage
    (4, @CompraLaura, 23000.00, 1.00),

    -- Compra 5: Irene compra 2 Honda Civic
    (6, @CompraIrene, 16000.00, 2.00),

    -- Compra 6: Marc compra 1 Volkswagen Golf y 1 Honda Civic
    (2, @CompraMarc, 15000.00, 1.00),
    (6, @CompraMarc, 16000.00, 1.00);

-- -----------------
-- Reseñas
SET IDENTITY_INSERT [dbo].[Reseñas] ON;
INSERT INTO [dbo].[Reseñas]
([Id], [Usuario], [Pais], [TipoConductor], [Creado], [ApplicationUserId])
VALUES
(1, N'Lucas', N'España', N'Titular', '2025-10-10T10:00:00', '1');
INSERT INTO [dbo].[Reseñas]
([Id], [Usuario], [Pais], [TipoConductor], [Creado], [ApplicationUserId])
VALUES
(2, N'Elena', N'España', N'Adicional', '2025-11-12T11:00:00', '2');
SET IDENTITY_INSERT [dbo].[Reseñas] OFF;

-- -----------------
-- ReseñarItem (detalle de reseñas de coches)
INSERT INTO [dbo].[ReseñarItem]
([CocheId], [ReseñarId], [Calificacion], [Descripcion])
VALUES
(1, 1, 5, N'Me ha encantado el coche, experiencia muy positiva.');
INSERT INTO [dbo].[ReseñarItem]
([CocheId], [ReseñarId], [Calificacion], [Descripcion])
VALUES
(1, 2, 4, N'Buen comportamiento general, pequeños detalles a mejorar.');

-- -----------------
-- BORRADO DE PRUEBAS
/*
DELETE FROM [dbo].[ReseñarItem];
DELETE FROM [dbo].[Reseñas];
DELETE FROM [dbo].[ComprarItem];
DELETE FROM [dbo].[Compras];
DELETE FROM [dbo].[Coches];
DELETE FROM [dbo].[Modelos];
*/
