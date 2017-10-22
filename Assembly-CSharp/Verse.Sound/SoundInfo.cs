using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	public struct SoundInfo
	{
		private Dictionary<string, float> parameters;

		public float volumeFactor;

		public float pitchFactor;

		public bool testPlay;

		public bool IsOnCamera
		{
			get;
			private set;
		}

		public TargetInfo Maker
		{
			get;
			private set;
		}

		public MaintenanceType Maintenance
		{
			get;
			private set;
		}

		public IEnumerable<KeyValuePair<string, float>> DefinedParameters
		{
			get
			{
				if (this.parameters != null)
				{
					Dictionary<string, float>.Enumerator enumerator = this.parameters.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, float> kvp = enumerator.Current;
							yield return kvp;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
				}
			}
		}

		public static SoundInfo OnCamera(MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = new SoundInfo
			{
				IsOnCamera = true,
				Maintenance = maint,
				Maker = TargetInfo.Invalid,
				testPlay = false
			};
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		public static SoundInfo InMap(TargetInfo maker, MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = new SoundInfo
			{
				IsOnCamera = false,
				Maintenance = maint,
				Maker = maker,
				testPlay = false
			};
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		public override string ToString()
		{
			string text = (string)null;
			if (this.parameters != null && this.parameters.Count > 0)
			{
				text = "parameters=";
				Dictionary<string, float>.Enumerator enumerator = this.parameters.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, float> current = enumerator.Current;
						string text2 = text;
						text = text2 + current.Key.ToString() + "-" + current.Value.ToString() + " ";
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
			string text3 = (string)null;
			if (this.Maker.HasThing || this.Maker.Cell.IsValid)
			{
				text3 = this.Maker.ToString();
			}
			string text4 = (string)null;
			if (this.Maintenance != 0)
			{
				text4 = ", Maint=" + this.Maintenance;
			}
			return "(" + ((!this.IsOnCamera) ? "World from " : "Camera") + text3 + text + text4 + ")";
		}

		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}
	}
}
