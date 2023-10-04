﻿#nullable enable
using Com.TheFallenGames.OSA.Core;
using UnityEngine;

namespace UI.Shell.InfoPanel
{
	public abstract class AutoSizedItemsViewsHolder : BaseItemViewsHolder
	{
		protected virtual bool Horizontal => false;
		
		/// <inheritdoc />
		public override void MarkForRebuild()
		{
			base.MarkForRebuild();
			if (Horizontal)
                root.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((RectTransform) root.GetChild(0)).rect.width);
			else
				root.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((RectTransform) root.GetChild(0)).rect.height);
		}
	}
}