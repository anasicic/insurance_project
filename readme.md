# InsuranceApp

InsuranceApp is a web application built with **ASP.NET Core**, designed to manage **partners** and their **insurance policies**. The application allows users to track partner information and manage partners and their policies.

## Contents

- [Overview](#overview)
- [Technologies](#technologies)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Installation Steps](#installation-steps)
  - [Running the Server](#running-the-server)
- [Application Structure](#application-structure)
- [Database](#database)
- [Database Setup](#database-setup)
- [Features](#features)

## Overview

InsuranceApp enables users to:

- View a list of partners with key details.
- Access detailed partner information.
- Add new partners to the system.
- Manage insurance policies associated with partners.

## Technologies

This application uses the following technologies:

- **ASP.NET Core 9**: Web framework for building the backend.
- **Dapper**: A lightweight ORM for accessing the database.
- **SQLite**: A local database for storing partner and policy data.
- **Bootstrap**: For frontend styling and responsiveness.
- **JavaScript (ES6)**: For dynamic functionality like form validation and AJAX requests.

## Installation

Follow the steps below to set up and run the application on your local machine.

### Prerequisites

Make sure you have the following installed on your system:

- **.NET 5** or higher (for .NET Core 9)
- **SQLite** or another database of your choice (make sure to configure the connection string accordingly)

### Installation Steps

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/anasicic/MyApp.git
    ```

2. **Navigate to the Project Directory**:
    After cloning the repository, navigate to the project folder:
    ```bash
    cd InsuranceApp
    ```

3. **Restore NuGet Packages**:
    Use the following command to restore all the necessary NuGet packages:
    ```bash
    dotnet restore
    ```

4. **Run the Application**:
    To run the application, use the command:
    ```bash
    dotnet run
    ```

5. **Open a Browser**:
    After running the application, open your browser and go to:
    ```bash
    http://localhost:5000
    ```

## Application Structure

The application is structured as follows:

### Controllers

- **PartnerController**: This API controller enables dynamic management of partners and their data, and can be used to enhance the application with functionalities for retrieving, adding, and displaying partner details via RESTful services.
--**PolicyController**: This is an API controller for managing policies, providing endpoints to retrieve all policies, get policies by partner ID, and add new policies to the system.
- **HomeController**: This controller manages the display and handling of partner information in the application. It retrieves a list of all partners or detailed information for a specific partner, and processes the data into DTOs for use in views or JSON responses.
- **CreatePartnerController**: This MVC controller handles the creation of a new partner by displaying a form to the user (via the GET method) and processing the form submission (via the POST method). It validates the input data, interacts with the PartnerService to add the partner, and handles success or error scenarios accordingly.
- **CreatePolicyController**: This MVC controller includes a GET action to display the policy creation form and a POST action to validate the form, check for existing policies, and save the new policy if valid.

### Models

- **Partner**: Represents the data of a partner in the system (including properties like `PartnerId`, `FirstName`, `LastName`, `CroatianPIN`, etc.).
- **Policy**: Represents the insurance policies associated with a partner.
- **City**: The City model represents a city with properties like CityId, CityName, and StateId, where StateId establishes a foreign key relationship to the State model, linking each city to a specific state.
- **State**: The State model represents a state with an ID and a name.


### Views

The views display the data to the user and provide a user interface for interacting with the system. It includes pages for:
- Viewing partners
- Viewing detailed partner information
- Adding partners
- Adding and managing policies

### Database

SQLite is used to store data for the application. The database contains tables for storing partners, policies, cities and states.The database tables were manually created using SQL commands in the terminal.Dapper is used for data access in this project, and no Entity Framework migrations are involved.

## Database Setup

To configure and set up the database, follow these steps:

## Configure the Database Connection
    Edit the `appsettings.json` file to configure the connection string for your database. If you are using SQLite, the connection string should look like this:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Data Source=InsuranceApp.db"
      }
    }
    ```

## Features

InsuranceApp includes the following features:

- **Partner Management**: Add and view partner information.
- **Insurance Policy Management**: Manage the policies for each partner, including tracking the total policy amounts and counts.
- **Data Validation**: Ensure that all data (such as Croatian PIN and external codes) is unique before adding a partner.

