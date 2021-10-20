using System;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Client.Stores
{
    public class StoreBase<TModel>
    {
        private readonly ILogger _logger;

        public StoreBase(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public event Action OnChange;

        public TModel Model { get; private set; }

        public virtual void Set(TModel model)
        {
            Model = model;

            _logger.LogDebug("Store model '{modelType}' changed.", typeof(TModel).Name);
            NotifyStateChanged();
        }

        public virtual void Clear()
        {
            Model = default;

            _logger.LogDebug("Store model '{modelType}' cleared.", typeof(TModel).Name);
            NotifyStateChanged();
        }

        public virtual void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}