using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0E RID: 2830
	public class CompProperties
	{
		// Token: 0x040027E7 RID: 10215
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);

		// Token: 0x06003E9B RID: 16027 RVA: 0x0005E5F0 File Offset: 0x0005C9F0
		public CompProperties()
		{
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0005E609 File Offset: 0x0005CA09
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0005E629 File Offset: 0x0005CA29
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude)
		{
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0005E62C File Offset: 0x0005CA2C
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0005E65D File Offset: 0x0005CA5D
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x0005E660 File Offset: 0x0005CA60
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield break;
		}
	}
}
