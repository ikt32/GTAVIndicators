using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace tk0wnz_indicators
{
    class BlinkVehicle
    {
	    public Vehicle Vehicle;
	    private BlinkerParams blinkerParams;

	    private string animLib;

		private bool mIndicatorLeft = false;
	    private bool mIndicatorRight = false;
	    private bool mIndicatorHaz = false;
	    private bool rIndicatorLeft = false;
	    private bool rIndicatorRight = false;
	    private bool rIndicatorHaz = false;
	    private bool timerSet = false;
	    private DateTime Beginning = DateTime.Now;

		
		public BlinkVehicle(Vehicle vehicle, BlinkerParams blinkerParams)
        {
            this.Vehicle = vehicle;
            this.blinkerParams = blinkerParams;
            animLib = "va_" + blinkerParams.ModelName;
        }


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
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indilefton", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorHaz = true;
					rIndicatorHaz = true;
					rIndicatorRight = false;
					mIndicatorRight = false;
				}
				else if (!mIndicatorHaz && rIndicatorLeft)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirighton", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorHaz = true;
					rIndicatorHaz = true;
					rIndicatorLeft = false;
					mIndicatorLeft = false;
				}
				else if (!mIndicatorHaz)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indihazon", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorHaz = true;
					rIndicatorHaz = true;
				}
			}
			else if ((lightStates & 0x100) == 0x100)
			{
				//blinkyBois = "Left";
				if (!mIndicatorLeft && rIndicatorHaz)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirightoff", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorLeft = true;
					rIndicatorLeft = true;
					rIndicatorHaz = false;
					mIndicatorHaz = false;
				}
				else if (!mIndicatorLeft && rIndicatorRight)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirtol", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorLeft = true;
					rIndicatorLeft = true;
					rIndicatorRight = false;
					mIndicatorRight = false;
				}
				else if (!mIndicatorLeft)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indilefton", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorLeft = true;
					rIndicatorLeft = true;
				}
			}
			else if ((lightStates & 0x200) == 0x200)
			{
				//blinkyBois = "Right";
				if (!mIndicatorRight && rIndicatorHaz)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indileftoff", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorRight = true;
					rIndicatorRight = true;
					rIndicatorHaz = false;
					mIndicatorHaz = false;
				}
				else if (!mIndicatorRight && rIndicatorLeft)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indiltor", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorRight = true;
					rIndicatorRight = true;
					rIndicatorLeft = false;
					mIndicatorLeft = false;
				}
				else if (!mIndicatorRight)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirighton", animLib, 8.0f, false, false, 0, 0, 0.0f);

					mIndicatorRight = true;
					rIndicatorRight = true;
				}
			}
			else
			{
				if (rIndicatorLeft)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indileftoff", animLib, 8.0f, false, false, 0, 0, 0.0f);

					rIndicatorLeft = false;
				}
				else if (rIndicatorRight)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirightoff", animLib, 8.0f, false, false, 0, 0, 0.0f);

					rIndicatorRight = false;
				}
				else if (rIndicatorHaz)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indihazoff", animLib,  8.0f, false, false, 0, 0, 0.0f);

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

			bool playinghazoff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, animLib, "indihazoff", 3);
			bool playingloff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, animLib, "indileftoff", 3);
			bool playingroff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, animLib, "indirightoff", 3);
			bool playinghazon = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, animLib, "indihazon", 3);
			bool playinglon = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, animLib, "indilefton", 3);
			bool playingron = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, animLib, "indirighton", 3);

			if ((lightStates & 0x300) == 0x300)
			{
				blinkStateTxt = "Hazard";
				if (rIndicatorRight)
				{
					if (!playingroff)
					{
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirightoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indileftoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
							Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indihazon", animLib, 8.0f, false, false, 0, 0, 0.0f);
		
							mIndicatorHaz = true;
							timerSet = false;
						}
					}
					DateTime Now = DateTime.Now;
					TimeSpan Waitn = Now - Beginning;
					if (Waitn.TotalMilliseconds > blinkerParams.Duration)
					{
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indihazon", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirightoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indihazoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
							Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indilefton", animLib, 8.0f, false, false, 0, 0, 0.0f);
							mIndicatorLeft = true;
							timerSet = false;
						}
					}
					DateTime Now = DateTime.Now;
					TimeSpan Waitn = Now - Beginning;
					if (Waitn.TotalMilliseconds > blinkerParams.Duration)
					{
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indilefton", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indileftoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indihazoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
							Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirighton", animLib, 8.0f, false, false, 0, 0, 0.0f);
							mIndicatorRight = true;
							timerSet = false;
						}
					}
					DateTime Now = DateTime.Now;
					TimeSpan Waitn = Now - Beginning;
					if (Waitn.TotalMilliseconds > blinkerParams.Duration)
					{
						Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirighton", animLib, 8.0f, false, false, 0, 0, 0.0f);
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
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indileftoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
					rIndicatorLeft = false;
				}
				else if (rIndicatorRight)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indirightoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
					rIndicatorRight = false;
				}
				else if (rIndicatorHaz)
				{
					Function.Call(Hash.PLAY_ENTITY_ANIM, vehicle, "indihazoff", animLib, 8.0f, false, false, 0, 0, 0.0f);
					rIndicatorHaz = false;
				}
				else
				{
					if (blinkerParams.Debug == 1)
					{
						float hazOffTime = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, vehicle,
							animLib, "indihazoff");
						float lOffTime = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, vehicle,
							animLib, "indileftoff");
						float rOffTime = Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, vehicle,
							animLib, "indirightoff");

						var pos = vehicle.Position;
						pos.Z += 2.0f;

						int currRoofState = Function.Call<int>(Hash.GET_CONVERTIBLE_ROOF_STATE, vehicle);

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
