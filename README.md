# RivaDemo

## Overview
RivaDemo is a simple batch synchronization system designed to process multiple sync jobs for CRM users across different platforms. It uses a modular, testable architecture leveraging dependency injection, interfaces, and clear separation of concerns.

## Architecture Summary
- Models:
  - SyncJob and CrmUser represent the core domain data.  
  - Models are clean and extensible, avoiding redundancy.

- Services:  
  - BatchSyncProcessor handles batch processing of sync jobs.  
  - ISyncValidator interface defines validation rules; implemented by SimpleTokenValidator to validate CRM tokens.

- Dependency Injection: 
  - Services and seed data are registered via Microsoft.Extensions.Hosting DI container.  
  - This improves modularity, testability, and maintainability.

## Key Features
- Processes jobs independently, marking success or failure with error messages.
- Validates jobs before processing.
- Clear console output for each job's status.
- Easily extendable validation and processing logic.

## How to Run
1. Make sure you have .NET 8.0 installed.
2. Open the solution in your IDE Visual Studio.
3. Run the project to see console output for batch processing.

## Folder Structure
RivaDemo/
│
├── Models/ # Domain models: SyncJob, CrmUser
├── Services/ # Business logic & validation services
├── Configuration/ # Input seed data (static job data)
├── Program.cs # Entry point and DI configuration
└── RivaDemo.csproj # Project file





# TestProject

## Overview
Unit tests for the RivaDemo project ensuring correctness of batch sync processing logic and validation rules.

## Testing Approach
- Data-driven tests using JSON seed data for realistic scenarios.
- Tests focus on validating:
  - Token presence and validity.
  - Correct status updates on sync jobs.
  - Proper error message propagation.

## Folder Structure
TestProject/
│
├── TestCases/ # JSON files with test seed data
├── TestClass/ # NUnit test classes
└── TestProject.csproj # Test project file


## Running Tests
1. Ensure you have NUnit and NUnit3TestAdapter installed.
2. Run tests via your IDE Test Explorer or command line:
