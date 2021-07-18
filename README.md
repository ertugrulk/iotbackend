# IoTBackend

A simple REST API to provide various sensor information gathered by Azure IoT hub which is then stored in Azure Blob Storage.

## Documentation

Documentation is available via Swagger on  `/swagger` endpoint.

## Starting the app

1. Set `BLOBSTORAGE_CONSTR` and `BLOBSTORAGE_CONTAINER` environment variables for blob storage:

```
export BLOBSTORAGE_CONSTR="BlobEndpoint=https://blobstorageaddress.blob.core.windows.net/QueueEndpoint=......."
export BLOBSTORAGE_CONTAINER="iotbackend"
```
2. Run the app in IoTBackend folder
```
dotnet run
```
3. Swagger should then be available on https://localhost:5001/swagger/index.html


## Technology & design decisions:
- Stack: ASP.NET Core, CQRS with MediatR, FluentValidation, FluentAssertions, MSUNIT & Moq.
- Sensors are "generic": if a sensor data exists for a given day, it will be loaded and converted to decimal if possible. Sensor types (rainfall, humidity, temperature) may be modified without having to change the project.

## Possible areas of improvement
1. Paging support in the endpoints (offset/limit).
2. More unit tests in Repository project to test Historical archive coverage.
3. Validation error messages could be returned as a JSON content instead of a string.
4. Caching can be implemented to improve the speed. 
5. API is open as per requirements, however users of the API would need to know the device names. It can be argued that a new endpoint which returns list of devices is necessary.