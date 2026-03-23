IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Dni NVARCHAR(20) NOT NULL UNIQUE,
        Email NVARCHAR(256) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(512) NOT NULL,
        IsActive BIT NOT NULL,
        FailedLoginAttempts INT NOT NULL CONSTRAINT DF_Users_FailedLoginAttempts DEFAULT(0),
        LockoutEndUtc DATETIME2(0) NULL,
        LastLoginAt DATETIME2(0) NULL,
        CreatedAtUtc DATETIME2(0) NOT NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH('dbo.Users', 'Dni') IS NULL
        ALTER TABLE dbo.Users ADD Dni NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_Dni DEFAULT('00000000');
    IF COL_LENGTH('dbo.Users', 'FailedLoginAttempts') IS NULL
        ALTER TABLE dbo.Users ADD FailedLoginAttempts INT NOT NULL CONSTRAINT DF_Users_FailedLoginAttempts DEFAULT(0);
    IF COL_LENGTH('dbo.Users', 'LockoutEndUtc') IS NULL
        ALTER TABLE dbo.Users ADD LockoutEndUtc DATETIME2(0) NULL;
    IF COL_LENGTH('dbo.Users', 'LastLoginAt') IS NULL
        ALTER TABLE dbo.Users ADD LastLoginAt DATETIME2(0) NULL;
END

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'luis.eduardo.200325@gmail.com')
BEGIN
    INSERT INTO dbo.Users (Id, Dni, Email, PasswordHash, IsActive, FailedLoginAttempts, LockoutEndUtc, LastLoginAt, CreatedAtUtc)
    VALUES ('11111111-1111-1111-1111-111111111111', '87654321', 'luis.eduardo.200325@gmail.com', 'nTFmW+1XStgvZ3cx4FLP3w==.Z4HkGGbw7YA4cRprqE3Fvj1AiMSrfFf+RGFwo2WOENY=', 1, 0, NULL, NULL, SYSUTCDATETIME());
END

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'inactive@example.com')
BEGIN
    INSERT INTO dbo.Users (Id, Dni, Email, PasswordHash, IsActive, FailedLoginAttempts, LockoutEndUtc, LastLoginAt, CreatedAtUtc)
    VALUES ('22222222-2222-2222-2222-222222222222', '12345678', 'inactive@example.com', 'F4KLotBbFOZsazdeee3a7g==.BFef+2q6o9Q7f93kf2Px7jIzq/kZvFvXtgM5VYQCk7s=', 0, 0, NULL, NULL, SYSUTCDATETIME());
END
