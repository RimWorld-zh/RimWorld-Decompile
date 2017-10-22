using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Tame : Designator
	{
		private List<Pawn> justDesignated = new List<Pawn>();

		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		public Designator_Tame()
		{
			base.defaultLabel = "DesignatorTame".Translate();
			base.defaultDesc = "DesignatorTameDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/Tame", true);
			base.soundDragSustain = SoundDefOf.DesignateDragStandard;
			base.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
			base.useMouseIcon = true;
			base.soundSucceeded = SoundDefOf.DesignateClaim;
			base.hotKey = KeyBindingDefOf.Misc4;
			base.tutorTag = "Tame";
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.TameablesInCell(c).Any())
			{
				return "MessageMustDesignateTameable".Translate();
			}
			return true;
		}

		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn item in this.TameablesInCell(loc))
			{
				this.DesignateThing(item);
			}
		}

		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.def.race.Animal && pawn.Faction == null && pawn.RaceProps.wildness < 1.0 && !pawn.HostileTo(t) && base.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Tame) == null)
			{
				return true;
			}
			return false;
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct().GetEnumerator())
			{
				PawnKindDef kind;
				while (enumerator.MoveNext())
				{
					kind = enumerator.Current;
					if (kind.RaceProps.manhunterOnTameFailChance > 0.0)
					{
						Messages.Message("MessageAnimalManhuntsOnTameFailed".Translate(kind.label, kind.RaceProps.manhunterOnTameFailChance.ToStringPercent("F2")), (Thing)this.justDesignated.First((Func<Pawn, bool>)((Pawn x) => x.kindDef == kind)), MessageSound.Standard);
					}
				}
			}
			IEnumerable<Pawn> source = from c in base.Map.mapPawns.FreeColonistsSpawned
			where c.workSettings.WorkIsActive(WorkTypeDefOf.Handling)
			select c;
			if (!source.Any())
			{
				source = base.Map.mapPawns.FreeColonistsSpawned;
			}
			if (source.Any())
			{
				Pawn pawn = source.MaxBy((Func<Pawn, int>)((Pawn c) => c.skills.GetSkill(SkillDefOf.Animals).Level));
				int level = pawn.skills.GetSkill(SkillDefOf.Animals).Level;
				using (IEnumerator<ThingDef> enumerator2 = (from t in this.justDesignated
				select t.def).Distinct().GetEnumerator())
				{
					ThingDef ad;
					while (enumerator2.MoveNext())
					{
						ad = enumerator2.Current;
						int num = Mathf.RoundToInt(ad.GetStatValueAbstract(StatDefOf.MinimumHandlingSkill, null));
						if (num > level)
						{
							Messages.Message("MessageNoHandlerSkilledEnough".Translate(ad.label, num.ToStringCached(), SkillDefOf.Animals.LabelCap, pawn.LabelShort, level), (Thing)this.justDesignated.First((Func<Pawn, bool>)((Pawn x) => x.def == ad)), MessageSound.Negative);
						}
					}
				}
			}
			this.justDesignated.Clear();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTaming, KnowledgeAmount.Total);
		}

		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Tame));
			this.justDesignated.Add((Pawn)t);
		}

		private IEnumerable<Pawn> TameablesInCell(IntVec3 c)
		{
			if (!c.Fogged(base.Map))
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (this.CanDesignateThing(thingList[i]).Accepted)
					{
						yield return (Pawn)thingList[i];
					}
				}
			}
		}
	}
}
