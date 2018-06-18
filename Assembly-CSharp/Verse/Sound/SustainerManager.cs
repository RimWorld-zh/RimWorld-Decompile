using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Profiling;

namespace Verse.Sound
{
	// Token: 0x02000DC5 RID: 3525
	public class SustainerManager
	{
		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06004E99 RID: 20121 RVA: 0x002906A0 File Offset: 0x0028EAA0
		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x002906BB File Offset: 0x0028EABB
		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x002906CA File Offset: 0x0028EACA
		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x002906DC File Offset: 0x0028EADC
		public bool SustainerExists(SoundDef def)
		{
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				if (this.allSustainers[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x00290730 File Offset: 0x0028EB30
		public void SustainerManagerUpdate()
		{
			Profiler.BeginSample("Updating sustainers");
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				this.allSustainers[i].SustainerUpdate();
			}
			Profiler.EndSample();
			this.UpdateAllSustainerScopes();
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x00290784 File Offset: 0x0028EB84
		public void UpdateAllSustainerScopes()
		{
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				Sustainer sustainer = this.allSustainers[i];
				if (!SustainerManager.playingPerDef.ContainsKey(sustainer.def))
				{
					List<Sustainer> list = SimplePool<List<Sustainer>>.Get();
					list.Add(sustainer);
					SustainerManager.playingPerDef.Add(sustainer.def, list);
				}
				else
				{
					SustainerManager.playingPerDef[sustainer.def].Add(sustainer);
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair in SustainerManager.playingPerDef)
			{
				SoundDef key = keyValuePair.Key;
				List<Sustainer> value = keyValuePair.Value;
				int num = value.Count - key.maxVoices;
				if (num < 0)
				{
					for (int j = 0; j < value.Count; j++)
					{
						value[j].scopeFader.inScope = true;
					}
				}
				else
				{
					for (int k = 0; k < value.Count; k++)
					{
						value[k].scopeFader.inScope = false;
					}
					int num2 = 0;
					foreach (Sustainer sustainer2 in from lo in value
					orderby lo.CameraDistanceSquared
					select lo)
					{
						sustainer2.scopeFader.inScope = true;
						num2++;
						if (num2 >= key.maxVoices)
						{
							break;
						}
					}
					for (int l = 0; l < value.Count; l++)
					{
						if (!value[l].scopeFader.inScope)
						{
							value[l].scopeFader.inScopePercent = 0f;
						}
					}
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair2 in SustainerManager.playingPerDef)
			{
				keyValuePair2.Value.Clear();
				SimplePool<List<Sustainer>>.Return(keyValuePair2.Value);
			}
			SustainerManager.playingPerDef.Clear();
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x00290A5C File Offset: 0x0028EE5C
		public void EndAllInMap(Map map)
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				if (this.allSustainers[i].info.Maker.Map == map)
				{
					this.allSustainers[i].End();
				}
			}
		}

		// Token: 0x04003459 RID: 13401
		private List<Sustainer> allSustainers = new List<Sustainer>();

		// Token: 0x0400345A RID: 13402
		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();
	}
}
