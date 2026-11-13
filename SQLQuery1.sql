CREATE DATABASE RestauranteITQ;
GO
USE RestauranteITQ;
GO
CREATE TABLE Categorias (
    Id     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(60) NOT NULL UNIQUE
);
CREATE TABLE Platos (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(300) NULL,
    Precio      DECIMAL(10,2) NOT NULL CHECK (Precio > 0),
    Disponible  BIT NOT NULL DEFAULT 1,
    CategoriaId INT NOT NULL REFERENCES Categorias(Id)
);
CREATE TABLE Mesas (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Numero    INT NOT NULL UNIQUE,
    Capacidad INT NOT NULL CHECK (Capacidad > 0)
);
CREATE TABLE Pedidos (
    Id     INT IDENTITY(1,1) PRIMARY KEY,
    MesaId INT NOT NULL REFERENCES Mesas(Id),
    Fecha  DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Estado NVARCHAR(20) NOT NULL DEFAULT N'Pendiente'
);
CREATE TABLE DetallesPedido (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId       INT NOT NULL REFERENCES Pedidos(Id),
    PlatoId        INT NOT NULL REFERENCES Platos(Id),
    Cantidad       INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(10,2) NOT NULL
);

-- insertando datos de prueba
INSERT INTO Categorias (Nombre)
VALUES (N'Sopas'), (N'Platos fuertes'), (N'Bebidas');

INSERT INTO Platos (Nombre, Descripcion, Precio, CategoriaId)
VALUES (N'Encebollado', N'Sopa de albacora con yuca', 6.50, 1),
       (N'Seco de pollo', N'Con arroz y maduro', 7.00, 2),
       (N'Limonada', N'Natural, sin azúcar', 1.50, 3);

INSERT INTO Mesas (Numero, Capacidad) VALUES (1, 4), (2, 2);

-- La mesa 1 pide 2 encebollados y 1 limonada
INSERT INTO Pedidos (MesaId) VALUES (1);
INSERT INTO DetallesPedido (PedidoId, PlatoId, Cantidad, PrecioUnitario)
VALUES (1, 1, 2, 6.50), (1, 3, 1, 1.50);

-- ¿Cuánto debe la mesa 1?
SELECT m.Numero AS Mesa, p.Nombre, d.Cantidad,
       d.Cantidad * d.PrecioUnitario AS Subtotal
FROM DetallesPedido d
JOIN Pedidos pe ON pe.Id = d.PedidoId
JOIN Mesas  m  ON m.Id  = pe.MesaId
JOIN Platos p  ON p.Id  = d.PlatoId;

-- Prueba la integridad: esto debe FALLAR
INSERT INTO Platos (Nombre, Precio, CategoriaId)
VALUES (N'Ceviche', 8.00, 99);