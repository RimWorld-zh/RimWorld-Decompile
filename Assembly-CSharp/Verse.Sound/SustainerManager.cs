using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	public class SustainerManager
	{
		private List<Sustainer> allSustainers = new List<Sustainer>();

		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();

		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

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

		public void SustainerManagerUpdate()
		{
			for (int num = this.allSustainers.Count - 1; num >= 0; num--)
			{
				this.allSustainers[num].SustainerUpdate();
			}
			this.UpdateAllSustainerScopes();
		}

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
			foreach (KeyValuePair<SoundDef, List<Sustainer>> item in SustainerManager.playingPerDef)
			{
				SoundDef key = item.Key;
				List<Sustainer> value = item.Value;
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
					foreach (Sustainer item2 in from lo in value
					orderby lo.CameraDistanceSquared
					select lo)
					{
						item2.scopeFader.inScope = true;
						num2++;
						if (num2 >= key.maxVoices)
							break;
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
			foreach (KeyValuePair<SoundDef, List<Sustainer>> item3 in SustainerManager.playingPerDef)
			{
				item3.Value.Clear();
				SimplePool<List<Sustainer>>.Return(item3.Value);
			}
			SustainerManager.playingPerDef.Clear();
		}

		public void EndAllInMap(Map map)
		{
			for (int num = this.allSustainers.Count - 1; num >= 0; num--)
			{
				if (this.allSustainers[num].info.Maker.Map == map)
				{
					this.allSustainers[num].End();
				}
			}
		}
	}
}
