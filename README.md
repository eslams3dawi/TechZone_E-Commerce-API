# 🚀 TechZone API | E-Commerce Project

## 🛒 What is TechZone?

**TechZone** is an online electronics shopping platform providing a complete backend solution for managing accounts, products, orders, payments, and more.

---

## 🔐 1. Account & Authentication

- JWT Authentication with ASP.NET Core Identity  
- User Registration & Login  
- Role-Based Access Control: `Admin`, `Customer`  

![Login](Postman Testing/1.Login.png)  
![Get Profile](Postman Testing/2.Get%20Profile.png)  

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

![Diagram](Postman Testing/Diagram.png)  

---

## 🛍️ 3. Product Management

- Full CRUD Operations  
- Pagination for performance  
- Soft Delete (Global Query Filter)  
- Partial Update with PATCH  
- Filtering by Brand & Searching by Product Name  

![Filtering & Search](Postman Testing/3.Filtering&Search&Pagination.png)  

---

## 📦 4. Shopping Cart

- Add, Remove, Update Items  
- Clear Entire Cart  

![Add Item to Cart](Postman Testing/4.Add%20item%20to%20cart.png)  

---

## 📑 5. Order Management

- Create, Read, Delete Orders  
- Update Status & Shipping Date  
- Track Order Status  

![Create Order](Postman Testing/5.Create%20order.png)  

---

## 💸 6. Payments (Stripe Integration)

- Secure checkout via Stripe  
- Auto-redirect to payment session  

![Payment Checkout](Postman Testing/7.PaymentCheckout.png)  
![Payment](Postman Testing/8.Payment.png)  

---

## ✉️ 7. Email Notifications (SMTP)

- Order Confirmation Emails  
- Status Update Notifications  

![Email Step 1](Postman Testing/EmailOne.png)  
![Email Step 2](Postman Testing/EmailTwo.png)  
![Email Step 3](Postman Testing/EmailThree.png)  
![Email Step 4](Postman Testing/EmailFour.png)  
![Email Step 5](Postman Testing/EmailFive.png)  

---

## 🧪 8. Testing

- Full Postman Collection covering all endpoints  

![API Endpoints](Postman Testing/API.png)  

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

![Assign Role To User](Postman Testing/AssignRoleToUser.png)  
![Get All Accounts](Postman Testing/GetAllAccounts.png)  
