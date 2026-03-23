IF COL_LENGTH('dbo.Users', 'Dni') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD Dni NVARCHAR(20) NULL;
    UPDATE dbo.Users SET Dni = '87654321' WHERE Email = 'luis.eduardo.200325@gmail.com' AND Dni IS NULL;
    UPDATE dbo.Users SET Dni = '12345678' WHERE Email = 'inactive@example.com' AND Dni IS NULL;
    UPDATE dbo.Users SET Dni = CONCAT('DNI', ABS(CHECKSUM(NEWID())) % 100000000) WHERE Dni IS NULL;
    ALTER TABLE dbo.Users ALTER COLUMN Dni NVARCHAR(20) NOT NULL;
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Users_Dni')
        CREATE UNIQUE INDEX IX_Users_Dni ON dbo.Users(Dni);
END

IF COL_LENGTH('dbo.Users', 'FailedLoginAttempts') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD FailedLoginAttempts INT NOT NULL CONSTRAINT DF_Users_FailedLoginAttempts DEFAULT(0);
END

IF COL_LENGTH('dbo.Users', 'LockoutEndUtc') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD LockoutEndUtc DATETIME2(0) NULL;
END

IF COL_LENGTH('dbo.Users', 'LastLoginAt') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD LastLoginAt DATETIME2(0) NULL;
END
