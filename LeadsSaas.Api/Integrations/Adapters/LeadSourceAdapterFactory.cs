using Microsoft.Extensions.DependencyInjection;

namespace LeadsSaas.Api.Integrations.Adapters;

public interface ILeadSourceAdapterFactory
{
    ILeadSourceAdapter GetAdapter(string source);
}

public class LeadSourceAdapterFactory : ILeadSourceAdapterFactory
{
    private readonly IServiceProvider _serviceProvider;

    public LeadSourceAdapterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ILeadSourceAdapter GetAdapter(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentNullException(nameof(source));

        // normalizamos para evitar problemas de mayúsculas/minúsculas
        var normalized = source.Trim().ToLowerInvariant();

        switch (normalized)
        {
            case "googleads":
                return _serviceProvider.GetRequiredService<GoogleAdsLeadAdapter>();

            case "facebookads":
                return _serviceProvider.GetRequiredService<FacebookAdsLeadAdapter>();

            default:
                throw new InvalidOperationException(
                    $"No existe adapter configurado para la fuente '{source}'.");
        }
    }
}
