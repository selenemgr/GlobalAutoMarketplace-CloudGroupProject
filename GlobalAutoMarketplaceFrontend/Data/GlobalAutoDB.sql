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


-- Brands Table 
CREATE TABLE Brands (
    BrandId     INT             IDENTITY (1, 1) PRIMARY KEY,
    BName       NVARCHAR (50)   NOT NULL UNIQUE -- Brand Name
);
GO

-- Insert 10 brands
INSERT INTO Brands (BName) VALUES 
('Toyota'), ('Honda'), ('Ford'), 
('BMW'), ('Audi'), ('Nissan'), 
('Kia'), ('Hyundai'), ('Chevrolet'), 
('Mazda');


-- VehicleTypes Table 
CREATE TABLE VehicleTypes (
    VehicleTypeId   INT             IDENTITY (1, 1) PRIMARY KEY,
    TypeName        NVARCHAR (50)   NOT NULL UNIQUE, -- SUV, Sedan, etc.
    Description     NVARCHAR (200)  NULL
);
GO

-- Insert 11 Vehicle Types
INSERT INTO VehicleTypes (TypeName, Description) VALUES 
('Sedan', 'Standard car with a separate trunk'),
('SUV', 'Sport Utility Vehicle'),
('Truck', 'Pickup truck for cargo'),
('Coupe', 'Two-door sporty car'),
('Convertible', 'Roof can retract'),
('Hatchback', 'Rear door opens upward'),
('Minivan', 'Family transport vehicle'),
('Electric', 'Battery Powered'),
('Hybrid', 'Gas and Electric'),
('Luxury', 'High-end vehicle with premium features'),
('Crossover', 'Mix of SUV and Sedan features');


-- Cars Table
CREATE TABLE Cars (
    CarId           INT             IDENTITY (1, 1) PRIMARY KEY,
    BrandId         INT             NOT NULL, -- FK to Brands
    VehicleTypeId   INT             NOT NULL, -- FK to VehicleTypes
    Model           NVARCHAR (50)   NOT NULL,
    Year            INT             NOT NULL,
    Price           DECIMAL (18, 2) NOT NULL,
    Color           NVARCHAR (30)   NULL,
    VIN             CHAR (17)       NOT NULL UNIQUE,
    
    CONSTRAINT FK_Cars_Brands_BrandId FOREIGN KEY (BrandId) REFERENCES Brands (BrandId) ON DELETE CASCADE,
    CONSTRAINT FK_Cars_VehicleTypes_TypeId FOREIGN KEY (VehicleTypeId) REFERENCES VehicleTypes (VehicleTypeId) ON DELETE CASCADE
);
GO

-- Insert 30 Cars 

-- 10 Toyota (Brand 1)
INSERT INTO Cars (BrandId, VehicleTypeId, Model, Year, Price, Color, VIN) VALUES 
(1, 1, 'Corolla', 2022, 25500.00, 'Red', 'JTD90123456789012'), -- Sedan
(1, 1, 'Camry', 2023, 31000.00, 'White', 'JTD90123456789013'), -- Sedan
(1, 2, 'RAV4', 2021, 28900.00, 'Blue', 'JTD90123456789014'), -- SUV
(1, 2, 'Highlander', 2023, 45000.00, 'Silver', 'JTD90123456789015'), -- SUV
(1, 3, 'Tacoma', 2024, 38000.00, 'Gray', 'JTD90123456789016'), -- Truck
(1, 7, 'Sienna', 2020, 32500.00, 'Black', 'JTD90123456789017'), -- Minivan
(1, 3, 'Tundra', 2024, 55000.00, 'Black', 'JTD90123456789018'), -- Truck
(1, 9, 'Prius', 2023, 27500.00, 'Green', 'JTD90123456789019'), -- Hybrid
(1, 2, '4Runner', 2022, 42000.00, 'Army Green', 'JTD90123456789020'), -- SUV
(1, 4, 'Supra', 2024, 60000.00, 'Yellow', 'JTD90123456789021'); -- Coupe

-- 10 Honda (Brand 2)
INSERT INTO Cars (BrandId, VehicleTypeId, Model, Year, Price, Color, VIN) VALUES 
(2, 1, 'Civic', 2023, 24000.00, 'Red', '1HG0123456789018'), -- Sedan
(2, 2, 'CRV', 2024, 30500.00, 'White', '1HG0123456789019'), -- SUV
(2, 1, 'Accord', 2022, 28000.00, 'Black', '1HG0123456789020'), -- Sedan
(2, 2, 'Pilot', 2023, 40000.00, 'Silver', '1HG0123456789021'), -- SUV
(2, 3, 'Ridgeline', 2024, 45000.00, 'Blue', '1HG0123456789022'), -- Truck
(2, 7, 'Odyssey', 2021, 35000.00, 'Gray', '1HG0123456789023'), -- Minivan
(2, 2, 'HR-V', 2024, 25000.00, 'Green', '1HG0123456789024'), -- SUV
(2, 2, 'Passport', 2022, 38000.00, 'Orange', '1HG0123456789025'), -- SUV
(2, 9, 'Insight', 2021, 23000.00, 'Red', '1HG0123456789026'), -- Hybrid
(2, 9, 'Clarity', 2020, 29000.00, 'White', '1HG0123456789027'); -- Hybrid

-- 10 Ford (Brand 3)
INSERT INTO Cars (BrandId, VehicleTypeId, Model, Year, Price, Color, VIN) VALUES 
(3, 3, 'F-150', 2022, 52000.00, 'Blue', '1FT0123456789020'), -- Truck
(3, 4, 'Mustang', 2023, 41000.00, 'Yellow', '1FT0123456789021'), -- Coupe
(3, 2, 'Escape', 2024, 27000.00, 'Black', '1FT0123456789022'), -- SUV
(3, 2, 'Explorer', 2023, 35000.00, 'Silver', '1FT0123456789023'), -- SUV
(3, 2, 'Bronco', 2022, 45000.00, 'White', '1FT0123456789024'), -- SUV
(3, 3, 'Maverick', 2024, 23000.00, 'Red', '1FT0123456789025'), -- Truck
(3, 3, 'Ranger', 2021, 32000.00, 'Gray', '1FT0123456789026'), -- Truck
(3, 2, 'Edge', 2023, 30000.00, 'Blue', '1FT0123456789027'), -- SUV
(3, 1, 'Focus', 2020, 18000.00, 'Black', '1FT0123456789028'), -- Sedan
(3, 1, 'Taurus', 2019, 15000.00, 'Red', '1FT0123456789029'); -- Sedan