using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000250 RID: 592
	public class CompProperties_Rottable : CompProperties
	{
		// Token: 0x06000A88 RID: 2696 RVA: 0x0005F980 File Offset: 0x0005DD80
		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0005F9D8 File Offset: 0x0005DDD8
		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0005FA28 File Offset: 0x0005DE28
		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0005FA50 File Offset: 0x0005DE50
		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0005FA78 File Offset: 0x0005DE78
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			if (parentDef.tickerType != TickerType.Normal && parentDef.tickerType != TickerType.Rare)
			{
				yield return string.Concat(new object[]
				{
					"CompRottable needs tickerType ",
					TickerType.Rare,
					" or ",
					TickerType.Normal,
					", has ",
					parentDef.tickerType
				});
			}
			yield break;
		}

		// Token: 0x040004A5 RID: 1189
		public float daysToRotStart = 2f;

		// Token: 0x040004A6 RID: 1190
		public bool rotDestroys = false;

		// Token: 0x040004A7 RID: 1191
		public float rotDamagePerDay = 40f;

		// Token: 0x040004A8 RID: 1192
		public float daysToDessicated = 999f;

		// Token: 0x040004A9 RID: 1193
		public float dessicatedDamagePerDay = 0f;

		// Token: 0x040004AA RID: 1194
		public bool disableIfHatcher;
	}
}
