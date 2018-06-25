using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000683 RID: 1667
	public class Building_BlastingCharge : Building
	{
		// Token: 0x0600231D RID: 8989 RVA: 0x0012E3D4 File Offset: 0x0012C7D4
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

		// Token: 0x0600231E RID: 8990 RVA: 0x0012E3FE File Offset: 0x0012C7FE
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
