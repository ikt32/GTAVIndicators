using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExtensionMethods;
using GTA;
using GTA.Native;
using tk0wnz_indicators;

public class BlinkerStates : Script
{
	private ScriptSettings settings;
	private List<BlinkerParams> blinkerParamsList = new List<BlinkerParams>();

	private Timer timer;
	private List<Vehicle> vehicles;
	private List<BlinkVehicle> blinkVehicles;

	public BlinkerStates()
	{
		Tick += OnTick;
		ReadIni();

		timer = new Timer(1000);
		vehicles = new List<Vehicle>();
		blinkVehicles = new List<BlinkVehicle>();
	}

	void ReadIni()
	{
		settings = ScriptSettings.Load("scripts\\tk0wnz-indicators.ini");
		int i = 0;
		while (true)
		{
			string modelName = settings.GetValue("CARS", "ModelName" + i, "RESERVED_NO_VALUE");
			int duration = settings.GetValue("CARS", "Duration" + i, -1);
			int debug = settings.GetValue("CARS", "Debug" + i, -1);

			if (modelName == null || modelName == "RESERVED_NO_VALUE" || duration == -1)
			{
				break;
			}

			blinkerParamsList.Add(
				new BlinkerParams
				{
					ModelName = modelName,
					Duration = duration,
					Debug = debug
				}
			);
			++i;
		}
	}

	void UpdateVehicleCollection()
	{
		var allVehicles = World.GetAllVehicles();
		foreach (Vehicle vehicle in allVehicles)
		{
			BlinkerParams bla = blinkerParamsList.FirstOrDefault(x => Game.GenerateHash(x.ModelName) == vehicle.Model);
			if (bla == null)
				continue;

			vehicles.Add(vehicle);

			if (!blinkVehicles.Exists(x => x.Vehicle == vehicle))
				blinkVehicles.Add(new BlinkVehicle(vehicle, bla));
		}
	}

	void OnTick(object sender, EventArgs e)
	{
		if (timer.Expired())
		{
			timer.Reset();
			UpdateVehicleCollection();
		}

		List<BlinkVehicle> markForDelete = new List<BlinkVehicle>();

		foreach (var blinkVehicle in blinkVehicles)
		{
			if (!blinkVehicle.Vehicle.Exists())
			{
				markForDelete.Add(blinkVehicle);
				continue;
			}
			blinkVehicle.Update();
		}


		foreach (var vehToDelete in markForDelete)
		{
			blinkVehicles.Remove(vehToDelete);
		}
	}
}

namespace ExtensionMethods
{
	public static class Extensions
	{
		public static unsafe UInt32 GetLightStates(this Vehicle veh)
		{
			// uint32_t?
			const ulong LightStatesOffset = 0x928;
			return *(UInt32*)((ulong)veh.MemoryAddress + LightStatesOffset);
		}
	}
}

public static class Utils
{
	public static void ShowText(float x, float y, string text, float size = 0.5f)
	{
		Function.Call(Hash.SET_TEXT_FONT, 0);
		Function.Call(Hash.SET_TEXT_SCALE, size, size);
		Function.Call(Hash.SET_TEXT_COLOUR, 255, 255, 255, 255);
		Function.Call(Hash.SET_TEXT_WRAP, 0.0, 1.0);
		Function.Call(Hash.SET_TEXT_CENTRE, 0);
		Function.Call(Hash.SET_TEXT_OUTLINE, true);
		Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
		Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
		Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y);
	}
}