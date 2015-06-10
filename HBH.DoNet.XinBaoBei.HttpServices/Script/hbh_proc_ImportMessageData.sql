CREATE DEFINER=``@`` PROCEDURE `hbh_proc_ImportMessageData`(
	in pi_GUID varchar(125)
)
begin



	-- 閸掓稑缂撻棁鈧弴瀛樻煀   濞戝牊浼? 閻ㄥ嫪澶嶉弮鎯般€?
	drop table if exists tmp_NeedUpdate_Message  ;
	create table tmp_NeedUpdate_Message (
	select hbh_Message.ID,hbh_T_ImportData.message_Title,hbh_T_ImportData.aboutAgebegin
  ,hbh_T_ImportData.aboutAgeEnd,hbh_t_importData.message_Content
			,hbh_t_importData.messDate
	from hbh_Message,hbh_t_importData
	where hbh_t_importData.GUID = pi_GUID
		and hbh_message.aboutAgeBegin = hbh_t_importData.aboutAgeBegin
   and hbh_message.aboutAgeEnd =hbh_t_importData.aboutAgeEnd
		and (hbh_message.message_Title != hbh_t_importData.message_title
			or hbh_message.message_Content != hbh_t_importData.message_Content
			
			)
		)
	;
	-- 濞戝牊浼呴弴瀛樻煀
	update hbh_message,tmp_NeedUpdate_Message imp2
	set
		hbh_message.aboutAgeBegin = trim(imp2.aboutAgeBegin)
		,hbh_message.message_Title = imp2.message_Title
		,hbh_message.message_content = imp2.message_Content
		,hbh_message.aboutAgeEnd = imp2.aboutAgeEnd
		,hbh_message.messDate = imp2.messDate
   
	where hbh_message.aboutAgeBegin = imp2.aboutAgeBegin and hbh_message.aboutAgeEnd = imp2.aboutAgeEnd
	;
	drop table if exists tmp_NeedUpdate_Message
	;

	-- 濞戝牊浼?
	insert into hbh_Message
	(
		message_Title,message_Content,aboutAgeBegin,aboutAgeEnd,messDate
	)
	select 
	  message_title,message_Content,aboutAgeBegin,aboutAgeEnd,messDate
	from hbh_t_importData
	where GUID = pi_GUID
		and aboutageBegin not in(select tb.aboutAgeBegin from hbh_message tb)
		and aboutageEnd not in(select tb.aboutAgeEnd from hbh_message tb)
   and message_title !='' and message_content != ''

	;

/*	-- 已做成触发器，以使 前台修改、新增，导入，都可以触发
	-- 更新 年月周日
    
update hbh_Message
set aboutAgeBegin_string = Replace(Replace(Replace(Replace(aboutAgeBegin,'岁',','),'个月',','),'天',','),'周第',',')
	,aboutAgeEnd_string = Replace(Replace(Replace(Replace(aboutAgeEnd,'岁',','),'个月',','),'天',','),'周第',',')
;


update hbh_Message
set 
    aboutAgeBegin_age = substring_index(aboutAgeBegin_string,',',1)
    ,aboutAgeBegin_month = substring_index(substring_index(aboutAgeBegin_string,',',2),',',-1)
    ,aboutAgeBegin_week = substring_index(substring_index(aboutAgeBegin_string,',',3),',',-1)
    ,aboutAgeBegin_day = substring_index(substring_index(aboutAgeBegin_string,',',4),',',-1)
    
    ,aboutAgeEnd_age = substring_index(aboutAgeEnd_string,',',1)
    ,aboutAgeEnd_month = substring_index(substring_index(aboutAgeEnd_string,',',2),',',-1)
    ,aboutAgeEnd_week = substring_index(substring_index(aboutAgeEnd_string,',',3),',',-1)
    ,aboutAgeEnd_day = substring_index(substring_index(aboutAgeEnd_string,',',4),',',-1)
;
   */ 




end