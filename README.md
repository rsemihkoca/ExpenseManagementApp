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



<table>

  <tbody>
  <tr width="600px">
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

  </tbody>
  <tfoot>
    <tr>
      <th width="500px">Request Example</th>
      <th width="500px">Response Example</th>
    </tr>
  </tfoot>
</table>




<div align="center">
  <table>
    <thead>
        <tr>
          <th width="200px">Method</th>
          <th width="800px">Path </th>
        </tr>
    </thead>
    <tbody>
      <tr width="600px">
        <td>
          <img src=".github/assets/POST.png" alt="POST" width="100px"/>
        </td>
        <td >/api/Expense/</td>
      </tr>
  </table>
</div>








<table>
    <thead>
        <tr>
          <th width="10%">Method</th>
          <th width="40%">Path </th>
        </tr>
    </thead>
    <tbody>
      <tr width="100%">
        <td align="center">
          <img src=".github/assets/POST.png" alt="POST" width="30%"/>
        </td>
        <td align="center"><b>/api/Expense/</b></td>
      </tr>
  <tr width="100%">
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

  </tbody>
    <tfoot>
        <tr>
        <th width="50%">Request Example</th>
        <th width="50%">Response Example</th>
        </tr>
</table>


