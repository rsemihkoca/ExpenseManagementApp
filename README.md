cd ExpenseApplication/

dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api
dotnet ef database update --project Infrastructure --startup-project Api



![erd.png](.github%2Fassets%2Ferd.png)


Documentation details https://documenter.getpostman.com/view/23348379/2s9YymFQ2G


<table>
  <tr>
    <th>Method</th>
    <th>Path</th>
    <th>Auth</th>
  </tr>
  <tr>
    <td>
      <img src=".github/assets/POST.png" alt="POST" width="100"/>
    </td>    <td>/api/Expense/</td>

  </tr>
</table>

# Project Name

A brief description of your project.

## Description

Provide a more detailed description of your project here.

## Validations

List any validations or requirements for using your project.

## API Details

### API Method

Specify the HTTP method used by your API (e.g., GET, POST, PUT, DELETE).

### API Path

Provide the endpoint or path for your API.

### API Authentication

Explain the authentication method required for accessing your API.

## Test



### Test Payload

Provide an example of the payload or input data for testing your API.
<style>
    table {
        width: 100%;
    }
</style>

<div align="center">

<table>
  <tr>
    <th>Method</th>
    <th>Path</th>
    <th>Auth</th>
  </tr>
  <tr>
    <td>
      <img src=".github/assets/POST.png" alt="POST" width="100"/>
    </td>    <td>/api/Expense/</td>

  </tr>
</table>
</div>

<table>
<tr>
<th>Response</th>
<th>Request</th>
</tr>
<tr>
<td>
<pre>
{
  "id": 1,
  "username": "joe",
  "email": "joe@example.com",
  "order_id": "3544fc0"
}
</pre>
</td>
<td>

```json
{
  "id": 5,
  "username": "mary",
  "email": "mary@example.com",
  "order_id": "f7177da"
}
```

</td>
</tr>
</table>
