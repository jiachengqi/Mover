## Technologies Used

### Backend:
- ASP.NET Core 8.0
- Google Routes Directions API
- Polly for resilience and transient-fault handling
- Docker for containerization

### Frontend:
- Blazor WebAssembly
- [BlazorGoogleMaps](https://github.com/rungwiroon/BlazorGoogleMaps) for Google Maps integration
- Radzen.Blazor for UI components

### Shared:
- Shared Models between backend and frontend

### Others:
- Docker Compose for orchestrating multi-container applications

## Prerequisites
- **Docker**: Ensure Docker is installed on your machine. [Download Docker](https://www.docker.com/products/docker-desktop)
- **Docker Compose**: Comes bundled with Docker Desktop.
- **.NET 8.0 SDK**: Required if you plan to build or run the projects locally without Docker. [Download .NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Port**: Make sure port 5253, 5258, 8080 are not in use

## Setup and Installation

### Clone the Repository
```bash
git clone https://github.com/jiachengqi/Mover.git
cd mover
```

### Build and Run with Docker
1. **Build Docker Images and Start Containers**:
   ```bash
   docker-compose up --build
   ```

   This command will:
   - Build the backend and frontend Docker images.
   - Start the backend, frontend, and Nginx proxy containers.
   - Expose the backend on `http://localhost:5258`, frontend on `http://localhost:5253`, and proxy on `http://localhost:8080`.

2. **Access the Application**:
   - Open your browser and navigate to [http://localhost:8080](http://localhost:8080) to access the frontend via the Nginx proxy.

## Running the Application
Once the Docker containers are up and running:

1. **Frontend**: Access the Blazor application at [http://localhost:8080](http://localhost:8080).
2. **Swagger UI**: Access the backend API documentation at [http://localhost:5258/swagger](http://localhost:5258/swagger) for testing API endpoints.

## API Documentation

### Route Optimization Endpoint
- **URL**: `/api/RouteOptimization/optimize`
- **Method**: `POST`
- **Description**: Optimizes the route based on the provided list of delivery addresses.

#### Request
- **Headers**: `Content-Type: application/json`
- **Body**: JSON array of address strings.

```json
[
  "1600 Amphitheatre Parkway, Mountain View, CA",
  "1 Infinite Loop, Cupertino, CA"
]
```

## Optional: Configuring the Google API Key
To integrate Google Maps services into the Mover application, you need to configure the Google API Key in three key files:

1. `backend/appsettings.yaml`
2. `frontend/Program.cs`

For demonstration purposes, the API key will be hardcoded directly into these files. **Note**: In a production environment, it is highly recommended to use a secret manager to securely handle API keys.

### Best Practices
While hardcoding API keys is acceptable for demonstration purposes, it's important to follow best practices for managing sensitive information in production environments:

1. **Use Secret Managers**: Utilize secret management tools like Azure Key Vault, AWS Secrets Manager, or HashiCorp Vault to store and manage API keys securely.
2. **Environment Variables**: Store API keys in environment variables and access them within the application code.
