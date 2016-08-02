using CommandLine;
using CommandLine.Text;

namespace DeployActors
{
    class AppOptions
    {
        // [Option('m', "monitor", DefaultValue = false, HelpText = "start as monitor node")]
        // public bool MonitorNode { get; set; }

        [Option('p', "port", DefaultValue = 0, HelpText = "port to listen on")]
        public int Port { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
