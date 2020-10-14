using System.Diagnostics;

namespace IWA.TTF.Taxonomy
{
	public static class SvcShell
	{
		public static string Bash(this string cmd, string path)
		{
			var escapedArgs = cmd.Replace("\"", "\\\"");
            
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "/bin/bash",
					Arguments = $"-c \"{escapedArgs}\"",
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true,
					WorkingDirectory = path
				}
			};
			process.Start();
			var result = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			return result;
		}
	}
}