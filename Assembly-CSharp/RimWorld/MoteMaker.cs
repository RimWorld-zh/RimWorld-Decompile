using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D3 RID: 1747
	public static class MoteMaker
	{
		// Token: 0x060025C7 RID: 9671 RVA: 0x00143628 File Offset: 0x00141A28
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

		// Token: 0x060025C8 RID: 9672 RVA: 0x0014370E File Offset: 0x00141B0E
		public static void MakeStaticMote(IntVec3 cell, Map map, ThingDef moteDef, float scale = 1f)
		{
			MoteMaker.MakeStaticMote(cell.ToVector3Shifted(), map, moteDef, scale);
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00143724 File Offset: 0x00141B24
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

		// Token: 0x060025CA RID: 9674 RVA: 0x00143787 File Offset: 0x00141B87
		public static void ThrowText(Vector3 loc, Map map, string text, float timeBeforeStartFadeout = -1f)
		{
			MoteMaker.ThrowText(loc, map, text, Color.white, timeBeforeStartFadeout);
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x00143798 File Offset: 0x00141B98
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

		// Token: 0x060025CC RID: 9676 RVA: 0x00143820 File Offset: 0x00141C20
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

		// Token: 0x060025CD RID: 9677 RVA: 0x00143894 File Offset: 0x00141C94
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

		// Token: 0x060025CE RID: 9678 RVA: 0x0014392C File Offset: 0x00141D2C
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

		// Token: 0x060025CF RID: 9679 RVA: 0x001439B0 File Offset: 0x00141DB0
		private static MoteThrown NewBaseAirPuff()
		{
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_AirPuff, null);
			moteThrown.Scale = 1.5f;
			moteThrown.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			return moteThrown;
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x001439F8 File Offset: 0x00141DF8
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

		// Token: 0x060025D1 RID: 9681 RVA: 0x00143AA0 File Offset: 0x00141EA0
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

		// Token: 0x060025D2 RID: 9682 RVA: 0x00143B7C File Offset: 0x00141F7C
		public static void ThrowDustPuff(IntVec3 cell, Map map, float scale)
		{
			Vector3 loc = cell.ToVector3() + new Vector3(Rand.Value, 0f, Rand.Value);
			MoteMaker.ThrowDustPuff(loc, map, scale);
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00143BB4 File Offset: 0x00141FB4
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

		// Token: 0x060025D4 RID: 9684 RVA: 0x00143C48 File Offset: 0x00142048
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

		// Token: 0x060025D5 RID: 9685 RVA: 0x00143CE0 File Offset: 0x001420E0
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

		// Token: 0x060025D6 RID: 9686 RVA: 0x00143D7C File Offset: 0x0014217C
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

		// Token: 0x060025D7 RID: 9687 RVA: 0x00143E20 File Offset: 0x00142220
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

		// Token: 0x060025D8 RID: 9688 RVA: 0x00143F00 File Offset: 0x00142300
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

		// Token: 0x060025D9 RID: 9689 RVA: 0x00143FE0 File Offset: 0x001423E0
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

		// Token: 0x060025DA RID: 9690 RVA: 0x001440C0 File Offset: 0x001424C0
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

		// Token: 0x060025DB RID: 9691 RVA: 0x00144184 File Offset: 0x00142584
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

		// Token: 0x060025DC RID: 9692 RVA: 0x001441EC File Offset: 0x001425EC
		public static void ThrowHorseshoe(Pawn thrower, IntVec3 targetCell)
		{
			MoteMaker.ThrowObjectAt(thrower, targetCell, ThingDefOf.Mote_Horseshoe);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x001441FB File Offset: 0x001425FB
		public static void ThrowStone(Pawn thrower, IntVec3 targetCell)
		{
			MoteMaker.ThrowObjectAt(thrower, targetCell, ThingDefOf.Mote_Stone);
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x0014420C File Offset: 0x0014260C
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

		// Token: 0x060025DF RID: 9695 RVA: 0x00144330 File Offset: 0x00142730
		public static Mote MakeStunOverlay(Thing stunnedThing)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Stun, null);
			mote.Attach(stunnedThing);
			GenSpawn.Spawn(mote, stunnedThing.Position, stunnedThing.Map, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00144378 File Offset: 0x00142778
		public static MoteDualAttached MakeInteractionOverlay(ThingDef moteDef, TargetInfo A, TargetInfo B)
		{
			MoteDualAttached moteDualAttached = (MoteDualAttached)ThingMaker.MakeThing(moteDef, null);
			moteDualAttached.Scale = 0.5f;
			moteDualAttached.Attach(A, B);
			GenSpawn.Spawn(moteDualAttached, A.Cell, A.Map ?? B.Map, WipeMode.Vanish);
			return moteDualAttached;
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x001443D4 File Offset: 0x001427D4
		public static void MakeColonistActionOverlay(Pawn pawn, ThingDef moteDef)
		{
			MoteThrownAttached moteThrownAttached = (MoteThrownAttached)ThingMaker.MakeThing(moteDef, null);
			moteThrownAttached.Attach(pawn);
			moteThrownAttached.Scale = 1.5f;
			moteThrownAttached.SetVelocity(Rand.Range(20f, 25f), 0.4f);
			GenSpawn.Spawn(moteThrownAttached, pawn.Position, pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00144434 File Offset: 0x00142834
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

		// Token: 0x060025E3 RID: 9699 RVA: 0x0014453C File Offset: 0x0014293C
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

		// Token: 0x060025E4 RID: 9700 RVA: 0x00144638 File Offset: 0x00142A38
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

		// Token: 0x060025E5 RID: 9701 RVA: 0x001446D0 File Offset: 0x00142AD0
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

		// Token: 0x060025E6 RID: 9702 RVA: 0x00144748 File Offset: 0x00142B48
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

		// Token: 0x060025E7 RID: 9703 RVA: 0x001447E8 File Offset: 0x00142BE8
		public static void MakeWaterSplash(Vector3 loc, Map map, float size, float velocity)
		{
			if (loc.ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteSplash moteSplash = (MoteSplash)ThingMaker.MakeThing(ThingDefOf.Mote_WaterSplash, null);
				moteSplash.Initialize(loc, size, velocity);
				GenSpawn.Spawn(moteSplash, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x00144840 File Offset: 0x00142C40
		public static void MakeBombardmentMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Bombardment, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = (float)Mathf.Max(23, 25) * 6f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00144898 File Offset: 0x00142C98
		public static void MakePowerBeamMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_PowerBeam, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 90f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x001448E4 File Offset: 0x00142CE4
		public static void PlaceTempRoof(IntVec3 cell, Map map)
		{
			if (cell.ShouldSpawnMotesAt(map))
			{
				Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_TempRoof, null);
				mote.exactPosition = cell.ToVector3Shifted();
				GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
			}
		}

		// Token: 0x0400151C RID: 5404
		private static IntVec3[] UpRightPattern = new IntVec3[]
		{
			new IntVec3(0, 0, 0),
			new IntVec3(1, 0, 0),
			new IntVec3(0, 0, 1),
			new IntVec3(1, 0, 1)
		};
	}
}
