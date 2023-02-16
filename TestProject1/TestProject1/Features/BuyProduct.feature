Feature: Buy a product
	In order to allow the website to make money
	the CFO wants users to be able to buy products

Scenario: Add product to cart
Given I am on the shop page
When I add a product to my cart
Then The product should be in my cart

Scenario: Apply coupon code
Given I am on the checkout page
And I have at least one product in my cart
When I enter a valid coupon code
Then The appropriate discount should be applied

Scenario: Log into website
Given I am a user with an account
When I enter my valid logon details
And I submit the details
Then I will be logged int
