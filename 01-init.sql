-- Criar banco de dados
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'UxComexDB')
BEGIN
    CREATE DATABASE UxComexDB;
END
GO

USE UxComexDB;
GO

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
(1, '2024-05-11 10:20:00', 9340.00, 'Pendente'),
(2, '2024-05-20 15:45:00', 289.99, 'Enviado'),
(3, '2024-06-01 12:10:00', 499.00, 'Entregue'),
(1, '2024-06-05 08:30:00', 1399.90, 'Cancelado'),
(4, '2024-04-15 16:00:00', 8500.00, 'Processando');
GO

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

INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
(1, 1, 1, 8500.00),
(1, 2, 2, 420.00),
(2, 4, 1, 289.99),
(3, 5, 1, 499.00),
(4, 3, 1, 1399.90),
(5, 1, 1, 8500.00);
GO

CREATE TABLE OrderNotifications (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    OldStatus NVARCHAR(50),
    NewStatus NVARCHAR(50),
    ChangedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_OrderNotifications_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);
GO

INSERT INTO OrderNotifications (OrderId, OldStatus, NewStatus, ChangedAt) VALUES
(1, 'Pendente', 'Processando', '2024-05-11 12:00:00'),
(1, 'Processando', 'Enviado', '2024-05-11 15:30:00'),
(2, 'Pendente', 'Enviado', '2024-05-20 17:00:00'),
(2, 'Enviado', 'Entregue', '2024-05-23 09:00:00'),
(3, 'Pendente', 'Cancelado', '2024-06-01 14:00:00'),
(4, 'Pendente', 'Processando', '2024-06-05 10:00:00'),
(4, 'Processando', 'Cancelado', '2024-06-06 09:15:00'),
(5, 'Pendente', 'Processando', '2024-04-15 18:00:00');