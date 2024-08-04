build:
	dotnet build IntegrationTesting.sln

run:
	dotnet run --project ./src/Customers.Api/Customers.Api.csproj

up:
	docker-compose up -d

down:
	docker-compose down