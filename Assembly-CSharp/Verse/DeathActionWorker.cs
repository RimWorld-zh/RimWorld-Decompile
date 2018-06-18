using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B23 RID: 2851
	public abstract class DeathActionWorker
	{
		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003ECA RID: 16074 RVA: 0x000A9050 File Offset: 0x000A7450
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06003ECB RID: 16075 RVA: 0x000A906C File Offset: 0x000A746C
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003ECC RID: 16076
		public abstract void PawnDied(Corpse corpse);
	}
}
