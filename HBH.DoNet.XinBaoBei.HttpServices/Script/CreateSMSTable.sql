


create table hbh_SMS (
	ID bigint not null AUTO_INCREMENT
    ,CreatedOn timestamp default current_timestamp
    ,CreatedBy varchar(125)
    ,ModifiedOn datetime
    ,ModifiedBy varchar(125)
    
    ,PhoneNumber varchar(125)
    ,IdentifyCode varchar(125)
    ,SMSUrl varchar(125)
    ,SMSMessage varchar(1000)
    ,PostData varchar(1500)
    ,Result varchar(125)
    
	,primary key (id)
)
