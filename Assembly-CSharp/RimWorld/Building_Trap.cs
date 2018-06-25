using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000684 RID: 1668
	public abstract class Building_Trap : Building
	{
		// Token: 0x040013C2 RID: 5058
		private List<Pawn> touchingPawns = new List<Pawn>();

		// Token: 0x040013C3 RID: 5059
		private const float KnowerSpringChance = 0.004f;

		// Token: 0x040013C4 RID: 5060
		private const ushort KnowerPathFindCost = 800;

		// Token: 0x040013C5 RID: 5061
		private const ushort KnowerPathWalkCost = 30;

		// Token: 0x040013C6 RID: 5062
		private const float AnimalSpringChanceFactor = 0.1f;

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x0600231F RID: 8991 RVA: 0x0012E818 File Offset: 0x0012CC18
		public virtual bool Armed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0012E82E File Offset: 0x0012CC2E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.touchingPawns, "testees", LookMode.Reference, new object[0]);
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x0012E850 File Offset: 0x0012CC50
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

		// Token: 0x06002322 RID: 8994 RVA: 0x0012E934 File Offset: 0x0012CD34
		protected virtual float SpringChance(Pawn p)
		{
			float num;
			if (this.KnowsOfTrap(p))
			{
				num = 0.004f;
			}
			else
			{
				num = this.GetStatValue(StatDefOf.TrapSpringChance, true);
			}
			num *= GenMath.LerpDouble(0.4f, 0.8f, 0f, 1f, p.BodySize);
			if (p.RaceProps.Animal)
			{
				num *= 0.1f;
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0012E9B0 File Offset: 0x0012CDB0
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

		// Token: 0x06002324 RID: 8996 RVA: 0x0012EA54 File Offset: 0x0012CE54
		public bool KnowsOfTrap(Pawn p)
		{
			return (p.Faction != null && !p.Faction.HostileTo(base.Faction)) || (p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState) || (p.guest != null && p.guest.Released) || (p.RaceProps.Humanlike && p.IsFormingCaravan());
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0012EB04 File Offset: 0x0012CF04
		public override ushort PathFindCostFor(Pawn p)
		{
			ushort result;
			if (!this.Armed)
			{
				result = 0;
			}
			else if (this.KnowsOfTrap(p))
			{
				result = 800;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0012EB44 File Offset: 0x0012CF44
		public override ushort PathWalkCostFor(Pawn p)
		{
			ushort result;
			if (!this.Armed)
			{
				result = 0;
			}
			else if (this.KnowsOfTrap(p))
			{
				result = 30;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0012EB80 File Offset: 0x0012CF80
		public override bool IsDangerousFor(Pawn p)
		{
			return this.Armed && this.KnowsOfTrap(p);
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0012EBAC File Offset: 0x0012CFAC
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

		// Token: 0x06002329 RID: 9001 RVA: 0x0012EC14 File Offset: 0x0012D014
		public void Spring(Pawn p)
		{
			SoundDefOf.DeadfallSpring.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			if (p != null && p.Faction != null)
			{
				p.Faction.TacticalMemory.TrapRevealed(base.Position, base.Map);
			}
			this.SpringSub(p);
		}

		// Token: 0x0600232A RID: 9002
		protected abstract void SpringSub(Pawn p);
	}
}
