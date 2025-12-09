📌 Project Overview
The Expense Tracker Application is a full-stack financial management system developed using ASP.NET Core MVC.
It enables users to efficiently manage their income, expenses, categories, and transactions with secure authentication and an interactive dashboard.

🛠️ Tech Stack
Backend: ASP.NET Core MVC, C#
Frontend: HTML, CSS, JavaScript, Bootstrap, jQuery
Database: Microsoft SQL Server
Charts: Chart.js (Pie Chart for Expense/Income visualization)
Authentication: JWT (JSON Web Token)
Hosting: IIS (Internet Information Services)

🔐 Authentication
Secure User Registration & Login
JWT-based authentication implemented after user profile creation
Token is stored and used to protect user-specific routes and actions

📊 Dashboard Features
After successful login, users see a fully dynamic dashboard showing:
Total Balance
Total Income
Total Expenses
Recent Transactions
Pie Chart visualization using Chart.js (Income vs Expense)
Clean UI with responsive layout

📁 Features & Modules
✔ 1. Categories Module
Users can create and manage:
Income Categories
Expense Categories

✔ 2. Transactions Module
Users can:
Add transactions with amount, category, description, and date
View, edit, and delete transactions
Filter transactions by category and date

✔ 3. User Profile
Manage basic profile details
Personalized dashboard data based on logged-in user

✔ 4. Secure Data Handling
JWT protects all sensitive pages
Database operations handled via SQL Server
Validations added on all input fields

🎨 UI & Design

Modern dark/light theme using Bootstrap
Clean table layouts for categories & transactions
Interactive charts for better financial insights
Fully responsive design

🚀 Deployment

Successfully hosted on IIS
Configured application pool, bindings, and connection strings
