Feature: As a WebAPI user, when requesting /products/{id}, I should be able to receive the information of the product matching the passed id.

Background: 
  * call clearProductDatabase()
  
  Scenario Outline: Requesting /products/{id} with an invalid product id should return a 400 bad response with the explanation of the error.
    
    Given url baseUrl + '/products/<id>'
    When method GET
    Then status 400
    #* def id_errors = $.errors.id
    * match $.status == 400
    #* match $.errors.id == ["'Id' is not in the correct format."]
    
    Examples:
      | id      |
      | 1       |
      | 1abc    |
      | -a      |
      | _a      |

  Scenario: Requesting /products/{id} with an non existing product id should return a 404 not found.

    Given url baseUrl + '/products/a3'
    When method GET
    Then status 404
    #* match $.status == 404
    
 Scenario Outline: Requesting /products/{id} with an existing product id should return a successful response with the product information.
    
    * call addProduct('<product_seed>')
    Given url baseUrl + '/products/<id>'
    When method GET
    Then status 200
    * match $.id == '<id>'
    * match $.name == '<name>'
    
    Examples:
      | id  | name      | product_seed    |
      | a1  | Product 1 | product_a1.json |
      | a2  | Product 2 | product_a2.json |