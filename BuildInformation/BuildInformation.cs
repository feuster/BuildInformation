namespace BuildInformation
{
	class Build
	{
		public static int BuildIncrement { get; } = 99;
		public static string BuildDate { get; } = "19.11.2021 20:42:46";
		public static string Revision { get; } = "2";
		public static string RevisionControlSystem { get; } = "git";
#if RELEASE
		public static string BuildType { get; } = "Release";
#elif TRACE
		public static string BuildType { get; } = "Trace";
#else
		public static string BuildType { get; } = "Debug";
#endif
	}
}
