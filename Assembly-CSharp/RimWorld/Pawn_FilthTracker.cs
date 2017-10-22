using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Pawn_FilthTracker : IExposable
	{
		private const float FilthPickupChance = 0.25f;

		private const float FilthDropChance = 0.05f;

		private const int MaxCarriedTerrainFilthThickness = 1;

		private Pawn pawn;

		private List<Filth> carriedFilth = new List<Filth>();

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

		public Pawn_FilthTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Filth>(ref this.carriedFilth, "carriedFilth", LookMode.Deep, new object[0]);
		}

		public void Notify_EnteredNewCell()
		{
			if (Rand.Value < 0.05000000074505806)
			{
				this.TryDropFilth();
			}
			if (Rand.Value < 0.25)
			{
				this.TryPickupFilth();
			}
			if (!this.pawn.RaceProps.Humanlike && Rand.Value < PawnUtility.AnimalFilthChancePerCell(this.pawn.def, this.pawn.BodySize) && this.pawn.Position.GetTerrain(this.pawn.Map).acceptTerrainSourceFilth)
			{
				FilthMaker.MakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.FilthAnimalFilth, 1);
			}
		}

		private void TryPickupFilth()
		{
			TerrainDef terrDef = this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position);
			if (terrDef.terrainFilthDef != null)
			{
				for (int num = this.carriedFilth.Count - 1; num >= 0; num--)
				{
					if (this.carriedFilth[num].def.filth.terrainSourced && this.carriedFilth[num].def != terrDef.terrainFilthDef)
					{
						this.ThinCarriedFilth(this.carriedFilth[num]);
					}
				}
				Filth filth = (from f in this.carriedFilth
				where f.def == terrDef.terrainFilthDef
				select f).FirstOrDefault();
				if (filth == null || filth.thickness < 1)
				{
					this.GainFilth(terrDef.terrainFilthDef);
				}
			}
			List<Thing> thingList = this.pawn.Position.GetThingList(this.pawn.Map);
			for (int num2 = thingList.Count - 1; num2 >= 0; num2--)
			{
				Filth filth2 = thingList[num2] as Filth;
				if (filth2 != null && filth2.CanFilthAttachNow)
				{
					this.GainFilth(filth2.def, filth2.sources);
					filth2.ThinFilth();
				}
			}
		}

		private void TryDropFilth()
		{
			if (this.carriedFilth.Count != 0)
			{
				for (int num = this.carriedFilth.Count - 1; num >= 0; num--)
				{
					if (this.carriedFilth[num].CanDropAt(this.pawn.Position, this.pawn.Map))
					{
						this.DropCarriedFilth(this.carriedFilth[num]);
					}
				}
			}
		}

		private void DropCarriedFilth(Filth f)
		{
			this.ThinCarriedFilth(f);
			FilthMaker.MakeFilth(this.pawn.Position, this.pawn.Map, f.def, f.sources);
		}

		private void ThinCarriedFilth(Filth f)
		{
			f.ThinFilth();
			if (f.thickness <= 0)
			{
				this.carriedFilth.Remove(f);
			}
		}

		public void GainFilth(ThingDef filthDef)
		{
			this.GainFilth(filthDef, null);
		}

		public void GainFilth(ThingDef filthDef, IEnumerable<string> sources)
		{
			Filth filth = null;
			int num = 0;
			while (num < this.carriedFilth.Count)
			{
				if (this.carriedFilth[num].def != filthDef)
				{
					num++;
					continue;
				}
				filth = this.carriedFilth[num];
				break;
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
