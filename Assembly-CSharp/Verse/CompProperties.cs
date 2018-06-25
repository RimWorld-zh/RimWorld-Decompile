using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0F RID: 2831
	public class CompProperties
	{
		// Token: 0x040027EE RID: 10222
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);

		// Token: 0x06003E9B RID: 16027 RVA: 0x0005E5EC File Offset: 0x0005C9EC
		public CompProperties()
		{
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0005E605 File Offset: 0x0005CA05
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0005E625 File Offset: 0x0005CA25
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude)
		{
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0005E628 File Offset: 0x0005CA28
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0005E659 File Offset: 0x0005CA59
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x0005E65C File Offset: 0x0005CA5C
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield break;
		}
	}
}
