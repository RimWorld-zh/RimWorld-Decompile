using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D1 RID: 1745
	public static class MoteMaker
	{
		// Token: 0x0400151E RID: 5406
		private static IntVec3[] UpRightPattern = new IntVec3[]
		{
			new IntVec3(0, 0, 0),
			new IntVec3(1, 0, 0),
			new IntVec3(0, 0, 1),
			new IntVec3(1, 0, 1)
		};

		// Token: 0x060025C2 RID: 9666 RVA: 0x00143B24 File Offset: 0x00141F24
		public static Mote ThrowMetaIcon(IntVec3 cell, Map map, ThingDef moteDef)
		{
			Mote result;
			if (!cell.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
			{
				result = null;
			}
			else
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(moteDef, null);
				moteThrown.Scale = 0.7f;
				moteThrown.rotationRate = Rand.Range(-3f, 3f);
				moteThrown.exactPosition = cell.ToVector3Shifted();
				moteThrown.exactPosition += new Vector3(0.35f, 0f, 0.35f);
				moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value) * 0.1f;
				moteThrown.SetVelocity((float)Rand.Range(30, 60), 0.42f);
				GenSpawn.Spawn(moteThrown, cell, map, WipeMode.Vanish);
				result = moteThrown;
			}
			return result;
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x00143C0A File Offset: 0x0014200A
		public static void MakeStaticMote(IntVec3 cell, Map map, ThingDef moteDef, float scale = 1f)
		{
			MoteMaker.MakeStaticMote(cell.ToVector3Shifted(), map, moteDef, scale);
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x00143C20 File Offset: 0x00142020
		public static Mote MakeStaticMote(Vector3 loc, Map map, ThingDef moteDef, float scale = 1f)
		{
			Mote result;
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
			{
				result = null;
			}
			else
			{
				Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
				mote.exactPosition = loc;
				mote.Scale = scale;
				GenSpawn.Spawn(mote, loc.ToIntVec3(), map, WipeMode.Vanish);
				result = mote;
			}
			return result;
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x00143C83 File Offset: 0x00142083
		public static void ThrowText(Vector3 loc, Map map, string text, float timeBeforeStartFadeout = -1f)
		{
			MoteMaker.ThrowText(loc, map, text, Color.white, timeBeforeStartFadeout);
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x00143C94 File Offset: 0x00142094
		public static void ThrowText(Vector3 loc, Map map, string text, Color color, float timeBeforeStartFadeout = -1f)
		{
			IntVec3 intVec = loc.ToIntVec3();
			if (intVec.InBounds(map))
			{
				MoteText moteText = (MoteText)ThingMaker.MakeThing(ThingDefOf.Mote_Text, null);
				moteText.exactPosition = loc;
				moteText.SetVelocity((float)Rand.Range(5, 35), Rand.Range(0.42f, 0.45f));
				moteText.text = text;
				moteText.textColor = color;
				if (timeBeforeStartFadeout >= 0f)
				{
					moteText.overrideTimeBeforeStartFadeout = timeBeforeStartFadeout;
				}
				GenSpawn.Spawn(moteText, intVec, map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x00143D1C File Offset: 0x0014211C
		public static void ThrowMetaPuffs(CellRect rect, Map map)
		{
			if (!Find.TickManager.Paused)
			{
				for (int i = rect.minX; i <= rect.maxX; i++)
				{
					for (int j = rect.minZ; j <= rect.maxZ; j++)
					{
						MoteMaker.ThrowMetaPuffs(new TargetInfo(new IntVec3(i, 0, j), map, false));
					}
				}
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x00143D90 File Offset: 0x00142190
		public static void ThrowMetaPuffs(TargetInfo targ)
		{
			Vector3 a = (!targ.HasThing) ? targ.Cell.ToVector3Shifted() : targ.Thing.TrueCenter();
			int num = Rand.RangeInclusive(4, 6);
			for (int i = 0; i < num; i++)
			{
				Vector3 loc = a + new Vector3(Rand.Range(-0.5f, 0.5f), 0f, Rand.Range(-0.5f, 0.5f));
				MoteMaker.ThrowMetaPuff(loc, targ.Map);
			}
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00143E28 File Offset: 0x00142228
		public static void ThrowMetaPuff(Vector3 loc, Map map)
		{
			if (loc.ShouldSpawnMotesAt(map))
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_MetaPuff, null);
				moteThrown.Scale = 1.9f;
				moteThrown.rotationRate = (float)Rand.Range(-60, 60);
				moteThrown.exactPosition = loc;
				moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.6f, 0.78f));
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x00143EAC File Offset: 0x001422AC
		private static MoteThrown NewBaseAirPuff()
		{
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_AirPuff, null);
			moteThrown.Scale = 1.5f;
			moteThrown.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			return moteThrown;
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x00143EF4 File Offset: 0x001422F4
		public static void ThrowAirPuffUp(Vector3 loc, Map map)
		{
			if (loc.ToIntVec3().ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = MoteMaker.NewBaseAirPuff();
				moteThrown.exactPosition = loc;
				moteThrown.exactPosition += new Vector3(Rand.Range(-0.02f, 0.02f), 0f, Rand.Range(-0.02f, 0.02f));
				moteThrown.SetVelocity((float)Rand.Range(-45, 45), Rand.Range(1.2f, 1.5f));
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x00143F9C File Offset: 0x0014239C
		internal static void ThrowBreathPuff(Vector3 loc, Map map, float throwAngle, Vector3 inheritVelocity)
		{
			if (loc.ToIntVec3().ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = MoteMaker.NewBaseAirPuff();
				moteThrown.exactPosition = loc;
				moteThrown.exactPosition += new Vector3(Rand.Range(-0.005f, 0.005f), 0f, Rand.Range(-0.005f, 0.005f));
				moteThrown.SetVelocity(throwAngle + (float)Rand.Range(-10, 10), Rand.Range(0.1f, 0.8f));
				moteThrown.Velocity += inheritVelocity * 0.5f;
				moteThrown.Scale = Rand.Range(0.6f, 0.7f);
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x00144078 File Offset: 0x00142478
		public static void ThrowDustPuff(IntVec3 cell, Map map, float scale)
		{
			Vector3 loc = cell.ToVector3() + new Vector3(Rand.Value, 0f, Rand.Value);
			MoteMaker.ThrowDustPuff(loc, map, scale);
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x001440B0 File Offset: 0x001424B0
		public static void ThrowDustPuff(Vector3 loc, Map map, float scale)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_DustPuff, null);
				moteThrown.Scale = 1.9f * scale;
				moteThrown.rotationRate = (float)Rand.Range(-60, 60);
				moteThrown.exactPosition = loc;
				moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.6f, 0.75f));
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00144144 File Offset: 0x00142544
		public static void ThrowDustPuffThick(Vector3 loc, Map map, float scale, Color color)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_DustPuffThick, null);
				moteThrown.Scale = scale;
				moteThrown.rotationRate = (float)Rand.Range(-60, 60);
				moteThrown.exactPosition = loc;
				moteThrown.instanceColor = color;
				moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.6f, 0.75f));
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x001441DC File Offset: 0x001425DC
		public static void ThrowTornadoDustPuff(Vector3 loc, Map map, float scale, Color color)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_TornadoDustPuff, null);
				moteThrown.Scale = 1.9f * scale;
				moteThrown.rotationRate = (float)Rand.Range(-60, 60);
				moteThrown.exactPosition = loc;
				moteThrown.instanceColor = color;
				moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.6f, 0.75f));
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x00144278 File Offset: 0x00142678
		public static void ThrowSmoke(Vector3 loc, Map map, float size)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_Smoke, null);
				moteThrown.Scale = Rand.Range(1.5f, 2.5f) * size;
				moteThrown.rotationRate = Rand.Range(-30f, 30f);
				moteThrown.exactPosition = loc;
				moteThrown.SetVelocity((float)Rand.Range(30, 40), Rand.Range(0.5f, 0.7f));
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x0014431C File Offset: 0x0014271C
		public static void ThrowFireGlow(IntVec3 c, Map map, float size)
		{
			Vector3 vector = c.ToVector3Shifted();
			if (vector.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
				if (vector.InBounds(map))
				{
					MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_FireGlow, null);
					moteThrown.Scale = Rand.Range(4f, 6f) * size;
					moteThrown.rotationRate = Rand.Range(-3f, 3f);
					moteThrown.exactPosition = vector;
					moteThrown.SetVelocity((float)Rand.Range(0, 360), 0.12f);
					GenSpawn.Spawn(moteThrown, vector.ToIntVec3(), map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x001443FC File Offset: 0x001427FC
		public static void ThrowHeatGlow(IntVec3 c, Map map, float size)
		{
			Vector3 vector = c.ToVector3Shifted();
			if (vector.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
				if (vector.InBounds(map))
				{
					MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_HeatGlow, null);
					moteThrown.Scale = Rand.Range(4f, 6f) * size;
					moteThrown.rotationRate = Rand.Range(-3f, 3f);
					moteThrown.exactPosition = vector;
					moteThrown.SetVelocity((float)Rand.Range(0, 360), 0.12f);
					GenSpawn.Spawn(moteThrown, vector.ToIntVec3(), map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x001444DC File Offset: 0x001428DC
		public static void ThrowMicroSparks(Vector3 loc, Map map)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_MicroSparks, null);
				moteThrown.Scale = Rand.Range(0.8f, 1.2f);
				moteThrown.rotationRate = Rand.Range(-12f, 12f);
				moteThrown.exactPosition = loc;
				moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
				moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
				moteThrown.SetVelocity((float)Rand.Range(35, 45), 1.2f);
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x001445BC File Offset: 0x001429BC
		public static void ThrowLightningGlow(Vector3 loc, Map map, float size)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_LightningGlow, null);
				moteThrown.Scale = Rand.Range(4f, 6f) * size;
				moteThrown.rotationRate = Rand.Range(-3f, 3f);
				moteThrown.exactPosition = loc + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
				moteThrown.SetVelocity((float)Rand.Range(0, 360), 1.2f);
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00144680 File Offset: 0x00142A80
		public static void PlaceFootprint(Vector3 loc, Map map, float rot)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_Footprint, null);
				moteThrown.Scale = 0.5f;
				moteThrown.exactRotation = rot;
				moteThrown.exactPosition = loc;
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x001446E8 File Offset: 0x00142AE8
		public static void ThrowHorseshoe(Pawn thrower, IntVec3 targetCell)
		{
			MoteMaker.ThrowObjectAt(thrower, targetCell, ThingDefOf.Mote_Horseshoe);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x001446F7 File Offset: 0x00142AF7
		public static void ThrowStone(Pawn thrower, IntVec3 targetCell)
		{
			MoteMaker.ThrowObjectAt(thrower, targetCell, ThingDefOf.Mote_Stone);
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x00144708 File Offset: 0x00142B08
		private static void ThrowObjectAt(Pawn thrower, IntVec3 targetCell, ThingDef mote)
		{
			if (thrower.Position.ShouldSpawnMotesAt(thrower.Map) && !thrower.Map.moteCounter.Saturated)
			{
				float num = Rand.Range(3.8f, 5.6f);
				Vector3 vector = targetCell.ToVector3Shifted() + Vector3Utility.RandomHorizontalOffset((1f - (float)thrower.skills.GetSkill(SkillDefOf.Shooting).Level / 20f) * 1.8f);
				vector.y = thrower.DrawPos.y;
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(mote, null);
				moteThrown.Scale = 1f;
				moteThrown.rotationRate = (float)Rand.Range(-300, 300);
				moteThrown.exactPosition = thrower.DrawPos;
				moteThrown.SetVelocity((vector - moteThrown.exactPosition).AngleFlat(), num);
				moteThrown.airTimeLeft = (float)Mathf.RoundToInt((moteThrown.exactPosition - vector).MagnitudeHorizontal() / num);
				GenSpawn.Spawn(moteThrown, thrower.Position, thrower.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x0014482C File Offset: 0x00142C2C
		public static Mote MakeStunOverlay(Thing stunnedThing)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Stun, null);
			mote.Attach(stunnedThing);
			GenSpawn.Spawn(mote, stunnedThing.Position, stunnedThing.Map, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00144874 File Offset: 0x00142C74
		public static MoteDualAttached MakeInteractionOverlay(ThingDef moteDef, TargetInfo A, TargetInfo B)
		{
			MoteDualAttached moteDualAttached = (MoteDualAttached)ThingMaker.MakeThing(moteDef, null);
			moteDualAttached.Scale = 0.5f;
			moteDualAttached.Attach(A, B);
			GenSpawn.Spawn(moteDualAttached, A.Cell, A.Map ?? B.Map, WipeMode.Vanish);
			return moteDualAttached;
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x001448D0 File Offset: 0x00142CD0
		public static void MakeColonistActionOverlay(Pawn pawn, ThingDef moteDef)
		{
			MoteThrownAttached moteThrownAttached = (MoteThrownAttached)ThingMaker.MakeThing(moteDef, null);
			moteThrownAttached.Attach(pawn);
			moteThrownAttached.Scale = 1.5f;
			moteThrownAttached.SetVelocity(Rand.Range(20f, 25f), 0.4f);
			GenSpawn.Spawn(moteThrownAttached, pawn.Position, pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x00144930 File Offset: 0x00142D30
		private static MoteBubble ExistingMoteBubbleOn(Pawn pawn)
		{
			MoteBubble result;
			if (!pawn.Spawned)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					IntVec3 c = pawn.Position + MoteMaker.UpRightPattern[i];
					if (c.InBounds(pawn.Map))
					{
						List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							Thing thing = thingList[j];
							MoteBubble moteBubble = thing as MoteBubble;
							if (moteBubble != null && moteBubble.link1.Linked && moteBubble.link1.Target.HasThing && moteBubble.link1.Target == pawn)
							{
								return moteBubble;
							}
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x00144A38 File Offset: 0x00142E38
		public static MoteBubble MakeMoodThoughtBubble(Pawn pawn, Thought thought)
		{
			MoteBubble result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = null;
			}
			else if (!pawn.Spawned)
			{
				result = null;
			}
			else
			{
				float num = thought.MoodOffset();
				if (num == 0f)
				{
					result = null;
				}
				else
				{
					MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
					if (moteBubble != null)
					{
						if (moteBubble.def == ThingDefOf.Mote_Speech)
						{
							return null;
						}
						if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
						{
							moteBubble.Destroy(DestroyMode.Vanish);
						}
					}
					ThingDef def = (num <= 0f) ? ThingDefOf.Mote_ThoughtBad : ThingDefOf.Mote_ThoughtGood;
					MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(def, null);
					moteBubble2.SetupMoteBubble(thought.Icon, null);
					moteBubble2.Attach(pawn);
					GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
					result = moteBubble2;
				}
			}
			return result;
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x00144B34 File Offset: 0x00142F34
		public static MoteBubble MakeInteractionBubble(Pawn initiator, Pawn recipient, ThingDef interactionMote, Texture2D symbol)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(interactionMote, null);
			moteBubble2.SetupMoteBubble(symbol, recipient);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00144BCC File Offset: 0x00142FCC
		public static void ThrowExplosionCell(IntVec3 cell, Map map, ThingDef moteDef, Color color)
		{
			if (cell.ShouldSpawnMotesAt(map))
			{
				Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
				mote.exactRotation = (float)(90 * Rand.RangeInclusive(0, 3));
				mote.exactPosition = cell.ToVector3Shifted();
				mote.instanceColor = color;
				GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
				if (Rand.Value < 0.7f)
				{
					MoteMaker.ThrowDustPuff(cell, map, 1.2f);
				}
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x00144C44 File Offset: 0x00143044
		public static void ThrowExplosionInteriorMote(Vector3 loc, Map map, ThingDef moteDef)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(moteDef, null);
				moteThrown.Scale = Rand.Range(3f, 4.5f);
				moteThrown.rotationRate = Rand.Range(-30f, 30f);
				moteThrown.exactPosition = loc;
				moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.48f, 0.72f));
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00144CE4 File Offset: 0x001430E4
		public static void MakeWaterSplash(Vector3 loc, Map map, float size, float velocity)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteSplash moteSplash = (MoteSplash)ThingMaker.MakeThing(ThingDefOf.Mote_WaterSplash, null);
				moteSplash.Initialize(loc, size, velocity);
				GenSpawn.Spawn(moteSplash, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x00144D3C File Offset: 0x0014313C
		public static void MakeBombardmentMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Bombardment, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = (float)Mathf.Max(23, 25) * 6f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00144D94 File Offset: 0x00143194
		public static void MakePowerBeamMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_PowerBeam, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 90f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x00144DE0 File Offset: 0x001431E0
		public static void PlaceTempRoof(IntVec3 cell, Map map)
		{
			if (cell.ShouldSpawnMotesAt(map))
			{
				Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_TempRoof, null);
				mote.exactPosition = cell.ToVector3Shifted();
				GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
			}
		}
	}
}
