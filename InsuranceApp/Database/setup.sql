-- Kreirajte tablicu State (prva tablica)
CREATE TABLE IF NOT EXISTS State (
    StateId INTEGER PRIMARY KEY AUTOINCREMENT,
    StateName TEXT NOT NULL
);

-- Kreirajte tablicu City (povezana s tablicom State)
CREATE TABLE IF NOT EXISTS City (
    CityId INTEGER PRIMARY KEY AUTOINCREMENT,
    CityName TEXT NOT NULL,
    PostCode TEXT,
    StateId INTEGER,  -- Vanjski ključ prema State
    FOREIGN KEY (StateId) REFERENCES State(StateId) ON DELETE CASCADE
);

-- Kreirajte tablicu Partner (povezana s tablicama City i State)
CREATE TABLE IF NOT EXISTS Partner (
    PartnerId INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Address TEXT,
    PartnerNumber TEXT NOT NULL,
    CroatianPIN TEXT,
    PartnerTypeId INTEGER NOT NULL,
    CreatedAtUtc TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreateByUser TEXT NOT NULL,
    IsForeign BOOLEAN NOT NULL,
    ExternalCode TEXT NOT NULL,
    Gender CHAR(1) CHECK (Gender IN ('M', 'F', 'N')),
    CityId INTEGER, 
    FOREIGN KEY (CityId) REFERENCES City(CityId) ON DELETE SET NULL
);

-- Kreirajte tablicu Policy (povezana s tablicom Partner)
CREATE TABLE IF NOT EXISTS Policy (
    PolicyId INTEGER PRIMARY KEY AUTOINCREMENT,
    PolicyNumber TEXT NOT NULL,
    PolicyAmount DECIMAL NOT NULL,
    PartnerId INTEGER,  -- Vanjski ključ prema Partner
    FOREIGN KEY (PartnerId) REFERENCES Partner(PartnerId) ON DELETE CASCADE
);
