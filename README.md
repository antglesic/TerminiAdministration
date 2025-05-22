# Termini Administration

## Overview
Termini Administration is a web-based application designed to manage football (soccer) events, player rosters, and player performance ratings. The solution enables users to create and maintain a list of players, schedule football events (referred to as "Termins"), assign players to these events, and rate player performances after each event. The system then calculates and updates each player's overall rating based on their historical event ratings.

## Purpose
The primary goal of this application is to streamline the organization of football events and provide a transparent, data-driven way to track and evaluate player performance over time. It is ideal for amateur football groups, clubs, or organizations that want to:
- Maintain a centralized player database
- Schedule and manage recurring or one-off football events
- Assign players to events
- Rate player performances per event
- Calculate and display overall player ratings

## Solution Architecture
The solution is built using modern .NET technologies and follows a modular, layered architecture:

- **Blazor WebAssembly Frontend**: Provides a responsive, interactive UI for users to manage players, events, and ratings.
- **ASP.NET Core Web API**: Exposes endpoints for CRUD operations on players, events, and ratings, and handles business logic.
- **Entity Framework Core**: Manages data persistence and retrieval from the underlying database.
- **Service Layer**: Contains business logic for player management, event scheduling, and rating calculations.
- **Infrastructure Layer**: Handles external service communication, configuration, and shared utilities.

## .NET Aspire Orchestration & Dashboard
Termini Administration leverages the power of **.NET Aspire** for orchestration and monitoring. Aspire provides a modern, unified dashboard for managing, observing, and troubleshooting all services and dependencies in the solution. This orchestration layer simplifies local development, deployment, and diagnostics by:
- Automatically wiring up service dependencies
- Providing a real-time dashboard for health, logs, and metrics
- Enabling rapid iteration and improved developer productivity
- Continuously improving with new features from the .NET Aspire team

With Aspire, developers and operators gain deep visibility into the application's health and performance, making it easier to maintain and scale the system.

## Key Features
### 1. Player Management
- Add, edit, and delete players
- View player details and overall ratings

### 2. Event (Termin) Scheduling
- Create new football events with date, time, and duration
- Assign a list of players to each event

### 3. Player Ratings
- After an event, assign performance ratings to each participating player
- Automatically calculate and update each player's overall rating based on all their event ratings

### 4. Data Integrity & Logging
- Uses robust error handling and logging (via Serilog) for maintainability and troubleshooting

## How It Works
1. **Player Creation**: Users add players to the system, specifying details such as name, surname, sex, and preferred foot.
2. **Event Scheduling**: Users create a new event (Termin), specifying the date, start time, duration, and selecting players to participate.
3. **Rating Players**: After an event is finished, users can assign a score/rating to each player for that event.
4. **Overall Rating Calculation**: The system recalculates each player's overall rating as the average of all their ratings from past events.
5. **Viewing Data**: Users can view lists of players, events, and detailed statistics, including player ratings and event participation.

## Project Structure
- **TerminiWeb**: Blazor WebAssembly frontend for user interaction.
- **TerminiAPI**: ASP.NET Core Web API backend for business logic and data access.
- **TerminiService**: Contains core business logic and service interfaces/implementations.
- **TerminiWeb.Infrastructure**: Handles communication between frontend and backend, DTOs, and shared utilities.
- **TerminiDataAccess**: Entity Framework Core data models and database context.
- **TerminiDomain**: Core domain models and shared logic.
- **TerminiAdministration.AppHost**: Host project for running the application.
- **TerminiAdministration.ServiceDefaults**: Shared service configuration and defaults.

## Technologies Used
- **.NET 9**
- **Blazor WebAssembly**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **Serilog (logging)**
- **C# 13**

## What Still Needs to Be Done
- **Balanced Team Creation**: Implement a feature to automatically generate balanced teams for each event, using players' overall ratings to ensure fair and competitive matches when scheduling an event.

## Getting Started
1. **Clone the repository** 
git clone <your-repo-url>
2. **Set up the database**
Use the provided Entity Framework migrations or SQL scripts.
3. **Configure API endpoints and authentication**
Update configuration files as needed for your environment.
4. **Run the solution**
Start both the Blazor frontend and the API backend.
5. **Access the application**
Open your browser and navigate to the provided URL.

## Contributing
Contributions are welcome! Please open issues or submit pull requests for bug fixes, improvements, or new features.

## License
This project is licensed under the MIT License.