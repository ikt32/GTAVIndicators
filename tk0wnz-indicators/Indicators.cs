using System;
using System.Collections.Generic;
using ExtensionMethods;
using GTA;
using GTA.Native;

internal class CarInfo
{
	public string ModelName;
	public int Duration;
	public int Debug;
}

public class BlinkerStates : Script
{
	private ScriptSettings settings;
	private List<CarInfo> carInfos = new List<CarInfo>();
	private Vehicle lastVehicle = null;
	private CarInfo currentCarInfo = null;
	private bool mIndicatorLeft = false;
	private bool mIndicatorRight = false;
	private bool mIndicatorHaz = false;
	private bool rIndicatorLeft = false;
	private bool rIndicatorRight = false;
	private bool rIndicatorHaz = false;
	private bool timerSet = false;
	private DateTime Beginning = DateTime.Now;

	public BlinkerStates()
	{
		Tick += OnTick;
		ReadIni();
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

			carInfos.Add(
				new CarInfo
				{
					ModelName = modelName,
					Duration = duration,
					Debug = debug
				}
			);
			++i;
		}
	}

	void OnTick(object sender, EventArgs e)
	{
		Ped playerPed = Game.Player.Character;
		Vehicle vehicle = playerPed.CurrentVehicle;

		if (vehicle != lastVehicle)
		{
			lastVehicle = vehicle;
			currentCarInfo = null;

			if (vehicle != null && vehicle.Exists())
			{
				currentCarInfo = carInfos.Find(x => Game.GenerateHash(x.ModelName) == vehicle.Model);
			}

			mIndicatorLeft = false;
			mIndicatorRight = false;
			mIndicatorHaz = false;
			rIndicatorLeft = false;
			rIndicatorRight = false;
			rIndicatorHaz = false;
		}

		if (playerPed.IsInVehicle() && currentCarInfo != null)
		{
			//ShowText(0.5f, 0.450f, string.Format($"Name: {currentCarInfo.ModelName}"));
			//ShowText(0.5f, 0.500f, string.Format($"Duration: {currentCarInfo.Duration}"));

			if (currentCarInfo.Duration == 0)
			{ // Using vanilla indicators
				UInt32 lightStates = vehicle.GetLightStates();
				if ((lightStates & 0x300) == 0x300)
				{
					//blinkyBois = "Hazard";
					if (!mIndicatorHaz && rIndicatorRight)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indilefton");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorHaz = true;
						rIndicatorHaz = true;
						rIndicatorRight = false;
						mIndicatorRight = false;
					}
					else if (!mIndicatorHaz && rIndicatorLeft)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirighton");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorHaz = true;
						rIndicatorHaz = true;
						rIndicatorLeft = false;
						mIndicatorLeft = false;
					}
					else if (!mIndicatorHaz)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indihazon");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorHaz = true;
						rIndicatorHaz = true;
					}
				}
				else if ((lightStates & 0x100) == 0x100)
				{
					//blinkyBois = "Left";
					if (!mIndicatorLeft && rIndicatorHaz)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirightoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorLeft = true;
						rIndicatorLeft = true;
						rIndicatorHaz = false;
						mIndicatorHaz = false;
					}
					else if (!mIndicatorLeft && rIndicatorRight)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirtol");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorLeft = true;
						rIndicatorLeft = true;
						rIndicatorRight = false;
						mIndicatorRight = false;
					}
					else if (!mIndicatorLeft)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indilefton");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorLeft = true;
						rIndicatorLeft = true;
					}
				}
				else if ((lightStates & 0x200) == 0x200)
				{
					//blinkyBois = "Right";
					if (!mIndicatorRight && rIndicatorHaz)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indileftoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorRight = true;
						rIndicatorRight = true;
						rIndicatorHaz = false;
						mIndicatorHaz = false;
					}
					else if (!mIndicatorRight && rIndicatorLeft)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indiltor");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorRight = true;
						rIndicatorRight = true;
						rIndicatorLeft = false;
						mIndicatorLeft = false;
					}
					else if (!mIndicatorRight)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirighton");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						mIndicatorRight = true;
						rIndicatorRight = true;
					}
				}
				else
				{
					if (rIndicatorLeft)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indileftoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						rIndicatorLeft = false;
					}
					else if (rIndicatorRight)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirightoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						rIndicatorRight = false;
					}
					else if (rIndicatorHaz)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indihazoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						rIndicatorHaz = false;
					}

					mIndicatorLeft = false;
					mIndicatorRight = false;
					mIndicatorHaz = false;

				}
				//ShowText(0.5f, 0.550f, string.Format("0x{0:X}", vehicle.GetLightStates()));
				//ShowText(0.5f, 0.600f, blinkyBois);
			}
			else if (currentCarInfo.Duration > 0)
			{ // Using timed indicators by animations
				UInt32 lightStates = vehicle.GetLightStates();
				string blinkyBois = "None";

				bool playinghazoff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indihazoff", 3);
				bool playingloff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indileftoff", 3);
				bool playingroff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirightoff", 3);
				bool finishedhazoff = Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, vehicle, "va_" + currentCarInfo.ModelName, "indihazoff", 3);
				bool finishedloff = Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, vehicle, "va_" + currentCarInfo.ModelName, "indileftoff", 3);
				bool finishedroff = Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, vehicle, "va_" + currentCarInfo.ModelName, "indirightoff", 3);

				if ((lightStates & 0x300) == 0x300)
				{
					blinkyBois = "Hazard lights are detected";
					if (rIndicatorRight)
					{
						if (playingroff == false && finishedroff == false)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirightoff");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						}
						else if (finishedroff == true)
						{
							rIndicatorRight = false;
							timerSet = false;
						}
					}
					else if (rIndicatorLeft)
					{
						if (playingloff == false && finishedloff == false)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indileftoff");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						}
						else if (finishedloff == true)
						{
							rIndicatorLeft = false;
							timerSet = false;
						}
					}
					else
					{
						if (timerSet == false)
						{
							Beginning = DateTime.Now;
							timerSet = true;
							if (!mIndicatorHaz) // first run starts directly
							{
								Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indihazon");
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
								mIndicatorHaz = true;
								timerSet = false;
							}
						}
						DateTime Now = DateTime.Now;
						TimeSpan Waitn = Now - Beginning;
						if (Waitn.TotalMilliseconds > currentCarInfo.Duration)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indihazon");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
							timerSet = false;

						}
						rIndicatorHaz = true;
						mIndicatorLeft = false;
						mIndicatorRight = false;
					}
				}
				else if ((lightStates & 0x100) == 0x100)
				{
					blinkyBois = "Left indicator is detected";
					if (rIndicatorRight)
					{
						if (playingroff == false && finishedroff == false)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirightoff");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						}
						else if (finishedroff == true)
						{
							rIndicatorRight = false;
							timerSet = false;
						}
					}
					else if (rIndicatorHaz)
					{
						if (playinghazoff == false && finishedhazoff == false)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indihazoff");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						}
						else if (finishedhazoff == true)
						{
							rIndicatorHaz = false;
							timerSet = false;
						}
					}
					else
					{
						if (timerSet == false)
						{
							Beginning = DateTime.Now;
							timerSet = true;
							if (!mIndicatorLeft) // first run starts directly
							{
								Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indilefton");
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
								mIndicatorLeft = true;
								timerSet = false;
							}
						}
						DateTime Now = DateTime.Now;
						TimeSpan Waitn = Now - Beginning;
						if (Waitn.TotalMilliseconds > currentCarInfo.Duration)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indilefton");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
							timerSet = false;

						}
						rIndicatorLeft = true;
						mIndicatorHaz = false;
						mIndicatorRight = false;
					}
				}
				else if ((lightStates & 0x200) == 0x200)
				{
					blinkyBois = "Right indicator is detected";
					if (rIndicatorLeft)
					{
						if (playingloff == false && finishedloff == false)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indileftoff");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						}
						else if (finishedloff == true)
						{
							rIndicatorLeft = false;
							timerSet = false;
						}
					}
					else if (rIndicatorHaz)
					{
						if (playinghazoff == false && finishedhazoff == false)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indihazoff");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						}
						else if (finishedhazoff == true)
						{
							rIndicatorHaz = false;
							timerSet = false;
						}
					}
					else
					{
						if (timerSet == false)
						{
							Beginning = DateTime.Now;
							timerSet = true;
							if (!mIndicatorRight) // first run starts directly
							{
								Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirighton");
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
								mIndicatorRight = true;
								timerSet = false;
							}
						}
						DateTime Now = DateTime.Now;
						TimeSpan Waitn = Now - Beginning;
						if (Waitn.TotalMilliseconds > currentCarInfo.Duration)
						{
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirighton");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
							timerSet = false;

						}
						rIndicatorRight = true;
						mIndicatorHaz = false;
						mIndicatorLeft = false;
					}
				}


				else
				{
					if (rIndicatorLeft)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indileftoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						rIndicatorLeft = false;
					}
					else if (rIndicatorRight)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indirightoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						rIndicatorRight = false;
					}
					else if (rIndicatorHaz)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + currentCarInfo.ModelName, "indihazoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
						rIndicatorHaz = false;
					}
					mIndicatorLeft = false;
					mIndicatorRight = false;
					mIndicatorHaz = false;
					rIndicatorLeft = false;
					rIndicatorRight = false;
					rIndicatorHaz = false;
					timerSet = false;

				}

				if (currentCarInfo.Debug == 1)
				{
					ShowText(0.5f, 0.450f, blinkyBois);
					ShowText(0.5f, 0.500f, string.Format("0x{0:X}", vehicle.GetLightStates()));
					ShowText(0.5f, 0.550f, "Currently playing animation indihazoff		: " + playinghazoff.ToString());
					ShowText(0.5f, 0.600f, "Currently playing animation indileftoff		: " + playingloff.ToString());
					ShowText(0.5f, 0.650f, "Currently playing animation indirightoff	: " + playingroff.ToString());
					ShowText(0.5f, 0.700f, "Finished playing animation: indihazoff		: " + finishedhazoff.ToString());
					ShowText(0.5f, 0.750f, "Finished playing animation: indileftoff		: " + finishedloff.ToString());
					ShowText(0.5f, 0.800f, "Finished playing animation: indirightoff	: " + finishedroff.ToString());
				}

			}
			else { }
		}
	}

	void ShowText(float x, float y, string text, float size = 0.5f)
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
