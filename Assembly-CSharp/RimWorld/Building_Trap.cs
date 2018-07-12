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

		private const float KnowerSpringChanceFactorSameFaction = 0.005f;

		private const float KnowerSpringChanceFactorWildAnimal = 0.2f;

		private const float KnowerSpringChanceFactorFactionlessHuman = 0.3f;

		private const float KnowerSpringChanceFactorOther = 0f;

		private const ushort KnowerPathFindCost = 800;

		private const ushort KnowerPathWalkCost = 40;

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
			if (Rand.Chance(this.SpringChance(p)))
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
			float num = this.GetStatValue(StatDefOf.TrapSpringChance, true);
			if (this.KnowsOfTrap(p))
			{
				if (p.Faction == null)
				{
					if (p.RaceProps.Animal)
					{
						num *= 0.2f;
					}
					else
					{
						num *= 0.3f;
					}
				}
				else if (p.Faction == base.Faction)
				{
					num *= 0.005f;
				}
				else
				{
					num *= 0f;
				}
			}
			return Mathf.Clamp01(num);
		}

		public bool KnowsOfTrap(Pawn p)
		{
			return (p.Faction != null && !p.Faction.HostileTo(base.Faction)) || (p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState) || (p.guest != null && p.guest.Released) || (p.RaceProps.Humanlike && p.IsFormingCaravan());
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
				result = 800;
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
				result = 40;
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
