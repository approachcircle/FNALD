using Godot;
using System;
using Sentry;

public partial class SentryController : Node
{
	/**
	 * no more UnhandledException event means no more Sentry.
	 */
	public override void _Ready()
	{
		throw new NotImplementedException("may be added in future");
	}

	public override void _Process(double delta)
	{
	}
}
