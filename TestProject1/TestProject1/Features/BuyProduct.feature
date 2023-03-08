Feature: Buy a product
	In order to be able to aquire the products they want
	the customers want to be able to buy products and use coupon codes

Background:
	Given I am logged in

Scenario Outline: Apply a coupon to a product
	When I add '<product>' to my cart
	And I apply the coupon 'edgewords'
	Then A discount of '15%' should be applied

Examples:
	| product |
	| Belt    |
	#| Polo    |

Scenario Outline: Buy a product
	When I add '<product>' to my cart
	And I checkout using valid information
	Then The order number presented should match the order in my account

Examples:
	| product |
	| Belt    |
	#| Polo    |
