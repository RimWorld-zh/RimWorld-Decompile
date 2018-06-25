using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000512 RID: 1298
	public class Pawn_FilthTracker : IExposable
	{
		// Token: 0x04000DD9 RID: 3545
		private Pawn pawn;

		// Token: 0x04000DDA RID: 3546
		private List<Filth> carriedFilth = new List<Filth>();

		// Token: 0x04000DDB RID: 3547
		private const float FilthPickupChance = 0.1f;

		// Token: 0x04000DDC RID: 3548
		private const float FilthDropChance = 0.05f;

		// Token: 0x04000DDD RID: 3549
		private const int MaxCarriedTerrainFilthThickness = 1;

		// Token: 0x06001771 RID: 6001 RVA: 0x000CD948 File Offset: 0x000CBD48
		public Pawn_FilthTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001772 RID: 6002 RVA: 0x000CD964 File Offset: 0x000CBD64
		public string FilthReport
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("FilthOnFeet".Translate());
				if (this.carriedFilth.Count == 0)
				{
					stringBuilder.Append("(" + "NoneLower".Translate() + ")");
				}
				else
				{
					for (int i = 0; i < this.carriedFilth.Count; i++)
					{
						stringBuilder.AppendLine(this.carriedFilth[i].LabelCap);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x000CDA05 File Offset: 0x000CBE05
		public void ExposeData()
		{
			Scribe_Collections.Look<Filth>(ref this.carriedFilth, "carriedFilth", LookMode.Deep, new object[0]);
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x000CDA20 File Offset: 0x000CBE20
		public void Notify_EnteredNewCell()
		{
			if (Rand.Value < 0.05f)
			{
				this.TryDropFilth();
			}
			if (Rand.Value < 0.1f)
			{
				this.TryPickupFilth();
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				if (Rand.Value < PawnUtility.AnimalFilthChancePerCell(this.pawn.def, this.pawn.BodySize))
				{
					if (this.pawn.Position.GetTerrain(this.pawn.Map).acceptTerrainSourceFilth)
					{
						FilthMaker.MakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_AnimalFilth, 1);
						FilthMonitor.Notify_FilthAnimalGenerated(ThingDefOf.Filth_AnimalFilth);
					}
				}
			}
			else if (Rand.Value < PawnUtility.HumanFilthChancePerCell(this.pawn.def, this.pawn.BodySize))
			{
				if (this.pawn.Position.GetTerrain(this.pawn.Map).acceptTerrainSourceFilth)
				{
					FilthMaker.MakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_HumanFilth, 1);
					FilthMonitor.Notify_FilthHumanGenerated(ThingDefOf.Filth_HumanFilth);
				}
			}
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x000CDB68 File Offset: 0x000CBF68
		private void TryPickupFilth()
		{
			TerrainDef terrDef = this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position);
			if (terrDef.generatedFilth != null)
			{
				for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
				{
					if (this.carriedFilth[i].def.filth.terrainSourced && this.carriedFilth[i].def != terrDef.generatedFilth)
					{
						this.ThinCarriedFilth(this.carriedFilth[i]);
					}
				}
				Filth filth = (from f in this.carriedFilth
				where f.def == terrDef.generatedFilth
				select f).FirstOrDefault<Filth>();
				if (filth == null || filth.thickness < 1)
				{
					this.GainFilth(terrDef.generatedFilth);
					FilthMonitor.Notify_FilthAccumulated(terrDef.generatedFilth);
				}
			}
			List<Thing> thingList = this.pawn.Position.GetThingList(this.pawn.Map);
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				Filth filth2 = thingList[j] as Filth;
				if (filth2 != null && filth2.CanFilthAttachNow)
				{
					this.GainFilth(filth2.def, filth2.sources);
					filth2.ThinFilth();
				}
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x000CDCF4 File Offset: 0x000CC0F4
		private void TryDropFilth()
		{
			if (this.carriedFilth.Count != 0)
			{
				for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
				{
					Filth filth = this.carriedFilth[i];
					if (filth.CanDropAt(this.pawn.Position, this.pawn.Map))
					{
						FilthMonitor.Notify_FilthDropped(filth.def);
						this.DropCarriedFilth(this.carriedFilth[i]);
					}
				}
			}
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x000CDD84 File Offset: 0x000CC184
		private void DropCarriedFilth(Filth f)
		{
			this.ThinCarriedFilth(f);
			FilthMaker.MakeFilth(this.pawn.Position, this.pawn.Map, f.def, f.sources);
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x000CDDB5 File Offset: 0x000CC1B5
		private void ThinCarriedFilth(Filth f)
		{
			f.ThinFilth();
			if (f.thickness <= 0)
			{
				this.carriedFilth.Remove(f);
			}
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000CDDD7 File Offset: 0x000CC1D7
		public void GainFilth(ThingDef filthDef)
		{
			this.GainFilth(filthDef, null);
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000CDDE4 File Offset: 0x000CC1E4
		public void GainFilth(ThingDef filthDef, IEnumerable<string> sources)
		{
			Filth filth = null;
			for (int i = 0; i < this.carriedFilth.Count; i++)
			{
				if (this.carriedFilth[i].def == filthDef)
				{
					filth = this.carriedFilth[i];
					break;
				}
			}
			if (filth != null)
			{
				if (filth.CanBeThickened)
				{
					filth.ThickenFilth();
					filth.AddSources(sources);
				}
			}
			else
			{
				Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
				filth2.AddSources(sources);
				this.carriedFilth.Add(filth2);
			}
		}
	}
}
