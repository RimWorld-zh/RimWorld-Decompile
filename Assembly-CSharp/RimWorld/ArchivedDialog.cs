using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F4 RID: 756
	public class ArchivedDialog : IArchivable, IExposable
	{
		// Token: 0x06000C90 RID: 3216 RVA: 0x0006F4F6 File Offset: 0x0006D8F6
		public ArchivedDialog()
		{
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0006F4FF File Offset: 0x0006D8FF
		public ArchivedDialog(string text, string title = null, Faction relatedFaction = null)
		{
			this.text = text;
			this.title = title;
			this.relatedFaction = relatedFaction;
			this.createdTick = GenTicks.TicksGame;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0006F528 File Offset: 0x0006D928
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0006F540 File Offset: 0x0006D940
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0006F55C File Offset: 0x0006D95C
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0006F57C File Offset: 0x0006D97C
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0006F598 File Offset: 0x0006D998
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0006F5B4 File Offset: 0x0006D9B4
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0006F5CC File Offset: 0x0006D9CC
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0006F5E4 File Offset: 0x0006D9E4
		void IArchivable.OpenArchived()
		{
			DiaNode diaNode = new DiaNode(this.text);
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			WindowStack windowStack = Find.WindowStack;
			DiaNode nodeRoot = diaNode;
			Faction faction = this.relatedFaction;
			string text = this.title;
			windowStack.Add(new Dialog_NodeTreeWithFactionInfo(nodeRoot, faction, false, false, text));
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0006F648 File Offset: 0x0006DA48
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}

		// Token: 0x04000838 RID: 2104
		public string text;

		// Token: 0x04000839 RID: 2105
		public string title;

		// Token: 0x0400083A RID: 2106
		public Faction relatedFaction;

		// Token: 0x0400083B RID: 2107
		public int createdTick;
	}
}
