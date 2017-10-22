using System;
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
			ProfilerThreadCheck.BeginSample("UpdateAllSustainerScopes");
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
			Dictionary<SoundDef, List<Sustainer>>.Enumerator enumerator = SustainerManager.playingPerDef.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<SoundDef, List<Sustainer>> current = enumerator.Current;
					SoundDef key = current.Key;
					List<Sustainer> value = current.Value;
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
						foreach (Sustainer item in from lo in value
						orderby lo.CameraDistanceSquared
						select lo)
						{
							item.scopeFader.inScope = true;
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
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			Dictionary<SoundDef, List<Sustainer>>.Enumerator enumerator3 = SustainerManager.playingPerDef.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					KeyValuePair<SoundDef, List<Sustainer>> current3 = enumerator3.Current;
					current3.Value.Clear();
					SimplePool<List<Sustainer>>.Return(current3.Value);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator3).Dispose();
			}
			SustainerManager.playingPerDef.Clear();
			ProfilerThreadCheck.EndSample();
		}

		public void RemoveAllFromMap(Map map)
		{
			this.allSustainers.RemoveAll((Predicate<Sustainer>)((Sustainer sustainer) => sustainer.info.Maker.Map == map));
		}
	}
}
