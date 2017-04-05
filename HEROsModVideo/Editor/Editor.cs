//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using GameikiMod.UIKit;
//using GameikiMod.GameikiVideo.Editor.WorldObjects;
//using System.IO;
//using GameikiMod.GameikiServices;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor
//{
//    class Editor : GameikiServices.GameikiService
//    {
//        public delegate void WorldObjectEvent(WorldObject obj);
//        public static event WorldObjectEvent WorldObjectSelected;
//        public static bool EditorModeEnabled { get; set; }
//        public static List<Stage> Stages { get; set; }
//        public static List<Stage> StageTemplates { get; set; }
//        public static UI.ActionsWindow ActionsWindow { get; set; }
//        public static UI.PropertiesWindow PropertiesWindow { get; set; }
//        public static UIScreen EditorScreen { get; set; }
//        public static EditorProperties Properties { get; set; }
//        public static UI.ObjectList ObjectList { get; set; }
//        public static int CurrentStage { get; set; }
//        public static List<Texture2D> Images { get; set; }
//        public static List<string> ImageNames { get; set; }

//        private UI.ObjectBrowser objectBrowser;
//        public static UI.StageTemplateWindow stageTemplateWindow;
//        public static UI.StagesWindow stageWindow;
//        private static Vector2 dragAnchor = Vector2.Zero;
//        private static Vector2 screenPositionAncor = Vector2.Zero;
//        private static Vector2 objectPositionAncor = Vector2.Zero;
//        private static bool draggingCamera = false;
//        private static WorldObject draggingObject = null;
//        UIButton bToggleEditorMode;
//        private static string saveDirectory;
//        public static int saveVersion = 4;
//        private static string workingFile = string.Empty;
//        private static UI.SaveSceneDialog saveDialog;
//        private static UI.LoadSceneDialog loadDialog;

//        public static Vector2? PendingScreenPosition { get; set; }

//        public static List<WorldObject> WorldObjects { get; set; }

//        private static KeyBinding kToggleEditMode;
//        private static KeyBinding kExecuteNextStage;
//        private static KeyBinding kExecutePreviousStage;
//        public Editor()
//        {
//            LoadImages();
//            CurrentStage = 0;
//            Properties = new EditorProperties();
//            EditorModeEnabled = false;
//            WorldObjects = new List<WorldObject>();
//            Stages = new List<Stage>();
//            StageTemplates = new List<Stage>();
//            EditorScreen = new UIScreen();
//            EditorScreen.Visible = false;
//            MasterView.gameScreen.AddChild(EditorScreen);
    
//            bToggleEditorMode = new UIButton("Editor");
//            bToggleEditorMode.X = UIView.LargeSpacing;
//            bToggleEditorMode.Y = Main.screenHeight / 2;
//            bToggleEditorMode.onLeftClick += bToggleEditorMode_onLeftClick;
//            //MasterView.gameScreen.AddChild(bToggleEditorMode);

//            UIButton bToggleObjectWindow = new UIButton("ObjectBrowser");
//            bToggleObjectWindow.X = bToggleEditorMode.X;
//            bToggleObjectWindow.Y = bToggleEditorMode.Y + bToggleEditorMode.Height + UIView.Spacing;
//            bToggleObjectWindow.onLeftClick += bToggleObjectWindow_onLeftClick;
//            EditorScreen.AddChild(bToggleObjectWindow);

//            UIButton bToggleObjectList = new UIButton("ObjectList");
//            bToggleObjectList.X = bToggleEditorMode.X;
//            bToggleObjectList.Y = bToggleObjectWindow.Y + bToggleObjectWindow.Height + UIView.Spacing;
//            bToggleObjectList.onLeftClick += bToggleObjectList_onLeftClick;
//            EditorScreen.AddChild(bToggleObjectList);

//            UIButton bToggleStageWindow = new UIButton("Stages");
//            bToggleStageWindow.X = bToggleEditorMode.X;
//            bToggleStageWindow.Y = bToggleObjectList.Y + bToggleObjectList.Height + UIView.Spacing;
//            bToggleStageWindow.onLeftClick += bToggleStageWindow_onLeftClick;
//            EditorScreen.AddChild(bToggleStageWindow);

//            UIButton bToggleStageTemplates = new UIButton("Stage Templates");
//            bToggleStageTemplates.X = bToggleEditorMode.X;
//            bToggleStageTemplates.Y = bToggleStageWindow.Y + bToggleStageWindow.Height + UIView.Spacing;
//            bToggleStageTemplates.onLeftClick += bToggleStageTemplates_onLeftClick;
//            EditorScreen.AddChild(bToggleStageTemplates);

//            UIButton bEditorProperties = new UIButton("Properties");
//            bEditorProperties.X = bToggleEditorMode.X;
//            bEditorProperties.Y = bToggleStageTemplates.Y + bToggleStageTemplates.Height + UIView.Spacing;
//            bEditorProperties.onLeftClick += bEditorProperties_onLeftClick;
//            EditorScreen.AddChild(bEditorProperties);

//            UIButton bSave = new UIButton("Save");
//            bSave.X = bToggleEditorMode.X;
//            bSave.Y = bEditorProperties.Y + bEditorProperties.Height + UIView.Spacing;
//            bSave.onLeftClick += bSave_onLeftClick;
//            EditorScreen.AddChild(bSave);

//            UIButton bSaveAs = new UIButton("Save As");
//            bSaveAs.X = bToggleEditorMode.X;
//            bSaveAs.Y = bSave.Y + bSave.Height + UIView.Spacing;
//            bSaveAs.onLeftClick += bSaveAs_onLeftClick;
//            EditorScreen.AddChild(bSaveAs);

//            UIButton bLoad = new UIButton("Load");
//            bLoad.X = bToggleEditorMode.X;
//            bLoad.Y = bSaveAs.Y + bSaveAs.Height + UIView.Spacing;
//            bLoad.onLeftClick += bLoad_onLeftClick;
//            EditorScreen.AddChild(bLoad);

//            objectBrowser = new UI.ObjectBrowser();
//            objectBrowser.Visible = false;
//            objectBrowser.X = UIView.LargeSpacing;
//            objectBrowser.Y = UIView.LargeSpacing;
//            EditorScreen.AddChild(objectBrowser);

//            ObjectList = new UI.ObjectList();
//            ObjectList.Visible = false;
//            ObjectList.X = UIView.LargeSpacing;
//            ObjectList.Y = objectBrowser.Y + objectBrowser.Height + UIView.LargeSpacing;
//            EditorScreen.AddChild(ObjectList);

//            stageWindow = new UI.StagesWindow();
//            stageWindow.Visible = false;
//            stageWindow.X = Main.screenWidth - stageWindow.Width - UIView.LargeSpacing;
//            stageWindow.Y = Main.screenHeight - stageWindow.Height - UIView.LargeSpacing;
//            EditorScreen.AddChild(stageWindow);

//            stageTemplateWindow = new UI.StageTemplateWindow();
//            stageTemplateWindow.Visible = false;
//            stageTemplateWindow.X = stageWindow.X - stageTemplateWindow.Width - UIView.LargeSpacing;
//            stageTemplateWindow.Y = Main.screenHeight - stageWindow.Height - UIView.LargeSpacing;
//            EditorScreen.AddChild(stageTemplateWindow);


//            PropertiesWindow = new UI.PropertiesWindow(null);
//            PropertiesWindow.Visible = false;
//            PropertiesWindow.X = Main.screenWidth - PropertiesWindow.Width - UIView.LargeSpacing;
//            PropertiesWindow.Y = UIView.LargeSpacing;
//            EditorScreen.AddChild(PropertiesWindow);

//            string currentDir = Directory.GetCurrentDirectory();
//            saveDirectory = currentDir + @"\" + "Scenes";

//            saveDialog = new UI.SaveSceneDialog();
//            EditorScreen.AddChild(saveDialog);
//            saveDialog.Visible = false;

//            loadDialog = new UI.LoadSceneDialog();
//            EditorScreen.AddChild(loadDialog);
//            loadDialog.Visible = false;
//        }

//        public static void LoadImages()
//        {
//            Images = new List<Texture2D>();
//            ImageNames = new List<string>();

//            string dir = Directory.GetCurrentDirectory() + @"\Images";
//            //string[] files = Directory.GetFiles(dir);
//            //foreach(string file in files)
//            //{
//            //    using( FileStream fileStream = new FileStream(file, FileMode.Open))
//            //    {
//            //        Texture2D image = Texture2D.FromStream(Gameiki.GraphicsDeviceReference, fileStream);
//            //        Images.Add(image);
//            //        ImageNames.Add(Path.GetFileNameWithoutExtension(file));
//            //    }
//            //}
//        }

//        void bToggleStageTemplates_onLeftClick(object sender, EventArgs e)
//        {
//            stageTemplateWindow.Visible = !stageTemplateWindow.Visible;
//        }

//        public static void SetKeyBindings()
//        {
//            //KeybindController.SetCatetory("Video Production Editor");
//            kToggleEditMode = KeybindController.AddKeyBinding("Toggle Edit Mode", "P");
//            kExecuteNextStage = KeybindController.AddKeyBinding("Execute Next Stage", "OemCloseBrackets");
//            kExecutePreviousStage = KeybindController.AddKeyBinding("Execute Previous Stage", "OemOpenBrackets");
//        }

//        public static void AddWorldObject(WorldObject obj)
//        {
//            WorldObjects.Add(obj);
//            ObjectList.Refresh();
//        }
//        public static void DeleteWorldObject(WorldObject obj)
//        {
//            WorldObjects.Remove(obj);
//            ObjectList.Refresh();
//            if(PropertiesWindow.ObjectBeingInspected == obj)
//            {
//                PropertiesWindow.SetObject(null);
//            }
//        }

//        public static void AddStage(Stage stage)
//        {
//            Stages.Add(stage);
//            stageWindow.Refresh();
//        }

//        public static void AddStageTemplate(Stage template)
//        {
//            StageTemplates.Add(template);
//            stageTemplateWindow.Refresh();
//        }

//        void bToggleObjectList_onLeftClick(object sender, EventArgs e)
//        {
//            ObjectList.Visible = !ObjectList.Visible;
//        }

//        void bEditorProperties_onLeftClick(object sender, EventArgs e)
//        {
//            PropertiesWindow.SetObject(Properties);
//        }

//        void bSaveAs_onLeftClick(object sender, EventArgs e)
//        {
//            saveDialog.Visible = true;
//        }

//        public static void Save(string fileName)
//        {
//            workingFile = fileName;
//            if (!Directory.Exists(saveDirectory))
//            {
//                Directory.CreateDirectory(saveDirectory);
//            }
//            FileStream fileStream = new FileStream(saveDirectory + @"\" + fileName + ".scene", FileMode.Create);
//            BinaryWriter writer = new BinaryWriter(fileStream);

//            writer.Write(saveVersion);
//            writer.Write(WorldObjects.Count);

//            foreach (WorldObject obj in WorldObjects)
//            {
//                obj.Save(ref writer);
//            }

//            writer.Write(StageTemplates.Count);
//            foreach(Stage template in StageTemplates)
//            {
//                template.Save(ref writer);
//            }

//            writer.Write(Stages.Count);
//            foreach (Stage stage in Stages)
//            {
//                stage.Save(ref writer);
//            }

//            writer.Close();
//            fileStream.Close();
//            writer.Dispose();
//            fileStream.Dispose();
//        }

//        public static void Load(string fileName)
//        {
//            string path = saveDirectory + @"\" + fileName;
//            if (File.Exists(path))
//            {
//                workingFile = fileName;
//                Editor.WorldObjects.Clear();
//                Editor.StageTemplates.Clear();
//                Editor.Stages.Clear();
//                FileStream fileStream = new FileStream(path, FileMode.Open);
//                BinaryReader reader = new BinaryReader(fileStream);

//                int fileVersion = reader.ReadInt32();
//                int numberOfWorldObjects = reader.ReadInt32();
//                for (int i = 0; i < numberOfWorldObjects; i++)
//                {
//                    Type type = Type.GetType(reader.ReadString());
//                    WorldObject wo = (WorldObject)Activator.CreateInstance(type);
//                    wo.Load(fileVersion, ref reader);
//                    Editor.WorldObjects.Add(wo);
//                }

//                int numOfTemplates = reader.ReadInt32();
//                for(int i= 0; i < numOfTemplates; i++)
//                {
//                    Stage template = new Stage("");
//                    template.Load(fileVersion, ref reader);
//                    StageTemplates.Add(template);
//                }

//                int numOfStages = reader.ReadInt32();
//                for (int i = 0; i < numOfStages; i++)
//                {
//                    Stage stage = new Stage("");
//                    stage.Load(fileVersion, ref reader);
//                    Stages.Add(stage);
//                }
//                stageWindow.Refresh();
//                stageTemplateWindow.Refresh();
//                ObjectList.Refresh();

//                reader.Close();
//                fileStream.Close();
//                reader.Dispose();
//                fileStream.Dispose();
//            }
//        }

//        public static string[] GetListOfSaves()
//        {
//            string[] files = Directory.GetFiles(saveDirectory);
//            for(int i = 0; i < files.Length; i++)
//            {
//                files[i] = Path.GetFileName(files[i]);
//            }
//            return files;
//        }

//        void bLoad_onLeftClick(object sender, EventArgs e)
//        {
//            loadDialog.PopulateScenes();
//            loadDialog.Visible = true;
//        }

//        void bSave_onLeftClick(object sender, EventArgs e)
//        {
//            string fileName = Path.GetFileNameWithoutExtension(workingFile);
//            if(workingFile.Length > 0)
//            {
//                Save(fileName);
//            }
//            else
//            {
//                saveDialog.Visible = true;
//            }
//        }

//        void bToggleStageWindow_onLeftClick(object sender, EventArgs e)
//        {
//            stageWindow.Visible = !stageWindow.Visible;
//        }

//        void bToggleObjectWindow_onLeftClick(object sender, EventArgs e)
//        {
//            objectBrowser.Visible = !objectBrowser.Visible;
//        }

//        void bToggleEditorMode_onLeftClick(object sender, EventArgs e)
//        {
//            EditorModeEnabled = !EditorModeEnabled;
//            EditorScreen.Visible = !EditorScreen.Visible;
//        }

//        public override void Update()
//        {
//            HandleKeyInput();
//            if(EditorModeEnabled)
//            {
//                HandleMouseInput();
//            }
//            foreach(Stage stage in Stages)
//            {
//                stage.Update();
//            }
//            for(int i = 0; i < WorldObjects.Count; i++)
//            {
//                WorldObjects[i].Update();
//            }
//        }

//        void HandleKeyInput()
//        {
//            if(kToggleEditMode.KeyPressed)
//            {
//                EditorModeEnabled = !EditorModeEnabled;
//                EditorScreen.Visible = !EditorScreen.Visible;
//                CurrentStage = 0;
//                ModUtils.FreeCamera = EditorModeEnabled;
//            }
//            if(kExecuteNextStage.KeyPressed)
//            {
//                if(!EditorModeEnabled)
//                {
//                    if(CurrentStage < Stages.Count)
//                    {
//                        Stages[CurrentStage].Execute();
//                        CurrentStage++;
//                    }
//                    else
//                    {
//                        Main.NewText("All stages complete");
//                    }
//                }
//                else
//                {
//                    Main.NewText("You can only do this when editing mode is off.");
//                }
//            }
//            if(kExecutePreviousStage.KeyPressed)
//            {
//                if(!EditorModeEnabled)
//                {
//                    if(CurrentStage > 0)
//                    {
//                        CurrentStage--;
//                        Stages[CurrentStage].Execute();
//                    }
//                    else
//                    {
//                        Main.NewText("Can't go back any further.");
//                    }
//                }
//                else
//                {
//                    Main.NewText("You can only do this when editing mode is off.");
//                }
//            }
//        }

//        void HandleMouseInput()
//        {
//            if (ModUtils.MouseState.LeftButton == ButtonState.Pressed && ModUtils.PreviousMouseState.LeftButton == ButtonState.Released)
//            {
//                if(!UIView.GameMouseOverwritten)
//                {
//                    if(objectBrowser.CreatingType != null)
//                    {
//                        WorldObject wo = (WorldObject)Activator.CreateInstance(objectBrowser.CreatingType);
//                        wo.Position = ModUtils.CursorWorldCoords;
//                        if (Properties.SnapToGrid)
//                        {
//                            wo.Position = PositionToSnapPosition(wo.Position);
//                        }
//                        AddWorldObject(wo);
//                        objectBrowser.CreatingType = null;
//                    }
//                    else
//                    {
                        
//                        for (int i = 0; i < WorldObjects.Count; i++)
//                        {
//                            WorldObject obj = WorldObjects[i];
//                            if (obj.Visible && (!obj.ScreenRelative && obj.Intersects(ModUtils.CursorWorldCoords)) || (obj.ScreenRelative && obj.Intersects(ModUtils.CursorPosition)))
//                            {
//                                if (WorldObjectSelected != null)
//                                {
//                                    WorldObjectSelected(obj);
//                                    Main.NewText("obj selected");
//                                    break;
//                                }
//                                else
//                                {
//                                    //Main.NewText("you clicked on node");
//                                    if(Main.keyState.IsKeyDown(Keys.LeftControl))
//                                    {
//                                        WorldObject clonedObject = obj.Clone();
//                                        AddWorldObject(clonedObject);
//                                        draggingObject = clonedObject;
//                                        objectPositionAncor = clonedObject.Position;
//                                        dragAnchor = ModUtils.CursorPosition;
//                                        break;
//                                    }
//                                    else
//                                    {

//                                        draggingObject = obj;
//                                        objectPositionAncor = obj.Position;
//                                        dragAnchor = ModUtils.CursorPosition;
//                                        break;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            if (draggingObject != null && ModUtils.MouseState.LeftButton == ButtonState.Pressed)
//            {
//                draggingObject.Position = objectPositionAncor + (ModUtils.CursorPosition - dragAnchor);
//                if(Properties.SnapToGrid)
//                {
//                    draggingObject.Position = PositionToSnapPosition(draggingObject.Position);
//                }
//            }
//            if (ModUtils.MouseState.LeftButton == ButtonState.Released && ModUtils.PreviousMouseState.LeftButton == ButtonState.Pressed)
//            {
//                draggingObject = null;
//            }

//            if(ModUtils.MouseState.RightButton == ButtonState.Pressed && ModUtils.PreviousMouseState.RightButton == ButtonState.Released)
//            {
//                if(!UIView.GameMouseOverwritten)
//                {
//                    foreach(WorldObject obj in WorldObjects)
//                    {
//                        if (obj.Visible && (!obj.ScreenRelative && obj.Intersects(ModUtils.CursorWorldCoords)) || (obj.ScreenRelative && obj.Intersects(ModUtils.CursorPosition)))
//                        {
//                            PropertiesWindow.SetObject(obj);
//                        }
//                    }
//                }
//            }

            
//        }

//        public static void HandleCameraDrag()
//        {
//            if (PendingScreenPosition != null)
//            {
//                Vector2 pos = (Vector2)PendingScreenPosition;
//                Main.screenPosition = pos;
//                PendingScreenPosition = null;
//            }
//            if (!EditorModeEnabled)
//                return;
//            //Middle Mouse Down
//            if (draggingObject == null && ModUtils.MouseState.MiddleButton == ButtonState.Pressed && ModUtils.PreviousMouseState.MiddleButton == ButtonState.Released)
//            {
//                dragAnchor = new Vector2(ModUtils.MouseState.X, ModUtils.MouseState.Y);
//                screenPositionAncor = Main.screenPosition;
//                draggingCamera = true;
//            }
//            //Middle Mouse Down
//            if (draggingCamera && ModUtils.MouseState.MiddleButton == ButtonState.Pressed)
//            {
//                Vector2 cursorPosition = new Vector2(ModUtils.MouseState.X, ModUtils.MouseState.Y);
//                Main.screenPosition = screenPositionAncor - (cursorPosition - dragAnchor);
//            }
//            //Middle Mouse Up
//            if (ModUtils.MouseState.MiddleButton == ButtonState.Released && ModUtils.PreviousMouseState.MiddleButton == ButtonState.Pressed)
//            {
//                draggingCamera = false;
//            }

            
//        }


//        public static void DrawLayer(SpriteBatch spriteBatch, DrawLayers drawLayer)
//        {
//            if (WorldObjects == null)
//                return;
//            for (int i = 0; i < WorldObjects.Count; i++)
//            {
//                if(WorldObjects[i].DrawLayer == drawLayer)
//                {
//                    WorldObjects[i].Draw(spriteBatch);
//                }
//            }
//            if(drawLayer == DrawLayers.FrontOfBlocks && Properties.DrawGrid)
//            {
//                DrawGrid();
//            }
//        }

//        private static void DrawGrid()
//        {
//            int num3 = (int)((float)((int)(Main.screenPosition.X / 16f) * 16) - Main.screenPosition.X);
//            int num4 = (int)((float)((int)(Main.screenPosition.Y / 16f) * 16) - Main.screenPosition.Y);
//            int num5 = Main.screenWidth / Main.gridTexture.Width;
//            int num6 = Main.screenHeight / Main.gridTexture.Height;
//            for (int i = 0; i <= num5 + 1; i++)
//            {
//                for (int j = 0; j <= num6 + 1; j++)
//                {
//                    Main.spriteBatch.Draw(Main.gridTexture, new Vector2((float)(i * Main.gridTexture.Width + num3), (float)(j * Main.gridTexture.Height + num4)), new Rectangle?(new Rectangle(0, 0, Main.gridTexture.Width, Main.gridTexture.Height)), new Color(100, 100, 100, 15), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
//                }
//            }
//        }

//        public static WorldObject GetWorldObjectByGuid(Guid guid)
//        {
//            foreach(WorldObject wo in WorldObjects)
//            {
//                if(wo.GUID == guid)
//                {
//                    return wo;
//                }
//            }
//            return null;
//        }

//        public static void MoveCameraTo(Vector2 position)
//        {
//            Main.BlackFadeIn = 255;
//            Lighting.BlackOut();
//            Main.screenLastPosition = Main.screenPosition;
//            if (Main.mapTime < 5)
//            {
//                Main.mapTime = 5;
//            }
//            Main.quickBG = 10;
//            Main.maxQ = true;
//            Main.renderNow = true;

//            Main.screenPosition = position;
//            Main.screenPosition.X -= Main.screenWidth / 2;
//            Main.screenPosition.Y -= Main.screenHeight / 2;
//        }

//        public static Vector2 PositionToSnapPosition(Vector2 position)
//        {
//            position.X = (int)position.X / Properties.SnapX * Properties.SnapX;
//            position.Y = (int)position.Y / Properties.SnapY * Properties.SnapY;
//            return position;
//        }
//    }
//}
