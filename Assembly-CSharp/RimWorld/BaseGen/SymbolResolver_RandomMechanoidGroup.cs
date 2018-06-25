using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_RandomMechanoidGroup : SymbolResolver
	{
		private static readonly IntRange DefaultMechanoidCountRange = new IntRange(1, 5);

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache1;

		public SymbolResolver_RandomMechanoidGroup()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			int? mechanoidsCount = rp.mechanoidsCount;
			int num = (mechanoidsCount == null) ? SymbolResolver_RandomMechanoidGroup.DefaultMechanoidCountRange.RandomInRange : mechanoidsCount.Value;
			Lord lord = rp.singlePawnLord;
			if (lord == null && num > 0)
			{
				Map map = BaseGen.globalSettings.map;
				IntVec3 point;
				LordJob lordJob;
				if (Rand.Bool && (from x in rp.rect.Cells
				where !x.Impassable(map)
				select x).TryRandomElement(out point))
				{
					lordJob = new LordJob_DefendPoint(point);
				}
				else
				{
					lordJob = new LordJob_AssaultColony(Faction.OfMechanoids, false, false, false, false, false);
				}
				lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob, map, null);
			}
			for (int i = 0; i < num; i++)
			{
				PawnKindDef pawnKindDef = rp.singlePawnKindDef;
				if (pawnKindDef == null)
				{
					pawnKindDef = (from kind in DefDatabase<PawnKindDef>.AllDefsListForReading
					where kind.RaceProps.IsMechanoid
					select kind).RandomElementByWeight((PawnKindDef kind) => 1f / kind.combatPower);
				}
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnKindDef = pawnKindDef;
				resolveParams.singlePawnLord = lord;
				resolveParams.faction = Faction.OfMechanoids;
				BaseGen.symbolStack.Push("pawn", resolveParams);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SymbolResolver_RandomMechanoidGroup()
		{
		}

		[CompilerGenerated]
		private static bool <Resolve>m__0(PawnKindDef kind)
		{
			return kind.RaceProps.IsMechanoid;
		}

		[CompilerGenerated]
		private static float <Resolve>m__1(PawnKindDef kind)
		{
			return 1f / kind.combatPower;
		}

		[CompilerGenerated]
		private sealed class <Resolve>c__AnonStorey0
		{
			internal Map map;

			public <Resolve>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return !x.Impassable(this.map);
			}
		}
	}
}
