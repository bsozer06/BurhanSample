﻿using BurhanSample.API.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurhanSample.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILoggerManager _logger;


        public WeatherForecastController(ILoggerManager logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("Here is info message from our values controller."); 
            _logger.LogDebug("Here is debug message from our values controller."); 
            _logger.LogWarn("Here is warn message from our values controller."); 
            _logger.LogError("Here is an error message from our values controller.");

            return new string[] { "test1", "test2" };
        }


    }
}
