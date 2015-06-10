
drop trigger hbh_message_trigger_update;
delimiter $
create trigger hbh_message_trigger_update
before update on hbh_message
for each row 

begin
	
		-- 更新 年月周日
		
	set new.aboutAgeBegin_string = Replace(Replace(Replace(Replace(new.aboutAgeBegin,'岁',','),'个月',','),'天',','),'周第',',')
		,new.aboutAgeEnd_string = Replace(Replace(Replace(Replace(new.aboutAgeEnd,'岁',','),'个月',','),'天',','),'周第',',')
		
    ;


	set 
		new.aboutAgeBegin_age = substring_index(new.aboutAgeBegin_string,',',1)
		,new.aboutAgeBegin_month = substring_index(substring_index(new.aboutAgeBegin_string,',',2),',',-1)
		,new.aboutAgeBegin_week = substring_index(substring_index(new.aboutAgeBegin_string,',',3),',',-1)
		,new.aboutAgeBegin_day = substring_index(substring_index(new.aboutAgeBegin_string,',',4),',',-1)
		
		,new.aboutAgeEnd_age = substring_index(new.aboutAgeEnd_string,',',1)
		,new.aboutAgeEnd_month = substring_index(substring_index(new.aboutAgeEnd_string,',',2),',',-1)
		,new.aboutAgeEnd_week = substring_index(substring_index(new.aboutAgeEnd_string,',',3),',',-1)
		,new.aboutAgeEnd_day = substring_index(substring_index(new.aboutAgeEnd_string,',',4),',',-1)

    ;
		
end$