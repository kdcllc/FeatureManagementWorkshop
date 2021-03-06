﻿using System.Diagnostics;
using System.Threading.Tasks;

using FeatureManagement.Core;

using FeatureManagementWeb.Models;
using FeatureManagementWeb.Options;
using FeatureManagementWeb.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace FeatureManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFeatureManagerSnapshot _featureSnapshot;
        private readonly ConfigurationService _configurationService;
        private readonly AppOptions _options;

        public HomeController(
            ILogger<HomeController> logger,
            IFeatureManagerSnapshot featureSnapshot,
            ConfigurationService configurationService,
            IOptionsSnapshot<AppOptions> options)
        {
            _logger = logger;
            _featureSnapshot = featureSnapshot;
            _configurationService = configurationService;
            _options = options.Value;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Message"] = $"Welcome";

            ViewData["BackgroundColor"] = _options.BackgroundColor;
            ViewData["OptionsMessage"] = _options.Message + " Is Refreshed Configurations: " + _configurationService.Referesh();

            if (await _featureSnapshot.IsEnabledAsync(nameof(FeatureFlags.BrowserRenderer)))
            {
                ViewData["Message"] = $"Welcome! You can see this message only if '{nameof(FeatureFlags.BrowserRenderer)}' is enabled.";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
