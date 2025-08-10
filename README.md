# 🚀 TechZone API | E-Commerce Project

## 🛒 What is TechZone?

**TechZone** is an online electronics shopping platform providing a complete backend solution for managing accounts, products, orders, payments, and more.

---

## 🔐 1. Account & Authentication

- JWT Authentication with ASP.NET Core Identity  
- User Registration & Login  
- Role-Based Access Control: `Admin`, `Customer`  

![Login](Postman%20Testing/1.Login.png)  
![Get Profile](Postman%20Testing/2.Get%20Profile.png)  

---

## 🧱 2. Clean Architecture & Design Patterns

- N-Tier Structure with clear separation of concerns  
- Generic Repository Pattern  
- AutoMapper for DTO ↔ Entity mapping  
- Result Wrapper Pattern for unified API responses  
- Global Exception Handling Middleware  
- Server-side & Database-level Validation  
- Asynchronous Programming  
- Serilog for structured logging (errors stored in DB)  
- IMemoryCache for category caching  

![Diagram](Postman%20Testing/Diagram.png)  

---

## 🛍️ 3. Product Management

- Full CRUD Operations  
- Pagination for performance  
- Soft Delete (Global Query Filter)  
- Partial Update with PATCH  
- Filtering by Brand & Searching by Product Name  

![Filtering & Search](Postman%20Testing/3.Filtering&Search&Pagination.png)  

---

## 📦 4. Shopping Cart

- Add, Remove, Update Items  
- Clear Entire Cart  

![Add Item to Cart](Postman%20Testing/4.Add%20item%20to%20cart.png)  

---

## 📑 5. Order Management

- Create, Read, Delete Orders  
- Update Status & Shipping Date  
- Track Order Status  

![Create Order](Postman%20Testing/5.Create%20order.png)  

---

## 💸 6. Payments (Stripe Integration)

- Secure checkout via Stripe  
- Auto-redirect to payment session  

![Payment Checkout](Postman%20Testing/7.PaymentCheckout.png)  
![Payment](Postman%20Testing/8.Payment.png)  

---

## ✉️ 7. Email Notifications (SMTP)

- Order Confirmation Emails  
- Status Update Notifications  

![Email Step 1](Postman%20Testing/EmailOne.png)  
![Email Step 2](Postman%20Testing/EmailTwo.png)  
![Email Step 3](Postman%20Testing/EmailThree.png)  
![Email Step 4](Postman%20Testing/EmailFour.png)  
![Email Step 5](Postman%20Testing/EmailFive.png)  

---

## 🧪 8. Testing

- Full Postman Collection covering all endpoints  

![API Endpoints](Postman%20Testing/API.png)  

---

## 🗂️ 9. Roles & Permissions

### 👤 Admin:
- Manage users & roles  
- Manage products, orders, categories  
- Full access to admin endpoints  

### 🛍️ Customer:
- Browse & search products  
- Add to cart, place orders  
- Make payments & track order status  

![Assign Role To User](Postman%20Testing/AssignRoleToUser.png)  
![Get All Accounts](Postman%20Testing/GetAllAccounts.png)  
