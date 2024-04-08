using Microsoft.Extensions.Options;
using Moedim.GenAI.Demos.Abstractions;

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
        _logger.LogInformation("Requested: {optionType}", _cliOptions.Type);

        var demo = _demos.FirstOrDefault(x => x.Name.Equals(_cliOptions?.Type?.Trim(), StringComparison.OrdinalIgnoreCase));

        await demo.RunAsync();
        
        // use this token for stopping the services
        _applicationLifetime.ApplicationStopping.ThrowIfCancellationRequested();

        return 0;
    }
}
