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
2. Create user in PostgreSQL:
   - Username: postgres
   - Password: password
3. Create database "webstore"
4. Open terminal and write command "cd WebstoreAIU $$ dotnet run"
5. Open browser and go to "http://localhost:5024/"

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

## Database Diagram
<img width="683" height="548" alt="Screenshot 2025-12-26 171707" src="https://github.com/user-attachments/assets/881e994b-3a87-4a19-94a6-8d638705690a" />

## TODO
- Implement mail registration and login
- Implement bank card binding

## Images
<img width="1885" height="1029" alt="Screenshot 2025-12-27 130552" src="https://github.com/user-attachments/assets/41014e68-a4d3-475e-81f0-5fce4b51676a" />
