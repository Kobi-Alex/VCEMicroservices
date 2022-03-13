using System;


namespace Report.Domain.SeedWork
{
    // That interface is an empty interface, sometimes called a marker interface, that is used just
    // to indicate that this entity class is also an aggregate root.
    // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/net-core-microservice-domain-model
    public interface IAggregateRoot {}
}
