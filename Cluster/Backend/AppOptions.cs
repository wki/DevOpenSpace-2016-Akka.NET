using CommandLine;
using CommandLine.Text;

namespace Backend
{
    public class AppOptions
    {
        [Option('p', "port", DefaultValue = null, HelpText = "port to listen on (choose 4056, 4057, 4058)")]
        public int? Port { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
