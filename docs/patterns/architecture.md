# nopCommerce Architecture

## Purpose

This document gives a straightforward view of how the repository is structured, how the event system works, and what that means for observability.

## 1. High level analysis

It's not split into separate services. Most behavior runs inside one application process, and plugins extend that same process rather than communicating over a message bus.

The architecture is modular, but it is not a strict clean boundary.

## 2. Main Layers

### Core

The core layer contains the foundational application blocks:

- shared abstractions
- configuration helpers
- engine and startup contracts
- common domain primitives
- base event contracts

### Data

The data layer handles persistence concerns:

- repositories
- database provider logic
- migrations
- query access

Its job is to move data in and out of storage, not to take care of business logic.

### Services

The services layer contains most business logic:

- pricing
- orders
- customers
- messages
- plugins
- scheduling
- caching
- event publishing implementation

### Web Framework

The web framework layer contains shared ASP.NET Core and MVC infrastructure:

- startup registration
- route and MVC helpers
- filters
- model events
- shared web utilities

### Web Application

The web app is the main storefront and admin host.

This is where the application is assembled, plugins are loaded, services are wired up.

### Plugins

Plugins are in-process extension modules.

They can:

- register services
- add controllers and views
- handle events
- extend menus and routes
- add scheduled behavior
- integrate with external systems

## 3. How Events Work Internally

The main pieces are:

- `IEventPublisher` publishes an event
- `IConsumer<T>` handles an event of type `T`

Consumers are resolved through dependency injection.

### What happens when an event is published

When code publishes an event, the system:

1. finds all consumers for that event type
2. resolves them from dependency injection
3. runs them one by one

- events are not queued (tho sequential)
- events are not durable
- events are not distributed
- handlers run as part of the same logical operation
