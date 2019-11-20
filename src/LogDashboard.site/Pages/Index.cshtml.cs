using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LogDashboard.site.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Warn { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("info message");
            _logger.LogDebug("debug message");

            _logger.LogError("debug message", new ArgumentException("this is a test exception"));

            _logger.LogWarning("warn message");

            _logger.LogTrace("trace message");
        }

        public void OnPost()
        {
            _logger.LogWarning(Warn);

        }
    }
}
