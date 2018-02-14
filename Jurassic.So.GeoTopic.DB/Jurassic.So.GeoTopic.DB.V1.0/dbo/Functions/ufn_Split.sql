
-- =============================================
-- Author:		CHENGKUN
-- Create date: 2015-6-30
-- Description:	将字符串@str用分隔符@split进行切割，返回table
-- =============================================
CREATE FUNCTION ufn_Split
(
	@str nvarchar(max),
	@split nchar(1)
)
RETURNS 
@t TABLE 
(
	Item nvarchar(100)
)
AS
BEGIN

	set @str = '<Root><Item>' + REPLACE(@str, @split, '</Item><Item>') + '</Item></Root>'

	declare @x xml
	set @x = convert(xml,@str)

	insert into @t
	select x.item.value('.[1]', 'nvarchar(100)') from @x.nodes('//Root/Item') as x(item)
	
	return
END
