using Nop.Core.Events;
using Nop.Services.Events;

namespace Nop.Web.Framework.Infrastructure.OpenTelemetry;

public class TracingEventPublisher : EventPublisher
{
    public override async Task PublishAsync<TEvent>(TEvent @event)
    {
        var eventType = typeof(TEvent);
        var eventTypeName = eventType.Name;

        using var activity = CatalogTracingHelper.ActivitySource
            .StartActivity($"{CatalogTracingHelper.EventPublishActivityName} {eventTypeName}");
        activity?.SetTag(CatalogTracingHelper.EventTypeTagName, eventType.FullName ?? eventTypeName);

        try
        {
            await base.PublishAsync(@event);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
    }
}
