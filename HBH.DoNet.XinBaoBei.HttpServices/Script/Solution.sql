


select 
	messDate as CreatedTime
    ,aboutAge as Age
    ,answer.sub_qes_t as QuestionID
    ,keywords as KeyWords
    ,user.id as UserID
    ,user.Account as UserAccount
    ,solution.SText as Solution
    
    ,answer.*
from HBH_SubQuestion answer
	inner join hbh_user user
    on answer.questioner = user.id
    left join t_solution solution
    on answer.sub_qes_t = solution.Question
    
	
    
where
	user.id = 3
    
    