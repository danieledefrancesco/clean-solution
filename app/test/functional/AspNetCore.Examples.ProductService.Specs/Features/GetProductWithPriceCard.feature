Feature: GetProductWithPriceCard
API for querying the products with their price cards

Link to a feature: [ProductService]($projectname$/Features/GetProductWithPriceCard.feature)

Scenario: Requesting /products/{id}/with-price-card will produce a 200 successful response along with the product's data if the product exists
	Given a product <product-id, product-name, 11.50>
	When I make a GET request to the /products/product-id/with-price-card endpoint
	Then the response status code is 200
	And the product doesn't have a price card
	And the product with price card id is product-id
	And the product with price card name is product-name
	And the product with price card price is 11.50
	And the product with price card final price is 11.50
	
Scenario: When an active price card exists for a given product its final price will be overriden with the price card new price
	Given a product <product-id, product-name, 11.50>
	And a price card <price-card-id, product-id, price-card-name, 9, 2020-01-01, 2120-01-01>
	When I make a GET request to the /products/product-id/with-price-card endpoint
	Then the response status code is 200
	And the product has a price card
	And the product with price card id is product-id
	And the product with price card name is product-name
	And the product with price card price is 11.50
	And the product with price card final price is 9
	
Scenario: If the price card associated with a product has a negative new price then a 422 error response will be returned
	Given a product <product-id, product-name, 11.50>
	And a price card <price-card-id, product-id, price-card-name, -1, 2020-01-01, 2120-01-01>
	When I make a GET request to the /products/product-id/with-price-card endpoint
	Then the response status code is 422
	
Scenario Outline: Requesting /products/{id}/with-price-card with an invalid product id should return a 400 bad response with the explanation of the error.
	When I make a GET request to the /products/<id>/with-price-card endpoint
	Then the response status code is 400
    
	Examples:
	  | id   |
	  | 1    |
	  | 1abc |
	  | -a   |
	  | _a   |
   
Scenario: Requesting /products/{id}/with-price-card will produce a 404 error response if the product doesn't exist
	When I make a GET request to the /products/product-id/with-price-card endpoint
	Then the response status code is 404
