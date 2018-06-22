using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Profiling;

namespace Verse.Sound
{
	// Token: 0x02000DC2 RID: 3522
	public class SustainerManager
	{
		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06004EAE RID: 20142 RVA: 0x00291C50 File Offset: 0x00290050
		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x00291C6B File Offset: 0x0029006B
		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x00291C7A File Offset: 0x0029007A
		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x00291C8C File Offset: 0x0029008C
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

		// Token: 0x06004EB2 RID: 20146 RVA: 0x00291CE0 File Offset: 0x002900E0
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

		// Token: 0x06004EB3 RID: 20147 RVA: 0x00291D34 File Offset: 0x00290134
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

		// Token: 0x06004EB4 RID: 20148 RVA: 0x0029200C File Offset: 0x0029040C
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

		// Token: 0x04003464 RID: 13412
		private List<Sustainer> allSustainers = new List<Sustainer>();

		// Token: 0x04003465 RID: 13413
		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();
	}
}
