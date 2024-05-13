# Building an URL Shortener Web API using Asp.Net Core 8

a project that takes a normal URL from the input and  returns the shortened form of that link.
# How to Use?
**.** first download the project and open it with vscode or any ide.<br>
**.** build the project and it will open a swager page in your default browser.<br>
**.** Try out the Post api for creating a new shortened link.<br>
**.** copy your URL in the input and the program returns the shortened form of your link.
# How it Works?
when you copy your URL in the input, the program first does a small search in the database to ensure that your URL is not already saved
in DB<br> after that by calling the ShortenedGenerator, generate the shortened form and in the end, Redirects the shortened url to original<br>
then returns it to the output. 
# Project Technologies
. Asp.Net Core 8<br>
. MVC Structure<br>
. Sql local database file<br>
. Database Migrations

# Additional Notes
for database i used a sql server db local file which is easy to make.<br>
 just open the SSMS, Create an empty database then Detach the database file and put it in the root directory of your project.<br>
for connecting a database local file to your program and configure your connection string should do some search in the internet.