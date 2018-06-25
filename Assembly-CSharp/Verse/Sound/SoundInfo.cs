using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DB6 RID: 3510
	public struct SoundInfo
	{
		// Token: 0x04003441 RID: 13377
		private Dictionary<string, float> parameters;

		// Token: 0x04003442 RID: 13378
		public float volumeFactor;

		// Token: 0x04003443 RID: 13379
		public float pitchFactor;

		// Token: 0x04003444 RID: 13380
		public bool testPlay;

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x00290674 File Offset: 0x0028EA74
		// (set) Token: 0x06004E75 RID: 20085 RVA: 0x0029068E File Offset: 0x0028EA8E
		public bool IsOnCamera { get; private set; }

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x00290698 File Offset: 0x0028EA98
		// (set) Token: 0x06004E77 RID: 20087 RVA: 0x002906B2 File Offset: 0x0028EAB2
		public TargetInfo Maker { get; private set; }

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004E78 RID: 20088 RVA: 0x002906BC File Offset: 0x0028EABC
		// (set) Token: 0x06004E79 RID: 20089 RVA: 0x002906D6 File Offset: 0x0028EAD6
		public MaintenanceType Maintenance { get; private set; }

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004E7A RID: 20090 RVA: 0x002906E0 File Offset: 0x0028EAE0
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

		// Token: 0x06004E7B RID: 20091 RVA: 0x00290710 File Offset: 0x0028EB10
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

		// Token: 0x06004E7C RID: 20092 RVA: 0x00290768 File Offset: 0x0028EB68
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

		// Token: 0x06004E7D RID: 20093 RVA: 0x002907BC File Offset: 0x0028EBBC
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x002907E4 File Offset: 0x0028EBE4
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x00290800 File Offset: 0x0028EC00
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x00290824 File Offset: 0x0028EC24
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
	}
}
