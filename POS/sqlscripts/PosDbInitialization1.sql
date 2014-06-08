USE [PosDb]
GO

--1
INSERT INTO Company
(
    --CompanyId - this column value is auto-generated
    Name,
    RUC,
    Address
)
VALUES
(
    -- CompanyId - tinyint
    'Mi empresa SCRL', -- Name - varchar
    '', -- RUC - varchar
    'Avenida Lima 468' -- Address - varchar
)

--2
INSERT INTO Division
(
    --DivisionId - this column value is auto-generated
    CompanyId,
    Name
)
VALUES
(
    -- DivisionId - tinyint
    1, -- CompanyId - tinyint
    'Lima West Division' -- Name - varchar
)

--3

INSERT INTO Premise
(
    DivisionId,
    --PremiseId - this column value is auto-generated
    Address,
    Name,
    PremiseType
)
VALUES
(
    1, -- DivisionId - tinyint
    -- PremiseId - tinyint
    'Av Lima 468', -- Address - varchar
    'Local San Miguel 1', -- Name - varchar
    'Ventas' -- PremiseType - varchar
)

--4
INSERT INTO Person
(
    --PersonId - this column value is auto-generated
    FirstName,
    LastName,
    DocNumber
)
VALUES
(
    -- PersonId - int
    'Jose', -- FirstName - varchar
    'Gutierrez', -- LastName - varchar
    '34563423' -- DocNumber - varchar
)
--5
INSERT INTO Employee
(
    --EmployeeId - this column value is auto-generated
    PersonId
)
VALUES
(
    -- EmployeeId - int
    1 -- PersonId - int
)

--6
INSERT INTO [User]
(
    EmployeeId,
    --UserId - this column value is auto-generated
    Login,
    Password,
	DefaultPremise
)
VALUES
(
    1, -- EmployeeId - int
    -- UserId - smallint
    'jgutierrez', -- Login - varchar
    'jgutierrez', -- Password - varchar
	1
)	

--7
INSERT INTO SystemRole
(
    --RoleId - this column value is auto-generated
    RoleName
)
VALUES
(
    -- RoleId - tinyint
    'Admin' -- RoleName - varchar
)
INSERT INTO SystemRole
(
    --RoleId - this column value is auto-generated
    RoleName
)
VALUES
(
    -- RoleId - tinyint
    'SalesPerson' -- RoleName - varchar
)
INSERT INTO SystemRole
(
    --RoleId - this column value is auto-generated
    RoleName
)
VALUES
(
    -- RoleId - tinyint
    'Full' -- RoleName - varchar
)

--8
INSERT INTO UserSystemRole
(
    UserId,
    RoleId
)
VALUES
(
    1, -- UserId - smallint
    1 -- RoleId - tinyint
)

--9
INSERT INTO [dbo].[Cashbox]
           ([PremiseId]
           ,[Name]
           ,[CurrentAmount])
     VALUES
           (1 --defalut premise
           ,'Caja Principal'
           ,0)
--10
insert into Unit([Name],[Abbreviation]) values ('Unidad','UND')
insert into Unit([Name],[Abbreviation]) values ('Docena','DOC')
insert into Unit([Name],[Abbreviation]) values ('Ciento','CTO')
insert into Unit([Name],[Abbreviation]) values ('Millar','MLL')

--11 UnitEquivalence
INSERT INTO [PosDb].[dbo].[UnitEquivalence]([LowerUnitAbbr],[HigherUnitAbbr],[EquivalenceFactor]) VALUES ('UND', 'UND', 1)
INSERT INTO [PosDb].[dbo].[UnitEquivalence]([LowerUnitAbbr],[HigherUnitAbbr],[EquivalenceFactor]) VALUES ('UND', 'DOC', 12)
INSERT INTO [PosDb].[dbo].[UnitEquivalence]([LowerUnitAbbr],[HigherUnitAbbr],[EquivalenceFactor]) VALUES ('UND', 'CTO', 100)
INSERT INTO [PosDb].[dbo].[UnitEquivalence]([LowerUnitAbbr],[HigherUnitAbbr],[EquivalenceFactor]) VALUES ('UND', 'MLL', 1000)
INSERT INTO [PosDb].[dbo].[UnitEquivalence]([LowerUnitAbbr],[HigherUnitAbbr],[EquivalenceFactor]) VALUES ('CTO', 'MLL', 10)


--12 MiscVariant
insert into GeneralConfigValues ([Name], [Value]) VALUES ('GrupoBoleta', '001')
insert into GeneralConfigValues ([Name], [Value]) VALUES ('UlitmoNroBoleta', '1')
insert into GeneralConfigValues ([Name], [Value]) VALUES ('GrupoFactura', '001')
insert into GeneralConfigValues ([Name], [Value]) VALUES ('UlitmoNroFactura', '1')