Feature: DropboxTests

@test123
Scenario Outline: DR01)_Dropbox_Upload file
	Given I Launch Dropbox URL <url> 
	And I Login to Dropbox with <username> and <password>
	Then I navigate to Folder 'Recordings'
	Then upload the file 'C:\test\file.txt'

Examples: 
| url                          | username				|	password			|
| https://www.dropbox.com/home | kiranmaroj@gmail.com	|	TestPass1234		|

@test123
Scenario Outline: DR02)_Dropbox_Upload Folder
	Given I Launch Dropbox URL <url> 
	And I Login to Dropbox with <username> and <password>
	Then I navigate to Folder 'Recordings'
	Then upload the folder 'C:\test'

Examples: 
| url                          | username				|	password			|
| https://www.dropbox.com/home | kiranmaroj@gmail.com	|	TestPass1234		|

@test123
Scenario Outline: DR03)_Dropbox_Drag_and_Drop
	Given I Launch Dropbox URL <url> 
	And I Login to Dropbox with <username> and <password>
	Then I navigate to Folder 'Recordings'
	Then Drag the file 'file.txt' and Drop into folder 'ShareX'

Examples: 
| url                          | username				|	password			|
| https://www.dropbox.com/home | kiranmaroj@gmail.com	|	TestPass1234		|


