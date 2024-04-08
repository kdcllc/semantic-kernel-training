using Microsoft.Extensions.Options;
using Moedim.GenAI.Demos.Abstractions;

public class Main : IMain
{
    private readonly ILogger<Main> _logger;
    private readonly CliOptions _cliOptions;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IEnumerable<IDemo> _demos;

    public Main(
        IOptions<CliOptions> cliOptions,
        IEnumerable<IDemo> demos,
        IHostApplicationLifetime applicationLifetime,
        IConfiguration configuration,
        ILogger<Main> logger)
    {
        _cliOptions = cliOptions?.Value ?? throw new ArgumentNullException(nameof(cliOptions));
        _demos = demos ?? throw new ArgumentNullException(nameof(demos));
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IConfiguration Configuration { get; set; }

    public async Task<int> RunAsync()
    {
        _logger.LogInformation("Requested: {optionType}", _cliOptions.Type);

        var demo = _demos.FirstOrDefault(x => x.Name.Equals(_cliOptions?.Type?.Trim(), StringComparison.OrdinalIgnoreCase));

        await demo.RunAsync();
        
        // use this token for stopping the services
        _applicationLifetime.ApplicationStopping.ThrowIfCancellationRequested();

        return 0;
    }
}
