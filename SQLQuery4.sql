--Create procedure ReadTable
--	AS
--BEGIN
--	SELECT * FROM Widget Order by WidgetID;
--END;
--Exec ReadTable;

--Create procedure DeleteWidget
--	@WID INT
--As
--Begin
--	Delete FROM Widget WHERE WidgetID = @WID
--End;
--Exec DeleteWidget @WID = 1;

--CREATE PROCEDURE UpdateWidget
--	@WID INT,
--	@ICode NVARCHAR(50),
--	@Desc NVARCHAR(max) = NULL,
--	@QoH Int,
--	@ReorderQ Int = NULL
--AS
--BEGIN
--	UPDATE Widget
--	SET
--		InventoryCode = ISNULL(@ICode, InventoryCode),			--Validating null here but will also check in code
--		Description = @Desc,
--		QuantityOnHand = ISNULL(@QoH, QuantityOnHand),
--		ReorderQuantity = @ReorderQ
--	WHERE WidgetID = @WID;
--END;

CREATE PROCEDURE NewWidget	
	@ICode NVARCHAR(50),
	@Desc NVARCHAR(max) = NULL,
	@QoH Int,
	@ReorderQ Int = NULL
AS
BEGIN			
		INSERT INTO Widget(InventoryCode, Description, QuantityOnHand, ReorderQuantity)
		Values(@Icode, @Desc, @QoH, @ReorderQ)	
END;




