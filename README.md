cd ExpenseApplication/

dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api
dotnet ef database update --project Infrastructure --startup-project Api



![erd.png](.github%2Fassets%2Ferd.png)


Documentation details https://documenter.getpostman.com/view/23348379/2s9YymFQ2G



<table style="padding: 0;margin: 0">
    <thead style="padding: 0;margin: 0">
        <tr style="padding: 0;margin: 0">
          <th width="10%">Method</th>
          <th width="40%">Path </th>
        </tr>
    </thead>
    <tbody style="padding: 0;margin: 0">
      <tr width="100%">
        <td align="center" style="padding: 0;margin: 0">
          <img src=".github/assets/POST.png" alt="POST" width="25%"/>
        </td>
        <td align="center" style="padding: 0;margin: 0"><b>/api/Expense/</b></td>
      </tr>

</tbody>
<thead>
    <tr>
    <th width="50%">Request Example</th>
    <th width="50%">Response Example</th>
    </tr>
</thead>
<tbody> 

<tr width="100%" style="padding: 0;margin: 0">
  <td height="0px" style="padding-bottom: 0%;padding-top: 0%; margin: 0%">

```json
{
  "id": 5,
  "username": "mary",
  "email": "mary@example.com",
  "order_id": "f7177da"
}
```
</td>
<td height="0px" style="padding-bottom: 0%;padding-top: 0%; margin: 0%">

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



## Expense

<pre></pre>


<table style="padding: 0;margin: 0">
    <thead style="padding: 0;margin: 0">
        <tr style="padding: 0;margin: 0">
          <th width="10%">Method</th>
          <th width="40%">Path </th>
        </tr>
    </thead>
    <tbody style="padding: 0;margin: 0">
      <tr width="100%">
        <td align="center" style="padding: 0;margin: 0">
          <img src=".github/assets/POST.png" alt="POST" width="25%"/>
        </td>
        <td align="center" style="padding: 0;margin: 0"><b>/api/Expense/</b></td>
      </tr>
      
</tbody>
<thead>
    <tr>
    <th width="50%">Request Example</th>
    <th width="50%">Response Example</th>
    </tr>
</thead>
<tbody> 

<tr width="100%">
  <td height="0px" style="padding-bottom: 0%;padding-top: 0%; margin: 0%">

```json
{
  "userId": 2,
  "amount": 50,
  "categoryId": 15,
  "paymentMethod": "Online",
  "paymentLocation": "Office",
  "documents": "hello.png",
  "description": "Hediye Odemesi"
}
```
</td>
<td height="0px">

```json
{
  "expenseRequestId": 43,
  "userId": 2,
  "categoryId": 15,
  "expenseStatus": "Pending",
  "paymentStatus": "Pending",
  "paymentDescription": "Payment Not Made",
  "amount": 50,
  "paymentMethod": "Online",
  "paymentLocation": "Office",
  "documents": "hello.png",
  "description": "Hediye Odemesi",
  "creationDate": "22/01/2024 01:42:40",
  "lastUpdateTime": "22/01/2024 01:42:40"
}
```
</td>
</tr>
</tbody> 

</table>

### API Authentication

Explain the authentication method required for accessing your API.
