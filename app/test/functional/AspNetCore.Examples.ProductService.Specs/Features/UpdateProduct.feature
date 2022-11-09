Feature: UpdateProduct
API for creating the products

Link to a feature: [ProductService]($projectname$/Features/UpdateProduct.feature)

    Scenario: Making a PATCH request to the /products/{id} endpoint with a valid UpdateProductRequest body should successfully update the product
        Given a product <product-id, product-name, 1>
        And an update product request <new-product-name, 10>
        When I make a PATCH request to the /products/product-id endpoint
        Then the response status code is 200
        And the product id is product-id
        And the product name is new-product-name
        And the product price is 10
        And the product product-id has been successfully updated in the database
        And an OnProductUpdatedEvent is created in the queue for product product-id
    
    Scenario Outline: Making a PATCH /products/{id} with an invalid product id should return a 400 bad response with the explanation of the error.
        Given an update product request <name, 1>
        When I make a PATCH request to the /products/<id> endpoint
        Then the response status code is 400

        Examples:
          | id   |
          | 1    |
          | 1abc |
          | -a   |
          | _a   |

    Scenario: Requesting /products/{id} will produce a 404 error response if the product doesn't exist
        Given an update product request <name, 1>
        When I make a PATCH request to the /products/product-id endpoint
        Then the response status code is 404