
---

# Expense Application Setup

## Prerequisites

- [.NET SDK 8](https://dotnet.microsoft.com/download) installed on your machine.
- Docker/Docker Compose installed on your machine.
- Git installed on your machine.

## Clone the Repository

```bash
git clone https://github.com/yourusername/ExpenseApplication.git
cd ExpenseApplication
```


## Docker Configuration


### 1. Restart Docker Containers

Execute the following command to bring down existing Docker containers, volumes, and remove orphaned containers, then bring the Docker environment back up:

```bash
docker-compose down -v --remove-orphans && docker-compose up
```

### 2. Apply Migrations

Run the following commands to apply Entity Framework migrations and update the database:

```bash
cd ExpenseApplication
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api
dotnet ef database update --project Infrastructure --startup-project Api
```
If you already have migrations, you can run the following command to drop the database and re-create it:

```bash
## Run the Application

```bash
cd ExpenseApplication
dotnet ef database update --project Infrastructure --startup-project Api
```

The application will be accessible at `http://localhost:5245` by default.

## Entity Relationship Diagram (ERD)

![erd.png](.github%2Fassets%2Ferd.png)

CreatedBy: Admin UserId \
Status: 0: Pending, 1: Approved, 2: Rejected, Only admin can approve or reject expense requests.\
Description: User add description for expense request.\
PaymentDescription: Admin add description for expense request. Or error code from background job.
---



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



Further information can be found in the [link](https://documenter.getpostman.com/view/23348379/2s9YymFQ2G).
Also in dcoumentation folder there is a postman collection. You can import it to postman and test the api.