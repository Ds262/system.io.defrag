namespace System.IO.Defrag
{
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    public class Defrag
    {
        private Process ownprocess;
public void Stop()
        {
            ownprocess?.Kill();
        }
        private IDictionary<Operations, string> vOperations = new Dictionary<Operations, string>()
        {
            {Operations.Analyze, "/A "},{ Operations.BootOptimize,"/B "},{ Operations.Defrag ,"/D "},
            {Operations.Optimize ,"/O "},{ Operations.FreespaceConsolidate,"/X "},{ Operations.PrintProgress,"/U "},
            {Operations.Retrim,"/L " },{ Operations.SlabConsolidate ,"/K "},{ Operations.TierOptimize,"/G "},
            {Operations.TrackProgress ,"/T "},{ Operations.Verbose,"/V"}
        };
        private IDictionary<Options, string> vOptions = new Dictionary<Options, string>()
        {
            {Options.MultiThread , "/M" },{ Options.MaxRuntime, "I"},{ Options.NormalPriority , "/H"},
            { Options.None,""}
        };
        public enum Options
        {
            NormalPriority,
            MaxRuntime,
            MultiThread,
            None
        }
        public enum Operations
        {
            Analyze,
            BootOptimize,
            Defrag,
            TierOptimize,
            SlabConsolidate,
            Retrim,
            Optimize,
            TrackProgress,
            PrintProgress,
            Verbose,
            FreespaceConsolidate
        }
        private string English = "437";
        public event DataReceivedEventHandler OutputData;

        public T GetValue<T>(string data)
        {
            object o = new object();
            T a = (T)o;
            if (a is string)
            {
                Regex he = new Regex(": (.*?)% ");
                var j = he.Match(data);
                o = j.Groups[1].Value;
                a = (T)o;
            }
            else if (a is int)
            {
                Regex he = new Regex(": (.*?)% ");
                var j = he.Match(data);
                o = Convert.ToInt32(j.Groups[1].Value);
                a = (T)o;
            }
            else if (a is double)
            {
                Regex he = new Regex(": (.*?)% ");
                var j = he.Match(data);
                o = Convert.ToDouble(j.Groups[1].Value);
                a = (T)o;
            }
            else
            {
                MessageBox.Show("This format is not supported.",$"Error 0x{Encoding.UTF8.GetBytes(a.GetType().Name).Length}",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            return a;
        }
        private void cmd(string arg)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("cmd",$"/c chcp {English} && {arg}");
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (OutputData != null)
            {
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += OutputData;
                ownprocess = process;
                process.Start();
                process.BeginOutputReadLine();
            }else
            {
                ownprocess = process;
                process.Start();

            }
        }
        public void Start(string Volumes = "C:", Operations dOparations  = Operations.Defrag, Options options = Options.None)
        {
                cmd($"defrag {Volumes} {vOperations[dOparations]}{vOptions[options]}");

        }

    }

}
