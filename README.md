# NBPWeb

## Project Description
NBPWeb is a web API that provides exchange rate information from the National Bank of Poland (NBP). The API allows users to retrieve current exchange rates for various currencies.

## Technologies Used
- **ASP.NET Core**: The framework used to build the web API.
- **MemoryCache**: For caching exchange rate data to improve performance and reduce the number of requests to the NBP API.
- **Rate Limiting**: To control the number of requests to the API and prevent abuse.
- **Dependency Injection**: For managing dependencies and promoting a modular architecture.

## Endpoints
- `GET /api/nbp/exchange-rates`: Retrieves a list of current exchange rates.
- `GET /api/nbp/exchange-rate/{currencyCode}`: Retrieves the current exchange rate for a specific currency.

## Getting Started
To run the project locally, follow these steps:
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Build the solution to restore dependencies.
4. Run the project using `Ctrl + F5` or the `dotnet run` command.

## Usage
You can test the API endpoints using tools like Postman or by using the provided HTTP request file `NBPWebAPI.http`.