Steps:
1.Clone the project from github link in one folder
2.Run the Project
3.Swagger will open
4.Hit the api multiple times 
	i.After 5 attempts it will get 429 Error
	ii.For Checking multiple users
	iii.In the postman add the request and Add the header Key="X-Api-Key" and Value=User1 and hit more than 5 times
	iv. Similarly add another request and add for another user Key="X-Api-Key" and Value=User2 and hit more than 5 times
	
I have configured "request per minute" to 5 and "segments per minute" to 6
	
