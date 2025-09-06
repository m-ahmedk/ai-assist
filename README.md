# ProductDemo API - RAG Pipeline in .NET

This project demonstrates how to build a **Retrieval-Augmented Generation (RAG)** pipeline in **.NET 8 Web API** with:  
- **Postgres (pgvector)** as the vector database (running via Docker).  
- **OpenAI Embeddings (`text-embedding-3-small`)** for semantic retrieval.  
- **OpenAI GPT (`gpt-5-mini`)** for contextual answering.  
- **User Secrets** for secure storage of API keys and connection strings.  

---

## Features
- **Products API** with JWT authentication.  
- **Embeddings Service**: Converts product descriptions into vectors using OpenAI Embeddings API.  
- **Vector Search**: Finds semantically similar products using pgvector & cosine similarity.  
- **Prompt Service (RAG)**:  
  - Classifies queries (COUNT, MIN, MAX, AVERAGE, RANK, FUZZY).  
  - Handles deterministic queries with LINQ (e.g., cheapest, most expensive, total count).  
  - Uses GPT to phrase fuzzy queries (e.g., "healthy snacks", "show me drinks").  
- **Global Error Handling** with RFC 7807 `ProblemDetails`.  
- **Serilog Logging** for structured logs.  
- **Input Sanitization** for all incoming requests.  

---

## Tech Stack
- **.NET 8 Web API**  
- **Entity Framework Core**  
- **Postgres with pgvector** (Dockerized)  
- **OpenAI .NET SDK** (`text-embedding-3-small`, `gpt-5-mini`)  
- **FluentValidation** for DTO validation  
- **AutoMapper** for DTO <-> Entity mapping  
- **Serilog** for logging  

---

## Running Postgres with pgvector
Run Postgres with the pgvector extension using Docker:  

```
docker run -d --name pgvector \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=Ahmed_123$ \
  -e POSTGRES_DB=productdemo_ai \
  -p 5432:5432 \
  ankane/pgvector
```

Confirm connection via DBeaver or psql.

## User Secrets

This project uses .NET User Secrets to avoid exposing sensitive config.

Initialize secrets:
```
dotnet user-secrets init
```

Set secrets:
```
dotnet user-secrets set "Jwt:Key" "A1b2C3d4E5..."
dotnet user-secrets set "Jwt:Issuer" "ProductDemoAPI"
dotnet user-secrets set "Jwt:Audience" "ProductDemoClient"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=productdemo_ai;Username=postgres;Password=Ahmed_123$"
dotnet user-secrets set "OpenAI:ApiKey" "sk-xxxx"

```

## Endpoints
**Authentication**
- POST /api/auth/register -> Register user
- POST /api/auth/login -> Login and receive JWT

**Products**
- GET /api/products -> List all products (requires JWT)
- POST /api/products -> Create a new product (requires JWT Admin role)

**AI - Semantic Search**
- GET /api/ai/search?q=chocolate
- Uses vector search on embeddings
- Returns semantically relevant products

**AI - Ask (RAG)**
- POST /api/ai/ask?question=What's the cheapest product?
- Classifies intent (COUNT, MIN, MAX, AVERAGE, RANK, FUZZY)
- Deterministic queries answered with EF Core (e.g., cheapest product)
- Fuzzy queries (e.g., "healthy snacks") answered by GPT using provided product context

## How RAG Works Here
**Retrieval**
1. Each product is embedded with text-embedding-3-small.
2. Stored in Postgres with pgvector.
3. User query -> embedded -> cosine similarity search returns relevant products.

**Augmentation**
1. Retrieved products are passed to the Prompt Service.
2. For deterministic queries -> calculations are done in C# (EF Core/LINQ).
3. For fuzzy queries -> product context is injected into GPT.

**Generation**
1. GPT (gpt-5-mini) formats a human-readable answer, grounded only in retrieved products.

Together, this makes a full Retrieval-Augmented Generation pipeline in .NET.

## Example Queries
- "How many products are there?" -> COUNT -> "There are 19 products available."
- "What's the cheapest product?" -> MIN -> "The cheapest product is KitKat, priced at $0.90."
- "Show me some drinks." -> FUZZY -> GPT lists Coke, Pepsi, Sprite, etc."
- "Second most expensive product?" -> RANK -> "The second most expensive product is Monster Energy at $2.30."