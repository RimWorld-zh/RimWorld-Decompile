using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Profiling;

namespace Verse.Sound
{
	// Token: 0x02000DC4 RID: 3524
	public class SustainerManager
	{
		// Token: 0x04003464 RID: 13412
		private List<Sustainer> allSustainers = new List<Sustainer>();

		// Token: 0x04003465 RID: 13413
		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06004EB2 RID: 20146 RVA: 0x00291D7C File Offset: 0x0029017C
		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x00291D97 File Offset: 0x00290197
		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x00291DA6 File Offset: 0x002901A6
		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x00291DB8 File Offset: 0x002901B8
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

		// Token: 0x06004EB6 RID: 20150 RVA: 0x00291E0C File Offset: 0x0029020C
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

		// Token: 0x06004EB7 RID: 20151 RVA: 0x00291E60 File Offset: 0x00290260
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

		// Token: 0x06004EB8 RID: 20152 RVA: 0x00292138 File Offset: 0x00290538
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
	}
}
