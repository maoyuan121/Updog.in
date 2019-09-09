CREATE TABLE User (
    Id SERIAL NOT NULL PRIMARY KEY,
    Username VARCHAR(24) NOT NULL UNIQUE,
    Email VARCHAR(64) UNIQUE,
    PasswordHash CHAR(60) NOT NULL,
    JoinedDate TIMESTAMP
);