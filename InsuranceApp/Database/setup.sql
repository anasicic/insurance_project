"""-- State Table
CREATE TABLE State (
    StateId INTEGER PRIMARY KEY AUTOINCREMENT,
    StateName TEXT NOT NULL CHECK(LENGTH(StateName) BETWEEN 1 AND 40)
);

-- City Table
CREATE TABLE City (
    CityId INTEGER PRIMARY KEY AUTOINCREMENT,
    CityName TEXT NOT NULL CHECK(LENGTH(CityName) BETWEEN 1 AND 40),
    StateId INTEGER NOT NULL,
    FOREIGN KEY(StateId) REFERENCES State(StateId)
);

-- Partner Table
CREATE TABLE Partner (
    PartnerId INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL CHECK(LENGTH(FirstName) BETWEEN 2 AND 255),
    LastName TEXT NOT NULL CHECK(LENGTH(LastName) BETWEEN 2 AND 255),
    Address TEXT,
    PartnerNumber TEXT NOT NULL CHECK(LENGTH(PartnerNumber) = 20),
    CroatianPIN TEXT CHECK(LENGTH(CroatianPIN) = 11),
    PartnerTypeId INTEGER NOT NULL,
    CreatedAtUtc TEXT DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreateByUser TEXT NOT NULL,
    IsForeign BOOLEAN NOT NULL,
    ExternalCode TEXT NOT NULL CHECK(LENGTH(ExternalCode) BETWEEN 10 AND 20),
    Gender CHAR(1) CHECK(Gender IN ('M', 'F', 'N')),
    CityId INTEGER NOT NULL,
    FOREIGN KEY(CityId) REFERENCES City(CityId)
);

-- Policy Table
CREATE TABLE Policy (
    PolicyId INTEGER PRIMARY KEY AUTOINCREMENT,
    PolicyNumber TEXT NOT NULL CHECK(LENGTH(PolicyNumber) BETWEEN 10 AND 15),
    PolicyAmount DECIMAL(18, 2) NOT NULL,
    PartnerId INTEGER NOT NULL,
    FOREIGN KEY(PartnerId) REFERENCES Partner(PartnerId)
);

-- Dodavanje jedinstvenog indeksa za CroatianPIN (OIB)
CREATE UNIQUE INDEX idx_partner_oib ON Partner (CroatianPIN);

-- Dodavanje jedinstvenog indeksa za ExternalCode
CREATE UNIQUE INDEX idx_partner_externalcode ON Partner (ExternalCode);

-- Insertion for State table
INSERT INTO State (StateName) 
VALUES 
('Croatia'),
('Austria');

-- Insertion for City table
INSERT INTO City (CityName, StateId) 
VALUES 
('Zagreb', 1),
('Split', 1),
('Zadar', 1);

-- Insertion for Partner table
INSERT INTO Partner (FirstName, LastName, Address, PartnerNumber, CroatianPIN, PartnerTypeId, CreateByUser, IsForeign, ExternalCode, Gender, CityId) 
VALUES 
('Ivan', 'Horvat', 'Zagrebačka cesta 50', '12345678901234567890', '12345678901', 1, 'admin@insurance.com', 0, 'EXTERNALCODE1234', 'M', 1),
('Ana', 'Kovač', 'Splitska 2', '09876543210987654321', '10987654321', 2, 'admin@insurance.com', 1, 'EXTERNALCODE5678', 'F', 1);

-- Insertion for Policy table
INSERT INTO Policy (PolicyNumber, PolicyAmount, PartnerId) 
VALUES 
('POLICY12345678', 1000.00, 1),
('POLICY09876543', 5500.00, 2);

"""