


declare @ID bigint
declare @Application bigint
declare @Code varchar(125)
declare @Name varchar(125)
declare @Group varchar(125)
declare @ProfileValueType int
declare @Type varchar(125)
declare @DefaultValue varchar(125)


set @ID = 9009201612150001
--	CBO		
set @Application = 3000
set @Code = 'IsUsedDMSAPI'
set @Name = '启用DMS接口'
set @Group = 'DMS接口'
-- 字符串,0		bool,3	
set @ProfileValueType = 3
set @Type = 'bool'
set @DefaultValue = 'true'

--delete from Base_Profile_Trl where ID = @ID
--delete from Base_Profile where ID = @ID
if not exists(select 1 from Base_Profile where Code = @Code)
begin
	insert into Base_Profile
	(ID,SysVersion,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy
	,Code,ShortName,ProfileValueType,SubTypeName,DefaultValue
	,Application,ControlScope,SensitiveType,Sort
	,ValidateSV,CanBeUpdatedSV,UpdatedProcessSV,ReferenceID,Hidden,ShowPecent,IsSend,IsModify
	)values(
	@ID,1,'2015-09-17','hbh','2015-09-17','hbh'
	,@Code,@Name,@ProfileValueType,@Type,@DefaultValue
	,@Application,1,0,0
	,null,null,null,null,0,0,0,0
	)

	insert into Base_Profile_Trl
	(ID,SysMLFlag,ProfileGroup,Name,Description
	)values(
	@ID,'zh-CN',@Group,@Name,@Name
	)
end ;

--select ProfileValueType,SubTypeName
--from Base_Profile

