using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000BD2 RID: 3026
	public class TickList
	{
		// Token: 0x04002D15 RID: 11541
		private TickerType tickType;

		// Token: 0x04002D16 RID: 11542
		private List<List<Thing>> thingLists = new List<List<Thing>>();

		// Token: 0x04002D17 RID: 11543
		private List<Thing> thingsToRegister = new List<Thing>();

		// Token: 0x04002D18 RID: 11544
		private List<Thing> thingsToDeregister = new List<Thing>();

		// Token: 0x060041EE RID: 16878 RVA: 0x0022C414 File Offset: 0x0022A814
		public TickList(TickerType tickType)
		{
			this.tickType = tickType;
			for (int i = 0; i < this.TickInterval; i++)
			{
				this.thingLists.Add(new List<Thing>());
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x060041EF RID: 16879 RVA: 0x0022C47C File Offset: 0x0022A87C
		private int TickInterval
		{
			get
			{
				TickerType tickerType = this.tickType;
				int result;
				if (tickerType != TickerType.Normal)
				{
					if (tickerType != TickerType.Rare)
					{
						if (tickerType != TickerType.Long)
						{
							result = -1;
						}
						else
						{
							result = 2000;
						}
					}
					else
					{
						result = 250;
					}
				}
				else
				{
					result = 1;
				}
				return result;
			}
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x0022C4D0 File Offset: 0x0022A8D0
		public void Reset()
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].Clear();
			}
			this.thingsToRegister.Clear();
			this.thingsToDeregister.Clear();
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x0022C524 File Offset: 0x0022A924
		public void RemoveWhere(Predicate<Thing> predicate)
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].RemoveAll(predicate);
			}
			this.thingsToRegister.RemoveAll(predicate);
			this.thingsToDeregister.RemoveAll(predicate);
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x0022C57D File Offset: 0x0022A97D
		public void RegisterThing(Thing t)
		{
			this.thingsToRegister.Add(t);
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x0022C58C File Offset: 0x0022A98C
		public void DeregisterThing(Thing t)
		{
			this.thingsToDeregister.Add(t);
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x0022C59C File Offset: 0x0022A99C
		public void Tick()
		{
			for (int i = 0; i < this.thingsToRegister.Count; i++)
			{
				this.BucketOf(this.thingsToRegister[i]).Add(this.thingsToRegister[i]);
			}
			this.thingsToRegister.Clear();
			for (int j = 0; j < this.thingsToDeregister.Count; j++)
			{
				this.BucketOf(this.thingsToDeregister[j]).Remove(this.thingsToDeregister[j]);
			}
			this.thingsToDeregister.Clear();
			if (DebugSettings.fastEcology)
			{
				Find.World.tileTemperatures.ClearCaches();
				for (int k = 0; k < this.thingLists.Count; k++)
				{
					List<Thing> list = this.thingLists[k];
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].def.category == ThingCategory.Plant)
						{
							list[l].TickLong();
						}
					}
				}
			}
			List<Thing> list2 = this.thingLists[Find.TickManager.TicksGame % this.TickInterval];
			for (int m = 0; m < list2.Count; m++)
			{
				if (!list2[m].Destroyed)
				{
					try
					{
						Profiler.BeginSample(list2[m].def.defName);
						TickerType tickerType = this.tickType;
						if (tickerType != TickerType.Normal)
						{
							if (tickerType != TickerType.Rare)
							{
								if (tickerType == TickerType.Long)
								{
									list2[m].TickLong();
								}
							}
							else
							{
								list2[m].TickRare();
							}
						}
						else
						{
							list2[m].Tick();
						}
						Profiler.EndSample();
					}
					catch (Exception ex)
					{
						string text = (!list2[m].Spawned) ? "" : (" (at " + list2[m].Position + ")");
						if (Prefs.DevMode)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception ticking ",
								list2[m].ToStringSafe<Thing>(),
								text,
								": ",
								ex
							}), false);
						}
						else
						{
							Log.ErrorOnce(string.Concat(new object[]
							{
								"Exception ticking ",
								list2[m].ToStringSafe<Thing>(),
								text,
								". Suppressing further errors. Exception: ",
								ex
							}), list2[m].thingIDNumber ^ 576876901, false);
						}
					}
				}
			}
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x0022C898 File Offset: 0x0022AC98
		private List<Thing> BucketOf(Thing t)
		{
			int num = t.GetHashCode();
			if (num < 0)
			{
				num *= -1;
			}
			int index = num % this.TickInterval;
			return this.thingLists[index];
		}
	}
}
