using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000685 RID: 1669
	public class Building_BlastingCharge : Building
	{
		// Token: 0x0600231F RID: 8991 RVA: 0x0012E0C4 File Offset: 0x0012C4C4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Command_Action com = new Command_Action();
			com.icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate", true);
			com.defaultDesc = "CommandDetonateDesc".Translate();
			com.action = new Action(this.Command_Detonate);
			if (base.GetComp<CompExplosive>().wickStarted)
			{
				com.Disable(null);
			}
			com.defaultLabel = "CommandDetonateLabel".Translate();
			yield return com;
			yield break;
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0012E0EE File Offset: 0x0012C4EE
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
