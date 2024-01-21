insert into menu_T ( menutypeid,shopid,menu,description,priceuponselection,rate,discount,offer)
select * from menu_T where shopid=54


insert into menutype_t (shopid,menutype)   
select 54, menutype from [dbo].[MenuType_T]  where shopid=53

insert into [MenuSubItemGroup_T] (shopid,menusubitemgroup) 
select 54,menusubitemgroup from [dbo].[MenuSubItemGroup_T] where shopid=53

insert into [Menu-MenuSubItemGroup_T] (menuid,)
select * from [dbo].[Menu-MenuSubItemGroup_T] where menusubitemgroupid in (select menusubitemgroupid from [dbo].[MenuSubItemGroup_T] where shopid=54)

select top 10 * from [dbo].[MenuSubItem_T] where menusubitegroupid in (select menusubitemgroup)