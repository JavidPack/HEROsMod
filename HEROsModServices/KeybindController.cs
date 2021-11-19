using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class KeybindController
	{
		//public static List<BindCategory> bindCategories = new List<BindCategory>();
		//static BindCategory currentCategory = null;

		public static List<KeyBinding> bindings = new List<KeyBinding>();

		public static KeyBinding AddKeyBinding(string bindName, Keys key)
		{
			for (int i = 0; i < bindings.Count; i++)
			{
				if (bindings[i].name == bindName)
				{
					return bindings[i];
				}
			}
			ModUtils.DebugText("Binding " + bindName);
			KeybindLoader.RegisterKeybind(HEROsMod.instance, bindName, key);

			KeyBinding bind = new KeyBinding("HEROsMod: " + bindName);
			bindings.Add(bind);
			return bind;
			//return currentCategory.AddKeyBinding(bindName, key);
		}

		internal static void HotKeyPressed(/*string name*/Dictionary<string, bool> keyStatus)
		{
			foreach (var hotkey in bindings)
			{
				// TODO: ???
				if (keyStatus[hotkey.name])
				{
					hotkey.Down = true;
					break;
				}
			}
		}

		internal static void DoPreviousKeyState()
		{
			foreach (var hotkey in bindings)
			{
				hotkey.PreviousDown = hotkey.Down;
				hotkey.Down = false;
			}
		}

		//public static void SetCatetory(string categoryName)
		//{
		//    currentCategory = null;
		//    foreach (BindCategory category in bindCategories)
		//    {
		//        if (category.name == categoryName)
		//            currentCategory = category;
		//    }
		//    if (currentCategory == null)
		//    {
		//        bindCategories.Add(new BindCategory(categoryName));
		//        currentCategory = bindCategories.Last();
		//    }
		//}

		//private static KeyBinding GetKeyBindingByName(string name)
		//{
		//    KeyBinding result = null;
		//    for (int i = 0; i < currentCategory.bindings.Count; i++)
		//    {
		//        if (currentCategory.bindings[i].name == name)
		//        {
		//            result = currentCategory.bindings[i];
		//            break;
		//        }
		//    }
		//    return result;
		//}

		//private static bool CategoryExists(string name)
		//{
		//    for (int j = 0; j < bindCategories.Count; j++)
		//    {
		//        if (bindCategories[j].name == name)
		//        {
		//            return true;
		//        }
		//    }
		//    return false;
		//}

		//public static void LoadDefaults()
		//{
		//    foreach (BindCategory category in bindCategories)
		//    {
		//        foreach (KeyBinding bind in category.bindings)
		//        {
		//            bind.RestoreDefault();
		//        }
		//    }
		//}
		/*
		public static void SaveBindings()
		{
			using (FileStream fileStream = new FileStream(Main.SavePath + Path.DirectorySeparatorChar + "KeyBindings.dat", FileMode.Create))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
				{
					binaryWriter.Write(bindCategories.Count); //Number of categorest
					for (int i = 0; i < bindCategories.Count; i++)
					{
						BindCategory category = bindCategories[i];
						binaryWriter.Write(category.name);
						binaryWriter.Write(category.bindings.Count); //Number of bindings
						for (int j = 0; j < category.bindings.Count; j++)
						{
							KeyBinding binding = category.bindings[j];
							binaryWriter.Write(binding.name);
							binaryWriter.Write(binding.key.ToString());
						}
					}

					binaryWriter.Close();
				}
			}
		}
		public static void LoadBindings() //Call After all bindings have been initialized
		{
			string fileName = Main.SavePath + Path.DirectorySeparatorChar + "KeyBindings.dat";
			if (File.Exists(fileName))
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
				{
					using (BinaryReader binaryReader = new BinaryReader(fileStream))
					{
						int numOfCategories = binaryReader.ReadInt32();
						for (int i = 0; i < numOfCategories; i++)
						{
							string nameOfCategory = binaryReader.ReadString();
							if (CategoryExists(nameOfCategory)) SetCatetory(nameOfCategory);
							else continue;

							int numOfBindings = binaryReader.ReadInt32();
							for (int j = 0; j < numOfBindings; j++)
							{
								string nameOfBinding = binaryReader.ReadString();
								string bindKey = binaryReader.ReadString();
								KeyBinding binding = GetKeyBindingByName(nameOfBinding);
								if (binding != null)
								{
									binding.key = (Keys)Enum.Parse(typeof(Keys), bindKey);
								}
							}
						}
						binaryReader.Close();
					}
				}
			}
		}
		*/
	}

	internal class KeyBinding
	{
		public string name;

		private bool _keyInputLocked
		{
			get
			{
				return Main.blockInput || Main.editChest || Main.editSign || Main.drawingPlayerChat;
			}
		}

		public bool Down;
		public bool PreviousDown;

		public bool KeyPressed
		{
			get
			{
				if (_keyInputLocked) return false;
				//if (key != Keys.None)
				{
					//if (Main.keyState.IsKeyDown(key) && ModUtils.PreviousKeyboardState.IsKeyUp(key))
					if (Down && !PreviousDown)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool KeyUp
		{
			get
			{
				if (_keyInputLocked) return true;
				//if (key != Keys.None)
				{
					if (!Down)
					//if (Main.keyState.IsKeyUp(key))
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool KeyDown
		{
			get
			{
				if (_keyInputLocked) return false;
				//if (key != Keys.None)
				{
					//if (Main.keyState.IsKeyDown(key))
					if (Down)
					{
						return true;
					}
				}
				return false;
			}
		}

		public KeyBinding(string name)
		{
			this.name = name;
		}
	}

	//class BindCategory
	//{
	//    public string name;
	//    public List<KeyBinding> bindings = new List<KeyBinding>();
	//    public BindCategory(string name)
	//    {
	//        this.name = name;
	//    }

	//    public KeyBinding AddKeyBinding(string bindName, Keys key = Keys.None)
	//    {
	//        for(int i = 0;i < bindings.Count; i++)
	//        {
	//            if(bindings[i].name == bindName)
	//            {
	//                return bindings[i];
	//            }
	//        }
	//        KeyBinding bind = new KeyBinding(bindName, key);
	//        bindings.Add(bind);
	//        return bind;
	//    }
	//}
}