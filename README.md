# CSharp-ASP.NET-Core-StudentAPI

A RESTful Student Management API built using ASP.NET Core, Entity Framework Core, and SQLite.
This project demonstrates CRUD operations, REST API development, data validation, pagination, filtering, soft delete functionality, and modern API documentation using Scalar OpenAPI.

---

## 📌 Project Overview

This application provides a fully functional Student Management REST API that allows users to create, retrieve, update, and archive student records securely.

The API includes advanced filtering, pagination, validation, soft delete functionality, and interactive API documentation support.

### Features:

* Create student records
* Retrieve all students
* Get student by ID
* Update student information
* Soft delete students
* Pagination support
* Query filtering
* Email validation
* CGPA and age validation
* Duplicate email checking
* SQLite database integration
* OpenAPI documentation with Scalar UI

---

## 🧠 Key Concepts Demonstrated

* ASP.NET Core Web API
* RESTful API Design
* Entity Framework Core
* SQLite Database Integration
* CRUD Operations
* LINQ Query Filtering
* Pagination
* Soft Delete Implementation
* Input Validation
* Regex Email Validation
* Dependency Injection
* Asynchronous Programming (`async/await`)
* OpenAPI Documentation
* API Routing and Controllers

---

## 🛠 Technologies Used

* C#
* ASP.NET Core
* .NET 10.0
* Entity Framework Core
* SQLite
* LINQ
* Scalar OpenAPI
* REST API

---

## 📂 Project Structure

```bash id="x7m6u2"
CSharp-ASP.NET-Core-StudentAPI/
│── Controllers/
│   └── StudentsController.cs
│
│── Models/
│   └── Student.cs
│
│── Data/
│   └── AppDbContext.cs
│
│── Program.cs
│── students.db (auto-generated)
│── appsettings.json
│── CSharp-ASP.NET-Core-StudentAPI.csproj
│── README.md
```

---

## ▶️ How to Run

### Clone the Repository

```bash id="5j2l3z"
git clone <your-repository-url>
```

### Navigate to Project Folder

```bash id="w8d0l4"
cd CSharp-ASP.NET-Core-StudentAPI
```

### Restore Dependencies

```bash id="x9r7v1"
dotnet restore
```

### Run the Application

```bash id="n6c1y5"
dotnet run
```

---

## 🌐 API Base URL

```bash id="jj1m5p"
http://localhost:5125/api/students

```

### OpenAPI Documentation

```bash Scalar OpenAPI
http://localhost:5125/scalar/v1
```

---

## 📘 API Endpoints

### Get All Students

```http id="f8r1w2"
GET /api/students
```

### Get Student By ID

```http id="u2k4a6"
GET /api/students/{id}
```

### Create Student

```http id="d7m3n8"
POST /api/students
```

### Update Student

```http id="q9v2s7"
PUT /api/students/{id}
```

### Soft Delete Student

```http id="k3c8y1"
DELETE /api/students/{id}
```

---

## 🔍 Query Filtering & Pagination

The API supports filtering and pagination through query parameters:

### Example:

```http id="m1p7t4"
GET /api/students?name=seam&cgpa=3.50&pageNumber=1&pageSize=5
```

### Supported Query Parameters

| Parameter    | Description                       |
| ------------ | --------------------------------- |
| `id`         | Filter by student ID              |
| `name`       | Search by student name            |
| `cgpa`       | Filter students with minimum CGPA |
| `address`    | Search by address                 |
| `pageNumber` | Current page number               |
| `pageSize`   | Number of records per page        |

---

## 🛡 Validation Rules

The API includes several validation rules:

* Name and Email are required
* Email must be valid
* CGPA must be between `0.00 - 4.00`
* Age must be between `18 - 60`
* Duplicate emails are not allowed
* Student IDs cannot be modified during update

---

## 🗄 Soft Delete System

Instead of permanently removing records, the API uses a soft delete mechanism:

```csharp id="a4v8q2"
student.IsDeleted = true;
```

Deleted students are automatically excluded from API responses.

---

## 📊 Sample Student JSON

```json id="o5n9l1"
{
  "id": 1,
  "name": "Seam Ahmed",
  "email": "seam@example.com",
  "department": "Computer Science",
  "cgpa": 3.85,
  "phone": "017XXXXXXXX",
  "address": "Dhaka, Bangladesh",
  "age": 22
}
```

---

## 📚 API Documentation Dashboard

This project uses Scalar OpenAPI Dashboard for interactive API testing and documentation.

### Features:

* Interactive API explorer
* Live endpoint testing
* Request/response visualization
* Modern UI theme

---

## 🎯 Learning Objectives

* Building RESTful APIs using ASP.NET Core
* Understanding Entity Framework Core workflow
* Implementing pagination and filtering
* Writing scalable controller logic
* Applying validation and error handling
* Using SQLite for persistent storage
* Designing production-style APIs
* Implementing soft delete architecture

---

## 👨‍💻 Author

Seam
Full-stack Developer

---
