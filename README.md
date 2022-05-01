# CorlevoAPI
Demo CRUD API for Corlevo. Developed for basic CRUD operations.

Used Technologies:

* .Net Core 6.0
* Entity Framework
* SQLite
* Open API (Swagger)

### How To Run:
The application can be run with the `dotnet run` command.

The API will be served on https://localhost:7220

The API document URL: https://localhost:7220/swagger/index.html


### Endpoints:
* **/Products [GET]:** For get all products
* **/Products/Search [GET]:** For search products with `SearchText`, `MinPrice`, and `MaxPrice` parameters.
* **/Products/{Product ID} [GET]:** For get a product with the ID value.
* **/Products [POST]:** For add new product
* **/Products/{Product ID} [PUT]:** For update an existing product
* **/Products/{Product ID} [DELETE]:** For delete a product with the ID value.
