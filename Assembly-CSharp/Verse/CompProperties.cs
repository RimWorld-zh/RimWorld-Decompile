using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B10 RID: 2832
	public class CompProperties
	{
		// Token: 0x06003E9B RID: 16027 RVA: 0x0005E444 File Offset: 0x0005C844
		public CompProperties()
		{
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0005E45D File Offset: 0x0005C85D
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0005E47D File Offset: 0x0005C87D
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude)
		{
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0005E480 File Offset: 0x0005C880
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0005E4B1 File Offset: 0x0005C8B1
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x0005E4B4 File Offset: 0x0005C8B4
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield break;
		}

		// Token: 0x040027EA RID: 10218
		public Type compClass = typeof(ThingComp);
	}
}
