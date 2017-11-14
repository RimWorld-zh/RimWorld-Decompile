using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Hunt : Designator
	{
		private List<Pawn> justDesignated = new List<Pawn>();

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Hunt()
		{
			base.defaultLabel = "DesignatorHunt".Translate();
			base.defaultDesc = "DesignatorHuntDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateHunt;
			base.hotKey = KeyBindingDefOf.Misc11;
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.HuntablesInCell(c).Any())
			{
				return "MessageMustDesignateHuntable".Translate();
			}
			return true;
		}

		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn item in this.HuntablesInCell(loc))
			{
				this.DesignateThing(item);
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.AnimalOrWildMan() && pawn.Faction == null && base.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Hunt) == null)
			{
				return true;
			}
			return false;
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Hunt));
			this.justDesignated.Add((Pawn)t);
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			foreach (PawnKindDef item in (from p in this.justDesignated
			select p.kindDef).Distinct())
			{
				float num = (float)((item != PawnKindDefOf.WildMan) ? item.RaceProps.manhunterOnDamageChance : 0.5);
				if (num > 0.20000000298023224)
				{
					Messages.Message("MessageAnimalsGoPsychoHunted".Translate(item.GetLabelPlural(-1)), this.justDesignated.First((Pawn x) => x.kindDef == item), MessageTypeDefOf.CautionInput);
				}
			}
			this.justDesignated.Clear();
		}

		private IEnumerable<Pawn> HuntablesInCell(IntVec3 c)
		{
			if (!c.Fogged(base.Map))
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				int i = 0;
				while (true)
				{
					if (i < thingList.Count)
					{
						if (!this.CanDesignateThing(thingList[i]).Accepted)
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return (Pawn)thingList[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
