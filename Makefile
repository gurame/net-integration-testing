build:
	dotnet build IntegrationTesting.sln

run-api:
	dotnet run --project ./src/Customers.Api/Customers.Api.csproj

test-api:
	dotnet test ./tests/Customers.Api.Tests.Integration/Customers.Api.Tests.Integration.csproj

run-webapp:
	dotnet run --project ./src/Customers.WebApp/Customers.WebApp.csproj

up:
	docker-compose up -d

down:
	docker-compose down