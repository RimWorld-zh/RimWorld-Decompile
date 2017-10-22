using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public static class PawnBanishUtility
	{
		private const float DeathChanceForCaravanPawnBanishedToDie = 0.8f;

		private static List<Hediff> tmpHediffs = new List<Hediff>();

		public static void Banish(Pawn pawn, int tile = -1)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				Log.Warning("Tried to banish " + pawn + " but he's neither a colonist, tame animal, nor prisoner.");
			}
			else
			{
				if (tile == -1)
				{
					tile = pawn.Tile;
				}
				bool flag = PawnBanishUtility.WouldBeLeftToDie(pawn, tile);
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, default(DamageInfo?), (PawnDiedOrDownedThoughtsKind)((!flag) ? 1 : 2));
				Caravan caravan = pawn.GetCaravan();
				if (caravan != null)
				{
					CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
					caravan.RemovePawn(pawn);
					if (flag)
					{
						if (Rand.Value < 0.800000011920929)
						{
							pawn.Kill(default(DamageInfo?), null);
						}
						else
						{
							PawnBanishUtility.HealIfPossible(pawn);
						}
					}
					if (pawn.guest != null)
					{
						pawn.guest.SetGuestStatus(null, false);
					}
					if (pawn.Faction == Faction.OfPlayer)
					{
						Faction newFaction = default(Faction);
						if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out newFaction, pawn.Faction != null && (int)pawn.Faction.def.techLevel >= 3, false, TechLevel.Undefined))
						{
							pawn.SetFaction(newFaction, null);
						}
						else
						{
							pawn.SetFaction(Faction.OfSpacer, null);
						}
					}
				}
				else
				{
					if (pawn.guest != null)
					{
						pawn.guest.SetGuestStatus(null, false);
					}
					if (pawn.Faction == Faction.OfPlayer)
					{
						pawn.SetFaction(Faction.OfSpacer, null);
					}
				}
			}
		}

		public static bool WouldBeLeftToDie(Pawn p, int tile)
		{
			bool result;
			if (p.Downed)
			{
				result = true;
			}
			else if (p.health.hediffSet.BleedRateTotal > 0.40000000596046448)
			{
				result = true;
			}
			else
			{
				if (tile != -1)
				{
					float f = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, GenLocalDate.Twelfth(p));
					if (!p.SafeTemperatureRange().Includes(f))
					{
						result = true;
						goto IL_00c3;
					}
				}
				List<Hediff> hediffs = p.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					HediffStage curStage = hediffs[i].CurStage;
					if (curStage != null && curStage.lifeThreatening)
						goto IL_00a1;
				}
				result = false;
			}
			goto IL_00c3;
			IL_00a1:
			result = true;
			goto IL_00c3;
			IL_00c3:
			return result;
		}

		public static string GetBanishPawnDialogText(Pawn banishedPawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = PawnBanishUtility.WouldBeLeftToDie(banishedPawn, banishedPawn.Tile);
			stringBuilder.Append("ConfirmBanishPawnDialog".Translate(banishedPawn.Label));
			if (flag)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmBanishPawnDialog_LeftToDie".Translate(banishedPawn.LabelShort).CapitalizeFirst());
			}
			List<ThingWithComps> list = (banishedPawn.equipment == null) ? null : banishedPawn.equipment.AllEquipmentListForReading;
			List<Apparel> list2 = (banishedPawn.apparel == null) ? null : banishedPawn.apparel.WornApparel;
			ThingOwner<Thing> thingOwner = (banishedPawn.inventory == null || !PawnBanishUtility.WillTakeInventoryIfBanished(banishedPawn)) ? null : banishedPawn.inventory.innerContainer;
			if (!list.NullOrEmpty() || !list2.NullOrEmpty() || !thingOwner.NullOrEmpty())
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmBanishPawnDialog_Items".Translate(banishedPawn.LabelShort).CapitalizeFirst().AdjustedFor(banishedPawn));
				stringBuilder.AppendLine();
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + list[i].LabelCap);
					}
				}
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + list2[j].LabelCap);
					}
				}
				if (thingOwner != null)
				{
					for (int k = 0; k < thingOwner.Count; k++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + thingOwner[k].LabelCap);
					}
				}
			}
			PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(banishedPawn, default(DamageInfo?), (PawnDiedOrDownedThoughtsKind)((!flag) ? 1 : 2), stringBuilder, "\n\n" + "ConfirmBanishPawnDialog_IndividualThoughts".Translate(banishedPawn.LabelShort), "\n\n" + "ConfirmBanishPawnDialog_AllColonistsThoughts".Translate());
			return stringBuilder.ToString();
		}

		public static void ShowBanishPawnConfirmationDialog(Pawn pawn)
		{
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(PawnBanishUtility.GetBanishPawnDialogText(pawn), (Action)delegate()
			{
				PawnBanishUtility.Banish(pawn, -1);
			}, true, (string)null);
			Find.WindowStack.Add(window);
		}

		public static string GetBanishButtonTip(Pawn pawn)
		{
			return (!PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Tile)) ? "BanishTip".Translate() : ("BanishTip".Translate() + "\n\n" + "BanishTipWillDie".Translate(pawn.LabelShort).CapitalizeFirst());
		}

		private static void HealIfPossible(Pawn p)
		{
			PawnBanishUtility.tmpHediffs.Clear();
			PawnBanishUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			for (int i = 0; i < PawnBanishUtility.tmpHediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = PawnBanishUtility.tmpHediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsOld())
				{
					p.health.RemoveHediff(hediff_Injury);
				}
				else
				{
					ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(PawnBanishUtility.tmpHediffs[i].def);
					if (immunityRecord != null)
					{
						immunityRecord.immunity = 1f;
					}
				}
			}
		}

		private static bool WillTakeInventoryIfBanished(Pawn pawn)
		{
			return !pawn.IsCaravanMember();
		}
	}
}
