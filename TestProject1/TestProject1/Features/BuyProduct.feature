Feature: Buy a product
	In order to allow the website to make money
	the CFO wants users to be able to buy products

Background:
	Given I am logged in

Scenario Outline: Apply a coupon to a product
	When I add '<product>' to my cart
	And I apply the coupon 'edgewords'
	Then A discount of '15%' should be applied

Examples:
	| product |
	| Belt    |

Scenario Outline: Buy a product
	When I add '<product>' to my cart
	And I checkout using valid information
	Then The order number presented should match the order in my account

Examples:
	| product |
	| Belt    |
