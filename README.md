# WebstoreAIU

Simple weebstore application for learning purposes

## SRS Links
https://docs.google.com/document/d/102yc3yGTBVDwYqTLRlfxWvusoIQiT6Sp2-7euN9ACdA/edit?usp=sharing
[Webstore AIU SRS.pdf](https://github.com/user-attachments/files/24353428/Webstore.AIU.SRS.pdf)

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
<img width="1904" height="1000" alt="Screenshot 2025-12-27 130608" src="https://github.com/user-attachments/assets/9f691f85-2ba2-44ef-b244-32aa14379142" />
<img width="1897" height="840" alt="Screenshot 2025-12-27 130621" src="https://github.com/user-attachments/assets/d877f741-1de7-4c98-a214-44b042ab8f9a" />
<img width="1899" height="612" alt="Screenshot 2025-12-27 130646" src="https://github.com/user-attachments/assets/8c87eba0-89ac-4409-a0e6-0e78af1a2894" />
<img width="1885" height="1029" alt="Screenshot 2025-12-27 130552" src="https://github.com/user-attachments/assets/41014e68-a4d3-475e-81f0-5fce4b51676a" />
<img width="1692" height="1027" alt="Screenshot 2025-12-27 130725" src="https://github.com/user-attachments/assets/60748832-b886-40ab-8b3f-de0ac8f1acc5" />
<img width="1883" height="967" alt="Screenshot 2025-12-27 130734" src="https://github.com/user-attachments/assets/36e9ef9e-ff94-4c8e-8894-e6e7d5df59b0" />

