Feature: As a WebAPI user, I should be able to create a product making a POST or PUT request to /products

  Background:
    * call clearProductDatabase()
    
  Scenario Outline: Making a POST or PUT request to /products with valid data should successfully create a product
    Given url baseUrl + '/products'
    And request { "Id" : "<id>", "Name" : "<name>" }
    When method <method>
    Then status 200
    * match getDbProductCount('<id>') == '1'
    
    Examples:
      | id | name      | method |
      | b1 | Product 1 | POST   |
      | b1 | Product 1 | PUT    |

  Scenario Outline: Making a POST or PUT request to /products with an existing id should return a 409 response
    * call addProduct('<product_seed>')
    Given url baseUrl + '/products'
    And request { "Id" : "<id>", "Name" : "<name>" }
    When method <method>
    Then status 409

    Examples:
      | id | name      | method | product_seed    |
      | a1 | Product 1 | POST   | product_a1.json |
      | a1 | Product 1 | PUT    | product_a1.json |

  Scenario Outline: Making a POST or PUT request to /products with an invalid name should return a 400 response
    Given url baseUrl + '/products'
    And request { "Id" : "<id>", "Name" : <name> }
    When method POST
    Then status 400

    Examples:
      | id | name  | 
      | b1 | ""    |
      | b1 | " "   |
      | b1 | null  |

  Scenario Outline: Making a POST or PUT request to /products with an invalid id should return a 400 response
    Given url baseUrl + '/products'
    And request { "Id" : <id>, "Name" : <name> }
    When method POST
    Then status 400

    Examples:
      | id       | name   |
      | ""       | "Name" |
      | null     | "Name" |
      | " "      | "Name" |
      | "!-@"    | "Name" |
      | "A!-@"   | "Name" |
      | "1ABC"   | "Name" |
      | "-ABC"   | "Name" |
      | "_ABC"   | "Name" |
      | " ABC"   | "Name" |