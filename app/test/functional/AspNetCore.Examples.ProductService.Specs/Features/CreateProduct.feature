Feature: CreateProduct
API for creating the products

Link to a feature: [ProductService]($projectname$/Features/CreateProduct.feature)

    Scenario: Making a POST request to the /products endpoint with a valid CreateProductRequest body should successfully create the product
        Given a create product request <product-id, product-name, 1>
        When I make a POST request to the /products endpoint
        Then the response status code is 200
        And the product id is product-id
        And the product name is product-name
        And the product price is 1
        And the product has been successfully created in the database

    Scenario: Making a POST request to /products with an existing id should return a 409 response
        Given a product <product-id, product-name, 1>
        And a create product request <product-id, product-name, 1>
        When I make a POST request to the /products endpoint
        Then the response status code is 409

    Scenario Outline: Making a POST or PUT request to /products with an invalid name should return a 400 response
        Given a create product request <<id>, <name>, 1>
        When I make a POST request to the /products endpoint
        Then the response status code is 400

        Examples:
          | id | name |
          | b1 |      |

    Scenario Outline: Making a POST or PUT request to /products with an invalid id should return a 400 response
        Given a create product request <<id>, <name>, 1>
        When I make a POST request to the /products endpoint
        Then the response status code is 400

        Examples:
          | id   | name |
          |      | Name |
          | !-@  | Name |
          | A!-@ | Name |
          | 1ABC | Name |
          | -ABC | Name |
          | _ABC | Name |