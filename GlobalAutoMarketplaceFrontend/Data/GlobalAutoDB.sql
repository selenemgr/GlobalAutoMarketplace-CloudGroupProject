-- Delete the database if it exists
IF EXISTS(SELECT * from sys.databases WHERE name='GlobalAutoDB')
BEGIN
    DROP DATABASE GlobalAutoDB;
END
GO

CREATE DATABASE GlobalAutoDB;
GO

USE GlobalAutoDB
GO


--  Brands Table 

CREATE TABLE dbo.Brands (
    BrandId     INT             IDENTITY (1, 1) PRIMARY KEY,
    BrandName   NVARCHAR (50)   NOT NULL UNIQUE
);
GO

-- Insert 10  brands
INSERT INTO Brands (BrandName) VALUES 
('Toyota'), ('Honda'), ('Ford'), 
('BMW'), ('Audi'), ('Nissan'), 
('Kia'), ('Hyundai'), ('Chevrolet'), 
('Mazda');



--  Users Table

CREATE TABLE dbo.Users (
    UserId      INT             IDENTITY (1, 1) PRIMARY KEY,
    Username    NVARCHAR (50)   NOT NULL UNIQUE,
    Email       NVARCHAR (100)  NOT NULL UNIQUE,
    UserRole    NVARCHAR (20)   NOT NULL, -- e.g., 'Seller', 'Buyer', 'Admin'
    Password    NVARCHAR (50)   NOT NULL 
);
GO

-- Insert 11 User 
INSERT INTO Users (Username, Email, UserRole, Password) VALUES 
('seller_toyota_1', 'seller.t1@auto.com', 'Seller', 'admin123'),
('seller_toyota_2', 'seller.t2@auto.com', 'Seller', 'admin123'),
('seller_honda',    'seller.h@auto.com',  'Seller', 'admin123'),
('seller_ford',     'seller.f@auto.com',  'Seller', 'admin123'),
('admin_user',      'admin@auto.com',     'Admin', 'admin123'),
('buyer_a',         'buyer.a@auto.com',   'Buyer', 'admin123'),
('buyer_b',         'buyer.b@auto.com',   'Buyer', 'admin123'),
('buyer_c',         'buyer.c@auto.com',   'Buyer', 'admin123'),
('test_user_9',     'test.9@auto.com',    'Buyer', 'admin123'),
('test_user_10',    'test.10@auto.com',   'Buyer', 'admin123'),
('test_user_11',    'test.11@auto.com',   'Seller', 'admin123');


--  Cars Table 

CREATE TABLE dbo.Cars (
    CarId       INT             IDENTITY (1, 1) PRIMARY KEY,
    BrandId     INT             NOT NULL, 
    SellerId    INT             NOT NULL, 
    Model       NVARCHAR (50)   NOT NULL,
    Year        INT             NOT NULL,
    Price       DECIMAL (18, 2) NOT NULL,
    Color       NVARCHAR (30)   NULL,
    VIN         CHAR (17)       NOT NULL UNIQUE,
    CONSTRAINT FK_Cars_Brands_BrandId FOREIGN KEY (BrandId) REFERENCES Brands (BrandId) ON DELETE CASCADE,
    CONSTRAINT FK_Cars_Users_SellerId FOREIGN KEY (SellerId) REFERENCES Users (UserId) ON DELETE NO ACTION
);
GO

-- Insert 30 Cars (10 for Toyota, 10 for Honda, 10 for Ford)

-- 10 Toyota 
INSERT INTO Cars (BrandId, SellerId, Model, Year, Price, Color, VIN) VALUES 
(1, 1, 'Corolla', 2022, 25500.00, 'Red', 'JTD90123456789012'),
(1, 2, 'Camry', 2023, 31000.00, 'White', 'JTD90123456789013'),
(1, 1, 'RAV4', 2021, 28900.00, 'Blue', 'JTD90123456789014'),
(1, 2, 'Highlander', 2023, 45000.00, 'Silver', 'JTD90123456789015'),
(1, 11, 'Tacoma', 2024, 38000.00, 'Gray', 'JTD90123456789016'),
(1, 1, 'Sienna', 2020, 32500.00, 'Black', 'JTD90123456789017'),
(1, 2, 'Tundra', 2024, 55000.00, 'Black', 'JTD90123456789018'),
(1, 11, 'Prius', 2023, 27500.00, 'Green', 'JTD90123456789019'),
(1, 1, '4Runner', 2022, 42000.00, 'Army Green', 'JTD90123456789020'),
(1, 2, 'Supra', 2024, 60000.00, 'Yellow', 'JTD90123456789021');

-- 10 Honda 
INSERT INTO Cars (BrandId, SellerId, Model, Year, Price, Color, VIN) VALUES 
(2, 3, 'Civic', 2023, 24000.00, 'Red', '1HG0123456789018'),
(2, 3, 'CRV', 2024, 30500.00, 'White', '1HG0123456789019'),
(2, 3, 'Accord', 2022, 28000.00, 'Black', '1HG0123456789020'),
(2, 3, 'Pilot', 2023, 40000.00, 'Silver', '1HG0123456789021'),
(2, 3, 'Ridgeline', 2024, 45000.00, 'Blue', '1HG0123456789022'),
(2, 3, 'Odyssey', 2021, 35000.00, 'Gray', '1HG0123456789023'),
(2, 3, 'HR-V', 2024, 25000.00, 'Green', '1HG0123456789024'),
(2, 3, 'Passport', 2022, 38000.00, 'Orange', '1HG0123456789025'),
(2, 3, 'Insight', 2021, 23000.00, 'Red', '1HG0123456789026'),
(2, 3, 'Clarity', 2020, 29000.00, 'White', '1HG0123456789027');

-- 10 Ford 
INSERT INTO Cars (BrandId, SellerId, Model, Year, Price, Color, VIN) VALUES 
(3, 4, 'F-150', 2022, 52000.00, 'Blue', '1FT0123456789020'),
(3, 4, 'Mustang', 2023, 41000.00, 'Yellow', '1FT0123456789021'),
(3, 4, 'Escape', 2024, 27000.00, 'Black', '1FT0123456789022'),
(3, 4, 'Explorer', 2023, 35000.00, 'Silver', '1FT0123456789023'),
(3, 4, 'Bronco', 2022, 45000.00, 'White', '1FT0123456789024'),
(3, 4, 'Maverick', 2024, 23000.00, 'Red', '1FT0123456789025'),
(3, 4, 'Ranger', 2021, 32000.00, 'Gray', '1FT0123456789026'),
(3, 4, 'Edge', 2023, 30000.00, 'Blue', '1FT0123456789027'),
(3, 4, 'Focus', 2020, 18000.00, 'Black', '1FT0123456789028'),
(3, 4, 'Taurus', 2019, 15000.00, 'Red', '1FT0123456789029');