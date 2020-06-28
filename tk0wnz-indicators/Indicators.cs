using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
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
