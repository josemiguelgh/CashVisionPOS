--Dev procedures

USE [PosDb]
GO

create procedure devInsertProduct
(
    @ProductName varchar(100),
    @ProductDescription varchar(200),
    @SaleDefaultAbbr varchar(20),
    @StockDefaultAbbr varchar(20),
    @PremiseId tinyint,
    @CurrentStock int,
    @MinimunStock int,
    @StockDefaultUnitAbbr varchar(20)    
)
as

begin transaction

begin try
INSERT INTO [PosDb].[dbo].[ProductBase]
           ([Name]
           ,[Description]
           ,[SaleDefaultUnitAbbr])
     VALUES
           (@ProductName,
			@ProductDescription,
			@SaleDefaultAbbr)

INSERT INTO [PosDb].[dbo].[Product]
           ([ProductBaseId]
           ,[MiscVariantId])
     VALUES
           (scope_identity(),
           null)

declare @ProductId int;
set @ProductId = scope_identity();

INSERT INTO [PosDb].[dbo].[ProductPremise]
           ([ProductId]
           ,[PremiseId]
           ,[CurrentStock]
           ,[MinimunStock]
           ,[StockDefaultUnitAbbr])
     VALUES
           (@ProductId,
           @PremiseId,
           @CurrentStock,
           @MinimunStock,
           @StockDefaultUnitAbbr)

declare @saleprice decimal = round((1000 - 2 - 1)*rand() + 2,0);
INSERT INTO [PosDb].[dbo].[ProductUnit]
           ([ProductId]
           ,[UnitId]
           ,[CostPrice]
           ,[SalePrice])
     VALUES
           (@ProductId,
           1,
           50,
           @saleprice)
           
declare @saleprice2 decimal =  round((1000 - 2 - 1)*rand() + 2,0);
INSERT INTO [PosDb].[dbo].[ProductUnit]
           ([ProductId]
           ,[UnitId]
           ,[CostPrice]
           ,[SalePrice])
     VALUES
           (@ProductId,
           2,
           100,
           @saleprice2)
end try        
begin catch
	IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
end catch;
IF @@TRANCOUNT > 0
        commit TRANSACTION;
GO


exec devInsertProduct 'Demo1','Demo1','UND', 'DOC', 1, 100,20,'UND';
exec devInsertProduct 'Demo2','Demo2','UND', 'DOC', 1, 100,20,'UND';
exec devInsertProduct 'Demo3','Demo3','UND', 'DOC', 1, 100,20,'UND';