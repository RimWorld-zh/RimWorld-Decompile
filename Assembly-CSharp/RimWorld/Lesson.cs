using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CC RID: 2252
	public abstract class Lesson : IExposable
	{
		// Token: 0x04001BA9 RID: 7081
		public float startRealTime = -999f;

		// Token: 0x04001BAA RID: 7082
		public const float KnowledgeForAutoVanish = 0.2f;

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x001B74A8 File Offset: 0x001B58A8
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
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x001B74E4 File Offset: 0x001B58E4
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003384 RID: 13188 RVA: 0x001B74FC File Offset: 0x001B58FC
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x001B7514 File Offset: 0x001B5914
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x001B752E File Offset: 0x001B592E
		public virtual void ExposeData()
		{
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x001B7531 File Offset: 0x001B5931
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x001B753F File Offset: 0x001B593F
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x06003389 RID: 13193
		public abstract void LessonOnGUI();

		// Token: 0x0600338A RID: 13194 RVA: 0x001B7542 File Offset: 0x001B5942
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x001B7545 File Offset: 0x001B5945
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x001B7548 File Offset: 0x001B5948
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x001B754C File Offset: 0x001B594C
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x001B7568 File Offset: 0x001B5968
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}
	}
}
