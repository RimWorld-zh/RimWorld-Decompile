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
		// Token: 0x04001C20 RID: 7200
		public const float MassColumnWidth = 60f;

		// Token: 0x04001C21 RID: 7201
		public const float SpaceAroundIcon = 4f;

		// Token: 0x04001C22 RID: 7202
		public const float SpecificTabButtonSize = 24f;

		// Token: 0x04001C23 RID: 7203
		public const float AbandonButtonSize = 24f;

		// Token: 0x04001C24 RID: 7204
		public const float AbandonSpecificCountButtonSize = 24f;

		// Token: 0x04001C25 RID: 7205
		public static readonly Texture2D AbandonButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/Abandon", true);

		// Token: 0x04001C26 RID: 7206
		public static readonly Texture2D AbandonSpecificCountButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/AbandonSpecificCount", true);

		// Token: 0x04001C27 RID: 7207
		public static readonly Texture2D SpecificTabButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/OpenSpecificTab", true);

		// Token: 0x04001C28 RID: 7208
		public static readonly Color OpenedSpecificTabButtonColor = new Color(0f, 0.8f, 0f);

		// Token: 0x04001C29 RID: 7209
		public static readonly Color OpenedSpecificTabButtonMouseoverColor = new Color(0f, 0.5f, 0f);

		// Token: 0x06003416 RID: 13334 RVA: 0x001BDE58 File Offset: 0x001BC258
		public static void DoAbandonButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 1383004931));
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x001BDEE8 File Offset: 0x001BC2E8
		public static void DoAbandonButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 8476546));
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x001BDF78 File Offset: 0x001BC378
		public static void DoAbandonSpecificCountButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x001BE008 File Offset: 0x001BC408
		public static void DoAbandonSpecificCountButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x001BE098 File Offset: 0x001BC498
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

		// Token: 0x0600341B RID: 13339 RVA: 0x001BE168 File Offset: 0x001BC568
		public static void DrawMass(TransferableImmutable transferable, Rect rect)
		{
			float num = 0f;
			for (int i = 0; i < transferable.things.Count; i++)
			{
				num += transferable.things[i].GetStatValue(StatDefOf.Mass, true) * (float)transferable.things[i].stackCount;
			}
			CaravanThingsTabUtility.DrawMass(num, rect);
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x001BE1D0 File Offset: 0x001BC5D0
		public static void DrawMass(Thing thing, Rect rect)
		{
			float mass = thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount;
			CaravanThingsTabUtility.DrawMass(mass, rect);
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x001BE1FA File Offset: 0x001BC5FA
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
