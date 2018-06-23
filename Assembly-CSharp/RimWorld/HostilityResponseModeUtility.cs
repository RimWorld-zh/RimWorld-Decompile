using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200051C RID: 1308
	[StaticConstructorOnStartup]
	public static class HostilityResponseModeUtility
	{
		// Token: 0x04000E0D RID: 3597
		private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore", true);

		// Token: 0x04000E0E RID: 3598
		private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack", true);

		// Token: 0x04000E0F RID: 3599
		private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee", true);

		// Token: 0x04000E10 RID: 3600
		[CompilerGenerated]
		private static Func<Pawn, HostilityResponseMode> <>f__mg$cache0;

		// Token: 0x04000E11 RID: 3601
		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>> <>f__mg$cache1;

		// Token: 0x060017D9 RID: 6105 RVA: 0x000D08BC File Offset: 0x000CECBC
		public static Texture2D GetIcon(this HostilityResponseMode response)
		{
			Texture2D result;
			switch (response)
			{
			case HostilityResponseMode.Ignore:
				result = HostilityResponseModeUtility.IgnoreIcon;
				break;
			case HostilityResponseMode.Attack:
				result = HostilityResponseModeUtility.AttackIcon;
				break;
			case HostilityResponseMode.Flee:
				result = HostilityResponseModeUtility.FleeIcon;
				break;
			default:
				result = BaseContent.BadTex;
				break;
			}
			return result;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x000D0910 File Offset: 0x000CED10
		public static HostilityResponseMode GetNextResponse(Pawn pawn)
		{
			HostilityResponseMode result;
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					result = HostilityResponseMode.Flee;
				}
				else
				{
					result = HostilityResponseMode.Attack;
				}
				break;
			case HostilityResponseMode.Attack:
				result = HostilityResponseMode.Flee;
				break;
			case HostilityResponseMode.Flee:
				result = HostilityResponseMode.Ignore;
				break;
			default:
				result = HostilityResponseMode.Ignore;
				break;
			}
			return result;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x000D0984 File Offset: 0x000CED84
		public static string GetLabel(this HostilityResponseMode response)
		{
			return ("HostilityResponseMode_" + response).Translate();
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x000D09B0 File Offset: 0x000CEDB0
		public static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
		{
			if (HostilityResponseModeUtility.<>f__mg$cache0 == null)
			{
				HostilityResponseModeUtility.<>f__mg$cache0 = new Func<Pawn, HostilityResponseMode>(HostilityResponseModeUtility.DrawResponseButton_GetResponse);
			}
			Func<Pawn, HostilityResponseMode> getPayload = HostilityResponseModeUtility.<>f__mg$cache0;
			if (HostilityResponseModeUtility.<>f__mg$cache1 == null)
			{
				HostilityResponseModeUtility.<>f__mg$cache1 = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>>(HostilityResponseModeUtility.DrawResponseButton_GenerateMenu);
			}
			Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>> menuGenerator = HostilityResponseModeUtility.<>f__mg$cache1;
			Texture2D icon = pawn.playerSettings.hostilityResponse.GetIcon();
			Widgets.Dropdown<Pawn, HostilityResponseMode>(rect, pawn, getPayload, menuGenerator, null, icon, null, null, delegate
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
			}, paintable);
			UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
			TooltipHandler.TipRegion(rect, string.Concat(new string[]
			{
				"HostilityReponseTip".Translate(),
				"\n\n",
				"HostilityResponseCurrentMode".Translate(),
				": ",
				pawn.playerSettings.hostilityResponse.GetLabel()
			}));
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000D0A9C File Offset: 0x000CEE9C
		private static HostilityResponseMode DrawResponseButton_GetResponse(Pawn pawn)
		{
			return pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x000D0ABC File Offset: 0x000CEEBC
		private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(Pawn p)
		{
			IEnumerator enumerator = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HostilityResponseMode response = (HostilityResponseMode)enumerator.Current;
					yield return new Widgets.DropdownMenuElement<HostilityResponseMode>
					{
						option = new FloatMenuOption(response.GetLabel(), delegate()
						{
							p.playerSettings.hostilityResponse = response;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = response
					};
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			yield break;
		}
	}
}
