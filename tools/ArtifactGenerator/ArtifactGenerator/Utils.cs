using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using log4net;
using log4net.Config;

namespace ArtifactGenerator
{
	public static class Utils
	{

		public static void InitLog()
		{
			var xmlDocument = new XmlDocument();
			try
			{
				if (Os.IsWindows())
					xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\log4net.config"));
				else
					xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/log4net.config"));
			}
			catch (Exception)
			{
				if (Os.IsWindows())
					xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\log4net.config"));
				else
					xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/log4net.config"));
			}
			XmlConfigurator.Configure(LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof (log4net.Repository.Hierarchy.Hierarchy)), xmlDocument["log4net"]);
		}

	}
	public static class Os
	{
		public static bool IsWindows()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		}

		public static bool IsMacOs()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		}

		public static bool IsLinux()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
		}

		public static string WhatIs()
		{
			var os = (IsWindows() ? "win" : null) ??
			         (IsMacOs() ? "mac" : null) ??
			         (IsLinux() ? "gnu" : null);
			return os;
		}
	}
}