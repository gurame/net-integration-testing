build:
	dotnet build IntegrationTesting.sln

run:
	dotnet run --project ./src/Customers.Api/Customers.Api.csproj

test:
	dotnet test ./tests/Customers.Api.Tests.Integration/Customers.Api.Tests.Integration.csproj

up:
	docker-compose up -d

down:
	docker-compose down