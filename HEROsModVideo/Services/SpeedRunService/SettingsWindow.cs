//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.UIKit.UIComponents;
//using Microsoft.Xna.Framework;
//using System.IO;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    class SettingsWindow : UIWindow
//    {

//        public event TimerEvent TimerAdded;

//        UIButton bAddTimer;
//        UIButton bClearTimers;
//        Slot itemSlot;
//        UITextbox timerName;
//        UITextbox numberOfSteps;
//        UIDropdown timerType;
//        UITextbox npcID;
//        UIButton bSave;
//        UIButton bLoad;
//        UIImage bClose;
//        UIView extraFields;
//        UITextbox health;
        
//        public SettingsWindow()
//        {
//            this.CanMove = true;

//            bAddTimer = new UIButton("Add Timer");
//            bClearTimers = new UIButton("Clear Timer");
//            itemSlot = new Slot(0);
//            itemSlot.functionalSlot = true;
//            timerName = new UITextbox();

//            itemSlot.X = LargeSpacing;
//            itemSlot.Y = LargeSpacing;

//            timerName.Anchor = AnchorPosition.Left;
//            timerName.X = itemSlot.X + itemSlot.Width + Spacing;
//            timerName.Y = itemSlot.Y + itemSlot.Height / 2;

            
            

            

//            timerType = new UIDropdown();
//            timerType.AddItem("Custom");
//            timerType.AddItem("Item");
//            timerType.AddItem("NPC");
//            timerType.AddItem("Health");
//            timerType.AddItem("Craft Item");
//            timerType.X = LargeSpacing;
//            timerType.Y = itemSlot.Y + itemSlot.Height + Spacing;
//            timerType.Width = 150;

//            extraFields = new UIView();
//            extraFields.X = 0;
//            extraFields.Y = timerType.Y + timerType.Height + Spacing;

//            bAddTimer.X = LargeSpacing;
//            bAddTimer.Y = extraFields.Y + extraFields.Height + LargeSpacing;
//            bClearTimers.X = bAddTimer.X + bAddTimer.Width + Spacing;
//            bClearTimers.Y = bAddTimer.Y;

//            bSave = new UIButton("Save");
//            bSave.X = LargeSpacing;
//            bSave.Y = bAddTimer.Y + bAddTimer.Height + Spacing;

//            bLoad = new UIButton("Load");
//            bLoad.X = bSave.X + bSave.Width + Spacing;
//            bLoad.Y = bSave.Y;

//            this.Width = bClearTimers.X + bClearTimers.Width + LargeSpacing;
//            this.Height = bSave.Y + bSave.Height + LargeSpacing;

//            extraFields.Width = this.Width;
//            bClose = new UIImage(closeTexture);
//            bClose.X = this.Width - bClose.Width - LargeSpacing;
//            bClose.Y = LargeSpacing;

//            bAddTimer.onLeftClick += bAddTimer_onLeftClick;
//            bClearTimers.onLeftClick += bClearTimers_onLeftClick;
//            bSave.onLeftClick += bSave_onLeftClick;
//            bLoad.onLeftClick += bLoad_onLeftClick;
//            bClose.onLeftClick += bClose_onLeftClick;
//            timerType.selectedChanged += timerType_selectedChanged;

//            AddChild(itemSlot);
//            AddChild(timerName);
//            AddChild(extraFields);
//            AddChild(bAddTimer);
//            AddChild(bClearTimers);
//            AddChild(bSave);
//            AddChild(bLoad);
//            AddChild(bClose);
//            AddChild(timerType);

//            PopulateExtraFields();
//        }

//        void timerType_selectedChanged(object sender, EventArgs e)
//        {
//            PopulateExtraFields();
//        }

//        void PopulateExtraFields()
//        {
//            extraFields.RemoveAllChildren();

//            float yPos = 0f;
//            if (timerType.SelectedItem == 0 || timerType.SelectedItem == 1 || timerType.SelectedItem == 2)
//            {
//                UILabel lSteps = new UILabel("Steps");
//                lSteps.Scale = .5f;
//                lSteps.X = itemSlot.X;
//                lSteps.Y = yPos;
//                yPos += lSteps.Height + Spacing;

//                numberOfSteps = new UITextbox();
//                numberOfSteps.Numeric = true;
//                numberOfSteps.Width = 100;
//                numberOfSteps.Anchor = AnchorPosition.Left;
//                numberOfSteps.Text = "1";
//                numberOfSteps.X = lSteps.X + lSteps.Width + Spacing;
//                numberOfSteps.Y = lSteps.Y + lSteps.Height / 2;
//                extraFields.AddChild(lSteps);
//                extraFields.AddChild(numberOfSteps);
//            }
//            if(timerType.SelectedItem == 2)
//            {
//                UILabel lnpcID = new UILabel("NPC ID");
//                lnpcID.Scale = .5f;
//                lnpcID.X = itemSlot.X;
//                lnpcID.Y = yPos;
//                yPos += lnpcID.Height + Spacing;

//                npcID = new UITextbox();
//                npcID.Numeric = true;
//                npcID.Width = 100;
//                npcID.Anchor = AnchorPosition.Left;
//                npcID.Text = "1";
//                npcID.X = lnpcID.X + lnpcID.Width + Spacing;
//                npcID.Y = lnpcID.Y + lnpcID.Height / 2;
//                extraFields.AddChild(lnpcID);
//                extraFields.AddChild(npcID);
//            }
//            if (timerType.SelectedItem == 3)
//            {
//                UILabel lHealth = new UILabel("Health");
//                lHealth.Scale = .5f;
//                lHealth.X = itemSlot.X;
//                lHealth.Y = yPos;
//                yPos += lHealth.Height + Spacing;

//                health = new UITextbox();
//                health.Numeric = true;
//                health.Width = 100;
//                health.Anchor = AnchorPosition.Left;
//                health.Text = "500";
//                health.X = lHealth.X + lHealth.Width + Spacing;
//                health.Y = lHealth.Y + lHealth.Height / 2;
//                extraFields.AddChild(lHealth);
//                extraFields.AddChild(health);
//            }
//            extraFields.Height = yPos;
//            bAddTimer.Y = extraFields.Y + extraFields.Height + LargeSpacing;
//            bClearTimers.Y = bAddTimer.Y;
//            bSave.Y = bAddTimer.Y + bAddTimer.Height + Spacing;
//            bLoad.Y = bSave.Y;
//            this.Height = bSave.Y + bSave.Height + LargeSpacing;
//        }

//        void bClose_onLeftClick(object sender, EventArgs e)
//        {
//            this.Visible = false;
//        }

//        void bLoad_onLeftClick(object sender, EventArgs e)
//        {
//            string[] files = SpeedRunTimer.GetSavedSettingsFiles();
//            if (files.Length == 0)
//            {
//                Main.NewText("No Settings Save files found.");
//                return;
//            }
//            LoadWindow lw = new LoadWindow();
//            lw.SettingsFileSelected += lw_SettingsFileSelected;
//            lw.FileSelectedForDeletion += lw_FileSelectedForDeletion;
//            Parent.AddChild(lw);
//        }

//        void lw_FileSelectedForDeletion(string fileName)
//        {
//            if (File.Exists(Main.SavePath + @"\SpeedRunSaves\" + fileName))
//            {
//                File.Delete(Main.SavePath + @"\SpeedRunSaves\" + fileName);
//            }
//            else Main.NewText("Could not delete the file");
//        }

//        void lw_SettingsFileSelected(string fileName)
//        {
//            Load(fileName);
//        }

//        void bSave_onLeftClick(object sender, EventArgs e)
//        {
//            SaveWindow sw = new SaveWindow();
//            sw.NameSubmited += sw_NameSubmited;
//            Parent.AddChild(sw);
//        }

//        void sw_NameSubmited(string name)
//        {
//            Save(name);
//        }

//        void bClearTimers_onLeftClick(object sender, EventArgs e)
//        {
//            SpeedRunTimer.ClearTimers();
//        }

//        void bAddTimer_onLeftClick(object sender, EventArgs e)
//        {
//            int steps = int.Parse(numberOfSteps.Text);
//            if(itemSlot.item.type != 0 && timerName.Text.Length > 0)
//            {
//                if(TimerAdded != null)
//                {
//                    if (timerType.SelectedItem == 0 && steps > 0)
//                    {
//                        TimerAdded(new EventTimer(timerName.Text, itemSlot.item.type, steps));
//                    }
//                    else if (timerType.SelectedItem == 1 && steps > 0)
//                    {
//                        TimerAdded(new CollectItemsTimer(timerName.Text, itemSlot.item.type, steps));
//                    }
//                    else if (timerType.SelectedItem == 2 && steps > 0)
//                    {
//                        int npcNetID = int.Parse(npcID.Text);
//                        if(npcNetID != 0)
//                        {
//                            TimerAdded(new KillBossTimer(timerName.Text, itemSlot.item.type, steps, npcNetID));
//                        }
//                    }
//                    else if (timerType.SelectedItem == 3)
//                    {
//                        int targetHealth = int.Parse(health.Text);
//                        if(targetHealth > Main.player[Main.myPlayer].statLifeMax)
//                        {
//                            TimerAdded(new TargetHealthTimer(timerName.Text, itemSlot.item.type, targetHealth));

//                        }
//                    }
//                    else if(timerType.SelectedItem == 4)
//                    {
//                        CollectItemsTimer timer = new CollectItemsTimer(timerName.Text, itemSlot.item.type, steps);
//                        Item[] materials = RecipeDatabaseBuilder.Builder.GetMRequiredMaterialsToCraftItem(itemSlot.item);
//                        for (int i = 0; i < materials.Length; i++)
//                        {
//                            timer.AddSubTimer(new CollectItemsTimer(materials[i].name, materials[i].type, materials[i].stack, true));
//                        }
//                        TimerAdded(timer);
//                    }
//                }
//            }
//        }

//        int saveVersion = 0;
//        public void Save(string name)
//        {
//            string path = Main.SavePath + @"\SpeedRunSaves\" + name;
//            string dir = Path.GetDirectoryName(path);
//            if(!Directory.Exists(dir))
//            {
//                Directory.CreateDirectory(dir);
//            }
//            FileStream fileStream = new FileStream(path, FileMode.Create);
//            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
     
//            binaryWriter.Write(saveVersion);
//            binaryWriter.Write(SpeedRunTimer.EventTimers.Count);
//            for (int i = 0; i < SpeedRunTimer.EventTimers.Count; i++ )
//            {
//                SpeedRunTimer.EventTimers[i].Save(ref binaryWriter);
//            }
//            binaryWriter.Close();           
//        }

//        public void Load(string fileName)
//        {
//            string path = Main.SavePath + @"\SpeedRunSaves\" + fileName;
//            if (File.Exists(path))
//            {
//                SpeedRunTimer.ClearTimers();
//                FileStream fileStream = new FileStream(path, FileMode.Open);
//                BinaryReader binaryReader = new BinaryReader(fileStream);

//                int sVersion = binaryReader.ReadInt32();
//                if(sVersion == saveVersion)
//                {
//                    int numberOfTimers = binaryReader.ReadInt32();
//                    for (int i = 0; i < numberOfTimers; i++)
//                    {
//                        int timerType = binaryReader.ReadInt32();
//                        EventTimer timer = null;
//                        switch(timerType)
//                        {
//                            case 0:
//                                timer = new EventTimer("", 0, 0);
//                                timer.Load(ref binaryReader);
//                                break;
//                            case 1:
//                                timer = new CollectItemsTimer("", 0, 0);
//                                timer.Load(ref binaryReader);
//                                break;
//                            case 2:
//                                timer = new KillBossTimer("", 0, 0, 0);
//                                timer.Load(ref binaryReader);
//                                break;
//                            case 3:
//                                timer = new TargetHealthTimer("", 0, 0);
//                                timer.Load(ref binaryReader);
//                                break;
                                
//                        }
//                        if(TimerAdded != null)
//                        {
//                            TimerAdded(timer);
//                        }
//                    }
//                }
//                binaryReader.Close();
//            }
//        }
//    }
//}
