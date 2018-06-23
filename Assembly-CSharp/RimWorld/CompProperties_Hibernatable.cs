using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000260 RID: 608
	public class CompProperties_Hibernatable : CompProperties
	{
		// Token: 0x040004C5 RID: 1221
		public float startupDays = 15f;

		// Token: 0x040004C6 RID: 1222
		public IncidentTargetTypeDef incidentTargetWhileStarting = null;

		// Token: 0x06000A9D RID: 2717 RVA: 0x0005FF75 File Offset: 0x0005E375
		public CompProperties_Hibernatable()
		{
			this.compClass = typeof(CompHibernatable);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0005FFA0 File Offset: 0x0005E3A0
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return err;
			}
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Concat(new object[]
				{
					"CompHibernatable needs tickerType ",
					TickerType.Normal,
					", has ",
					parentDef.tickerType
				});
			}
			yield break;
		}
	}
}
