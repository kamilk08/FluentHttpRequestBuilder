# FluentHttpRequestBuilder
Simple library to create http requests in fluent way
## Get started

**Instal package via nuget package manager console** 

```
Install-Package FluentHttpRequestBuilderLibrary -Version 1.0.2
```

## Examples
  - Basic example   
```csharp
var request = new HttpRequestBuilder()
      .InitializeRequest()
      .WithMethod(HttpMethod.Post)
      .WithUri("http://www.google.com")
      .AddBearerToken("Token")
      .AddHeader("accept", "application/json")
      .AddHeader("Connection", "keep-alive")
      .AddHeader("Cache-Control", "no-cache");
      
   ```
  - Request with string content
  ```csharp
  var obj = new TestObject(Guid.NewGuid(), new List<int>());

var request = new HttpRequestBuilder()
      .InitializeRequest()
      .WithMethod(HttpMethod.Post)
      .WithUri("http://www.google.com")
      .WithStringContent(data)
      .GetRequest();
  ```


  - Request with x-www-form-urlencoded content type

```csharp
var request = new HttpRequestBuilder()
      .InitializeRequest()
      .WithMethod(HttpMethod.Post)
      .WithUri(AppManager.GetConfigValue("login"))
      .WithFormUrlEncodedContent()
      .AddFormKeyValue("username", "username")
      .AddFormKeyValue("password", "password")
      .AddFormKeyValue("grant_type", "password")
      .AddFormKeyValue("client_id", "FDCBD1AA-F696-4DC7-9E7C-A0C2FC50B2DB")
      .AddFormKeyValue("client_secret", "secret")
      .PassFormValuesAsHttpContent()
      .AddHeader("Cache-Control", "no-cache")
      .AddHeader("Connection", "keep-alive")
      .GetRequest();

   ```
  - Request with multipart/form-data content type 
```csharp
var request = new HttpRequestBuilder()
      .WithMultiPartFormData()
      .AddFileParam("path_to_file")
      .AddStringParam("key", "value")
      .AddByteArrayParam("bytes", Enumerable.Repeat<byte>(1, 4096).ToArray())
      .AddUrlEncodedParam("url", "urlvalue")
      .PassAsMultiPartDataContent()
      .GetRequest();
```
