# 🎟️ Ticket Reservation System (.NET 10)

A high-performance, distributed ticket booking engine built with **.NET 10** and **Hexagonal Architecture**. This system is designed to handle "Flash Sale" scenarios where thousands of users compete for limited inventory across a horizontally scaled server cluster.

## 🎯 Project Focus & Intent
> **Note:** This repository is a technical demonstration. The **business logic is intentionally kept minimal** to maintain a clear focus on implementing **Hexagonal Architecture** and solving complex **Distributed Race Conditions** using Redis.

---

## 🏗️ Architecture: Hexagonal (Ports & Adapters)

The core motivation for choosing **Hexagonal Architecture** (also known as Ports and Adapters) is to ensure the **Domain Logic** remains the "Source of Truth," completely isolated from external technologies and infrastructure.


### Why this matters:
* **Technology Agnostic:** The business rules (e.g., "is this seat available?") are in a pure C# project with zero dependencies on Entity Framework, Redis, or Web APIs.
* **Highly Testable:** Since the Domain has no external dependencies, we can run high-speed unit tests without spinning up databases or web servers.
* **Future-Proofing:** Upgrading from .NET 10 to future versions or swapping your database (e.g., SQL Server to PostgreSQL) is seamless because changes are confined to the **Adapters** layer.

### 🛡️ Complete Isolation with Repository Pattern
To achieve strict separation of concerns, all database interaction is encapsulated within the **Repository Pattern**:
* **Persistence Ignorance:** The Core layer has no knowledge of SQL, Entity Framework, or Database schemas.
* **Interfaces (Ports):** Repository interfaces are defined in the Core layer.
* **Implementations (Adapters):** The actual database query logic is written in the [Database-Type].Adapter layer. This ensures that changing a query or a database provider never leaks into the business logic.

---

## ⚡ Solving Race Conditions in Distributed Systems

The biggest challenge in ticket reservations is the **"Double-Book" Problem**. If two users click "Book" at the exact same millisecond on two different servers, a standard database check might allow both to succeed, resulting in overselling.

### The Solution: Distributed Locking with Redlock
To prevent this, I implemented a locking mechanism using **Redis** and the **Redlock** algorithm.

1.  **Horizontal Scaling & No SPOF:** The backend is published across **multiple server instances** to ensure high availability and eliminate any Single Point of Failure (SPOF). 
2.  **The Distributed Lock:** Since local C# `lock` statements only work within a single process, I use Redis as a global synchronization layer.
3.  **Atomic Operations:** Before a reservation transaction begins, the system must acquire a global lock in Redis for that specific `TicketId`.
4.  **Consistency:** If Server A holds the lock, Server B's request is rejected or queued, ensuring that **only one process** can modify the ticket status at any given

---

## 🛠️ Tech Stack

* **Runtime:** .NET 10 (ASP.NET Core)
* **Architecture:** Hexagonal / Clean Architecture
* **Database:** Entity Framework Core (MSSQL)
* **Concurrency Control:** Redis (using `Redlock.net`)
* **DevOps:** Docker

---

## 🚦 Getting Started

### 1. Prerequisites
* .NET 10 SDK
* Docker Desktop

### 2. Setup Infrastructure
Spin up Redis instances:
```bash
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
