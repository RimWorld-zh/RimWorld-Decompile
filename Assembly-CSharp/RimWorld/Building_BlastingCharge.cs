using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000681 RID: 1665
	public class Building_BlastingCharge : Building
	{
		// Token: 0x06002319 RID: 8985 RVA: 0x0012E284 File Offset: 0x0012C684
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

		// Token: 0x0600231A RID: 8986 RVA: 0x0012E2AE File Offset: 0x0012C6AE
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
