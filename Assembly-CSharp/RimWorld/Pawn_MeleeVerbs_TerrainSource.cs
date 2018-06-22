using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000513 RID: 1299
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x000CE598 File Offset: 0x000CC998
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x000CE5B4 File Offset: 0x000CC9B4
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x000CE5CC File Offset: 0x000CC9CC
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x000CE5EC File Offset: 0x000CC9EC
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x000CE622 File Offset: 0x000CCA22
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000CE650 File Offset: 0x000CCA50
		public string UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}

		// Token: 0x04000DE7 RID: 3559
		public Pawn_MeleeVerbs parent;

		// Token: 0x04000DE8 RID: 3560
		public TerrainDef def;

		// Token: 0x04000DE9 RID: 3561
		public VerbTracker tracker;
	}
}
