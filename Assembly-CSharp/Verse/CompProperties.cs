using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0C RID: 2828
	public class CompProperties
	{
		// Token: 0x06003E97 RID: 16023 RVA: 0x0005E4A0 File Offset: 0x0005C8A0
		public CompProperties()
		{
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0005E4B9 File Offset: 0x0005C8B9
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0005E4D9 File Offset: 0x0005C8D9
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude)
		{
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0005E4DC File Offset: 0x0005C8DC
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x0005E50D File Offset: 0x0005C90D
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0005E510 File Offset: 0x0005C910
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield break;
		}

		// Token: 0x040027E6 RID: 10214
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);
	}
}
