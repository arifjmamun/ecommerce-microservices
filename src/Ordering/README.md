# Order Service

UI / Front-end Developer assessment

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