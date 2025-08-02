# CORS Configuration Documentation

## Overview
This document describes the CORS (Cross-Origin Resource Sharing) configuration for the DigitekShop API, including setup, policies, and best practices.

## Configuration

### 1. CORS Settings in appsettings.json

```json
{
  "CorsSettings": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "http://localhost:4200",
      "http://localhost:8080",
      "http://localhost:5173"
    ],
    "AllowedMethods": [
      "GET",
      "POST",
      "PUT",
      "DELETE",
      "PATCH",
      "OPTIONS"
    ],
    "AllowedHeaders": [
      "Content-Type",
      "Authorization",
      "X-Requested-With",
      "Accept",
      "Origin"
    ],
    "AllowCredentials": true,
    "MaxAge": 86400
  }
}
```

### 2. CORS Policies

#### Development Policy
- **Name**: `DevelopmentPolicy`
- **Usage**: Development environment
- **Features**: 
  - Configurable origins from appsettings
  - All common HTTP methods
  - Standard headers
  - Credentials allowed
  - Preflight caching

#### Production Policy
- **Name**: `ProductionPolicy`
- **Usage**: Production environment
- **Features**:
  - Specific allowed origins
  - Restricted methods
  - Security-focused headers
  - Credentials allowed

#### Allow All Policy
- **Name**: `AllowAll`
- **Usage**: Testing only
- **Features**:
  - Any origin
  - Any method
  - Any header
  - **Warning**: Use only for testing

## Usage Examples

### 1. Global CORS Configuration

The CORS is configured globally in `Program.cs`:

```csharp
// Add CORS with configuration
builder.Services.AddCorsWithConfiguration(builder.Configuration);

// Use CORS with configuration
app.UseCorsWithConfiguration(app.Environment);
```

### 2. Controller-Level CORS

```csharp
[ApiController]
[Route("api/[controller]")]
[CorsPolicy] // Uses DevelopmentPolicy by default
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        // Your code here
    }
}
```

### 3. Action-Level CORS

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    [CorsPolicy("ProductionPolicy")]
    public async Task<IActionResult> GetProducts()
    {
        // Your code here
    }

    [HttpPost]
    [AllowAllCors] // For testing only
    public async Task<IActionResult> CreateProduct()
    {
        // Your code here
    }
}
```

### 4. Custom CORS Policy

```csharp
[ApiController]
[Route("api/[controller]")]
[EnableCors("CustomPolicy")]
public class AdminController : ControllerBase
{
    // Your code here
}
```

## Frontend Integration

### 1. React Example

```javascript
// API call with CORS
const fetchProducts = async () => {
  try {
    const response = await fetch('https://localhost:7001/api/products', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      credentials: 'include' // For cookies
    });
    
    const products = await response.json();
    return products;
  } catch (error) {
    console.error('Error fetching products:', error);
  }
};
```

### 2. Angular Example

```typescript
// API service with CORS
@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'https://localhost:7001/api/products';

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl, {
      withCredentials: true // For cookies
    });
  }
}
```

### 3. Vue.js Example

```javascript
// API call with CORS
const fetchProducts = async () => {
  try {
    const response = await axios.get('https://localhost:7001/api/products', {
      withCredentials: true,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      }
    });
    
    return response.data;
  } catch (error) {
    console.error('Error fetching products:', error);
  }
};
```

## Security Considerations

### 1. Production Settings

```json
{
  "CorsSettings": {
    "AllowedOrigins": [
      "https://yourdomain.com",
      "https://www.yourdomain.com"
    ],
    "AllowedMethods": [
      "GET",
      "POST",
      "PUT",
      "DELETE"
    ],
    "AllowedHeaders": [
      "Content-Type",
      "Authorization"
    ],
    "AllowCredentials": true,
    "MaxAge": 86400
  }
}
```

### 2. Security Best Practices

- **Never use `AllowAll` in production**
- **Specify exact origins instead of wildcards**
- **Limit allowed methods to what you need**
- **Use HTTPS in production**
- **Implement proper authentication**
- **Validate all incoming requests**

### 3. Common CORS Headers

```http
Access-Control-Allow-Origin: https://yourdomain.com
Access-Control-Allow-Methods: GET, POST, PUT, DELETE
Access-Control-Allow-Headers: Content-Type, Authorization
Access-Control-Allow-Credentials: true
Access-Control-Max-Age: 86400
```

## Troubleshooting

### 1. Common CORS Errors

#### Error: "No 'Access-Control-Allow-Origin' header"
**Solution**: Check if the origin is in the allowed origins list

#### Error: "Method not allowed"
**Solution**: Ensure the HTTP method is in the allowed methods list

#### Error: "Header not allowed"
**Solution**: Add the required header to the allowed headers list

### 2. Debugging CORS

Enable CORS logging in development:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseCorsLogging();
}
```

### 3. Testing CORS

```bash
# Test CORS with curl
curl -H "Origin: http://localhost:3000" \
     -H "Access-Control-Request-Method: POST" \
     -H "Access-Control-Request-Headers: Content-Type" \
     -X OPTIONS \
     https://localhost:7001/api/products
```

## Environment-Specific Configuration

### Development
- More permissive settings
- Multiple localhost origins
- CORS logging enabled
- Debug information

### Production
- Restrictive settings
- Specific domain origins
- Security-focused headers
- Minimal logging

### Staging
- Similar to production
- Test domain origins
- Full logging for debugging

## Migration Guide

### From Basic CORS to Advanced Configuration

1. **Update appsettings.json** with CORS settings
2. **Replace basic CORS setup** with extension methods
3. **Add CORS attributes** to controllers as needed
4. **Test thoroughly** in all environments
5. **Monitor logs** for CORS issues

## Performance Considerations

### 1. Preflight Caching
- Set appropriate `MaxAge` values
- Cache preflight responses
- Reduce OPTIONS requests

### 2. Header Optimization
- Only include necessary headers
- Minimize response size
- Use efficient header combinations

### 3. Origin Validation
- Validate origins efficiently
- Use hash-based lookups
- Cache validation results

## Monitoring and Logging

### 1. CORS Request Logging
```csharp
// Logs CORS requests in development
_logger.LogInformation("CORS Request: {Method} {Path} from {Origin}", 
    method, path, origin);
```

### 2. CORS Response Logging
```csharp
// Logs CORS response headers
_logger.LogDebug("CORS Response Headers: {@CorsHeaders}", corsHeaders);
```

### 3. Error Monitoring
- Monitor CORS-related errors
- Track blocked requests
- Alert on security issues

## Future Enhancements

1. **Dynamic CORS Configuration**: Runtime CORS policy updates
2. **Origin Validation Service**: External origin validation
3. **Rate Limiting**: CORS-specific rate limiting
4. **Analytics**: CORS request analytics
5. **Security Scanning**: Automated CORS security checks 