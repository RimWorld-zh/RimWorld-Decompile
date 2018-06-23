using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200019B RID: 411
	public class LordToilData_Siege : LordToilData
	{
		// Token: 0x0400039C RID: 924
		public IntVec3 siegeCenter;

		// Token: 0x0400039D RID: 925
		public float baseRadius = 16f;

		// Token: 0x0400039E RID: 926
		public float blueprintPoints;

		// Token: 0x0400039F RID: 927
		public float desiredBuilderFraction = 0.5f;

		// Token: 0x040003A0 RID: 928
		public List<Blueprint> blueprints = new List<Blueprint>();

		// Token: 0x0600088C RID: 2188 RVA: 0x000516D0 File Offset: 0x0004FAD0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.siegeCenter, "siegeCenter", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.baseRadius, "baseRadius", 16f, false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.desiredBuilderFraction, "desiredBuilderFraction", 0.5f, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			}
			Scribe_Collections.Look<Blueprint>(ref this.blueprints, "blueprints", LookMode.Reference, new object[0]);
		}
	}
}
