namespace BuildInformation
{
	class Build
	{
		public static int BuildIncrement { get; } = 107;
		public static string BuildDate { get; } = "19.11.2021 21:05:12";
		public static string Revision { get; } = "9ac8204";
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
