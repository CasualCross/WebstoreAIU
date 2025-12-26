# WebstoreAIU

Simple weebstore application for learning purposes

## Functions

- User registration and login (password hashed)
- Product catalog with item listings
- Item detail pages with descriptions
- Shopping cart functionality
- Image handling system

## Required Technologies

- C# .NET 8.0
- PostgreSQL

## Setup Instructions

1. Install PostgreSQL
2. Create user:
   - Username: postgres
   - Password: password
3. Run the application

//The database will be created automatically when you first run the application
//Program.cs will automatically create example items

Database Default Configuration:
- Database: webstore
- Username: postgres
- Password: password
- Port: 5432

## Project Structure

- Models/ - Database models (User, Item, CartItem)
- Data/ - DbContext and database configuration
- Pages/ - UI (Login, Register, Catalog, Item Details, Cart)
- Services/ - Password hashing service and image handling service
