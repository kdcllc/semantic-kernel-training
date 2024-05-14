using Dumpify;

using Microsoft.Extensions.Options;

using Moedim.GenAI.Demos.Abstractions;

using System.Drawing;

public class Main(
    IOptions<CliOptions> cliOptions,
    IEnumerable<IDemo> demos,
    IHostApplicationLifetime applicationLifetime,
    IConfiguration configuration,
    ILogger<Main> logger) : IMain
{
    private readonly ILogger<Main> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly CliOptions _cliOptions = cliOptions?.Value ?? throw new ArgumentNullException(nameof(cliOptions));
    private readonly IHostApplicationLifetime _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
    private readonly IEnumerable<IDemo> _demos = demos ?? throw new ArgumentNullException(nameof(demos));

    public IConfiguration Configuration { get; set; } = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public async Task<int> RunAsync()
    {
        if (DisplayAllDemos())
        {
            return -1;
        }

        _logger.LogInformation("Requested demo: {optionType}", _cliOptions.Type);

        var demo = _demos.FirstOrDefault(x => x.Name.Equals(_cliOptions?.Type?.Trim(), StringComparison.OrdinalIgnoreCase));

        if (demo != null)
        {
            var cancellationToken = _applicationLifetime.ApplicationStopping;
            await demo.RunAsync(cancellationToken);
            return 0;
        }

        return -1;
    }

    private bool DisplayAllDemos()
    {
        if (string.IsNullOrEmpty(_cliOptions.Type))
        {
            // display all of the possible demos
            "No demo requested. Available demos:".Dump(colors: new ColorConfig { PropertyValueColor = Color.Red });

            foreach (var d in _demos)
            {
                d.Name.Dump(colors: new ColorConfig { PropertyValueColor = Color.Red });
            }

            _logger.LogWarning("No demo requested.");

            return true;
        }

        return false;
    }
}
