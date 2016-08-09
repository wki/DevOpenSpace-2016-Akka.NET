using CommandLine;
using CommandLine.Text;

namespace SeedNode
{
    public class AppOptions
    {
        [Option('p', "port", DefaultValue = null, HelpText = "port to listen on (choose 4053, 4054)")]
        public int? Port { get; set; }

        [Option('i', "ip", DefaultValue = null, HelpText = "IP Address to bind to")]
        public string IpAddress { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
