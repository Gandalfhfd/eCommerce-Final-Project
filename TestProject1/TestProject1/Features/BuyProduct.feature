Feature: Buy a product
	In order to allow the website to make money
	the CFO wants users to be able to buy products

Background:
	Given I am logged in

Scenario: Apply a coupon to a product
	When I add an item to my cart
	And I apply a valid coupon
	Then The appropriate discount should be applied

Scenario: Buy a product
	When I add an item to my cart
	And I checkout using valid information
	Then The order number presented should match the order in my account
