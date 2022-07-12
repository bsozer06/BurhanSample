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

        /// <summary>
        /// Write the log message for the debug mode.
        /// Author=Burhan Sözer
        /// </summary>
        /// <param name="message"></param>
        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        /// <summary>
        /// Write the log message for the error sitatuations.
        /// Author=Burhan Sözer
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        /// <summary>
        /// Write the log message for informing to users.
        /// Author=Burhan Sözer
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        /// <summary>
        /// Write the log message for warning to users.
        /// Author=Burhan Sözer
        /// </summary>
        /// <param name="message"></param>
        public void LogWarn(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
