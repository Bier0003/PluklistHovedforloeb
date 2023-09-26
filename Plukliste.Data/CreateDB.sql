USE Plukliste
GO

CREATE TABLE Pluklist (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name varchar(255),
    Forsendelse varchar(255),
    Adresse varchar(255)
);
GO

CREATE TABLE Item (
    ID int IDENTITY(1,1) PRIMARY KEY,
    ProductID varchar(255),
    Title varchar(255),
    ItemType tinyint,
    Amount int
);
GO

CREATE TABLE ItemLine (
    PluklisteID INT NOT NULL,
    ItemID INT NOT NULL,
    Amount INT NOT NULL,
    PRIMARY KEY (PluklisteID, ItemID),
    CONSTRAINT FK_PluklisteID FOREIGN KEY (PluklisteID)
    REFERENCES Pluklist(ID),
    CONSTRAINT FK_ItemID FOREIGN KEY (ItemID)
    REFERENCES Item(ID)
);
GO