using BurhanSample.Core.Services.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Core.Services.Concrete
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger<ILoggerManager> _logger;

        public LoggerManager(ILogger<ILoggerManager> logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarn(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
