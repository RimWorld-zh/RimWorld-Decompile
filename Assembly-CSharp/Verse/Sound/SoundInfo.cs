using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DB7 RID: 3511
	public struct SoundInfo
	{
		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06004E5D RID: 20061 RVA: 0x0028ECD8 File Offset: 0x0028D0D8
		// (set) Token: 0x06004E5E RID: 20062 RVA: 0x0028ECF2 File Offset: 0x0028D0F2
		public bool IsOnCamera { get; private set; }

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004E5F RID: 20063 RVA: 0x0028ECFC File Offset: 0x0028D0FC
		// (set) Token: 0x06004E60 RID: 20064 RVA: 0x0028ED16 File Offset: 0x0028D116
		public TargetInfo Maker { get; private set; }

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004E61 RID: 20065 RVA: 0x0028ED20 File Offset: 0x0028D120
		// (set) Token: 0x06004E62 RID: 20066 RVA: 0x0028ED3A File Offset: 0x0028D13A
		public MaintenanceType Maintenance { get; private set; }

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004E63 RID: 20067 RVA: 0x0028ED44 File Offset: 0x0028D144
		public IEnumerable<KeyValuePair<string, float>> DefinedParameters
		{
			get
			{
				if (this.parameters == null)
				{
					yield break;
				}
				foreach (KeyValuePair<string, float> kvp in this.parameters)
				{
					yield return kvp;
				}
				yield break;
			}
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x0028ED74 File Offset: 0x0028D174
		public static SoundInfo OnCamera(MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = true;
			result.Maintenance = maint;
			result.Maker = TargetInfo.Invalid;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		// Token: 0x06004E65 RID: 20069 RVA: 0x0028EDCC File Offset: 0x0028D1CC
		public static SoundInfo InMap(TargetInfo maker, MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = false;
			result.Maintenance = maint;
			result.Maker = maker;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x0028EE20 File Offset: 0x0028D220
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x0028EE48 File Offset: 0x0028D248
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x0028EE64 File Offset: 0x0028D264
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x0028EE88 File Offset: 0x0028D288
		public override string ToString()
		{
			string text = null;
			if (this.parameters != null && this.parameters.Count > 0)
			{
				text = "parameters=";
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						keyValuePair.Key.ToString(),
						"-",
						keyValuePair.Value.ToString(),
						" "
					});
				}
			}
			string text3 = null;
			if (this.Maker.HasThing || this.Maker.Cell.IsValid)
			{
				text3 = this.Maker.ToString();
			}
			string text4 = null;
			if (this.Maintenance != MaintenanceType.None)
			{
				text4 = ", Maint=" + this.Maintenance;
			}
			return string.Concat(new string[]
			{
				"(",
				(!this.IsOnCamera) ? "World from " : "Camera",
				text3,
				text,
				text4,
				")"
			});
		}

		// Token: 0x04003431 RID: 13361
		private Dictionary<string, float> parameters;

		// Token: 0x04003432 RID: 13362
		public float volumeFactor;

		// Token: 0x04003433 RID: 13363
		public float pitchFactor;

		// Token: 0x04003434 RID: 13364
		public bool testPlay;
	}
}
