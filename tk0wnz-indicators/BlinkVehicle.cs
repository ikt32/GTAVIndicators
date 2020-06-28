using System;
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
			string blinkyBois = "None";

			bool playinghazoff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff", 3);
			bool playingloff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff", 3);
			bool playingroff = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff", 3);
			bool finishedhazoff = Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, vehicle, "va_" + blinkerParams.ModelName, "indihazoff", 3);
			bool finishedloff = Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, vehicle, "va_" + blinkerParams.ModelName, "indileftoff", 3);
			bool finishedroff = Function.Call<bool>(Hash.HAS_ENTITY_ANIM_FINISHED, vehicle, "va_" + blinkerParams.ModelName, "indirightoff", 3);

			if ((lightStates & 0x300) == 0x300)
			{
				blinkyBois = "Hazard lights are detected";
				if (rIndicatorRight)
				{
					if (playingroff == false && finishedroff == false)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff");
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
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff");
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
				blinkyBois = "Left indicator is detected";
				if (rIndicatorRight)
				{
					if (playingroff == false && finishedroff == false)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff");
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
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff");
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
				blinkyBois = "Right indicator is detected";
				if (rIndicatorLeft)
				{
					if (playingloff == false && finishedloff == false)
					{
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff");
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
						Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff");
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
				if (rIndicatorLeft && !finishedloff)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indileftoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					rIndicatorLeft = false;
				}
				else if (rIndicatorRight && !finishedroff)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirightoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					rIndicatorRight = false;
				}
				else if (rIndicatorHaz && !finishedloff)
				{
					Function.Call(Hash.TASK_VEHICLE_PLAY_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazoff");
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, vehicle);
					rIndicatorHaz = false;
				}

				bool playinghazon = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indihazon", 3);
				bool playinglon = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indilefton", 3);
				bool playingron = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, vehicle, "va_" + blinkerParams.ModelName, "indirighton", 3);

				if (playingloff)
					Function.Call(Hash.STOP_ANIM_TASK, vehicle, "va_" + blinkerParams.ModelName, "indileftoff", -8.0f);

				if (playingroff)
					Function.Call(Hash.STOP_ANIM_TASK, vehicle, "va_" + blinkerParams.ModelName, "indirightoff", -8.0f);

				if (playinghazoff)
					Function.Call(Hash.STOP_ANIM_TASK, vehicle, "va_" + blinkerParams.ModelName, "indihazoff", -8.0f);

				if (playinglon)
					Function.Call(Hash.STOP_ANIM_TASK, vehicle, "va_" + blinkerParams.ModelName, "indilefton", -8.0f);

				if (playingron)
					Function.Call(Hash.STOP_ANIM_TASK, vehicle, "va_" + blinkerParams.ModelName, "indirighton", -8.0f);

				if (playinghazon)
					Function.Call(Hash.STOP_ANIM_TASK, vehicle, "va_" + blinkerParams.ModelName, "indihazon", -8.0f);


				mIndicatorLeft = false;
				mIndicatorRight = false;
				mIndicatorHaz = false;
				rIndicatorLeft = false;
				rIndicatorRight = false;
				rIndicatorHaz = false;
				timerSet = false;
			}

			if (blinkerParams.Debug == 1)
			{
				Utils.ShowText(0.5f, 0.450f, blinkyBois);
				Utils.ShowText(0.5f, 0.500f, string.Format("0x{0:X}", vehicle.GetLightStates()));
				Utils.ShowText(0.5f, 0.550f, "Currently playing animation indihazoff		: " + playinghazoff.ToString());
				Utils.ShowText(0.5f, 0.600f, "Currently playing animation indileftoff		: " + playingloff.ToString());
				Utils.ShowText(0.5f, 0.650f, "Currently playing animation indirightoff	: " + playingroff.ToString());
				Utils.ShowText(0.5f, 0.700f, "Finished playing animation: indihazoff		: " + finishedhazoff.ToString());
				Utils.ShowText(0.5f, 0.750f, "Finished playing animation: indileftoff		: " + finishedloff.ToString());
				Utils.ShowText(0.5f, 0.800f, "Finished playing animation: indirightoff	: " + finishedroff.ToString());
			}
		}

	}
}
