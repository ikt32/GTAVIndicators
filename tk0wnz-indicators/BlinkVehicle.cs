using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace tk0wnz_indicators
{
    class BlinkVehicle
    {
	    private bool mIndicatorLeft = false;
	    private bool mIndicatorRight = false;
	    private bool mIndicatorHaz = false;
	    private bool rIndicatorLeft = false;
	    private bool rIndicatorRight = false;
	    private bool rIndicatorHaz = false;
	    private bool timerSet = false;
	    private DateTime Beginning = DateTime.Now;

		private BlinkerParams blinkerParams;

		public BlinkVehicle(Vehicle vehicle, BlinkerParams blinkerParams)
        {
            this.Vehicle = vehicle;
            this.blinkerParams = blinkerParams;
        }

        public Vehicle Vehicle;

        public void Update()
        {
	        if (blinkerParams.Duration == 0)
	        { // Using vanilla indicators
		        UpdateVanillaIndicators(this.Vehicle);
	        }
	        else if (blinkerParams.Duration > 0)
	        { // Using timed indicators by animations
		        UpdateAnimatedIndicators(this.Vehicle);
	        }
		}

		void UpdateVanillaIndicators(Vehicle vehicle)
		{
			UInt32 lightStates = vehicle.GetLightStates();
			if ((lightStates & 0x300) == 0x300)
			{
				//blinkyBois = "Hazard";
				if (!mIndicatorHaz && rIndicatorRight)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indilefton");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					mIndicatorHaz = true;
					rIndicatorHaz = true;
					rIndicatorRight = false;
					mIndicatorRight = false;
				}
				else if (!mIndicatorHaz && rIndicatorLeft)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirighton");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					mIndicatorHaz = true;
					rIndicatorHaz = true;
					rIndicatorLeft = false;
					mIndicatorLeft = false;
				}
				else if (!mIndicatorHaz)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazon");
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
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					mIndicatorLeft = true;
					rIndicatorLeft = true;
					rIndicatorHaz = false;
					mIndicatorHaz = false;
				}
				else if (!mIndicatorLeft && rIndicatorRight)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirtol");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					mIndicatorLeft = true;
					rIndicatorLeft = true;
					rIndicatorRight = false;
					mIndicatorRight = false;
				}
				else if (!mIndicatorLeft)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indilefton");
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
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					mIndicatorRight = true;
					rIndicatorRight = true;
					rIndicatorHaz = false;
					mIndicatorHaz = false;
				}
				else if (!mIndicatorRight && rIndicatorLeft)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indiltor");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					mIndicatorRight = true;
					rIndicatorRight = true;
					rIndicatorLeft = false;
					mIndicatorLeft = false;
				}
				else if (!mIndicatorRight)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirighton");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					mIndicatorRight = true;
					rIndicatorRight = true;
				}
			}
			else
			{
				if (rIndicatorLeft)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					rIndicatorLeft = false;
				}
				else if (rIndicatorRight)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					rIndicatorRight = false;
				}
				else if (rIndicatorHaz)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff");
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

		void UpdateAnimatedIndicators(Vehicle vehicle)
		{
			UInt32 lightStates = vehicle.GetLightStates();
			string blinkStateTxt = "???";

			bool playinghazoff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff", 3);
			bool playingloff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff", 3);
			bool playingroff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff", 3);
			bool playinghazon = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazon", 3);
			bool playinglon = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indilefton", 3);
			bool playingron = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirighton", 3);

			if ((lightStates & 0x300) == 0x300)
			{
				blinkStateTxt = "Hazard";
				if (rIndicatorRight)
				{
					if (!playingroff)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					}
					else
					{
						rIndicatorRight = false;
						timerSet = false;
					}
				}
				else if (rIndicatorLeft)
				{
					if (playingloff == false)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					}
					else
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
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazon");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
							mIndicatorHaz = true;
							timerSet = false;
						}
					}
					DateTime Now = DateTime.Now;
					TimeSpan Waitn = Now - Beginning;
					if (Waitn.TotalMilliseconds > blinkerParams.Duration)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazon");
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
				blinkStateTxt = "Left indicator";
				if (rIndicatorRight)
				{
					if (playingroff == false)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					}
					else
					{
						rIndicatorRight = false;
						timerSet = false;
					}
				}
				else if (rIndicatorHaz)
				{
					if (playinghazoff == false)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					}
					else
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
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indilefton");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
							mIndicatorLeft = true;
							timerSet = false;
						}
					}
					DateTime Now = DateTime.Now;
					TimeSpan Waitn = Now - Beginning;
					if (Waitn.TotalMilliseconds > blinkerParams.Duration)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indilefton");
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
				blinkStateTxt = "Right indicator";
				if (rIndicatorLeft)
				{
					if (playingloff == false)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					}
					else
					{
						rIndicatorLeft = false;
						timerSet = false;
					}
				}
				else if (rIndicatorHaz)
				{
					if (playinghazoff == false)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff");
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					}
					else
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
							Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirighton");
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
							mIndicatorRight = true;
							timerSet = false;
						}
					}
					DateTime Now = DateTime.Now;
					TimeSpan Waitn = Now - Beginning;
					if (Waitn.TotalMilliseconds > blinkerParams.Duration)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirighton");
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
				blinkStateTxt = "Off";

				if (rIndicatorLeft)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					rIndicatorLeft = false;
				}
				else if (rIndicatorRight)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					rIndicatorRight = false;
				}
				else if (rIndicatorHaz)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff");
					rIndicatorHaz = false;
				}
				else
				{
					float hazOffTime = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, vehicle,
						"va_" + blinkerParams.ModelName, "indihazoff");
					float lOffTime = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, vehicle,
						"va_" + blinkerParams.ModelName, "indileftoff");
					float rOffTime = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, vehicle,
						"va_" + blinkerParams.ModelName, "indirightoff");

					var pos = vehicle.Position;
					pos.Z += 2.0f;

					int currRoofState = Function.Call<int>(Hash.GET_CONVERTIBLE_ROOF_STATE, vehicle);

					if (blinkerParams.Debug == 1)
					{
						Utils.ShowText3D(pos, 10.0f, new List<string>
						{
							$"{currRoofState}",
							$"{playinghazoff} {hazOffTime}",
							$"{playingloff} {lOffTime}",
							$"{playingroff} {rOffTime}",
						});
					}


					mIndicatorLeft = false;
					mIndicatorRight = false;
					mIndicatorHaz = false;
					rIndicatorLeft = false;
					rIndicatorRight = false;
					rIndicatorHaz = false;
					timerSet = false;
				}
			}

			if (blinkerParams.Debug == 1)
			{
				var pos = vehicle.Position;
				pos.Z += 1.0f;
				Utils.ShowText3D(pos, 10.0f,
					new List<string>
					{
						$"0x{vehicle.GetLightStates():X} {blinkStateTxt}",
						$"Playing animation:  indihazoff: {playinghazoff}",
						$"Playing animation:  indileftoff: {playingloff}",
						$"Playing animation:  indirightoff: {playingroff}",
						$"Finished animation: indihazon: {playinghazon}",
						$"Finished animation: indilefton: {playinglon}",
						$"Finished animation: indirighton: {playingron}",
					});
			}
		}

	}
}
