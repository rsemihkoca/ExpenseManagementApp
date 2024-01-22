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


### Create Expense
<table>
<thead>
    <tr>
      <th width="200px">Method</th>
      <th width="800px">Path </th>
    </tr>
</thead>
<tbody>
  <tr width="600px">
    <td align="center">POST    </td>
    <td  align="center">/api/Expense/</td>
  </tr>
</table>

#### Description: 
Create an expense record. Authorized users: <b>[Admin, Personnel]</b>

### Get All Expenses
<table>
<thead>
    <tr>
      <th width="200px">Method</th>
      <th width="800px">Path </th>
    </tr>
</thead>
<tbody>
  <tr width="600px">
    <td align="center">GET    </td>
    <td  align="center">/api/Expense/</td>
  </tr>
</table>

#### Description:
Retrieve expense records. Authorized users: <b>[Admin]</b>

### Update Expense
<table>
<thead>
    <tr>
      <th width="200px">Method</th>
      <th width="800px">Path </th>
    </tr>
</thead>
<tbody>
  <tr width="600px">
    <td align="center">PUT    </td>
    <td  align="center">/api/Expense/{expenseRequestId}</td>
  </tr>
</table>

#### Description: 
Update an expense record by expense request ID. Authorized users: <b>[Admin]</b>.\
Updating expense as approved do not fire background job. Only approve endpoint fires background job.

### Delete Expense
<table>
<thead>
    <tr>
      <th width="200px">Method</th>
      <th width="800px">Path </th>
    </tr>
</thead>
<tbody>
  <tr width="600px">
    <td align="center">DELETE    </td>
    <td  align="center">/api/Expense/{expenseRequestId}</td>
  </tr>
</table>

#### Description: 
Delete an expense record by expense request ID. Authorized users: <b>[Admin]</b>


### Get Expense by ID
<table>
<thead>
    <tr>
      <th width="200px">Method</th>
      <th width="800px">Path </th>
    </tr>
</thead>
<tbody>
  <tr width="600px">
    <td align="center">GET    </td>
    <td  align="center">/api/Expense/{expenseRequestId}</td>
  </tr>
</table>

#### Description: 
Retrieve an expense record by expense request ID. Authorized users: <b>[Admin]</b>


### Approve Expense
<table>
<thead>
    <tr>
      <th width="200px">Method</th>
      <th width="800px">Path </th>
    </tr>
</thead>
<tbody>
  <tr width="600px">
    <td align="center">PATCH    </td>
    <td  align="center">/api/Expense/Approve/{expenseRequestId}</td>
  </tr>
</table>

#### Description: 
Approve an expense record by expense request ID. Authorized users: <b>[Admin]</b>
Admin users approve expense records by giving expense request id. This endpoint fires background job to update expense record.

### Reject Expense
<table>
<thead>
    <tr>
      <th width="200px">Method</th>
      <th width="800px">Path </th>
    </tr>
</thead>
<tbody>
  <tr width="600px">
    <td align="center">PATCH    </td>
    <td  align="center">/api/Expense/Reject/{expenseRequestId}</td>
  </tr>
</table>

#### Description: 
Reject an expense record by expense request ID. Authorized users: <b>[Admin]</b>
Reject endpoint do not fire background job. Updates expense record immediately. And relevant fields are updated. Payment description, status, last updated date...

### Get Expenses by User
<table>
<thead>
    <tr>
      <th width="200px">Method</th>
      <th width="800px">Path </th>
    </tr>
</thead>
<tbody>
  <tr width="600px">
    <td align="center">GET    </td>
    <td  align="center">/api/Expense/ByUser</td>
  </tr>
</table>

#### Description: 
Retrieve expense records for the authenticated user. Authorized users: <b>[Admin, Personnel]</b>.\
Admin users can give null user id to retrieve all expenses.
Personnel can only retrieve their own expenses.
Personnel can filter expenses by status and date range. Also Admin can filter expenses.
This api is queryable.