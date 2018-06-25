using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000515 RID: 1301
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x04000DE7 RID: 3559
		public Pawn_MeleeVerbs parent;

		// Token: 0x04000DE8 RID: 3560
		public TerrainDef def;

		// Token: 0x04000DE9 RID: 3561
		public VerbTracker tracker;

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x000CE6E8 File Offset: 0x000CCAE8
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x000CE704 File Offset: 0x000CCB04
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x000CE71C File Offset: 0x000CCB1C
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x000CE73C File Offset: 0x000CCB3C
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x000CE772 File Offset: 0x000CCB72
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000CE7A0 File Offset: 0x000CCBA0
		public string UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}
	}
}
