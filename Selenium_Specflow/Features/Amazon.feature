Feature: AmazonTests
	

@test123
Scenario: AM01)_Amazon Select item and  hover over image and get price
	Given I navigate to URL 'https://www.amazon.in/'
	And I enter search word in Search field
	Then I select the Item
	Then I select item color and size
	Then I hover over the images
