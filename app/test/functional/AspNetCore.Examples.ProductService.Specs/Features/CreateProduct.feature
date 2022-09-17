Feature: CreateProduct
API for creating the products

Link to a feature: [ProductService]($projectname$/Features/CreateProduct.feature)

    Scenario Outline: Making a POST ot PUT request to the /products endpoint with a valid CreateProductRequest body should successfully create the product
        Given a create product request <product-id, product-name, 1>
        When I make a <method> request to the /products endpoint
        Then the response status code is 201
        And the product id is product-id
        And the product name is product-name
        And the product price is 1
        And the product has been successfully created in the database

        Examples:
          | method |
          | POST   |
          | PUT    |

    Scenario Outline: Creating a new product should raise an OnProductCreated domain event
        Given a create product request <product-id, product-name, 1>
        When I make a <method> request to the /products endpoint
        Then the response status code is 201
        And the product id is product-id
        And the product name is product-name
        And the product price is 1
        And the OnProductCreatedEvent is created in the queue

        Examples:
          | method |
          | POST   |
          | PUT    |

    Scenario Outline: Making a POST request to /products with an existing id should return a 409 response
        Given a product <product-id, product-name, 1>
        And a create product request <product-id, product-name, 1>
        When I make a <method> request to the /products endpoint
        Then the response status code is 409

        Examples:
          | method |
          | POST   |
          | PUT    |

    Scenario Outline: Making a POST or PUT request to /products with an invalid name should return a 400 response
        Given a create product request <<id>, <name>, 1>
        When I make a <method> request to the /products endpoint
        Then the response status code is 400

        Examples:
          | id | name | method |
          | b1 |      | POST   |
          | b1 |      | PUT    |

    Scenario Outline: Making a POST or PUT request to /products with an invalid id should return a 400 response
        Given a create product request <<id>, <name>, 1>
        When I make a <method> request to the /products endpoint
        Then the response status code is 400

        Examples:
          | id   | name | method |
          |      | Name | POST   |
          |      | Name | PUT    |
          | !-@  | Name | POST   |
          | !-@  | Name | PUT    |
          | A!-@ | Name | POST   |
          | A!-@ | Name | PUT    |
          | 1ABC | Name | POST   |
          | 1ABC | Name | PUT    |
          | -ABC | Name | POST   |
          | -ABC | Name | PUT    |
          | _ABC | Name | POST   |
          | _ABC | Name | PUT    |