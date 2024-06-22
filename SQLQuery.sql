-- Crear la base de datos Personas
CREATE DATABASE Personas;
GO

-- Usar la base de datos Personas
USE Personas;
GO

-- Crear la tabla Personas
CREATE TABLE Personas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    NumeroIdentificacion NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    TipoIdentificacion NVARCHAR(50) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
GO


-- Crear la tabla Usuario
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    Pass NVARCHAR(100) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
GO


--Agregar usuario administrador
INSERT INTO [dbo].[Usuarios]
           ([Nombre]
           ,[Pass]
           ,[FechaCreacion])
     VALUES
           ('Admin'
           ,'$2a$11$PlgZ9BjO7GvKhjUVso5tZOli/Od5P0YD2qyTBKCTr0rBcMilZrx0m'
           ,GETDATE())



