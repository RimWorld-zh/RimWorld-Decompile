using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B1F RID: 2847
	public abstract class DeathActionWorker
	{
		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06003EC6 RID: 16070 RVA: 0x000A9060 File Offset: 0x000A7460
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06003EC7 RID: 16071 RVA: 0x000A907C File Offset: 0x000A747C
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003EC8 RID: 16072
		public abstract void PawnDied(Corpse corpse);
	}
}
