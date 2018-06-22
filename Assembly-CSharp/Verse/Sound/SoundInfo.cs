using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DB3 RID: 3507
	public struct SoundInfo
	{
		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004E70 RID: 20080 RVA: 0x00290268 File Offset: 0x0028E668
		// (set) Token: 0x06004E71 RID: 20081 RVA: 0x00290282 File Offset: 0x0028E682
		public bool IsOnCamera { get; private set; }

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004E72 RID: 20082 RVA: 0x0029028C File Offset: 0x0028E68C
		// (set) Token: 0x06004E73 RID: 20083 RVA: 0x002902A6 File Offset: 0x0028E6A6
		public TargetInfo Maker { get; private set; }

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x002902B0 File Offset: 0x0028E6B0
		// (set) Token: 0x06004E75 RID: 20085 RVA: 0x002902CA File Offset: 0x0028E6CA
		public MaintenanceType Maintenance { get; private set; }

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x002902D4 File Offset: 0x0028E6D4
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

		// Token: 0x06004E77 RID: 20087 RVA: 0x00290304 File Offset: 0x0028E704
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

		// Token: 0x06004E78 RID: 20088 RVA: 0x0029035C File Offset: 0x0028E75C
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

		// Token: 0x06004E79 RID: 20089 RVA: 0x002903B0 File Offset: 0x0028E7B0
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x002903D8 File Offset: 0x0028E7D8
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x002903F4 File Offset: 0x0028E7F4
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x00290418 File Offset: 0x0028E818
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

		// Token: 0x0400343A RID: 13370
		private Dictionary<string, float> parameters;

		// Token: 0x0400343B RID: 13371
		public float volumeFactor;

		// Token: 0x0400343C RID: 13372
		public float pitchFactor;

		// Token: 0x0400343D RID: 13373
		public bool testPlay;
	}
}
