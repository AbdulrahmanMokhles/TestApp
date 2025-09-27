Select * from MacAddresses;
Delete from MacAddresses;
truncate table MacAddresses;

CREATE TYPE MacAddressTempTableType AS TABLE
(
    Mac NVARCHAR(17) NOT NULL,
    ProductId INT NOT NULL,
	PRIMARY KEY (Mac)
);

CREATE PROC InsertIgnore
AS
BEGIN
	INSERT IGNORE INTO dbo.MacAddresses (Mac , ProductId)
	SELECT T.Mac , T.ProductId
	FROM MacAddressTempTableType T
	LEFT JOIN MacAddresses M ON M.Mac = T.Mac
	WHERE M.Mac IS NULL;
END;

CREATE PROC AddBulkMacs 
	@toAddMacs MacAddressTempTableType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	MERGE INTO dbo.MacAddresses AS Dest
	USING @toAddMacs AS Src
	ON Dest.Mac = Src.Mac
	WHEN NOT MATCHED THEN
		INSERT (Mac , ProductId) VALUES (Src.Mac , Src.ProductID);
END;

exec AddBulkMacs

