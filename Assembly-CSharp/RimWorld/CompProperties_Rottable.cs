using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000252 RID: 594
	public class CompProperties_Rottable : CompProperties
	{
		// Token: 0x040004A7 RID: 1191
		public float daysToRotStart = 2f;

		// Token: 0x040004A8 RID: 1192
		public bool rotDestroys = false;

		// Token: 0x040004A9 RID: 1193
		public float rotDamagePerDay = 40f;

		// Token: 0x040004AA RID: 1194
		public float daysToDessicated = 999f;

		// Token: 0x040004AB RID: 1195
		public float dessicatedDamagePerDay = 0f;

		// Token: 0x040004AC RID: 1196
		public bool disableIfHatcher;

		// Token: 0x06000A8B RID: 2699 RVA: 0x0005FACC File Offset: 0x0005DECC
		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0005FB24 File Offset: 0x0005DF24
		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0005FB74 File Offset: 0x0005DF74
		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0005FB9C File Offset: 0x0005DF9C
		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0005FBC4 File Offset: 0x0005DFC4
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
	}
}
