using Godot;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Global
{
	public enum CamerasState
	{
		Up,
		Down
	}
	public static string DataPath { get; } = "user://fnald.json";
	public static CamerasState Cameras { get; set; } = CamerasState.Down;
	public static bool SkippedToOffice { get; set; } = false;
	public static int Time { get; set; } = 12;
	public static int DisplayedAnger { get; set; } = 0;
	public static int AngerRate { get; set; } = 1;
	public static int StartInternalAnger { get; } = 48000;
	public static int InternalAnger { get; set; } = StartInternalAnger;
	public static bool PlayerDisabled { get; set; } = false;
	public static bool PrematurePhoneEnd { get; set; } = false;
	public static bool PhoneActive { get; set; } = false;
	public static double RemainingTime { get; set; } = 0;
	public static bool CamerasEnabled { get; set; } = true;
	public static bool Distracted { get; set; } = false;
	public static bool MonstersEnabled { get; set; } = true;
	public static bool MidDoorClosed { get; set; } = false;
	public static bool LeftDoorClosed { get; set; } = false;
	public static bool KnockingMid { get; set; } = false;
	public static bool KnockingLeft { get; set; } = false;
	public static AttackType AttackType { get; set; }

    public static void SetNight(long night)
	{
		using (FileAccess file = FileAccess.Open(DataPath, FileAccess.ModeFlags.Write))
		{
			var dict = new Godot.Collections.Dictionary<string, long>
			{
				{ "night", night }
			};
			file.StoreString(Json.Stringify(dict));
		}   
	}

	public static long GetNight()
	{
		if (!FileAccess.FileExists(DataPath))
			SetNight(1);
        using FileAccess file = FileAccess.Open(DataPath, FileAccess.ModeFlags.Read);
        Variant data = Json.ParseString(file.GetAsText());
        if (data.VariantType is Variant.Type.Nil)
            SetNight(1);
        return data.AsGodotDictionary<string, long>().GetValueOrDefault("night");
    }

	public static void ModulateNodeAlpha(CanvasItem node, float alpha)
	{
		Color colour = node.Modulate;
		colour.A = alpha;
		node.Modulate = colour;
	}
}
