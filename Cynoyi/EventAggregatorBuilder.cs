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
                _eventHandlerFactory = new WeakEventHandlerFactory();
            return new EventAggregator(_eventHandlerFactory);
        }
    }
}