namespace BuildInformation
{
	class Build
	{
		public static int BuildIncrement { get; } = 114;
		public static string BuildDate { get; } = "21.11.2021 14:15:04";
		public static string Revision { get; } = "2b349d7";
		public static string RevisionControlSystem { get; } = "git";
#if RELEASE
		public static string BuildType { get; } = "Release";
#elif DEBUG
		public static string BuildType { get; } = "Debug";
#else
		public static string BuildType { get; } = "";
#endif
	}
}
