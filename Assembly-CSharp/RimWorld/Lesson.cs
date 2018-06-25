using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CC RID: 2252
	public abstract class Lesson : IExposable
	{
		// Token: 0x04001BA3 RID: 7075
		public float startRealTime = -999f;

		// Token: 0x04001BA4 RID: 7076
		public const float KnowledgeForAutoVanish = 0.2f;

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x001B71D4 File Offset: 0x001B55D4
		protected float AgeSeconds
		{
			get
			{
				if (this.startRealTime < 0f)
				{
					this.startRealTime = Time.realtimeSinceStartup;
				}
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x001B7210 File Offset: 0x001B5610
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003384 RID: 13188 RVA: 0x001B7228 File Offset: 0x001B5628
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x001B7240 File Offset: 0x001B5640
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x001B725A File Offset: 0x001B565A
		public virtual void ExposeData()
		{
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x001B725D File Offset: 0x001B565D
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x001B726B File Offset: 0x001B566B
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x06003389 RID: 13193
		public abstract void LessonOnGUI();

		// Token: 0x0600338A RID: 13194 RVA: 0x001B726E File Offset: 0x001B566E
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x001B7271 File Offset: 0x001B5671
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x001B7274 File Offset: 0x001B5674
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x001B7278 File Offset: 0x001B5678
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x001B7294 File Offset: 0x001B5694
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}
	}
}
