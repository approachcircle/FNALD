using Godot;
using System;
using Sentry;
using Sentry.Protocol;

[Obsolete]
public partial class SentryController : Node
{
	/**
	 * no more UnhandledException event means no more Sentry.
	 */
	public override void _Ready()
	{
		SentrySdk.Init(options =>
		{
			options.Dsn = "https://5a893df96e44482d8f22be46de1b0e59@o4504855501602816.ingest.sentry.io/4504861693378560";
			options.TracesSampleRate = 1.0;
		});

		AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
		{
			SentrySdk.CaptureException((Exception) args.ExceptionObject);
		};
	}

	public override void _Process(double delta)
	{
	}
}
