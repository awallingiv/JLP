--Create procedure ReadTable
--	AS
--BEGIN
--	SELECT * FROM Widget Order by WidgetID;
--END;
Exec ReadTable;

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
--		InventoryCode = ISNULL(@ICode, InventoryCode),
--		Description = @Desc,
--		QuantityOnHand = ISNULL(@QoH, QuantityOnHand),
--		ReorderQuantity = @ReorderQ
--	WHERE WidgetID = @WID;
--END;

EXEC UpdateWidget @WID = 6, @ICode = 'InvCode6', @Desc = 'Desc6', @QoH = 78, @ReorderQ = 77


Insert into widget (InventoryCode, Description, QuantityOnHand, ReorderQuantity)values ('InvCode4', 'Desc4', 6, 7);


