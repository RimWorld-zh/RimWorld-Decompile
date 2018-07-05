using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public abstract class Building_Trap : Building
	{
		private List<Pawn> touchingPawns = new List<Pawn>();

		private const float KnowerSpringChance = 0f;

		private const ushort KnowerPathFindCost = 60;

		private const ushort KnowerPathWalkCost = 60;

		protected Building_Trap()
		{
		}

		public virtual bool Armed
		{
			get
			{
				return true;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.touchingPawns, "testees", LookMode.Reference, new object[0]);
		}

		public override void Tick()
		{
			if (this.Armed)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && !this.touchingPawns.Contains(pawn))
					{
						this.touchingPawns.Add(pawn);
						this.CheckSpring(pawn);
					}
				}
			}
			for (int j = 0; j < this.touchingPawns.Count; j++)
			{
				Pawn pawn2 = this.touchingPawns[j];
				if (!pawn2.Spawned || pawn2.Position != base.Position)
				{
					this.touchingPawns.Remove(pawn2);
				}
			}
			base.Tick();
		}

		private void CheckSpring(Pawn p)
		{
			if (Rand.Value < this.SpringChance(p))
			{
				this.Spring(p);
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(new object[]
					{
						p.LabelShort
					}), "LetterFriendlyTrapSprung".Translate(new object[]
					{
						p.LabelShort
					}), LetterDefOf.NegativeEvent, new TargetInfo(base.Position, base.Map, false), null, null);
				}
			}
		}

		protected virtual float SpringChance(Pawn p)
		{
			float value;
			if (this.KnowsOfTrap(p))
			{
				value = 0f;
			}
			else
			{
				value = this.GetStatValue(StatDefOf.TrapSpringChance, true);
			}
			return Mathf.Clamp01(value);
		}

		public bool KnowsOfTrap(Pawn p)
		{
			return (p.Faction != null && !p.Faction.HostileTo(base.Faction)) || (p.Faction == null && this.def.building.trapWildAnimalsAvoid && p.RaceProps.Animal && !p.InAggroMentalState) || (p.guest != null && p.guest.Released) || (p.RaceProps.Humanlike && p.IsFormingCaravan());
		}

		public override ushort PathFindCostFor(Pawn p)
		{
			ushort result;
			if (!this.Armed || !this.KnowsOfTrap(p))
			{
				result = 0;
			}
			else
			{
				result = 60;
			}
			return result;
		}

		public override ushort PathWalkCostFor(Pawn p)
		{
			ushort result;
			if (!this.Armed || !this.KnowsOfTrap(p))
			{
				result = 0;
			}
			else
			{
				result = 60;
			}
			return result;
		}

		public override bool IsDangerousFor(Pawn p)
		{
			return this.Armed && this.KnowsOfTrap(p);
		}

		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			if (this.Armed)
			{
				text += "TrapArmed".Translate();
			}
			else
			{
				text += "TrapNotArmed".Translate();
			}
			return text;
		}

		public void Spring(Pawn p)
		{
			SoundDefOf.DeadfallSpring.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			this.SpringSub(p);
		}

		protected abstract void SpringSub(Pawn p);
	}
}
