namespace AvalonAssets.Cynoyi
{
    public class EventAggregatorBuilder
    {
        private IEventHandlerFactory _eventHandlerFactory;

        public EventAggregatorBuilder SetEventHandlerFactory(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
            return this;
        }

        public IEventAggregator Build()
        {
            if (_eventHandlerFactory == null)
                _eventHandlerFactory = new ReflectEventHandlerFactory();
            return new EventAggregator(_eventHandlerFactory);
        }
    }
}