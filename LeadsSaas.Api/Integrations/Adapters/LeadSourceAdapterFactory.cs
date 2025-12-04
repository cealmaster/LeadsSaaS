namespace LeadsSaas.Api.Integrations.Adapters
{
    public interface ILeadSourceAdapterFactory
    {
        ILeadSourceAdapter GetAdapter(string source);
    }

    public class LeadSourceAdapterFactory : ILeadSourceAdapterFactory
    {
        private readonly IEnumerable<ILeadSourceAdapter> _adapters;

        public LeadSourceAdapterFactory(IEnumerable<ILeadSourceAdapter> adapters)
        {
            _adapters = adapters;
        }

        public ILeadSourceAdapter GetAdapter(string source)
        {
            var adapter = _adapters.FirstOrDefault(a =>
                a.SourceName.Equals(source, StringComparison.OrdinalIgnoreCase));

            if (adapter == null)
                throw new InvalidOperationException($"No existe adapter configurado para la fuente '{source}'.");

            return adapter;
        }
    }

}
