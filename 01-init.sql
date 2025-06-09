-- Criar banco de dados
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'UxComexDB')
BEGIN
    CREATE DATABASE UxComexDB;
END
GO

USE UxComexDB;
GO

-- =========================
-- TABELA: Clients
-- =========================
CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    RegisteredAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

INSERT INTO Clients (Name, Email, PhoneNumber, RegisteredAt) VALUES
('Alice Costa', 'alice.costa@email.com', '+55 11 91234-5678', '2024-05-10 14:30:00'),
('Bruno Lima', 'bruno.lima@email.com', '+55 21 98765-4321', '2024-04-22 09:15:00'),
('Carla Mendes', 'carla.mendes@email.com', '+55 31 99887-6655', '2024-06-01 11:00:00'),
('Diego Souza', 'diego.souza@email.com', '+55 41 99999-1234', '2024-03-15 08:45:00'),
('Eduarda Ferreira', 'eduarda.ferreira@email.com', '+55 61 98888-0001', '2024-02-28 17:25:00');
GO

-- =========================
-- TABELA: Orders
-- =========================
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClientId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(10, 2) NOT NULL,
    Status NVARCHAR(50) NOT NULL,

    CONSTRAINT FK_Orders_Clients FOREIGN KEY (ClientId) REFERENCES Clients(Id)
);
GO

INSERT INTO Orders (ClientId, CreatedAt, Total, Status) VALUES
(1, '2024-05-11 10:20:00', 1520.50, 'Pending'),
(2, '2024-05-20 15:45:00', 230.00, 'Shipped'),
(3, '2024-06-01 12:10:00', 999.99, 'Delivered'),
(1, '2024-06-05 08:30:00', 350.00, 'Cancelled'),
(4, '2024-04-15 16:00:00', 4850.75, 'Processing');
GO

-- =========================
-- TABELA: Products
-- =========================
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(10, 2) NOT NULL,
    Quantity INT NOT NULL
);
GO

INSERT INTO Products (Name, Description, Price, Quantity) VALUES
('Notebook Dell XPS', 'Ultrabook com tela 13" Full HD, SSD 512GB, 16GB RAM', 8500.00, 10),
('Mouse Logitech MX Master 3', 'Mouse sem fio ergonômico, recarregável via USB-C', 420.00, 50),
('Monitor LG 29"', 'Monitor ultrawide 29" com resolução 2560x1080', 1399.90, 25),
('Teclado Mecânico Redragon', 'Switch red, com RGB, layout ABNT2', 289.99, 35),
('Headset HyperX Cloud', 'Headset gamer com microfone removível e áudio 7.1', 499.00, 20);
GO

CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,

    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO

-- Exemplo de INSERT na tabela OrderItems
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
(1, 1, 1, 8500.00), -- Pedido 1: 1x Notebook Dell XPS
(1, 2, 2, 420.00),  -- Pedido 1: 2x Mouse Logitech
(2, 4, 1, 289.99),  -- Pedido 2: 1x Teclado Redragon
(3, 5, 1, 499.00),  -- Pedido 3: 1x Headset HyperX
(4, 3, 1, 1399.90); -- Pedido 4: 1x Monitor LG
GO