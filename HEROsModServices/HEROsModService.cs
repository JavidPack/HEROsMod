using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HEROsMod.HEROsModServices
{
	internal class HEROsModService
	{
		private List<UIView> _UIViews = new List<UIView>();

		/// <summary>
		/// Name of the Service
		/// </summary>
		protected string _name;

		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Icon that gets used if the service is added to the ServiceHotbar
		/// </summary>
		protected UIImage _hotbarIcon;

		public UIImage HotbarIcon
		{
			get { return _hotbarIcon; }
		}

		public bool IsInHotbar { get; set; }
		public UIHotbar HotbarParent { get; set; }

		public bool IsHotbar { get; set; }
		public UIHotbar Hotbar { get; set; }

		public bool HasPermissionToUse { get; set; }
		public bool MultiplayerOnly { get; set; }

		// Network mode unknown at load time.
		public HEROsModService()
		{
			//        if(ModUtils.NetworkMode == NetworkMode.None)
			//        {
			HasPermissionToUse = true;
			ModUtils.DebugText("Permission granted: " + Name);
			//        }
			//        else
			//        {
			//            HasPermissionToUse = false;
			//ErrorLogger.Log("Permission not granted");
			//        }
		}

		/// <summary>
		/// Services logic is done here.  Called once per Update call from the Main thread
		/// </summary>
		public virtual void Update()
		{
		}

		/// <summary>
		/// This method must be called before the Service is desposed.
		/// </summary>
		public virtual void Destroy()
		{
			RemoveAllUIViews();
		}

		/// <summary>
		/// Add a UIView that belongs to this Service
		/// </summary>
		/// <param name="view">View to be added</param>
		public virtual void AddUIView(UIView view)
		{
			_UIViews.Add(view);
			MasterView.gameScreen.AddChild(view);
		}

		/// <summary>
		/// Remove a UIView from this Service
		/// </summary>
		/// <param name="view">View to be removed</param>
		public virtual void RemoveUIView(UIView view)
		{
			_UIViews.Remove(view);
			MasterView.gameScreen.RemoveChild(view);
		}

		/// <summary>
		/// Remove all UIViews from this Service
		/// </summary>
		public virtual void RemoveAllUIViews()
		{
			while (this._UIViews.Count > 0)
			{
				RemoveUIView(this._UIViews[0]);
			}
		}

		public virtual void MyGroupUpdated()
		{
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
		}

		/// <summary>
		/// Unload during mod unload.
		/// </summary>
		public virtual void Unload()
		{
		}

		/// <summary>
		/// SetupContent after all other mod content is loaded.
		/// </summary>
		public virtual void PostSetupContent()
		{
		}
	}
}