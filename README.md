# ASP.NET Core MVC Learning Project

A sample **ASP.NET Core MVC** application designed for **developer onboarding and training**. This project demonstrates the basic architecture and workflow of an enterprise application, including authentication, dynamic menu loading, CRUD operations, Master-Detail forms, loading indicators, success dialogs, and data management.

---

# Application Overview

This project helps new developers understand how a real-world ASP.NET Core MVC application works. It covers common business scenarios and coding practices used in enterprise applications.

---

# Features

- Secure Login
- Database Authentication
- Dynamic Menu Loading
- Simple CRUD Module
- Master Detail Module
- Server-side Validation
- Client-side Validation
- Loading Indicator
- Single Click Submit Protection
- Success Dialog
- Data Grid
- View Record
- Edit Record
- Delete Record
- Responsive UI

---

# Login

The application starts with a secure login page. User credentials are validated against the database.

### Features

- User Authentication
- Database Validation
- Secure Login
- Responsive Design

## Screenshot

> **Insert Login Page Screenshot Here**

```text
Screenshots/Login.png
```

---

# Dashboard

After successful login, the application loads the user-specific menu from the database.

### Features

- Dynamic Menu
- Database Driven Navigation
- Module Access

## Screenshot

> **Insert Dashboard Screenshot Here**

```text
Screenshots/Dashboard.png
```

---

# Setup Modules

The application contains two setup modules.

## 1. Simple CRUD Form

This module demonstrates the basic Create, Read, Update, and Delete operations.

### Features

- Add Record
- Update Record
- Delete Record
- View Record
- Validation
- Success Message

## Screenshot

> **Insert Simple CRUD Form Screenshot Here**

```text
Screenshots/CrudForm.png
```

---

## 2. Master Detail Form

This module demonstrates Master-Detail transactions commonly used in enterprise applications.

### Features

- Master Information
- Multiple Detail Records
- Add Detail Rows
- Edit Detail Rows
- Delete Detail Rows
- Save Complete Transaction

## Screenshot

> **Insert Master Detail Screenshot Here**

```text
Screenshots/MasterDetail.png
```

---

# List Page

Each setup module opens with a listing page.

The listing page contains:

- Add New Button
- Data Grid
- Record Listing
- Search (If Implemented)

## Screenshot

> **Insert List Page Screenshot Here**

```text
Screenshots/ListPage.png
```

---

# Grid Actions

Each row in the grid contains the following actions.

| Action | Description |
|---------|-------------|
| 👁 View | View complete record |
| ✏ Edit | Update existing record |
| 🗑 Delete | Delete record |
| 🔗 Link | Open related page/details |

## Screenshot

> **Insert Grid Action Screenshot Here**

```text
Screenshots/GridActions.png
```

---

# Add New

Clicking the **Add New** button opens the entry form where users can create a new record.

## Screenshot

> **Insert Add New Form Screenshot Here**

```text
Screenshots/AddNew.png
```

---

# Submit Processing

To prevent duplicate submissions, the application implements Single Click Submit.

When the user clicks **Submit**

- Button becomes disabled
- Button text changes to **Processing...**
- Loading indicator is displayed
- Multiple clicks are prevented

## Screenshot

> **Insert Processing Screenshot Here**

```text
Screenshots/Processing.png
```

---

# Success Dialog

After a successful Insert or Update operation, a success dialog is displayed.

Examples

- Record Saved Successfully
- Record Updated Successfully
- Record Deleted Successfully

## Screenshot

> **Insert Success Dialog Screenshot Here**

```text
Screenshots/SuccessDialog.png
```

---

# View Record

Users can open any record in View mode.

### Features

- Read Only
- Complete Record Information
- Easy Navigation

## Screenshot

> **Insert View Record Screenshot Here**

```text
Screenshots/ViewRecord.png
```

---

# Edit Record

Users can modify existing records.

### Features

- Load Existing Data
- Update Record
- Save Changes
- Validation

## Screenshot

> **Insert Edit Record Screenshot Here**

```text
Screenshots/EditRecord.png
```

---

# Delete Record

Users can remove records from the system.

### Features

- Delete Confirmation
- Safe Deletion
- Success Message

## Screenshot

> **Insert Delete Confirmation Screenshot Here**

```text
Screenshots/DeleteRecord.png
```

---

# Technologies Used

- ASP.NET Core MVC
- C#
- SQL Server
- Entity Framework Core / ADO.NET
- Razor Views
- HTML5
- CSS3
- JavaScript
- jQuery
- Bootstrap

---

# Learning Objectives

This project is intended to help developers understand:

- ASP.NET Core MVC Architecture
- Controllers
- Models
- Razor Views
- Routing
- Authentication
- CRUD Operations
- Master Detail Forms
- SQL Server Integration
- Dynamic Menu Loading
- Grid Operations
- Client-side Validation
- Server-side Validation
- Loading Indicators
- Success Dialogs
- Enterprise Application Flow

---

# Project Flow

```text
Login
   │
   ▼
Authenticate User
   │
   ▼
Load Menu From Database
   │
   ▼
Open Module
   │
   ▼
List Page
   │
   ├── Add New
   ├── View
   ├── Edit
   └── Delete
          │
          ▼
Entry Form
          │
          ▼
Validation
          │
          ▼
Submit
          │
          ▼
Processing...
          │
          ▼
Database Save
          │
          ▼
Success Dialog
          │
          ▼
Back to List Page
```

---

# Folder Structure

```text
Project
│
├── Controllers
├── Models
├── Views
├── Services
├── Data
├── Scripts
├── wwwroot
│
├── Screenshots
│   ├── Login.png
│   ├── Dashboard.png
│   ├── CrudForm.png
│   ├── MasterDetail.png
│   ├── ListPage.png
│   ├── GridActions.png
│   ├── AddNew.png
│   ├── Processing.png
│   ├── SuccessDialog.png
│   ├── ViewRecord.png
│   ├── EditRecord.png
│   └── DeleteRecord.png
│
└── README.md
```

---

# Future Enhancements

- Role Based Authorization
- User Management
- Audit Logs
- File Upload
- Excel Export
- PDF Export
- Dashboard Reports
- Email Notifications
- REST APIs
- Logging
- Exception Handling
- Unit Testing

---

# Purpose

This project was created as a learning application for new developers joining the team. It provides hands-on experience with ASP.NET Core MVC concepts and demonstrates the structure of a typical enterprise business application.

---

# Author

**ASP.NET Core MVC Learning Project**

Designed for **Developer Onboarding**, **Training**, and **Learning Enterprise Application Development**.
