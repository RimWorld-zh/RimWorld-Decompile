using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000BD3 RID: 3027
	public class TickList
	{
		// Token: 0x060041E9 RID: 16873 RVA: 0x0022B984 File Offset: 0x00229D84
		public TickList(TickerType tickType)
		{
			this.tickType = tickType;
			for (int i = 0; i < this.TickInterval; i++)
			{
				this.thingLists.Add(new List<Thing>());
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x060041EA RID: 16874 RVA: 0x0022B9EC File Offset: 0x00229DEC
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

		// Token: 0x060041EB RID: 16875 RVA: 0x0022BA40 File Offset: 0x00229E40
		public void Reset()
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].Clear();
			}
			this.thingsToRegister.Clear();
			this.thingsToDeregister.Clear();
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x0022BA94 File Offset: 0x00229E94
		public void RemoveWhere(Predicate<Thing> predicate)
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].RemoveAll(predicate);
			}
			this.thingsToRegister.RemoveAll(predicate);
			this.thingsToDeregister.RemoveAll(predicate);
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x0022BAED File Offset: 0x00229EED
		public void RegisterThing(Thing t)
		{
			this.thingsToRegister.Add(t);
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x0022BAFC File Offset: 0x00229EFC
		public void DeregisterThing(Thing t)
		{
			this.thingsToDeregister.Add(t);
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x0022BB0C File Offset: 0x00229F0C
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
						if (Prefs.DevMode)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception ticking ",
								list2[m].ToString(),
								": ",
								ex
							}), false);
						}
						else
						{
							Log.ErrorOnce(string.Concat(new object[]
							{
								"Exception ticking ",
								list2[m].ToString(),
								". Suppressing further errors. Exception: ",
								ex
							}), list2[m].thingIDNumber ^ 576876901, false);
						}
					}
				}
			}
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x0022BDC0 File Offset: 0x0022A1C0
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

		// Token: 0x04002D09 RID: 11529
		private TickerType tickType;

		// Token: 0x04002D0A RID: 11530
		private List<List<Thing>> thingLists = new List<List<Thing>>();

		// Token: 0x04002D0B RID: 11531
		private List<Thing> thingsToRegister = new List<Thing>();

		// Token: 0x04002D0C RID: 11532
		private List<Thing> thingsToDeregister = new List<Thing>();
	}
}
