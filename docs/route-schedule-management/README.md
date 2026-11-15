# Route Schedule Management

## Overview

Route Schedule Management is the bounded context responsible for configuring and publishing school transportation routes in Rumbo.

It manages route information, scheduled departure times, stops, student assignments, and route publication.

Trip execution and monitoring are managed by a separate bounded context.

## Architecture

This bounded context follows Domain-Driven Design (DDD), CQRS, and the layered architecture established in the Learning Center Platform reference.

The implementation is organized into four layers:

- **Domain:** Business entities, value objects, commands, queries, and repository contracts.
- **Application:** Command and query services that coordinate use cases.
- **Infrastructure:** Entity Framework Core configurations and repository implementations.
- **Interfaces:** REST controllers, resources, and assemblers.

## Implementation

### Commit 1 — Domain Foundations

The first implementation stage introduces the basic domain types required to construct the Route aggregate.

| File | Type | Responsibility |
|---|---|---|
| RouteId.cs | readonly record struct | Identifies a route. |
| DriverProfileId.cs | readonly record struct | References a driver profile. |
| VehicleId.cs | readonly record struct | References a vehicle. |
| StudentId.cs | readonly record struct | References a student. |
| ERouteShift.cs | enum | Defines morning and afternoon shifts. |
| ERouteStatus.cs | enum | Defines configurable and published route states. |
| RouteSchedule.cs | sealed record | Represents a scheduled day and departure time. |
| RouteStop.cs | class | Represents an ordered route stop. |
| RouteAssignment.cs | class | Associates a student with a route stop. |

These components form the foundation of the Route aggregate, which will be implemented in the following development stage.

## Technical Decisions

- Strongly typed identifiers follow the `IEntityId<int>` contract defined in Shared.
- External bounded contexts are referenced through identifiers without directly depending on their domain entities.
- RouteStop and RouteAssignment are modeled as child entities of the Route aggregate.
- RouteSchedule is an immutable value object.
- Route lifecycle states are explicitly represented by ERouteStatus.
- Domain rules will be enforced by the Route aggregate rather than REST controllers.

## API Endpoints

REST endpoints have not yet been implemented. This section will be completed when the Interfaces layer is developed.

## Persistence

Database persistence has not yet been implemented.

Entity Framework Core and MySQL configuration will be added in the Infrastructure stage, following the conventions of VehicleCredentialManagement.

## Deployment

The bounded context will be deployed as part of the Rumbo ASP.NET Core Web Services application on Render.

Deployment configuration and production validation will be documented after the API and persistence are implemented.
