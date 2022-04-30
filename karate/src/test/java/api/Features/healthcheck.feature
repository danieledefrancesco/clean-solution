Feature: As a WebAPI user, when requesting /healthcheck, I should be able to know whether the application is healthy


  Scenario: Requesting /healthcheck should return a 200 successful response.
    
    Given url baseUrl + '/healthcheck'
    When method GET
    Then status 200
    * match $.success == true