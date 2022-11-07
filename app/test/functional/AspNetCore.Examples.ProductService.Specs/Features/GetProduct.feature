Feature: GetProduct
API for querying the products

Link to a feature: [ProductService]($projectname$/Features/GetProduct.feature)

Scenario: Requesting /products/{id} will produce a 200 successful response along with the product's data if the product exists
	Given a product <product-id, product-name, 11.50>
	When I make a GET request to the /products/product-id endpoint
	Then the response status code is 200
	And the product id is product-id
	And the product name is product-name
	And the product price is 11.50
	
Scenario Outline: Requesting /products/{id} with an invalid product id should return a 400 bad response with the explanation of the error.
	When I make a GET request to the /products/<id> endpoint
	Then the response status code is 400
    
	Examples:
	  | id   |
	  | 1    |
	  | 1abc |
	  | -a   |
	  | _a   |
   
Scenario: Requesting /products/{id} will produce a 404 error response if the product doesn't exist
	When I make a GET request to the /products/product-id endpoint
	Then the response status code is 404
