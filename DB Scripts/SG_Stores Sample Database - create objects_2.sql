
GO
ALTER TABLE [production].[stocks]
ADD vendor_id int;
GO
ALTER TABLE [production].[stocks]
ADD quantity_kg decimal(10,2);
GO

ALTER TABLE [production].[stocks]
ADD MRP decimal(10,2)
GO
ALTER TABLE [production].[stocks]
ADD purchase_price decimal(10,2)
GO
ALTER TABLE [production].[stocks]
ADD total_bill_amount decimal(10,2)
GO
ALTER TABLE [production].[stocks]
ADD is_per_kg_item BIT
GO
ALTER TABLE [production].[stocks]
ADD is_active BIT
GO
ALTER TABLE [production].[stocks]
ADD added_on datetime
GO

ALTER TABLE [production].[stocks]
ADD CONSTRAINT fk_vendor_id
FOREIGN KEY (vendor_id) REFERENCES [production].[vendors];
GO
/*
ALTER TABLE [production].[products]
ADD MRP decimal(10,2)
GO
ALTER TABLE [production].[products]
ADD purchase_price decimal(10,2)
GO
ALTER TABLE [production].[products]
ADD is_per_kg_item BIT
GO
*/
ALTER TABLE [production].[products]
ADD is_active BIT
GO
ALTER TABLE [production].[products]
ADD added_on datetime
GO


ALTER TABLE [sales].[stores]
ALTER COLUMN zip_code varchar(6)
GO

GO
ALTER TABLE production.stocks
ADD sale_discount_percentage decimal(10,2)
GO
ALTER TABLE production.stocks
ADD sale_price decimal(10,2)
GO
ALTER TABLE production.stocks
ADD item_code NVARCHAR(20)
GO
ALTER TABLE production.stocks
ADD item_barcode NVARCHAR(500)
GO
ALTER TABLE production.stocks
ADD sold_quantity int
GO
ALTER TABLE production.stocks
ADD sold_quantity_kg int
GO
ALTER TABLE [production].[products]
DROP COLUMN MODEL_YEAR 
GO

ALTER TABLE [sales].[order_items]
ADD quantity_kg decimal(10,2);


GO
ALTER TABLE sales.orders
ADD sub_total decimal(18,2);
GO
ALTER TABLE sales.orders
ADD discount decimal(18,2);
GO
ALTER TABLE sales.orders
ADD total_tax decimal(18,2);
GO
ALTER TABLE sales.orders
ADD grand_total decimal(18,2);
GO
ALTER TABLE sales.order_items
ADD total_amount decimal(18,2);

GO
