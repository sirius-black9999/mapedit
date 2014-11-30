using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Gibbed.Rebirth.FileFormats;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
        private Button LoadButton;
        private ComboBox EntityBox;
        private Panel ControlsPanel;
        private ComboBox roomComboBox;
        private Label LabelSubtype;
        private Label LabelVariant;
        private Label LabelType;
        private TextBox TextboxType;
        private TextBox TextboxVariant;
        private TextBox TextboxSubType;
        private ListBox tileEntityListBox;
        private Button WeightButton;
        private TextBox weightTextBox;
        private Button RemoveButton;
        private Button addButton;
        private Button saveButton;
        private Panel TilePanel;
        private TabControl tabs;
        private Panel mainPanel;

        private bool leftSide;
        private bool topSide;
        private int selectedX;
        private int selectedY;
        private Button[ ] editorButtons;
        private IComponent components;
        private StageBinaryFile.Room CurrentRoom;
        private StageBinaryFile loadedFile;

        public Form1()
        {
            this.InitializeComponent();
            this.Resize += ( sender, args ) => this.doLayout (this.ClientRectangle);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CloseInput = true;
            EntityStore.clearEntities();
            var fullpath = Path.Combine ( Path.GetDirectoryName(Application.ExecutablePath), "entities2.xml" );
            Debug.WriteLine(fullpath);
            if ( File.Exists ( fullpath))
            {
                EntityStore.LoadEntities ( XmlReader.Create( File.Open (fullpath, FileMode.Open ), settings));
                this.EntityBox.DataSource = EntityStore.getNames ();
            }
            fullpath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "additions.xml");
            if ( File.Exists ( fullpath))
            {
                EntityStore.LoadEntities(XmlReader.Create(File.Open(fullpath, FileMode.Open), settings));
                this.EntityBox.DataSource = EntityStore.getNames ();
            }
        }

        private void InitializeComponent ()
        {
            this.tabs = new TabControl();
            TabPage page = new TabPage("edit rooms");
            this.mainPanel = new Panel();
            this.LoadButton = new Button();
            this.EntityBox = new ComboBox();
            this.ControlsPanel = new Panel();
            this.WeightButton = new Button();
            this.weightTextBox = new TextBox();
            this.TextboxType = new TextBox();
            this.TextboxVariant = new TextBox();
            this.TextboxSubType = new TextBox();
            this.RemoveButton = new Button();
            this.addButton = new Button();
            this.tileEntityListBox = new ListBox();
            this.LabelSubtype = new Label();
            this.LabelVariant = new Label();
            this.LabelType = new Label();
            this.roomComboBox = new ComboBox();
            this.saveButton = new Button();
            this.TilePanel = new Panel();


            this.mainPanel.Text = "edit rooms";
            this.mainPanel.Controls.Add(this.ControlsPanel);
            this.mainPanel.Controls.Add(TilePanel);
            page.Controls.Add(mainPanel);
            this.tabs.Controls.Add(page);
            // 
            // button1
            // 
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.TabIndex = 0;
            this.LoadButton.Text = "Load File";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += this.LoadFileClick;
            // 
            // comboBox1
            // 
            this.EntityBox.FormattingEnabled = true;
            this.EntityBox.Name = "EntityBox";
            this.EntityBox.TabIndex = 1;
            this.EntityBox.SelectedIndexChanged += ( sender, args ) =>
                                                   {

                                                       StageBinaryFile.Entity temp =
                                                           EntityStore.findByName (
                                                                                   this.EntityBox.SelectedItem as string );
                                                       TextboxType.Text = "" + temp.Type;
                                                       TextboxVariant.Text = "" + temp.Variant;
                                                       TextboxSubType.Text = "" + temp.Subtype;
                                                   };
            // 
            // panel1
            // 
            this.ControlsPanel.Controls.Add(this.WeightButton);
            this.ControlsPanel.Controls.Add(this.weightTextBox);
            this.ControlsPanel.Controls.Add(this.TextboxType);
            this.ControlsPanel.Controls.Add(this.TextboxVariant);
            this.ControlsPanel.Controls.Add(this.TextboxSubType);
            this.ControlsPanel.Controls.Add(this.RemoveButton);
            this.ControlsPanel.Controls.Add(this.addButton);
            this.ControlsPanel.Controls.Add(this.tileEntityListBox);
            this.ControlsPanel.Controls.Add(this.LabelSubtype);
            this.ControlsPanel.Controls.Add(this.LabelVariant);
            this.ControlsPanel.Controls.Add(this.LabelType);
            this.ControlsPanel.Controls.Add(this.roomComboBox);
            this.ControlsPanel.Controls.Add(this.EntityBox);
            this.ControlsPanel.Controls.Add(this.saveButton);
            this.ControlsPanel.Controls.Add(this.LoadButton);
            this.ControlsPanel.Name = "ControlsPanel";
            this.ControlsPanel.TabIndex = 2;

            //
            // Tile panel
            //
            this.TilePanel.Name = "TilePanel";
            this.TilePanel.AutoScroll = true;
            
            // 
            // button4
            // 
            this.WeightButton.Name = "WeightButton";
            this.WeightButton.TabIndex = 8;
            this.WeightButton.Text = "set weight";
            this.WeightButton.UseVisualStyleBackColor = true;
            this.WeightButton.Click += this.SetWeight;
            // 
            // textBox1
            // 
            this.weightTextBox.Name = "WeightTextbox";
            this.weightTextBox.TabIndex = 7;
            this.TextboxType.Name = "TextboxType";
            this.TextboxType.TabIndex = 7;
            this.TextboxVariant.Name = "TextboxVariant";
            this.TextboxVariant.TabIndex = 7;
            this.TextboxSubType.Name = "TextboxSubtype";
            this.TextboxSubType.TabIndex = 7;
            // 
            // button3
            // 
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.TabIndex = 6;
            this.RemoveButton.Text = "REMOVE";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += this.RemoveClick;
            // 
            // button2
            // 
            this.addButton.Name = "addButton";
            this.addButton.TabIndex = 5;
            this.addButton.Text = "ADD";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += this.AddClick;
            // 
            // listBox1
            // 
            this.tileEntityListBox.FormattingEnabled = true;
            this.tileEntityListBox.Name = "TileEntitiesList";
            this.tileEntityListBox.TabIndex = 4;
            this.tileEntityListBox.SelectedIndexChanged += this.SelectedEntityChanged;
            // 
            // label2
            // 
            this.LabelVariant.AutoSize = true;
            this.LabelVariant.Name = "LabelVariant";
            this.LabelVariant.TabIndex = 3;
            this.LabelVariant.Text = "Variant";

            this.LabelSubtype.AutoSize = true;
            this.LabelSubtype.Name = "LabelSubtype";
            this.LabelSubtype.TabIndex = 3;
            this.LabelSubtype.Text = "SubType";
            // 
            // label1
            // 
            this.LabelType.AutoSize = true;
            this.LabelType.Name = "LabelType";
            this.LabelType.TabIndex = 2;
            this.LabelType.Text = "Type";
            // 
            // comboBox2
            // 
            this.roomComboBox.FormattingEnabled = true;
            this.roomComboBox.Name = "roomComboBox";
            this.roomComboBox.TabIndex = 1;
            this.roomComboBox.SelectedIndexChanged += this.RoomChanged;
            // 
            // button5
            // 
            this.saveButton.Name = "saveButton";
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save file";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += this.saveFile;
            // 
            // Form1
            // 
            //this.AutoScaleDimensions = new SizeF(6F, 13F);
            //this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(898, 482);
            this.Controls.Add(tabs);
            this.Name = "LevelEditor";
            this.Text = "Binding of isaac: Rebirth level editor - no level loaded";

            doLayout(this.ClientRectangle);
        }

        private void doLayout ( Rectangle newRectangle )
        {

            this.ControlsPanel.SuspendLayout();
            this.SuspendLayout();

            this.tabs.Location = new Point(0, 0);
            this.tabs.Size = new Size(newRectangle.Width, newRectangle.Height);

            this.mainPanel.Location = new Point(0, 0);
            this.mainPanel.Size = new Size(tabs.Size.Width, tabs.Size.Height);

            this.ControlsPanel.Location = new Point((int) ( mainPanel.Size.Width*0.8f ), 0);
            this.ControlsPanel.Size = new Size((int)(mainPanel.Size.Width * 0.2f), mainPanel.Size.Height);
            this.LoadButton.Location = new Point(0,0);
            this.LoadButton.Size = new Size(ControlsPanel.Size.Width/2, 25);
            this.saveButton.Location = new Point(ControlsPanel.Size.Width / 2, 0);
            this.saveButton.Size = new Size(ControlsPanel.Size.Width / 2, 25);
            this.roomComboBox.Location = new Point(0, 30);
            this.roomComboBox.Size = new Size(ControlsPanel.Size.Width, 15);
            this.EntityBox.Location = new Point(0, 50);
            this.EntityBox.Size = new Size(ControlsPanel.Size.Width, 15);
            this.LabelType.Location = new Point(0, 75);
            this.LabelType.Size = new Size(ControlsPanel.Size.Width / 3, 15);
            this.LabelVariant.Location = new Point(ControlsPanel.Size.Width / 3, 75);
            this.LabelVariant.Size = new Size(ControlsPanel.Size.Width / 3, 15);
            this.LabelSubtype.Location = new Point(ControlsPanel.Size.Width / 3*2, 75);
            this.LabelSubtype.Size = new Size(ControlsPanel.Size.Width / 3, 15);


            this.TextboxType.Size = new Size(ControlsPanel.Size.Width / 3, 20);
            this.TextboxType.Location = new Point(0, 90);
            this.TextboxVariant.Size = new Size(ControlsPanel.Size.Width / 3, 20);
            this.TextboxVariant.Location = new Point(ControlsPanel.Size.Width / 3, 90);
            this.TextboxSubType.Size = new Size(ControlsPanel.Size.Width / 3, 20);
            this.TextboxSubType.Location = new Point(ControlsPanel.Size.Width / 3 * 2, 90);


            this.addButton.Location = new Point(0, 120);
            this.addButton.Size = new Size(ControlsPanel.Size.Width / 2, 30);
            this.RemoveButton.Location = new Point(ControlsPanel.Size.Width / 2, 120);
            this.RemoveButton.Size = new Size(ControlsPanel.Size.Width / 2, 30);
            this.tileEntityListBox.Location = new Point(0, 150);
            this.tileEntityListBox.Size = new Size(ControlsPanel.Size.Width,ControlsPanel.Height-190);
            this.WeightButton.Location = new Point(0, ControlsPanel.Height-20);
            this.WeightButton.Size = new Size(ControlsPanel.Width, 20);
            this.weightTextBox.Location = new Point(0, ControlsPanel.Height - 40);
            this.weightTextBox.Size = new Size(ControlsPanel.Width, 20);
            this.TilePanel.Location = new Point(10,10);
            this.TilePanel.Size = new Size((int)(mainPanel.Size.Width * 0.8f - 20), mainPanel.Size.Height - 20);
            //this.lrToggleButton.Location = new Point(12, 411);
            //this.lrToggleButton.Size = new Size(351, 59);
            //this.udToggleButton.Location = new Point(369, 411);
            //this.udToggleButton.Size = new Size(357, 59);
            if(loadedFile != null)
                if ( !CurrentRoom.Equals ( loadedFile.Rooms [ this.roomComboBox.SelectedIndex ] ))
                {
                    CurrentRoom = loadedFile.Rooms[this.roomComboBox.SelectedIndex];
                    this.TilePanel.Controls.Clear ();
                    this.editorButtons = new Button[ CurrentRoom.Width * CurrentRoom.Height ];
                    for ( int i = 0; i < CurrentRoom.Width; i++ )
                        for ( int j = 0; j < CurrentRoom.Height; j++ )
                        {
                            int index = i + j * CurrentRoom.Width;
                            this.editorButtons [ index ] = new Button ();
                            this.TilePanel.Controls.Add ( this.editorButtons [ index ] );
                            this.editorButtons [ index ].Click += this.OnClick;
                            this.editorButtons [ index ].Name = i + "," + j;
                            this.editorButtons [ index ].SetBounds (
                                                                    i * TilePanel.Width / 13,
                                                                    j * TilePanel.Width / 13,
                                                                    TilePanel.Width / 13,
                                                                    TilePanel.Width / 13);
                            this.editorButtons[index].BackColor = Color.White;
                            
                        }
                }
                else
                {
                    
                    CurrentRoom = loadedFile.Rooms[this.roomComboBox.SelectedIndex];
                    for ( int i = 0; i < CurrentRoom.Width; i++ )
                        for ( int j = 0; j < CurrentRoom.Height; j++ )
                        {
                            
                            int index = i + j * CurrentRoom.Width;
                            this.editorButtons [ index ].SetBounds (
                                                                    i * TilePanel.Width / 13,
                                                                    j * TilePanel.Width / 13,
                                                                    TilePanel.Width / 13,
                                                                    TilePanel.Width / 13);
                            this.editorButtons[index].BackColor = Color.White;
                        }
                    
                }
                
            this.ControlsPanel.ResumeLayout(false);
            this.ControlsPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private void OnClick ( object sender, EventArgs eventArgs )
        {
            this.selectedX = int.Parse ( ( (Button) sender ).Name.Split ( ',' ) [ 0 ] ) + ( this.leftSide ? 0 : 13 );
            this.selectedY = int.Parse ( ( (Button) sender ).Name.Split ( ',' ) [ 1 ] ) + ( this.topSide ? 0 : 7 );
            foreach ( var editorButton in this.editorButtons )
            {
                editorButton.BackColor = Color.White;
            }
            ( (Button) sender ).BackColor = Color.SlateGray;
            this.refreshRooms();
        }

        private void LoadFileClick(object sender, EventArgs e)
        {
            FileDialog loadDialog = new OpenFileDialog();
            loadDialog.ShowDialog ();
            if ( loadDialog.FileName.Equals ( "" ) )
            {
                return;
            }
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CloseInput = true;
            if ( !loadDialog.FileName.Equals ( "" ) )
                {
            if ( loadDialog.FileName.EndsWith ( "entities2.xml" ) )
            {
                var fileToLoad = XmlReader.Create(File.Open(loadDialog.FileName, FileMode.Open), settings);
                EntityStore.LoadEntities(fileToLoad);
                this.EntityBox.DataSource = EntityStore.getNames ();
            }
            else if ( loadDialog.FileName.EndsWith ( "animations.b" ) )
            {
                AnimationBinaryFile file = new AnimationBinaryFile();
                file.Deserialize(File.Open(loadDialog.FileName, FileMode.Open));
                XmlWriterSettings mySettings = new XmlWriterSettings();
                mySettings.CloseOutput = true;
                mySettings.Indent = true;
                mySettings.OmitXmlDeclaration = true;
                mySettings.ConformanceLevel = ConformanceLevel.Auto;
                StreamReader reader =
                    new StreamReader (
                        File.Open (
                                   Path.Combine (
                                                 Path.GetDirectoryName ( Application.ExecutablePath ),
                                                 "animations.animlist" ),
                                   FileMode.Open ));
                reader.ReadLine ();
                int a = 0;
                foreach ( var type2 in file.result.unknown1 )
                {
                    a++;

                    string filename = a+".anm2";
                    if(!reader.EndOfStream)
                        filename = reader.ReadLine ();
                    filename = Path.Combine (loadDialog.FileName.Replace ( ".b", "_unpacked" ),  filename );
                    Directory.CreateDirectory ( Path.GetDirectoryName(filename));
                    
                    XmlWriter writer = XmlWriter.Create(filename, mySettings);
                    writer.WriteStartElement("AnimatedActor");
                    writer.WriteWhitespace("\n\t");
                    writer.WriteStartElement("info");
                    writer.WriteAttributeString("NameHash", type2.unknown1.ToString());
                    writer.WriteAttributeString("BasePath", type2.unknown2);
                    writer.WriteAttributeString("DefaultAnimation", type2.unknown7);
                    writer.WriteEndElement();
                    writer.WriteWhitespace("\n\t");

                    writer.WriteStartElement("content");
                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteStartElement("Spritesheets");
                    foreach ( var type3 in type2.unknown3 )
                    {
                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteStartElement("Spritesheet");
                        writer.WriteAttributeString("Path", type3.unknown1.ToString());
                        writer.WriteAttributeString("Id", type3.unknown2);
                        writer.WriteEndElement();
                    }
                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteEndElement();

                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteStartElement("Layers");
                    foreach (var type4 in type2.unknown4)
                    {
                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteStartElement("Layer");
                        writer.WriteAttributeString("Name", type4.unknown1.ToString());
                        writer.WriteAttributeString("Id", type4.unknown2.ToString());
                        writer.WriteAttributeString("SpritesheetId", type4.unknown3);
                        writer.WriteEndElement();
                    }
                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteEndElement();



                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteStartElement("Nulls");
                    foreach (var type5 in type2.unknown5)
                    {
                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteStartElement("Null");
                        writer.WriteAttributeString("Id", type5.unknown1.ToString());
                        writer.WriteAttributeString("Name", type5.unknown2);
                        writer.WriteEndElement();
                    }

                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteEndElement();

                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteStartElement("Events");
                    foreach (var type6 in type2.unknown6)
                    {
                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteStartElement("Event");
                        writer.WriteAttributeString("Id", type6.unknown1.ToString());
                        writer.WriteAttributeString("Name", type6.unknown2);
                        writer.WriteEndElement();
                    }
                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteEndElement();

                    writer.WriteWhitespace("\n\t\t");
                    writer.WriteStartElement("Animations");
                    foreach (var type7 in type2.unknown8)
                    {
                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteStartElement("Animation");
                        writer.WriteAttributeString("Name", type7.unknown1);
                        writer.WriteAttributeString("FrameNum", type7.unknown2.ToString());
                        writer.WriteAttributeString("Loop", type7.unknown3.ToString());
                        writer.WriteStartElement("RootAnimation");
                        foreach ( var type8 in type7.unknown4 )
                        {
                            writer.WriteWhitespace("\n\t\t\t\t");
                            writer.WriteStartElement("RootFrame");
                            writer.WriteAttributeString("XPosition", type8.unknown1.ToString());
                            writer.WriteAttributeString("YPosition", type8.unknown2.ToString());
                            writer.WriteAttributeString("Delay", type8.unknown3.ToString());
                            writer.WriteAttributeString("Visible", type8.unknown4.ToString());
                            writer.WriteAttributeString("XScale", type8.unknown5.ToString());
                            writer.WriteAttributeString("YScale", type8.unknown6.ToString());
                            writer.WriteAttributeString("RedTint", type8.unknown7.ToString());
                            writer.WriteAttributeString("GreenTint", type8.unknown8.ToString());
                            writer.WriteAttributeString("BlueTint", type8.unknown9.ToString());
                            writer.WriteAttributeString("AlphaTint", type8.unknown10.ToString());
                            writer.WriteAttributeString("RedOffset", type8.unknown11.ToString());
                            writer.WriteAttributeString("GreenOffset", type8.unknown12.ToString());
                            writer.WriteAttributeString("BlueOffset", type8.unknown13.ToString());
                            writer.WriteAttributeString("Rotation", type8.unknown14.ToString());
                            writer.WriteAttributeString("Interpolated", type8.unknown15.ToString());
                            writer.WriteEndElement();
                        }

                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteEndElement();
                        writer.WriteWhitespace("\n\t\t\t");

                        writer.WriteStartElement("LayerAnimations");
                        foreach (var type9 in type7.unknown5)
                        {
                            writer.WriteWhitespace("\n\t\t\t\t");
                            writer.WriteStartElement("Layer");
                            writer.WriteAttributeString("LayerID", type9.unknown1.ToString());
                            writer.WriteAttributeString("Visible", type9.unknown2.ToString());
                            writer.WriteWhitespace("\n\t\t\t\t\t");
                            writer.WriteStartElement("KeyFrames");
                            foreach ( var type10 in type9.unknown3 )
                            {
                                writer.WriteWhitespace("\n\t\t\t\t\t\t");
                            writer.WriteStartElement("Frame");
                            writer.WriteAttributeString("XPosition", type10.unknown1.ToString());
                            writer.WriteAttributeString("YPosition", type10.unknown2.ToString());
                            writer.WriteAttributeString("XPivot", type10.unknown3.ToString());
                            writer.WriteAttributeString("YPivot", type10.unknown4.ToString());
                            writer.WriteAttributeString("Width", type10.unknown5.ToString());
                            writer.WriteAttributeString("Height", type10.unknown6.ToString());
                            writer.WriteAttributeString("XScale", type10.unknown7.ToString());
                            writer.WriteAttributeString("YScale", type10.unknown8.ToString());
                            writer.WriteAttributeString("Delay", type10.unknown9.ToString());
                            writer.WriteAttributeString("Visible", type10.unknown10.ToString());
                            writer.WriteAttributeString("XCrop", type10.unknown11.ToString());
                            writer.WriteAttributeString("YCrop", type10.unknown12.ToString());
                            writer.WriteAttributeString("RedTint", type10.unknown13.ToString());
                            writer.WriteAttributeString("GreenTint", type10.unknown14.ToString());
                            writer.WriteAttributeString("BlueTint", type10.unknown15.ToString());
                            writer.WriteAttributeString("AlphaTint", type10.unknown16.ToString());
                            writer.WriteAttributeString("RedOffset", type10.unknown17.ToString());
                            writer.WriteAttributeString("GreenOffset", type10.unknown18.ToString());
                            writer.WriteAttributeString("BlueOffset", type10.unknown19.ToString());
                            writer.WriteAttributeString("AlphaOffset", type10.unknown20.ToString());
                            writer.WriteAttributeString("Rotation", type10.unknown21.ToString());
                            writer.WriteAttributeString("Interpolated", type10.unknown21.ToString());
                            writer.WriteEndElement();
                            }

                            writer.WriteWhitespace("\n\t\t\t\t\t");
                            writer.WriteEndElement();
                            writer.WriteWhitespace("\n\t\t\t\t");
                            writer.WriteEndElement();
                        }

                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteEndElement();
                        foreach ( var type11 in type7.unknown6 )
                        {
                            writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteStartElement("NullAnimations");
                        writer.WriteAttributeString("NullId", type7.unknown2.ToString());
                        writer.WriteAttributeString("Visible", type7.unknown3.ToString());
                            foreach (var type8 in type11.unknown3)
                            {
                            writer.WriteWhitespace("\n\t\t\t\t");
                            writer.WriteStartElement("NullFrame");
                            writer.WriteAttributeString("XPosition", type8.unknown1.ToString());
                            writer.WriteAttributeString("YPosition", type8.unknown2.ToString());
                            writer.WriteAttributeString("Delay", type8.unknown3.ToString());
                            writer.WriteAttributeString("Visible", type8.unknown4.ToString());
                            writer.WriteAttributeString("XScale", type8.unknown5.ToString());
                            writer.WriteAttributeString("YScale", type8.unknown6.ToString());
                            writer.WriteAttributeString("RedTint", type8.unknown7.ToString());
                            writer.WriteAttributeString("GreenTint", type8.unknown8.ToString());
                            writer.WriteAttributeString("BlueTint", type8.unknown9.ToString());
                            writer.WriteAttributeString("AlphaTint", type8.unknown10.ToString());
                            writer.WriteAttributeString("RedOffset", type8.unknown11.ToString());
                            writer.WriteAttributeString("GreenOffset", type8.unknown12.ToString());
                            writer.WriteAttributeString("BlueOffset", type8.unknown13.ToString());
                            writer.WriteAttributeString("Rotation", type8.unknown14.ToString());
                            writer.WriteAttributeString("Interpolated", type8.unknown15.ToString());
                            writer.WriteEndElement();
                            }
                            writer.WriteWhitespace("\n\t\t\t");
                            writer.WriteEndElement();
                        }


                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteStartElement("Triggers");
                        foreach (var type11 in type7.unknown7)
                        {
                            writer.WriteWhitespace("\n\t\t\t\t");
                            writer.WriteStartElement("Trigger");
                            writer.WriteAttributeString("EventId", type11.unknown1.ToString());
                            writer.WriteAttributeString("AtFrame", type11.unknown2.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteWhitespace("\n\t\t\t");
                        writer.WriteEndElement();
                        writer.WriteWhitespace("\n\t\t");
                        writer.WriteEndElement();
                    }
                    writer.WriteWhitespace("\n\t");
                    writer.WriteEndElement();
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement();
                    writer.Close();
                }
                reader.Close();
            }
            else
            {

                loadedFile = new StageBinaryFile ();
                loadedFile.Deserialize(File.Open(loadDialog.FileName, FileMode.Open));
                    

                this.Text = "Binding of isaac: Rebirth level editor - "
                            + Path.GetFileNameWithoutExtension ( loadDialog.FileName );
                this.roomComboBox.DataSource = loadedFile.RoomNames;
            }
            }
        }

        private void RoomChanged(object sender, EventArgs e)
        {
            this.leftSide = true;
            this.topSide = true;
            this.refreshRooms();            
        }

        private void refreshRooms()
        {
            doLayout(ClientRectangle);
            var tempRoom = loadedFile.Rooms [ this.roomComboBox.SelectedIndex ];


                foreach (var button in this.editorButtons)
                {
                    button.Text = tempRoom.FindEntityNames(button.Name);
                    
                }
                var spawn = tempRoom.FindSpawn(this.selectedX,this.selectedY);

                this.tileEntityListBox.DataSource = null;
                if ( spawn.X != -1 )
                {
                    this.tileEntityListBox.DataSource = spawn.Names;
                    if(spawn.Entities.Length > 0 && this.tileEntityListBox.SelectedIndex >= 0)
                    this.weightTextBox.Text = "" + spawn.Entities[this.tileEntityListBox.SelectedIndex].Weight;
                }
                else
                {

                    this.weightTextBox.Text = "";
                    this.tileEntityListBox.DataSource = null;
                }
                this.tileEntityListBox.Refresh();
        }

        private void ToggleLeftRightClick ( object sender, EventArgs e )
        {
            this.leftSide = !this.leftSide;
            this.refreshRooms();
        }

        private void ToggleUpDownClick(object sender, EventArgs e)
        {
            this.topSide = !this.topSide;
            this.refreshRooms();
        }

        private void saveFile ( object sender, EventArgs e )
        {
            FileDialog saveDialog = new SaveFileDialog ();
            saveDialog.ShowDialog ();
            if ( saveDialog.FileName.Equals ( "" ) )
            {
                return;
            }
            loadedFile.Serialize(File.Create ( saveDialog.FileName ));
        }

        private void AddClick(object sender, EventArgs e)
        {
            var tempRoom = loadedFile.Rooms[this.roomComboBox.SelectedIndex];

            var spawn = tempRoom.FindSpawn(this.selectedX, this.selectedY);
            if ( spawn.X != -1 )
            {
                var temp = EntityStore.findByID( int.Parse(TextboxType.Text), int.Parse(TextboxVariant.Text), int.Parse(TextboxSubType.Text) );
                spawn.AddEntity(temp);
                this.refreshRooms();
            }
            else
            {
                var tempspawn = new StageBinaryFile.Spawn{X=(short) this.selectedX, Y=(short) this.selectedY};

                var tempEnt = EntityStore.findByID(int.Parse(TextboxType.Text), int.Parse(TextboxVariant.Text), int.Parse(TextboxSubType.Text));
                if (tempEnt.name != null)
                    tempspawn.AddEntity(tempEnt);
                tempRoom.AddSpawn(tempspawn);
                this.refreshRooms();
            }
        }

        private void RemoveClick(object sender, EventArgs e)
        {
            var tempRoom = loadedFile.Rooms[this.roomComboBox.SelectedIndex];

            var spawn = tempRoom.FindSpawn(this.selectedX, this.selectedY);
            if (spawn.X != -1)
            {
                spawn.RemoveAt(this.tileEntityListBox.SelectedIndex);
                if ( spawn.Entities.Length == 0 )
                {
                    for ( int i = 0; i < tempRoom.Spawns.Length; i++ )
                    {
                        if (tempRoom.Spawns[i].Equals(spawn))
                        {
                            tempRoom.RemoveSpawnAt(i);
                        }
                    }
                }
                this.tileEntityListBox.SelectedIndex = 0;
                this.refreshRooms();
            }
        }

        private void SetWeight(object sender, EventArgs e)
        {
            var tempRoom = loadedFile.Rooms[this.roomComboBox.SelectedIndex];

            var spawn = tempRoom.FindSpawn(this.selectedX, this.selectedY);
            if ( spawn.X == -1 )
            {
                return;
            }
            float.TryParse(this.weightTextBox.Text, out spawn.Entities[this.tileEntityListBox.SelectedIndex].Weight);

            this.refreshRooms();
        }

        private void SelectedEntityChanged(object sender, EventArgs e) { this.refreshRooms (); }
    }
}
