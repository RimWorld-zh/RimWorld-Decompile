using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000471 RID: 1137
	public static class PawnBanishUtility
	{
		// Token: 0x060013F9 RID: 5113 RVA: 0x000AE020 File Offset: 0x000AC420
		public static void Banish(Pawn pawn, int tile = -1)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				Log.Warning("Tried to banish " + pawn + " but he's neither a colonist, tame animal, nor prisoner.", false);
			}
			else
			{
				if (tile == -1)
				{
					tile = pawn.Tile;
				}
				bool flag = PawnBanishUtility.WouldBeLeftToDie(pawn, tile);
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, (!flag) ? PawnDiedOrDownedThoughtsKind.Banished : PawnDiedOrDownedThoughtsKind.BanishedToDie);
				Caravan caravan = pawn.GetCaravan();
				if (caravan != null)
				{
					CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
					caravan.RemovePawn(pawn);
					if (flag)
					{
						if (Rand.Value < 0.8f)
						{
							pawn.Kill(null, null);
						}
						else
						{
							PawnBanishUtility.HealIfPossible(pawn);
						}
					}
				}
				if (pawn.guest != null)
				{
					pawn.guest.SetGuestStatus(null, false);
				}
				if (pawn.Faction == Faction.OfPlayer)
				{
					Faction faction;
					if (!pawn.Spawned && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, pawn.Faction != null && pawn.Faction.def.techLevel >= TechLevel.Medieval, false, TechLevel.Undefined))
					{
						if (pawn.Faction != faction)
						{
							pawn.SetFaction(faction, null);
						}
					}
					else if (pawn.Faction != null)
					{
						pawn.SetFaction(null, null);
					}
				}
			}
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x000AE18C File Offset: 0x000AC58C
		public static bool WouldBeLeftToDie(Pawn p, int tile)
		{
			bool result;
			if (p.Downed)
			{
				result = true;
			}
			else if (p.health.hediffSet.BleedRateTotal > 0.4f)
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
						return true;
					}
				}
				List<Hediff> hediffs = p.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					HediffStage curStage = hediffs[i].CurStage;
					if (curStage != null && curStage.lifeThreatening)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x000AE260 File Offset: 0x000AC660
		public static string GetBanishPawnDialogText(Pawn banishedPawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = PawnBanishUtility.WouldBeLeftToDie(banishedPawn, banishedPawn.Tile);
			stringBuilder.Append("ConfirmBanishPawnDialog".Translate(new object[]
			{
				banishedPawn.Label
			}));
			if (flag)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmBanishPawnDialog_LeftToDie".Translate(new object[]
				{
					banishedPawn.LabelShort
				}).CapitalizeFirst());
			}
			List<ThingWithComps> list = (banishedPawn.equipment == null) ? null : banishedPawn.equipment.AllEquipmentListForReading;
			List<Apparel> list2 = (banishedPawn.apparel == null) ? null : banishedPawn.apparel.WornApparel;
			ThingOwner<Thing> thingOwner = (banishedPawn.inventory == null || !PawnBanishUtility.WillTakeInventoryIfBanished(banishedPawn)) ? null : banishedPawn.inventory.innerContainer;
			if (!list.NullOrEmpty<ThingWithComps>() || !list2.NullOrEmpty<Apparel>() || !thingOwner.NullOrEmpty<Thing>())
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmBanishPawnDialog_Items".Translate(new object[]
				{
					banishedPawn.LabelShort
				}).CapitalizeFirst().AdjustedFor(banishedPawn));
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
			PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(banishedPawn, null, (!flag) ? PawnDiedOrDownedThoughtsKind.Banished : PawnDiedOrDownedThoughtsKind.BanishedToDie, stringBuilder, "\n\n" + "ConfirmBanishPawnDialog_IndividualThoughts".Translate(new object[]
			{
				banishedPawn.LabelShort
			}), "\n\n" + "ConfirmBanishPawnDialog_AllColonistsThoughts".Translate());
			return stringBuilder.ToString();
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x000AE4E8 File Offset: 0x000AC8E8
		public static void ShowBanishPawnConfirmationDialog(Pawn pawn)
		{
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(PawnBanishUtility.GetBanishPawnDialogText(pawn), delegate
			{
				PawnBanishUtility.Banish(pawn, -1);
			}, true, null);
			Find.WindowStack.Add(window);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x000AE530 File Offset: 0x000AC930
		public static string GetBanishButtonTip(Pawn pawn)
		{
			string result;
			if (PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Tile))
			{
				result = "BanishTip".Translate() + "\n\n" + "BanishTipWillDie".Translate(new object[]
				{
					pawn.LabelShort
				}).CapitalizeFirst();
			}
			else
			{
				result = "BanishTip".Translate();
			}
			return result;
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x000AE598 File Offset: 0x000AC998
		private static void HealIfPossible(Pawn p)
		{
			PawnBanishUtility.tmpHediffs.Clear();
			PawnBanishUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			for (int i = 0; i < PawnBanishUtility.tmpHediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = PawnBanishUtility.tmpHediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
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

		// Token: 0x060013FF RID: 5119 RVA: 0x000AE650 File Offset: 0x000ACA50
		private static bool WillTakeInventoryIfBanished(Pawn pawn)
		{
			return !pawn.IsCaravanMember();
		}

		// Token: 0x04000C00 RID: 3072
		private const float DeathChanceForCaravanPawnBanishedToDie = 0.8f;

		// Token: 0x04000C01 RID: 3073
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
