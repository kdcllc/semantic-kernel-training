using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Planning.Handlebars;


using Moedim.GenAI.Demos.Abstractions;

namespace Moedim.GenAI.Demos.BasicDemos;

/// <summary>
/// Represents a demo class for the ChatTimePlugin.
/// </summary>
public class ChatTimePluginDemo(Kernel kernel) : BaseDemo(kernel)
{
#pragma warning disable SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    //private HandlebarsPlanner _planner = new HandlebarsPlanner(new HandlebarsPlannerOptions());

    /// <inheritdoc/>
    public override string Name => nameof(ChatTimePluginDemo);

    /// <inheritdoc/>
    public override string SystemMessage => "You're a AI assistant that only tells time, if other questions are asked reply with 'I only tell time'.";

    /// <inheritdoc/>
    public override string ScreenPrompt => "What do you want to know about time?";

    /// <inheritdoc/>
    protected override async Task<string?> HandlePromptAsync(Kernel kernel, string userPrompt, CancellationToken cancellationToken )
    {
        //const string promptDef = @"
        //    Today is: {{time.Date}}
        //    Current time is: {{time.Time}}

        //    Answer to the following questions using JSON syntax, including the data used.
        //    Is it morning, afternoon, evening, or night (morning/afternoon/evening/night)?
        //    Is it weekend time (weekend/not weekend)?
        //    ";

        //var promptTemplateFactory = new KernelPromptTemplateFactory();
        //var promptTemplate = promptTemplateFactory.Create(new PromptTemplateConfig(promptDef));
        //var renderedPrompt = await promptTemplate.RenderAsync(kernel);

        //// Run the prompt / prompt function
        //var kindOfDay = kernel.CreateFunctionFromPrompt(promptDef, new OpenAIPromptExecutionSettings() { MaxTokens = 100 });

        //return await kernel.InvokeAsync<string>(kindOfDay);

        //var settings = new OpenAIPromptExecutionSettings
        //{
        //    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        //};

        //var completionService = Services.GetRequiredService<IChatCompletionService>();

        //var result = await completionService.GetChatMessageContentAsync(userPrompt, kernel: kernel, executionSettings: settings);

        //return result.Content;



        //// Run the prompt / prompt function
        //var kindOfDay = kernel.CreateFunctionFromPrompt(
        //    userPrompt,
        //    new OpenAIPromptExecutionSettings()
        //    {
        //        MaxTokens = 100,
        //        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        //    });
        //var plugins = kernel.Plugins;
        //    var plugin = plugins.FirstOrDefault(p => p.Name == "ChatTimePlugin");
        //return await kernel.InvokeAsync<string>(kindOfDay);


        //"Creating plan".Dump(colors: new ColorConfig { PropertyValueColor = "Purple" });
        //var plan = await _planner.CreatePlanAsync(kernel, userPrompt);
        //plan.ToString().Dump(colors: new ColorConfig { PropertyValueColor = "Green" });
        //"Executing plan".Dump(colors: new ColorConfig { PropertyValueColor = "Purple" });
        //var result = await plan.InvokeAsync(kernel);
        //return result;

        // Get chat completion service
        var chatCompletionService = kernel?.GetRequiredService<IChatCompletionService>(nameof(ChatTimePluginDemo));

        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var history = new ChatHistory();

        // kernel.Plugins.Dump();

        history.AddUserMessage(userPrompt);

        // Get the response from the AI
        var result = await chatCompletionService!.GetChatMessageContentAsync(
                            History,
                            executionSettings: openAIPromptExecutionSettings,
                            kernel: kernel,
                            cancellationToken: cancellationToken);

        return result.Content;
    }
}
