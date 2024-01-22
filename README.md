cd ExpenseApplication/

dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api
dotnet ef database update --project Infrastructure --startup-project Api



![erd.png](.github%2Fassets%2Ferd.png)


Documentation details https://documenter.getpostman.com/view/23348379/2s9YymFQ2G

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