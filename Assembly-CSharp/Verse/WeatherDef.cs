using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public class WeatherDef : Def
	{
		public IntRange durationRange = new IntRange(16000, 160000);

		public bool repeatable;

		public Favorability favorability = Favorability.Neutral;

		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		public SimpleCurve commonalityRainfallFactor;

		public float rainRate;

		public float snowRate;

		public float windSpeedFactor = 1f;

		public float moveSpeedMultiplier = 1f;

		public float accuracyMultiplier = 1f;

		public List<SoundDef> ambientSounds = new List<SoundDef>();

		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		public List<Type> overlayClasses = new List<Type>();

		public SkyColorSet skyColorsNightMid;

		public SkyColorSet skyColorsNightEdge;

		public SkyColorSet skyColorsDay;

		public SkyColorSet skyColorsDusk;

		[Unsaved]
		private WeatherWorker workerInt;

		public WeatherWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = new WeatherWorker(this);
				}
				return this.workerInt;
			}
		}

		public override void PostLoad()
		{
			base.PostLoad();
			this.workerInt = new WeatherWorker(this);
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			WeatherDef.<ConfigErrors>c__Iterator1E1 <ConfigErrors>c__Iterator1E = new WeatherDef.<ConfigErrors>c__Iterator1E1();
			<ConfigErrors>c__Iterator1E.<>f__this = this;
			WeatherDef.<ConfigErrors>c__Iterator1E1 expr_0E = <ConfigErrors>c__Iterator1E;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}
	}
}
