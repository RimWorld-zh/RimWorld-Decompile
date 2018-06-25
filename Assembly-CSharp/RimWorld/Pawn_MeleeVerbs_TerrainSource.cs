using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000515 RID: 1301
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x04000DEB RID: 3563
		public Pawn_MeleeVerbs parent;

		// Token: 0x04000DEC RID: 3564
		public TerrainDef def;

		// Token: 0x04000DED RID: 3565
		public VerbTracker tracker;

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x000CE950 File Offset: 0x000CCD50
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x000CE96C File Offset: 0x000CCD6C
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x000CE984 File Offset: 0x000CCD84
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x000CE9A4 File Offset: 0x000CCDA4
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x000CE9DA File Offset: 0x000CCDDA
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x000CEA08 File Offset: 0x000CCE08
		public string UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}
	}
}
