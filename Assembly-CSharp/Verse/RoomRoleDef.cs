using System;
using System.Collections.Generic;

namespace Verse
{
	public class RoomRoleDef : Def
	{
		public Type workerClass;

		private List<RoomStatDef> relatedStats = null;

		[Unsaved]
		private RoomRoleWorker workerInt = null;

		public RoomRoleDef()
		{
		}

		public RoomRoleWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		public bool IsStatRelated(RoomStatDef def)
		{
			bool result;
			if (this.relatedStats == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.relatedStats.Count; i++)
				{
					if (this.relatedStats[i] == def)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
