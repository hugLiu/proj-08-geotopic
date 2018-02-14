create PROCEDURE [dbo].[usp_Pagination]
/**//*
***************************************************************
** 千万数量级分页存储过程 **
***************************************************************
参数说明:
.Tables :表名称,视图(试图这边目前还有点小问题)
.PrimaryKey :主关键字
.Sort :排序语句，不带Order By 比如：NewsID Desc,OrderRows Asc
.CurrentPage :当前页码
.PageSize :分页尺寸
.Filter :过滤语句，不带Where 
.Group :Group语句,不带Group By
***************************************************************/
(
@Tables varchar(2000),
@PrimaryKey varchar(500),
@Sort varchar(500) = NULL,
@CurrentPage int = 1,
@PageSize int = 5,
@Fields varchar(2000) = '*',
@Filter varchar(1000) = NULL,
@Group varchar(1000) = NULL
)
AS
/**//*默认排序*/
IF @Sort IS NULL OR @Sort = ''
SET @Sort = @PrimaryKey
DECLARE @SortTable varchar(1000)
DECLARE @SortName varchar(1000)
DECLARE @strSortColumn varchar(1000)
DECLARE @operator char(2)
DECLARE @type varchar(1000)
DECLARE @prec int
/**//*设定排序语句.*/
IF CHARINDEX('DESC',@Sort)>0
BEGIN
SET @strSortColumn = REPLACE(@Sort, 'DESC', '')
SET @operator = '<='
END
ELSE
BEGIN
IF CHARINDEX('ASC', @Sort) = 0
print '1'
print REPLACE(@Sort, 'ASC', '')
SET @strSortColumn = REPLACE(@Sort, 'ASC', '')
print @strSortColumn
SET @operator = '>='
print @operator
END
IF CHARINDEX('.', @strSortColumn) > 0
BEGIN
SET @SortTable = SUBSTRING(@strSortColumn, 0, CHARINDEX('.',@strSortColumn))
SET @SortName = SUBSTRING(@strSortColumn, CHARINDEX('.',@strSortColumn) + 1, LEN(@strSortColumn))
END
ELSE
BEGIN
SET @SortTable = @Tables
SET @SortName = @strSortColumn
print @SortTable
print @SortName
END
SELECT @type=t.name, @prec=c.prec
FROM sysobjects o 
JOIN syscolumns c on o.id=c.id
JOIN systypes t on c.xusertype=t.xusertype
WHERE o.name = @SortTable AND c.name = @SortName
--print @type
--print @prec
IF CHARINDEX('char', @type) > 0
SET @type = @type + '(' + CAST(@prec AS varchar) + ')'
DECLARE @strPageSize varchar(500)
DECLARE @strStartRow varchar(500)
DECLARE @strFilter varchar(1000)
DECLARE @strSimpleFilter varchar(1000)
DECLARE @strGroup varchar(1000)
/**//*默认当前页*/
IF @CurrentPage < 1
SET @CurrentPage = 1
/**//*设置分页参数.*/
SET @strPageSize = CAST(@PageSize AS varchar(500))
SET @strStartRow = CAST(((@CurrentPage - 1)*@PageSize + 1) AS varchar(500))
/**//*筛选以及分组语句.*/
IF @Filter IS NOT NULL AND @Filter != ''
BEGIN
SET @strFilter = ' WHERE ' + @Filter + ' '
SET @strSimpleFilter = ' AND ' + @Filter + ' '
END
ELSE
BEGIN
SET @strSimpleFilter = ''
SET @strFilter = ''
END
IF @Group IS NOT NULL AND @Group != ''
SET @strGroup = ' GROUP BY ' + @Group + ' '
ELSE
SET @strGroup = ''
/*print @type
print @strStartRow
print @strSortColumn
print @Tables
print @strFilter
print @strGroup
print @Sort*/
/**//*执行查询语句*/
EXEC(
'
DECLARE @SortColumn ' + @type + '
SET ROWCOUNT ' + @strStartRow + '
SELECT @SortColumn=' + @strSortColumn + ' FROM ' + @Tables + @strFilter + ' ' + @strGroup + ' ORDER BY ' + @Sort + '
SET ROWCOUNT ' + @strPageSize + '
SELECT ' + @Fields + ' FROM ' + @Tables + ' WHERE ' + @strSortColumn + @operator + ' @SortColumn ' + @strSimpleFilter + ' ' + @strGroup + ' ORDER BY ' + @Sort + '
'
)