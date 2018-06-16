using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B23 RID: 2851
	public abstract class DeathActionWorker
	{
		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003EC8 RID: 16072 RVA: 0x000A9044 File Offset: 0x000A7444
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x000A9060 File Offset: 0x000A7460
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003ECA RID: 16074
		public abstract void PawnDied(Corpse corpse);
	}
}
