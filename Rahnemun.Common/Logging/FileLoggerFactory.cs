using System;
using Edreamer.Framework.Composition;
using Edreamer.Framework.Context;
using Edreamer.Framework.Logging;
using Edreamer.Framework.Media.Storage;

namespace Rahnemun.Common.Logging
{
    [PartPriority(PartPriorityAttribute.Maximum)]
    public class FileLoggerFactory: ILoggerFactory
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IWorkContextAccessor _workContextAccessor;

        public FileLoggerFactory(IStorageProvider storageProvider, IWorkContextAccessor workContextAccessor)
        {
            _storageProvider = storageProvider;
            _workContextAccessor = workContextAccessor;
        }

        public ILogger CreateLogger(Type type)
        {
            return new FileLogger(_storageProvider, _workContextAccessor);
        }
    }
}