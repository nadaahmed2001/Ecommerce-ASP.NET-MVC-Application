# Ecommerce-ASP.NET-MVC-Application

## Table of Contents

- [Description](#description)
- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)

## Description

This ASP.NET MVC Ecommerce application is a fully-featured online shopping platform designed to meet the needs of both administrators and users. It provides seamless functionality for managing products, user accounts, shopping carts, and more. The project incorporates modern web development practices and is built on a foundation of ASP.NET MVC, Entity Framework, Cookie Authentication, and role-based authorization.

## Features

### Admin

- **Product Management**: Administrators can edit, add, and delete products in the system, ensuring an up-to-date product catalog.

### User

- **User Accounts**: Users can create accounts, enabling personalized shopping experiences.
- **Product Browsing**: Users can browse the extensive product catalog, view detailed product information.
- **Shopping Cart**: Users can add products to their shopping cart for convenient checkout.

### Common Features

- **User Profile**: Both administrators and users have access to their profile, allowing them to view and edit their information.
- **Authentication and Authorization**: Secure cookie-based authentication and role-based authorization ensure data privacy and user-specific functionality.
- **Validation**: Extensive input validation is implemented throughout the application to enhance security and user experience.

## Technologies

- **ASP.NET MVC**:\
      The application is built using the ASP.NET MVC framework, providing a structured and scalable architecture.
- **Entity Framework**:\
      Entity Framework is used for efficient data management, including database querying and CRUD operations.
- **Cookie Authentication**:\
      Secure user authentication is achieved through cookie-based mechanisms.
- **Role-Based Authorization**:\
       Different roles (admin and user) have access to specific parts of the application.
- **Validation**:\
       Data validation is enforced to maintain data integrity and prevent security vulnerabilities.
- **Database**:\
      SQL Server is used as the database backend to store product and user data.
- **Bootstrap Template**:\
      A Bootstrap-based template is utilized for a responsive and visually appealing user interface.

## Installation

To run the application locally, follow these steps:

1. Clone the repository:

   ```bash
   git clone https://github.com/nadaahmed2001/Ecommerce-ASP.NET-MVC-Application

2.  Open the project in your preferred development environment (e.g., Visual Studio).
3.  Update the database connection settings in the appsettings.json file. Change the server name and database name accordingly: (Also I have added a backup file for the database you can restore it but this is optional)
     ```bash
    "ConnectionStrings": {
        "DefaultConnection": "Server=your-server-name;Database=your-database-name;Trusted_Connection=True;MultipleActiveResultSets=true"
    }

4.  Build and run the application.



