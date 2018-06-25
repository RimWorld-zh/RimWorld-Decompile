using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020008E0 RID: 2272
	[StaticConstructorOnStartup]
	public static class CaravanThingsTabUtility
	{
		// Token: 0x04001C1A RID: 7194
		public const float MassColumnWidth = 60f;

		// Token: 0x04001C1B RID: 7195
		public const float SpaceAroundIcon = 4f;

		// Token: 0x04001C1C RID: 7196
		public const float SpecificTabButtonSize = 24f;

		// Token: 0x04001C1D RID: 7197
		public const float AbandonButtonSize = 24f;

		// Token: 0x04001C1E RID: 7198
		public const float AbandonSpecificCountButtonSize = 24f;

		// Token: 0x04001C1F RID: 7199
		public static readonly Texture2D AbandonButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/Abandon", true);

		// Token: 0x04001C20 RID: 7200
		public static readonly Texture2D AbandonSpecificCountButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/AbandonSpecificCount", true);

		// Token: 0x04001C21 RID: 7201
		public static readonly Texture2D SpecificTabButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/OpenSpecificTab", true);

		// Token: 0x04001C22 RID: 7202
		public static readonly Color OpenedSpecificTabButtonColor = new Color(0f, 0.8f, 0f);

		// Token: 0x04001C23 RID: 7203
		public static readonly Color OpenedSpecificTabButtonMouseoverColor = new Color(0f, 0.5f, 0f);

		// Token: 0x06003416 RID: 13334 RVA: 0x001BDB84 File Offset: 0x001BBF84
		public static void DoAbandonButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 1383004931));
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x001BDC14 File Offset: 0x001BC014
		public static void DoAbandonButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 8476546));
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x001BDCA4 File Offset: 0x001BC0A4
		public static void DoAbandonSpecificCountButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x001BDD34 File Offset: 0x001BC134
		public static void DoAbandonSpecificCountButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x001BDDC4 File Offset: 0x001BC1C4
		public static void DoOpenSpecificTabButton(Rect rowRect, Pawn p, ref Pawn specificTabForPawn)
		{
			Color baseColor = (p != specificTabForPawn) ? Color.white : CaravanThingsTabUtility.OpenedSpecificTabButtonColor;
			Color mouseoverColor = (p != specificTabForPawn) ? GenUI.MouseoverColor : CaravanThingsTabUtility.OpenedSpecificTabButtonMouseoverColor;
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.SpecificTabButtonTex, baseColor, mouseoverColor))
			{
				if (p == specificTabForPawn)
				{
					specificTabForPawn = null;
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					specificTabForPawn = p;
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegion(rect, "OpenSpecificTabButtonTip".Translate());
			GUI.color = Color.white;
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x001BDE94 File Offset: 0x001BC294
		public static void DrawMass(TransferableImmutable transferable, Rect rect)
		{
			float num = 0f;
			for (int i = 0; i < transferable.things.Count; i++)
			{
				num += transferable.things[i].GetStatValue(StatDefOf.Mass, true) * (float)transferable.things[i].stackCount;
			}
			CaravanThingsTabUtility.DrawMass(num, rect);
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x001BDEFC File Offset: 0x001BC2FC
		public static void DrawMass(Thing thing, Rect rect)
		{
			float mass = thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount;
			CaravanThingsTabUtility.DrawMass(mass, rect);
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x001BDF26 File Offset: 0x001BC326
		private static void DrawMass(float mass, Rect rect)
		{
			GUI.color = TransferableOneWayWidget.ItemMassColor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect, mass.ToStringMass());
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}
	}
}
