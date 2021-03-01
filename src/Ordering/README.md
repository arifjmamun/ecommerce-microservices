# Order Service

### Perform Migration
###### Note: Run the migration command from 'EcommerceMicroservices/src/Ordering/Ordering.Infrastructure'

#### To add a new migration run the following command
```sh
$ dotnet ef migrations add <MigrationName> --startup-project ../Ordering.API
```
#### To update the database
```sh
$ dotnet ef database update --startup-project ../Ordering.API
```
#### To remove the last migration
```sh
$ dotnet ef ef migrations remove --startup-project ../Ordering.API
```
#### Some docker command for development
docker run -d -p 27017:27017 --name mongo mongo
docker run -d --hostname my-rabbit --name some-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
docker run -p 6379:6379  --name some-redis -d redis
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=sa@123456' -p 1433:1433 -d mcr.microsoft.com/mssql/server