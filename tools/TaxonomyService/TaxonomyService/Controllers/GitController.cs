using System;
using System.Reflection;
using log4net;
using IWA.TTF.Taxonomy.Model.Artifact;

namespace IWA.TTF.Taxonomy.Controllers
{
	public static class GitController
	{
		
		private static readonly ILog _log;

		static GitController()
		{
			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
		
		internal static CommitUpdatesResponse Commit(string commitNotes)
		{
			_log.Info("Commit: " + commitNotes);
			try{
				
				var gitCmd = "git commit -m " + commitNotes;
				
				var results = gitCmd.Bash(TxService.ArtifactPath);
				_log.Info(results);
				return new CommitUpdatesResponse
				{
					Result = "true"
				};
			}
			catch (Exception e)
			{
				var error = "Exception during commit:  " + commitNotes + ": " + e;
				_log.Error(error);
				return new CommitUpdatesResponse
				{
					Result = error
				};
			}

		}
		
		internal static IssuePullResponse Pull()
		{
			_log.Info("Pull-Request: ");
			try{
				
				var gitCmd = "git pull-request";
				
				var results = gitCmd.Bash(TxService.ArtifactPath);
				_log.Info(results);
				return new IssuePullResponse
				{
					Response = "true"
				};
			}
			catch (Exception e)
			{
				var error = "Exception during pull-request: " + e;
				_log.Error(error);
				return new IssuePullResponse
				{
					Response = error
				};
			}
		}

		internal static ServiceConfiguration GetConfig()
		{
			return new ServiceConfiguration
			{
				GitId = "Not implemented yet."
			};
			/*
			const string gitBranch = "git branch | grep \\* | cut -d ' ' -f2";
			try
			{
				var retVal = new ServiceConfiguration
				{
					ReadOnly = TxService.ReadOnlyMode,
					GitId = TxService.GitId,
					GitBranch = gitBranch.Bash(TxService.ArtifactPath)
				};

				return retVal;
			}
			catch (Exception e)
			{
				return new ServiceConfiguration
				{
					GitId = e.Message
				};
			}
			*/
		}
		
		internal static void SetCredential()
		{
			//https://help.github.com/en/articles/caching-your-github-password-in-git
		}
		
	}
}