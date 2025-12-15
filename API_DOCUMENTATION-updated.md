# Sara Cafe API Documentation

## Table of Contents
- [Overview](#overview)
- [Base URL](#base-url)
- [Authentication](#authentication)
- [Endpoints](#endpoints)
  - [Authentication](#authentication-endpoints)
  - [Products](#products-endpoints)
  - [Categories](#categories-endpoints)
  - [Users](#users-endpoints)
  - [Contacts](#contacts-endpoints)
- [Data Models](#data-models)
- [Error Handling](#error-handling)
- [Setup & Configuration](#setup--configuration)

---

## Overview

Sara Cafe API is a RESTful API for managing a cafe menu system with support for Arabic and English languages. The API provides endpoints for managing products, categories, and users with JWT-based authentication.

### Features
- 🌍 **Multilanguage Support**: All content supports Arabic (AR) and English (EN), with Arabic as default
- 🔐 **JWT Authentication**: Secure token-based authentication
- 📸 **Image Upload**: Local image storage for product images
- 🔍 **Filtering**: Filter products by category
- 🛡️ **Authorization**: Read operations are public, write operations require authentication

---

## Base URL

```
Development: http://localhost:5064
Production: [Your production URL]
```

All endpoints are prefixed with `/api`

---

## Authentication

The API uses JWT (JSON Web Token) for authentication. 

### Getting a Token

1. Login via `POST /api/users/login` with username and password
2. Receive a JWT token in the response
3. Include the token in the Authorization header for protected endpoints

### Using the Token

Include the token in the Authorization header:

```
Authorization: Bearer {your-jwt-token}
```

### Token Expiration

Tokens expire after 24 hours. You'll need to login again to get a new token.

---

## Endpoints

### Authentication Endpoints

#### Login

Authenticate a user and receive a JWT token.

**Endpoint:** `POST /api/users/login`  
**Authentication:** Not required (Public)

**Request Body:**
```json
{
  "username": "admin",
  "password": "Admin@123"
}
```

**Success Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@saracafe.com",
    "firstName": "Admin",
    "lastName": "User",
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

**Error Response (401 Unauthorized):**
```json
"Invalid username or password"
```

---

### Products Endpoints

#### Get All Products

Retrieve all products, optionally filtered by category.

**Endpoint:** `GET /api/products`  
**Authentication:** Not required (Public)

**Query Parameters:**
- `categoryId` (optional, integer): Filter products by category ID

**Example Requests:**
```
GET /api/products
GET /api/products?categoryId=1
```

**Success Response (200 OK):**
```json
[
  {
    "id": 1,
    "nameAr": "إسبرسو",
    "nameEn": "Espresso",
    "descriptionAr": "قهوة إسبرسو إيطالية قوية",
    "descriptionEn": "Strong Italian espresso",
    "isActive": true,
    "imageUrl": "/uploads/images/image.png",
    "categoryId": 1,
    "categoryNameAr": "قهوة",
    "categoryNameEn": "Coffee"
  }
]
```

#### Get Product by ID

Retrieve a specific product by its ID.

**Endpoint:** `GET /api/products/{id}`  
**Authentication:** Not required (Public)

**Path Parameters:**
- `id` (integer, required): Product ID

**Example Request:**
```
GET /api/products/1
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "nameAr": "إسبرسو",
  "nameEn": "Espresso",
  "descriptionAr": "قهوة إسبرسو إيطالية قوية",
  "descriptionEn": "Strong Italian espresso",
  "isActive": true,
  "imageUrl": "/uploads/images/image.png",
  "categoryId": 1,
  "categoryNameAr": "قهوة",
  "categoryNameEn": "Coffee"
}
```

**Error Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

#### Get Products by Category

Retrieve all products in a specific category.

**Endpoint:** `GET /api/products/category/{categoryId}`  
**Authentication:** Not required (Public)

**Path Parameters:**
- `categoryId` (integer, required): Category ID

**Example Request:**
```
GET /api/products/category/1
```

**Success Response (200 OK):**
```json
[
  {
    "id": 1,
    "nameAr": "إسبرسو",
    "nameEn": "Espresso",
    "descriptionAr": "قهوة إسبرسو إيطالية قوية",
    "descriptionEn": "Strong Italian espresso",
    "isActive": true,
    "imageUrl": "/uploads/images/image.png",
    "categoryId": 1,
    "categoryNameAr": "قهوة",
    "categoryNameEn": "Coffee"
  }
]
```

**Error Response (404 Not Found):**
```json
"Category not found"
```

#### Create Product

Create a new product. You can optionally upload an image with the product creation, or upload it later using the image upload endpoint.

**Endpoint:** `POST /api/products`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
Content-Type: multipart/form-data
```

**Form Data:**
- `nameAr` (string, required): Arabic product name
- `nameEn` (string, required): English product name
- `descriptionAr` (string, optional): Arabic product description
- `descriptionEn` (string, optional): English product description
- `isActive` (boolean, optional): Active status (default: true)
- `categoryId` (integer, required): Category ID
- `imageFile` (file, optional): Product image file (jpg, jpeg, png, gif, webp)

**Example Request (using curl):**
```bash
curl -X POST http://localhost:5064/api/products \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "nameAr=منتج جديد" \
  -F "nameEn=New Product" \
  -F "descriptionAr=وصف المنتج" \
  -F "descriptionEn=Product description" \
  -F "categoryId=1" \
  -F "isActive=true" \
  -F "imageFile=@/path/to/image.png"
```

**Example Request (without image):**
```bash
curl -X POST http://localhost:5064/api/products \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "nameAr=منتج جديد" \
  -F "nameEn=New Product" \
  -F "categoryId=1"
```

**Success Response (201 Created):**
```json
{
  "id": 1,
  "nameAr": "منتج جديد",
  "nameEn": "New Product",
  "descriptionAr": "وصف المنتج بالعربية",
  "descriptionEn": "Product description in English",
  "isActive": true,
  "imageUrl": null,
  "categoryId": 1,
  "categoryNameAr": "قهوة",
  "categoryNameEn": "Coffee"
}
```

**Error Responses:**

*400 Bad Request - Category not found:*
```json
"Category not found"
```

*401 Unauthorized:*
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

#### Upload Product Image

Upload an image for a product.

**Endpoint:** `POST /api/products/{id}/image`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
Content-Type: multipart/form-data
```

**Path Parameters:**
- `id` (integer, required): Product ID

**Form Data:**
- `imageFile` (file, required): Image file (jpg, jpeg, png, gif, webp)

**Example Request (using curl):**
```bash
curl -X POST \
  http://localhost:5064/api/products/1/image \
  -H 'Authorization: Bearer {your-jwt-token}' \
  -F 'imageFile=@/path/to/image.png'
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "nameAr": "منتج جديد",
  "nameEn": "New Product",
  "descriptionAr": "وصف المنتج بالعربية",
  "descriptionEn": "Product description in English",
  "isActive": true,
  "imageUrl": "/uploads/images/guid.png",
  "categoryId": 1,
  "categoryNameAr": "قهوة",
  "categoryNameEn": "Coffee"
}
```

**Error Responses:**

*400 Bad Request - Invalid file:*
```json
"Error uploading image: Invalid file extension. Allowed: jpg, jpeg, png, gif, webp"
```

*404 Not Found:*
```json
"Product not found"
```

#### Update Product

Update an existing product. You can optionally upload a new image, which will replace the existing one.

**Endpoint:** `PUT /api/products/{id}`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
Content-Type: multipart/form-data
```

**Form Data:**
- `nameAr` (string, required): Arabic product name
- `nameEn` (string, required): English product name
- `descriptionAr` (string, optional): Arabic product description
- `descriptionEn` (string, optional): English product description
- `isActive` (boolean, required): Active status
- `categoryId` (integer, required): Category ID
- `imageFile` (file, optional): New product image file (replaces existing image)

**Path Parameters:**
- `id` (integer, required): Product ID

**Example Request (using curl):**
```bash
curl -X PUT http://localhost:5064/api/products/1 \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "nameAr=منتج محدث" \
  -F "nameEn=Updated Product" \
  -F "descriptionAr=وصف محدث" \
  -F "descriptionEn=Updated description" \
  -F "categoryId=2" \
  -F "isActive=true" \
  -F "imageFile=@/path/to/new-image.png"
```

**Example Request (without image update):**
```bash
curl -X PUT http://localhost:5064/api/products/1 \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "nameAr=منتج محدث" \
  -F "nameEn=Updated Product" \
  -F "categoryId=2" \
  -F "isActive=true"
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "nameAr": "منتج محدث",
  "nameEn": "Updated Product",
  "descriptionAr": "وصف محدث",
  "descriptionEn": "Updated description",
  "isActive": true,
  "imageUrl": "/uploads/images/image.png",
  "categoryId": 2,
  "categoryNameAr": "مشروبات ساخنة",
  "categoryNameEn": "Hot Beverages"
}
```

**Error Responses:**

*400 Bad Request:*
```json
"Category not found"
```

*404 Not Found:*
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

#### Delete Product

Delete a product and its associated image.

**Endpoint:** `DELETE /api/products/{id}`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Path Parameters:**
- `id` (integer, required): Product ID

**Example Request:**
```
DELETE /api/products/1
```

**Success Response (204 No Content):**
No response body

**Error Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

---

### Categories Endpoints

#### Get All Categories

Retrieve all categories.

**Endpoint:** `GET /api/categories`  
**Authentication:** Not required (Public)

**Example Request:**
```
GET /api/categories
```

**Success Response (200 OK):**
```json
[
  {
    "id": 1,
    "nameAr": "قهوة",
    "nameEn": "Coffee",
    "isActive": true
  },
  {
    "id": 2,
    "nameAr": "مشروبات ساخنة",
    "nameEn": "Hot Beverages",
    "isActive": true
  }
]
```

#### Get Category by ID

Retrieve a specific category by its ID.

**Endpoint:** `GET /api/categories/{id}`  
**Authentication:** Not required (Public)

**Path Parameters:**
- `id` (integer, required): Category ID

**Example Request:**
```
GET /api/categories/1
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "nameAr": "قهوة",
  "nameEn": "Coffee",
  "isActive": true
}
```

#### Create Category

Create a new category.

**Endpoint:** `POST /api/categories`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "nameAr": "فئة جديدة",
  "nameEn": "New Category",
  "isActive": true
}
```

**Success Response (201 Created):**
```json
{
  "id": 1,
  "nameAr": "فئة جديدة",
  "nameEn": "New Category",
  "isActive": true
}
```

#### Update Category

Update an existing category.

**Endpoint:** `PUT /api/categories/{id}`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
Content-Type: application/json
```

**Path Parameters:**
- `id` (integer, required): Category ID

**Request Body:**
```json
{
  "nameAr": "فئة محدثة",
  "nameEn": "Updated Category",
  "isActive": true
}
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "nameAr": "فئة محدثة",
  "nameEn": "Updated Category",
  "isActive": true
}
```

#### Delete Category

Delete a category.

**Endpoint:** `DELETE /api/categories/{id}`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Path Parameters:**
- `id` (integer, required): Category ID

**Example Request:**
```
DELETE /api/categories/1
```

**Success Response (204 No Content):**
No response body

---

### Users Endpoints

#### Get All Users

Retrieve all users.

**Endpoint:** `GET /api/users`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Example Request:**
```
GET /api/users
```

**Success Response (200 OK):**
```json
[
  {
    "id": 1,
    "username": "admin",
    "email": "admin@saracafe.com",
    "firstName": "Admin",
    "lastName": "User",
    "createdAt": "2024-01-01T00:00:00Z"
  }
]
```

#### Get User by ID

Retrieve a specific user by ID.

**Endpoint:** `GET /api/users/{id}`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Path Parameters:**
- `id` (integer, required): User ID

**Example Request:**
```
GET /api/users/1
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "username": "admin",
  "email": "admin@saracafe.com",
  "firstName": "Admin",
  "lastName": "User",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

#### Create User

Create a new user.

**Endpoint:** `POST /api/users`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "username": "newuser",
  "email": "user@example.com",
  "password": "SecurePassword123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Success Response (201 Created):**
```json
{
  "id": 2,
  "username": "newuser",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

**Error Responses:**

*409 Conflict:*
```json
"Username already exists"
```
or
```json
"Email already exists"
```

#### Update User

Update an existing user.

**Endpoint:** `PUT /api/users/{id}`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
Content-Type: application/json
```

**Path Parameters:**
- `id` (integer, required): User ID

**Request Body:**
```json
{
  "email": "updated@example.com",
  "firstName": "Jane",
  "lastName": "Smith",
  "password": "NewPassword123"
}
```

**Note:** Password is optional. If not provided, password will not be updated.

**Success Response (200 OK):**
```json
{
  "id": 1,
  "username": "admin",
  "email": "updated@example.com",
  "firstName": "Jane",
  "lastName": "Smith",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

#### Delete User

Delete a user.

**Endpoint:** `DELETE /api/users/{id}`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Path Parameters:**
- `id` (integer, required): User ID

**Example Request:**
```
DELETE /api/users/2
```

**Success Response (204 No Content):**
No response body

---

### Contacts Endpoints

#### Create Contact

Submit a contact form. This endpoint is public and does not require authentication.

**Endpoint:** `POST /api/contacts`  
**Authentication:** Not required (Public)

**Request Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "+1234567890",
  "message": "I would like to inquire about your menu options."
}
```

**Note:** Phone field is optional. You can omit it from the request.

**Success Response (201 Created):**
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "+1234567890",
  "message": "I would like to inquire about your menu options.",
  "createdAt": "2024-01-01T00:00:00Z",
  "isRead": false
}
```

#### Get All Contacts

Retrieve all contact form submissions. Contacts are ordered by creation date (newest first).

**Endpoint:** `GET /api/contacts`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Example Request:**
```
GET /api/contacts
```

**Success Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "phone": "+1234567890",
    "message": "I would like to inquire about your menu options.",
    "createdAt": "2024-01-01T00:00:00Z",
    "isRead": false
  },
  {
    "id": 2,
    "name": "Jane Smith",
    "email": "jane@example.com",
    "phone": null,
    "message": "Great service!",
    "createdAt": "2024-01-02T00:00:00Z",
    "isRead": true
  }
]
```

#### Get Contact by ID

Retrieve a specific contact form submission by ID. The contact will be automatically marked as read when viewed.

**Endpoint:** `GET /api/contacts/{id}`  
**Authentication:** Required

**Request Headers:**
```
Authorization: Bearer {your-jwt-token}
```

**Path Parameters:**
- `id` (integer, required): Contact ID

**Example Request:**
```
GET /api/contacts/1
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "phone": "+1234567890",
  "message": "I would like to inquire about your menu options.",
  "createdAt": "2024-01-01T00:00:00Z",
  "isRead": true
}
```

**Note:** The contact is automatically marked as read (`isRead: true`) when retrieved via this endpoint.

**Error Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

---

## Data Models

### Product

```typescript
{
  id: number;
  nameAr: string;           // Arabic name (required)
  nameEn: string;           // English name (required)
  descriptionAr?: string;   // Arabic description (optional)
  descriptionEn?: string;   // English description (optional)
  isActive: boolean;        // Active status (default: true)
  imageUrl?: string;        // Image URL (optional)
  categoryId: number;       // Category ID (required)
  categoryNameAr?: string;  // Category Arabic name (read-only)
  categoryNameEn?: string;  // Category English name (read-only)
}
```

### Category

```typescript
{
  id: number;
  nameAr: string;    // Arabic name (required)
  nameEn: string;    // English name (required)
  isActive: boolean; // Active status (default: true)
}
```

### User

```typescript
{
  id: number;
  username: string;    // Username (required, unique)
  email: string;       // Email (required, unique)
  firstName?: string;  // First name (optional)
  lastName?: string;   // Last name (optional)
  createdAt: string;   // Creation date (read-only, ISO 8601)
}
```

### Login Request

```typescript
{
  username: string; // Username
  password: string; // Password
}
```

### Login Response

```typescript
{
  token: string;  // JWT token
  user: User;     // User object
}
```

### Contact

```typescript
{
  id: number;
  name: string;        // Contact name (required)
  email: string;       // Email address (required, validated)
  phone?: string;      // Phone number (optional)
  message: string;     // Contact message (required, max 2000 chars)
  createdAt: string;   // Creation date (read-only, ISO 8601)
  isRead: boolean;     // Read status (read-only, auto-set to true when viewed)
}
```

### Create Contact Request

```typescript
{
  name: string;     // Contact name (required)
  email: string;    // Email address (required, validated)
  phone?: string;   // Phone number (optional)
  message: string;  // Contact message (required, max 2000 chars)
}
```

---

## Error Handling

The API uses standard HTTP status codes:

- `200 OK` - Request successful
- `201 Created` - Resource created successfully
- `204 No Content` - Request successful, no content to return
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Authentication required or invalid
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource conflict (e.g., duplicate username/email)
- `500 Internal Server Error` - Server error

### Error Response Format

Most errors return a simple string message:
```json
"Category not found"
```

Some errors return a structured response:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

---

## Setup & Configuration

### Prerequisites

- .NET 9.0 SDK
- MySQL Server 5.7+ or 8.0+
- IDE (Visual Studio, Rider, or VS Code)

### Installation Steps

1. **Clone the repository** (if applicable) or navigate to the project directory

2. **Configure the database connection**

   Update `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "server=127.0.0.1;port=3306;database=saracafe;user=root;password=yourpassword;"
     }
   }
   ```

3. **Configure JWT settings** (optional, defaults are provided)

   Update `appsettings.json`:
   ```json
   {
     "Jwt": {
       "Key": "your-secret-key-min-32-characters-long-for-security",
       "Issuer": "SaraCafe",
       "Audience": "SaraCafeUsers"
     }
   }
   ```

4. **Apply database migrations**

   ```bash
   cd SaraCafe.API
   dotnet ef database update
   ```

5. **Run the application**

   ```bash
   dotnet run
   ```

   The database will be automatically seeded on first startup with:
   - 5 categories (Coffee, Hot Beverages, Cold Beverages, Desserts, Snacks)
   - 21 sample products
   - 1 admin user (username: `admin`, password: `Admin@123`)

### Default Admin Credentials

- **Username:** `admin`
- **Password:** `Admin@123`
- **Email:** `admin@saracafe.com`

**⚠️ Important:** Change the default password after first login!

### Accessing Swagger Documentation

When running in Development mode, Swagger UI is available at:
```
http://localhost:5064/swagger
```

You can test all endpoints directly from Swagger UI. To test protected endpoints:
1. Use the `/api/users/login` endpoint to get a token
2. Click the "Authorize" button in Swagger UI
3. Enter: `Bearer {your-token-here}`
4. Now you can test protected endpoints

### Image Upload

Images are stored in: `wwwroot/uploads/images/`

Supported formats: JPG, JPEG, PNG, GIF, WEBP

Images are accessed via: `http://localhost:5064/uploads/images/{filename}`

---

## Examples

### Complete Flow Example

1. **Login to get a token:**
   ```bash
   curl -X POST http://localhost:5064/api/users/login \
     -H "Content-Type: application/json" \
     -d '{"username":"admin","password":"Admin@123"}'
   ```

2. **Create a category (using token from step 1):**
   ```bash
   curl -X POST http://localhost:5064/api/categories \
     -H "Authorization: Bearer YOUR_TOKEN_HERE" \
     -H "Content-Type: application/json" \
     -d '{"nameAr":"فئة جديدة","nameEn":"New Category","isActive":true}'
   ```

3. **Create a product (with image):**
   ```bash
   curl -X POST http://localhost:5064/api/products \
     -H "Authorization: Bearer YOUR_TOKEN_HERE" \
     -F "nameAr=منتج جديد" \
     -F "nameEn=New Product" \
     -F "descriptionAr=وصف" \
     -F "descriptionEn=Description" \
     -F "categoryId=1" \
     -F "isActive=true" \
     -F "imageFile=@/path/to/image.png"
   ```

   **Or create without image:**
   ```bash
   curl -X POST http://localhost:5064/api/products \
     -H "Authorization: Bearer YOUR_TOKEN_HERE" \
     -F "nameAr=منتج جديد" \
     -F "nameEn=New Product" \
     -F "categoryId=1" \
     -F "isActive=true"
   ```

4. **Upload product image separately (if not uploaded during creation):**
   ```bash
   curl -X POST http://localhost:5064/api/products/1/image \
     -H "Authorization: Bearer YOUR_TOKEN_HERE" \
     -F "imageFile=@/path/to/image.png"
   ```

5. **Get all products:**
   ```bash
   curl http://localhost:5064/api/products
   ```

6. **Submit a contact form (public, no auth required):**
   ```bash
   curl -X POST http://localhost:5064/api/contacts \
     -H "Content-Type: application/json" \
     -d '{
       "name":"John Doe",
       "email":"john@example.com",
       "phone":"+1234567890",
       "message":"I would like to inquire about your menu."
     }'
   ```

7. **Get all contacts (requires auth):**
   ```bash
   curl http://localhost:5064/api/contacts \
     -H "Authorization: Bearer YOUR_TOKEN_HERE"
   ```

---

## Notes

- All timestamps are in UTC and ISO 8601 format
- Arabic text is stored and returned as-is (UTF-8 encoded)
- The API supports CORS for cross-origin requests
- Image uploads are limited by the server's request size limits
- Passwords are hashed using SHA256 before storage
- JWT tokens expire after 24 hours

---

## Support

For issues or questions, please contact the development team.

---

**Last Updated:** January 2025  
**API Version:** 1.0

