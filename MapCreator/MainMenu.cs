using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using MapEngine.Altitude;
using MapEngine.ImportTiles;
using MapEngine.Logger;
using MapEngine.Terrain;
using MapEngine.Transition;
using MapEngine.UltimaSDK;

namespace MapCreator
{
    public partial class mainMenu : Form
    {
        /// Panel01 | Panel02
        private ClsTerrainTable iTerrain;
        private ClsAltitudeTable iAltitude;
        private loggerForm iLogger;


        /// Panel03
        private Bitmap i_Terrain;
        private Bitmap i_Altitude;

        private bool i_RandomStatic;


        public mainMenu()
        {
            MaximizeBox = false;
            MinimizeBox = false;

            this.iTerrain = new ClsTerrainTable();
            this.iAltitude = new ClsAltitudeTable();
            this.iLogger = new loggerForm();

            /// Panel01
            mainMenu makeMapImage = this;
            base.Load += new EventHandler(makeMapImage.Panel01_Load);


            /// Panel02
            mainMenu altImagePrep = this;
            base.Load += new EventHandler(altImagePrep.Panel02_Load);


            /// Panel03
            mainMenu uOMapMake = this;
            base.Load += new EventHandler(uOMapMake.Panel03_Load);
            this.iLogger = new loggerForm();
            this.i_RandomStatic = true;


            InitializeComponent();
        }

        private void mainMenu_Load(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                /// These Statements Hide Panel01 On When Loading The Form
                Control cFBF = this.mainMenu_groupBox01_createYourWorld_panel01_workBench;
                Thread.Sleep(50);
                cFBF.Hide();

                /// These Statements Hide Panel02 On When Loading The Form
                Control sYAB = this.mainMenu_groupBox01_createYourWorld_panel02_workBench;
                Thread.Sleep(50);
                sYAB.Hide();

                /// These Statements Hide Panel03 On Button Press
                Control cYNF = this.mainMenu_groupBox01_createYourWorld_panel03_workBench;
                Thread.Sleep(50);
                cYNF.Hide();
            }
        }

        private void mainMenu_groupBox01_createYourWorld_button01_createFacetBitmapFiles_Click(object sender, EventArgs e)
        {
            /// These Statements Show Panel01 On Button Press
            Control cFBF = this.mainMenu_groupBox01_createYourWorld_panel01_workBench;
            Thread.Sleep(1000);
            cFBF.Show();

            #region syncYourAltitudeBitmap_Hide

            /// These Statements Hide Panel02 On Button press 
            Control sYAB = this.mainMenu_groupBox01_createYourWorld_panel02_workBench;
            Thread.Sleep(75); // How Fast Does The Panel Appear After Button Press
            sYAB.Hide();

            /// These Statements Hide Panel03 On Button press 
            Control cYNF = this.mainMenu_groupBox01_createYourWorld_panel03_workBench;
            Thread.Sleep(75); // How Fast Does The Panel Appear After Button Press
            cYNF.Hide();

            #endregion
        }

        private void mainMenu_groupBox01_createYourWorld_button02_syncYourAltitudeBitmap_Click(object sender, EventArgs e)
        {
            /// These Statements Show Panel02 On Button Press
            Control sYAB = this.mainMenu_groupBox01_createYourWorld_panel02_workBench;
            Thread.Sleep(1000);
            sYAB.Show();

            #region createFacetBitmapFiles_Hide

            /// These Statements Hide Panel01 On Button press 
            Control cFBF = this.mainMenu_groupBox01_createYourWorld_panel01_workBench;
            Thread.Sleep(75); // How Fast Does The Panel Appear After Button Press
            cFBF.Hide();

            /// These Statements Hide Panel03 On Button press 
            Control cYNF = this.mainMenu_groupBox01_createYourWorld_panel03_workBench;
            Thread.Sleep(75); // How Fast Does The Panel Appear After Button Press
            cYNF.Hide();

            #endregion
        }

        private void mainMenu_groupBox01_createYourWorld_button03_compileYourNewFacet_Click(object sender, EventArgs e)
        {
            /// These Statements Show Panel03 On Button Press
            Control cYNF = this.mainMenu_groupBox01_createYourWorld_panel03_workBench;
            Thread.Sleep(1000);
            cYNF.Show();

            #region createFacetBitmapFiles_Hide

            /// These Statements Hide Panel01 On Button press 
            Control cFBF = this.mainMenu_groupBox01_createYourWorld_panel01_workBench;
            Thread.Sleep(75); // How Fast Does The Panel Appear After Button Press
            cFBF.Hide();

            /// These Statements Hide Panel02 On Button press 
            Control sYAB = this.mainMenu_groupBox01_createYourWorld_panel02_workBench;
            Thread.Sleep(75); // How Fast Does The Panel Appear After Button Press
            sYAB.Hide();

            #endregion
        }

        #region Panel01: CreateFacetBitmapFiles Workbench

        private void Panel01_Load(object sender, EventArgs e)
        {
            IEnumerator enumerator = null;
            this.iLogger.Show();
            int x = checked(this.iLogger.Location.X + 100);
            Point location = this.iLogger.Location;
            Point point = new Point(x, checked(location.Y + 100));
            this.Location = point;
            this.iTerrain.Load();
            this.iAltitude.Load();

            #region Data Directory Modification

            string str = string.Format("{0}\\Data\\Engine\\{1}", Directory.GetCurrentDirectory(), "MapInfo.xml");

            #endregion

            this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox01_projectPath.Text = Directory.GetCurrentDirectory();
            this.iTerrain.Display(this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_comboBox02_selectTerrain);
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(str);
                try
                {
                    this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_comboBox01_selectMapSize.Items.Clear();
                    try
                    {
                        enumerator = xmlDocument.SelectNodes("//Maps/Map").GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            RenderBitmaps facet = new RenderBitmaps((XmlElement)enumerator.Current);
                            this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_comboBox01_selectMapSize.Items.Add(facet);
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable)
                        {
                            ((IDisposable)enumerator).Dispose();
                        }
                    }
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    this.iLogger.LogMessage(string.Format("XML Error:{0}", exception.Message));
                    ProjectData.ClearProjectError();
                }
            }
            catch (Exception exception2)
            {
                ProjectData.SetProjectError(exception2);
                this.iLogger.LogMessage(string.Format("Unable to find:{0}", str));
                ProjectData.ClearProjectError();
            }
        }

        private void mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_button01_locateProjectFolderPath_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox01_projectPath.Text
            };
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox01_projectPath.Text = folderBrowserDialog.SelectedPath;

                // Checks To Make Sure The Terrain.bmp And The Altitude.bmp Are In THe Selected Directory:
                // If One, Or Both, Of The Files Are There It Will Populate The Terrain.bmp and Altitude.bmp Fields On The Panel With: "Terrain.bmp Found!"  |  "Altitude.bmp Found!"
                // If One, Or Both, Of The Files Are Missing Then It Will Populate The Terrain.bmp and Altitude.bmp Fields On The Panel With: "Terrain.bmp Missing!"  |  "Altitude.bmp Missing!"
                string BitmapPath = folderBrowserDialog.SelectedPath;

                string filename1 = "Terrain.bmp";
                string filename2 = "Altitude.bmp";

                DirectoryInfo directory = new DirectoryInfo(BitmapPath);
                FileInfo[] files = directory.GetFiles();

                bool terrainBitmapFound = false;
                bool altitudeBitmapFound = false;

                foreach (FileInfo file in files)
                {
                    if (String.Compare(file.Name, filename1) == 0)
                    {
                        terrainBitmapFound = true;
                        mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox02_terrainBitmap.Text = "Terrain.bmp Found!";
                    }
                    else if (String.Compare(file.Name, filename2) == 0)
                    {
                        altitudeBitmapFound = true;
                        mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox03_altitudeBitmap.Text = "Altitude.bmp Found!";
                    }
                }

                if (!terrainBitmapFound)
                {
                    Console.WriteLine("File does not exist in the specified directory!");
                    mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox02_terrainBitmap.Text = "Terrain.bmp Missing!";
                }
                else if (!altitudeBitmapFound)
                {
                    mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox03_altitudeBitmap.Text = "Altitude.bmp Missing!";
                }
            }
        }

        private void mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_button02_generateFacetBitmapFiles_Click(object sender, EventArgs e)
        {
            byte altID;
            byte groupID;
            RenderBitmaps selectedItem = (RenderBitmaps)this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_comboBox01_selectMapSize.SelectedItem;
            if (selectedItem == null)
            {
                this.iLogger.LogMessage("Error: Select a Map Type.");
            }
            else if (StringType.StrCmp(this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox04_projectName.Text, string.Empty, false) != 0)
            {
                string str = string.Format("{0}/{1}/Map{2}", this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox01_projectPath.Text, this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox04_projectName.Text, selectedItem.MapNumber);
                if (!Directory.Exists(str))
                {
                    Directory.CreateDirectory(str);
                }
                if (this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_comboBox02_selectTerrain.SelectedItem != null)
                {
                    ClsTerrain clsTerrain = (ClsTerrain)this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_comboBox02_selectTerrain.SelectedItem;
                    groupID = checked((byte)clsTerrain.GroupID);
                    altID = clsTerrain.AltID;
                }
                else
                {
                    groupID = 9;
                    altID = 66;
                }
                this.iLogger.LogMessage("Creating Terrain Image.");
                this.iLogger.StartTask();
                try
                {
                    string str1 = string.Format("{0}/{1}", str, this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox02_terrainBitmap.Text);
                    Bitmap palette = this.MakeTerrainMap(selectedItem.XSize, selectedItem.YSize, groupID, this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_checkBox_dungeonToggle.Checked);
                    palette.Palette = this.iTerrain.GetPalette();
                    palette.Save(str1, ImageFormat.Bmp);
                    palette.Dispose();
                }
                catch (Exception exception)
                {
                    ProjectData.SetProjectError(exception);
                    this.iLogger.LogMessage("Error: Problem creating Terrain Image.");
                    ProjectData.ClearProjectError();
                }
                //this.iLogger.EndTask();
                this.iLogger.LogTimeStamp();
                this.iLogger.LogMessage("Creating Altitude Image.");
                this.iLogger.StartTask();
                try
                {
                    string str2 = string.Format("{0}/{1}", str, this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_textBox03_altitudeBitmap.Text);
                    Bitmap altPalette = this.MakeAltMap(selectedItem.XSize, selectedItem.YSize, altID, this.mainMenu_groupBox01_createYourWorld_panel01_workBench_groupBox01_createFacetBitmapFiles_checkBox_dungeonToggle.Checked);
                    altPalette.Palette = this.iAltitude.GetAltPalette();
                    altPalette.Save(str2, ImageFormat.Bmp);
                    altPalette.Dispose();
                }
                catch (Exception exception2)
                {
                    ProjectData.SetProjectError(exception2);
                    Exception exception1 = exception2;
                    this.iLogger.LogMessage("Error: Problem creating Altitude Image.");
                    this.iLogger.LogMessage(exception1.Message);
                    ProjectData.ClearProjectError();
                }
                //this.iLogger.EndTask();
                this.iLogger.LogTimeStamp();
                this.iLogger.LogMessage("Done.");
            }
            else
            {
                this.iLogger.LogMessage("Error: Enter a project Name.");
            }
        }

        public Bitmap MakeTerrainMap(int xSize, int ySize, byte DefaultTerrain, bool Dungeon)
        {
            Bitmap bitmap = new Bitmap(xSize, ySize, PixelFormat.Format8bppIndexed)
            {
                Palette = this.iTerrain.GetPalette()
            };
            Rectangle rectangle = new Rectangle(0, 0, xSize, ySize);
            BitmapData bitmapDatum = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr scan0 = bitmapDatum.Scan0;
            int width = checked(bitmapDatum.Width * bitmapDatum.Height);
            byte[] defaultTerrain = new byte[checked(checked(width - 1) + 1)];
            Marshal.Copy(scan0, defaultTerrain, 0, width);
            if (!Dungeon)
            {
                int num = checked(xSize - 1);
                for (int i = 0; i <= num; i++)
                {
                    int num1 = checked(ySize - 1);
                    for (int j = 0; j <= num1; j++)
                    {
                        defaultTerrain[checked(checked(j * xSize) + i)] = DefaultTerrain;
                    }
                }
            }
            else
            {
                int num2 = checked(xSize - 1);
                for (int k = 0; k <= num2; k++)
                {
                    int num3 = checked(ySize - 1);
                    for (int l = 0; l <= num3; l++)
                    {
                        if (k <= 5119)
                        {
                            defaultTerrain[checked(checked(l * xSize) + k)] = DefaultTerrain;
                        }
                        else
                        {
                            defaultTerrain[checked(checked(l * xSize) + k)] = 19;
                        }
                    }
                }
            }
            Marshal.Copy(defaultTerrain, 0, scan0, width);
            bitmap.UnlockBits(bitmapDatum);
            return bitmap;
        }

        public Bitmap MakeAltMap(int xSize, int ySize, byte DefaultAlt, bool Dungeon)
        {
            Bitmap bitmap = new Bitmap(xSize, ySize, PixelFormat.Format8bppIndexed)
            {
                Palette = this.iAltitude.GetAltPalette()
            };
            Rectangle rectangle = new Rectangle(0, 0, xSize, ySize);
            BitmapData bitmapDatum = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr scan0 = bitmapDatum.Scan0;
            int width = checked(bitmapDatum.Width * bitmapDatum.Height);
            byte[] defaultAlt = new byte[checked(checked(width - 1) + 1)];
            Marshal.Copy(scan0, defaultAlt, 0, width);
            if (!Dungeon)
            {
                int num = checked(xSize - 1);
                for (int i = 0; i <= num; i++)
                {
                    int num1 = checked(ySize - 1);
                    for (int j = 0; j <= num1; j++)
                    {
                        defaultAlt[checked(checked(j * xSize) + i)] = DefaultAlt;
                    }
                }
            }
            else
            {
                int num2 = checked(xSize - 1);
                for (int k = 0; k <= num2; k++)
                {
                    int num3 = checked(ySize - 1);
                    for (int l = 0; l <= num3; l++)
                    {
                        if (k <= 5119)
                        {
                            defaultAlt[checked(checked(l * xSize) + k)] = DefaultAlt;
                        }
                        else
                        {
                            defaultAlt[checked(checked(l * xSize) + k)] = 72;
                        }
                    }
                }
            }
            Marshal.Copy(defaultAlt, 0, scan0, width);
            bitmap.UnlockBits(bitmapDatum);
            return bitmap;
        }

        public class RenderBitmaps
        {
            private string m_Name;
            private byte m_Num;
            private int m_XSize;
            private int m_YSize;

            #region Getters And Setters

            public string MapName
            {
                get
                {
                    return this.m_Name;
                }
            }

            public byte MapNumber
            {
                get
                {
                    return this.m_Num;
                }
            }

            public int XSize
            {
                get
                {
                    return this.m_XSize;
                }
            }

            public int YSize
            {
                get
                {
                    return this.m_YSize;
                }
            }

            #endregion

            public RenderBitmaps(XmlElement iXml)
            {
                this.m_Name = iXml.GetAttribute("Name");
                this.m_Num = ByteType.FromString(iXml.GetAttribute("Num"));
                this.m_XSize = IntegerType.FromString(iXml.GetAttribute("XSize"));
                this.m_YSize = IntegerType.FromString(iXml.GetAttribute("YSize"));
            }

            public override string ToString()
            {
                return string.Format("{0}", this.m_Name);
            }
        }

        #endregion

        #region Panel02: SyncYourAltitudeBitmap Workbench

        private void Panel02_Load(object sender, EventArgs e)
        {
            this.iLogger.Show();
            int x = checked(this.iLogger.Location.X + 100);
            Point location = this.iLogger.Location;
            Point point = new Point(x, checked(location.Y + 100));
            this.Location = point;
            this.mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox01_projectPath.Text = Directory.GetCurrentDirectory();
            this.iTerrain.Load();
            this.iAltitude.Load();
        }

        private void mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_button01_locateProjectFolderPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = this.mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox01_projectPath.Text
            };

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox01_projectPath.Text = folderBrowserDialog.SelectedPath;

                // Checks To Make Sure The Terrain.bmp And The Altitude.bmp Are In THe Selected Directory:
                // If One, Or Both, Of The Files Are There It Will Populate The Terrain.bmp and Altitude.bmp Fields On The Panel With: "Terrain.bmp Found!"  |  "Altitude.bmp Found!"
                // If One, Or Both, Of The Files Are Missing Then It Will Populate The Terrain.bmp and Altitude.bmp Fields On The Panel With: "Terrain.bmp Missing!"  |  "Altitude.bmp Missing!"
                string BitmapPath = folderBrowserDialog.SelectedPath;

                string filename1 = "Terrain.bmp";
                string filename2 = "Altitude.bmp";

                DirectoryInfo directory = new DirectoryInfo(BitmapPath);
                FileInfo[] files = directory.GetFiles();

                bool terrainBitmapFound = false;
                bool altitudeBitmapFound = false;

                foreach (FileInfo file in files)
                {
                    if (String.Compare(file.Name, filename1) == 0)
                    {
                        terrainBitmapFound = true;
                        mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox02_terrainBitmap.Text = "Terrain.bmp Found!";
                    }
                    else if (String.Compare(file.Name, filename2) == 0)
                    {
                        altitudeBitmapFound = true;
                        mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox03_altitudeBitmap.Text = "Altitude.bmp Found!";
                    }
                }

                if (!terrainBitmapFound)
                {
                    Console.WriteLine("File does not exist in the specified directory!");
                    mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox02_terrainBitmap.Text = "Terrain.bmp Missing!";
                }
                else if (!altitudeBitmapFound)
                {
                    mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox03_altitudeBitmap.Text = "Altitude.bmp Missing!";
                }
            }
        }

        private async void mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_button02_renderAltitudeBitmap_Click(object sender, EventArgs e)
        {
            Progress<int> progress = new Progress<int>(i => { mainMenu_groupBox01_progressBar.Value = Math.Abs(i); }); // TODO: temporary fix, i didn't get why it put -73
            Progress<string> logger = new Progress<string>(i => { iLogger.LogMessage(i); });
            Task resetProgress = new Task(() => { Thread.Sleep(1000); ((IProgress<int>)progress).Report(0); });

            await Task.Run(() => RenderAltitude.MakeAltitudeImage(mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox01_projectPath.Text, mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox02_terrainBitmap.Text, mainMenu_groupBox01_createYourWorld_panel02_workBench_groupBox01_syncYourAltitudeBitmap_textBox03_altitudeBitmap.Text, iAltitude, iTerrain, progress, logger)).ContinueWith(c => resetProgress.Start());           
        }

        public static class RenderAltitude
        {
            public static void MakeAltitudeImage(string projectPath, string terrainFile, string altitudeFile, ClsAltitudeTable iAltitude, ClsTerrainTable iTerrain, IProgress<int> Progress, IProgress<string> Logger)
            {
                Bitmap bitmap = null;
                Bitmap bitmap1 = null;
                try
                {
                    Logger.Report("Load Terrain Image Map.");
                    bitmap1 = new Bitmap(string.Format("{0}\\{1}", projectPath, terrainFile));
                    bitmap = new Bitmap(bitmap1.Width, bitmap1.Height, PixelFormat.Format8bppIndexed);
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    Logger.Report("Error in loading image maps.\r\n");
                    Logger.Report(exception.Message);
                    ProjectData.ClearProjectError();
                }
                bitmap.Palette = iAltitude.GetAltPalette();
                int width = bitmap1.Width;
                Rectangle rectangle = new Rectangle(0, 0, width, bitmap1.Height);
                BitmapData bitmapDatum = bitmap1.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                IntPtr scan0 = bitmapDatum.Scan0;
                int num = checked(bitmapDatum.Width * bitmapDatum.Height);
                byte[] numArray = new byte[checked(checked(num - 1) + 1)];
                Marshal.Copy(scan0, numArray, 0, num);
                BitmapData bitmapDatum1 = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                IntPtr intPtr = bitmapDatum1.Scan0;
                int width1 = checked(bitmapDatum1.Width * bitmapDatum1.Height);
                byte[] numArray1 = new byte[checked(checked(width1 - 1) + 1)];
                Marshal.Copy(intPtr, numArray1, 0, width1);
                Logger.Report("Creating Altitude Image Map.");
                int num1 = checked(num - 1);
                for (int i = 0; i <= num1; i++)
                {
                    if ((i % 1000) == 0) Progress.Report(i * 100 / width1);
                    byte altID = iTerrain.TerrianGroup(numArray[i]).AltID;
                    numArray1[i] = altID;
                }
                Marshal.Copy(numArray1, 0, intPtr, width1);
                bitmap.UnlockBits(bitmapDatum1);
                try
                {
                    string str = string.Format("{0}\\{1}", projectPath, altitudeFile);
                    Logger.Report("Saving Altitude Image Map.\r\n");
                    Logger.Report(str);
                    bitmap.Save(str, ImageFormat.Bmp);
                }
                catch (Exception exception3)
                {
                    ProjectData.SetProjectError(exception3);
                    Exception exception2 = exception3;
                    Logger.Report("Error in saving image.\r\n");
                    Logger.Report(exception2.Message);
                    ProjectData.ClearProjectError();
                }
                bitmap.Dispose();
                bitmap1.Dispose();
                Logger.Report("Done.");
            }
        }

        #endregion

        #region Panel03: CompileYourNewFacet Workbench

        private void Panel03_Load(object sender, EventArgs e)
        {
            this.iLogger.Show();
            int x = checked(this.iLogger.Location.X + 100);
            Point location = this.iLogger.Location;
            Point point = new Point(x, checked(location.Y + 100));
            this.Location = point;
            this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// Locate Project Folder Path: Search For The Directory Where You Created Your Project Templates
        private void mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_button01_locateProjectFolderPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text
            };

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text = folderBrowserDialog.SelectedPath;

                // Checks To Make Sure The Terrain.bmp And The Altitude.bmp Are In THe Selected Directory:
                // If One, Or Both, Of The Files Are There It Will Populate The Terrain.bmp and Altitude.bmp Fields On The Panel With: "Terrain.bmp Found!"  |  "Altitude.bmp Found!"
                // If One, Or Both, Of The Files Are Missing Then It Will Populate The Terrain.bmp and Altitude.bmp Fields On The Panel With: "Terrain.bmp Missing!"  |  "Altitude.bmp Missing!"
                string BitmapPath = folderBrowserDialog.SelectedPath;

                string filename1 = "Terrain.bmp";
                string filename2 = "Altitude.bmp";

                DirectoryInfo directory = new DirectoryInfo(BitmapPath);
                FileInfo[] files = directory.GetFiles();

                bool terrainBitmapFound = false;
                bool altitudeBitmapFound = false;

                foreach (FileInfo file in files)
                {
                    if (String.Compare(file.Name, filename1) == 0)
                    {
                        terrainBitmapFound = true;
                        mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox02_terrainBitmap.Text = "Terrain.bmp Found!";
                    }
                    else if (String.Compare(file.Name, filename2) == 0)
                    {
                        altitudeBitmapFound = true;
                        mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox03_altitudeBitmap.Text = "Altitude.bmp Found!";
                    }
                }

                if (!terrainBitmapFound)
                {
                    Console.WriteLine("File does not exist in the specified directory!");
                    mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox02_terrainBitmap.Text = "Terrain.bmp Missing!";
                }
                else if (!altitudeBitmapFound)
                {
                    mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox03_altitudeBitmap.Text = "Altitude.bmp Missing!";
                }
            }
        }

        /// Toggle Random Statics On: As The Facet Compiles, Thingss Like Random Rocks, Trees, And Vegetation Will Be Placed On It
        private void mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_radioButton01_toggleRandomStaticsON_CheckedChanged(object sender, EventArgs e)
        {
            this.i_RandomStatic = true;
            System.Media.SystemSounds.Beep.Play();
        }

        /// Toggle Random Statics Off: As The Facet Compiles, Random Statics Will NOT Be Placed In On The It
        private void mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_radioButton01_toggleRandomStaticsOFF_CheckedChanged(object sender, EventArgs e)
        {
            this.i_RandomStatic = false;
            System.Media.SystemSounds.Beep.Play();
        }

        /// Generate Your New Facet: Press This Button And MapCreator Will Generate:  Map#.mul  |  Statics#.mul  |  Staidx#.mul
        private void mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_button02_generateYourNewFacet_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("You are about to create the Mul Files\r\nAre you sure ?", MsgBoxStyle.YesNo, "Make UO Map") == MsgBoxResult.Yes)
            {
                mainMenu uOMapMake = this;
                (new Thread(new ThreadStart(uOMapMake.Make))).Start();
            }
        }

        private void Make()
        {
            short altID = 0;
            string str;
            byte num = 0;
            int num1;
            int num2;
            int num3;
            int num4;
            IEnumerator enumerator = null;
            TransitionTable transitionTable = new TransitionTable();
            DateTime now = DateTime.Now;
            this.iLogger.StartTask();
            this.iLogger.LogMessage("Loading Terrain Image.");
            try
            {
                str = string.Format("{0}\\{1}", this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text, this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox02_terrainBitmap.Text);
                this.iLogger.LogMessage(str);
                this.i_Terrain = new Bitmap(str);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this.iLogger.LogMessage("Problem with Loading Terrain Image.");
                this.iLogger.LogMessage(exception.Message);
                ProjectData.ClearProjectError();
                return;
            }
            this.iLogger.LogMessage("Loading Altitude Image.");
            try
            {
                str = string.Format("{0}\\{1}", this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text, this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox03_altitudeBitmap.Text);
                this.iLogger.LogMessage(str);
                this.i_Altitude = new Bitmap(str);
            }
            catch (Exception exception3)
            {
                ProjectData.SetProjectError(exception3);
                Exception exception2 = exception3;
                this.iLogger.LogMessage("Problem with Loading Altitude Image.");
                this.iLogger.LogMessage(exception2.Message);
                ProjectData.ClearProjectError();
                return;
            }
            //this.iLogger.EndTask();
            this.iLogger.LogTimeStamp();
            this.iLogger.LogMessage("Preparing Image Files.");
            this.iLogger.StartTask();
            int width = this.i_Terrain.Width;
            int height = this.i_Terrain.Height;
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            BitmapData bitmapDatum = this.i_Terrain.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr scan0 = bitmapDatum.Scan0;
            int width1 = checked(bitmapDatum.Width * bitmapDatum.Height);
            byte[] numArray = new byte[checked(checked(width1 - 1) + 1)];
            Marshal.Copy(scan0, numArray, 0, width1);
            BitmapData bitmapDatum1 = this.i_Altitude.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr intPtr = bitmapDatum1.Scan0;
            int width2 = checked(bitmapDatum1.Width * bitmapDatum1.Height);
            byte[] numArray1 = new byte[checked(checked(width2 - 1) + 1)];
            Marshal.Copy(intPtr, numArray1, 0, width2);
            //this.iLogger.EndTask();
            this.iLogger.LogTimeStamp();
            this.iLogger.LogMessage("Creating Master Terrian Table.");
            this.iLogger.StartTask();
            MapCell[,] mapCell = new MapCell[checked(width + 1), checked(height + 1)];
            ClsAltitudeTable clsAltitudeTable = new ClsAltitudeTable();
            clsAltitudeTable.Load();
            try
            {
                int num5 = checked(width - 1);
                for (int i = 0; i <= num5; i++)
                {
                    int num6 = checked(height - 1);
                    for (int j = 0; j <= num6; j++)
                    {
                        int num7 = checked(checked(j * width) + i);
                        ClsAltitude getAltitude = clsAltitudeTable.GetAltitude(numArray1[num7]);
                        mapCell[i, j] = new MapCell(numArray[num7], getAltitude.GetAltitude);
                    }
                }
            }
            catch (Exception exception4)
            {
                ProjectData.SetProjectError(exception4);
                this.iLogger.LogMessage("Altitude image needs to be rebuilt");
                ProjectData.ClearProjectError();
                return;
            }
            this.i_Terrain.Dispose();
            this.i_Altitude.Dispose();
            this.iLogger.LogTimeStamp();
            width--;
            height--;
            int num8 = checked((int)Math.Round((double)width / 8 - 1));
            int num9 = checked((int)Math.Round((double)height / 8 - 1));
            this.iLogger.LogMessage("Load Transition Tables.");
            this.iLogger.StartTask();
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            #region Data Directory Modification

            baseDirectory = string.Concat(baseDirectory, "Data\\Engine\\Transitions\\");

            #endregion

            if (Directory.Exists(baseDirectory))
            {
                transitionTable.MassLoad(baseDirectory);
                this.iLogger.LogTimeStamp();
                this.iLogger.LogMessage("Preparing Static Tables");
                Collection[,] collections = new Collection[checked(num8 + 1), checked(num9 + 1)];
                int num10 = num8;
                for (int k = 0; k <= num10; k++)
                {
                    int num11 = num9;
                    for (int l = 0; l <= num11; l++)
                    {
                        collections[k, l] = new Collection();
                    }
                }
                this.iLogger.LogMessage("Applying Transition Tables.");
                this.iLogger.StartTask();
                this.mainMenu_groupBox01_progressBar.Maximum = width;
                ClsTerrainTable clsTerrainTable = new ClsTerrainTable();
                clsTerrainTable.Load();
                MapTile mapTile = new MapTile();


                Transition transition = new Transition();
                /// Transition.Transition transition = new Transition.Transition();


                short[] numArray2 = new short[16];
                short num12 = checked((short)width);
                for (short m = 0; m <= num12; m = checked((short)(m + 1)))
                {
                    num1 = (m != 0 ? checked(m - 1) : width);
                    num2 = (m != width ? checked(m + 1) : 0);
                    short num13 = checked((short)height);
                    for (short n = 0; n <= num13; n = checked((short)(n + 1)))
                    {
                        num4 = (n != 0 ? checked(n - 1) : height);
                        num3 = (n != height ? checked(n + 1) : 0);
                        object[] groupID = new object[] { mapCell[num1, num4].GroupID, mapCell[m, num4].GroupID, mapCell[num2, num4].GroupID, mapCell[num1, n].GroupID, mapCell[m, n].GroupID, mapCell[num2, n].GroupID, mapCell[num1, num3].GroupID, mapCell[m, num3].GroupID, mapCell[num2, num3].GroupID };
                        string str1 = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}{8:X2}", groupID);
                        try
                        {
                            transition = (Transition)(transitionTable.GetTransitionTable[str1]);

                            /// transition = (Transition.Transition)(transitionTable.GetTransitionTable[str1]);

                            if (transition == null)
                            {
                                ClsTerrain terrianGroup = clsTerrainTable.TerrianGroup(mapCell[m, n].GroupID);
                                mapCell[m, n].TileID = terrianGroup.TileID;
                                mapCell[m, n].AltID = altID;
                                terrianGroup = null;
                            }
                            else
                            {
                                altID = mapCell[m, n].AltID;
                                mapTile = transition.GetRandomMapTile();
                                if (mapTile == null)
                                {
                                    ClsTerrain clsTerrain = clsTerrainTable.TerrianGroup(mapCell[m, n].GroupID);
                                    mapCell[m, n].TileID = clsTerrain.TileID;
                                    mapCell[m, n].ChangeAltID((short)clsTerrain.AltID);
                                    clsTerrain = null;
                                }
                                else
                                {
                                    MapTile mapTile1 = mapTile;
                                    mapCell[m, n].TileID = mapTile1.TileID;
                                    mapCell[m, n].ChangeAltID(mapTile1.AltIDMod);
                                    mapTile1 = null;
                                }
                                transition.GetRandomStaticTiles(m, n, altID, collections, this.i_RandomStatic);
                            }
                            if (mapCell[m, n].GroupID == 254)
                            {
                                mapCell[m, n].TileID = 1078;
                                mapCell[m, n].AltID = 0;
                            }
                        }
                        catch (Exception exception6)
                        {
                            ProjectData.SetProjectError(exception6);
                            Exception exception5 = exception6;
                            loggerForm loggerForm = this.iLogger;
                            groupID = new object[] { m, n, altID, str1 };
                            loggerForm.LogMessage(string.Format("\r\nLocation: X:{0}, Y:{1}, Z:{2} Hkey:{3}", groupID));
                            this.iLogger.LogMessage(exception5.ToString());
                            ProjectData.ClearProjectError();
                            return;
                        }
                    }
                    this.mainMenu_groupBox01_progressBar.Value = m;
                }
                this.iLogger.LogTimeStamp();
                this.iLogger.LogMessage("Second Pass.");
                this.iLogger.StartTask();
                short[] altID1 = new short[9];

                RenderFacet roughEdge = new RenderFacet();

                ///RoughEdge roughEdge = new RoughEdge();

                short num14 = checked((short)width);
                for (short o = 0; o <= num14; o = checked((short)(o + 1)))
                {
                    num1 = (o != 0 ? checked(o - 1) : width);
                    num2 = (o != width ? checked(o + 1) : 0);
                    short num15 = checked((short)height);
                    for (short p = 0; p <= num15; p = checked((short)(p + 1)))
                    {
                        num4 = (p != 0 ? checked(p - 1) : height);
                        num3 = (p != height ? checked(p + 1) : 0);
                        mapCell[o, p].ChangeAltID(roughEdge.CheckCorner(mapCell[num1, num4].TileID));
                        mapCell[o, p].ChangeAltID(roughEdge.CheckLeft(mapCell[num1, p].TileID));
                        mapCell[o, p].ChangeAltID(roughEdge.CheckTop(mapCell[o, num4].TileID));
                        if (mapCell[o, p].GroupID == 20)
                        {
                            altID1[0] = mapCell[num1, num4].AltID;
                            altID1[1] = mapCell[o, num4].AltID;
                            altID1[2] = mapCell[num2, num4].AltID;
                            altID1[3] = mapCell[num1, p].AltID;
                            altID1[4] = mapCell[o, p].AltID;
                            altID1[5] = mapCell[num2, p].AltID;
                            altID1[6] = mapCell[num1, num3].AltID;
                            altID1[7] = mapCell[o, num3].AltID;
                            altID1[8] = mapCell[num2, num3].AltID;
                            Array.Sort(altID1);
                            float single = 10f * VBMath.Rnd();
                            if (single == 0f)
                            {
                                mapCell[o, p].AltID = checked((short)(checked(altID1[8] - 4)));
                            }
                            else if (single >= 1f && single <= 2f)
                            {
                                mapCell[o, p].AltID = checked((short)(checked(altID1[8] - 2)));
                            }
                            else if (single >= 3f && single <= 7f)
                            {
                                mapCell[o, p].AltID = altID1[8];
                            }
                            else if (single >= 8f && single <= 9f)
                            {
                                mapCell[o, p].AltID = checked((short)(checked(altID1[8] + 2)));
                            }
                            else if (single == 10f)
                            {
                                mapCell[o, p].AltID = checked((short)(checked(altID1[8] + 4)));
                            }
                        }

                        if (clsTerrainTable.TerrianGroup(mapCell[o, p].GroupID).RandAlt)
                        {
                            float single1 = 10f * VBMath.Rnd();
                            if (single1 == 0f)
                            {
                                mapCell[o, p].ChangeAltID(-4);
                            }
                            else if (single1 >= 1f && single1 <= 2f)
                            {
                                mapCell[o, p].ChangeAltID(-2);
                            }
                            else if (single1 >= 8f && single1 <= 9f)
                            {
                                mapCell[o, p].ChangeAltID(2);
                            }
                            else if (single1 == 10f)
                            {
                                mapCell[o, p].ChangeAltID(4);
                            }
                        }
                    }
                    this.mainMenu_groupBox01_progressBar.Value = o;
                }
                this.iLogger.LogTimeStamp();
                int num16 = 1;
                int num17 = width;
                if (num17 == 6143)
                {
                    num = 0;
                }
                else if (num17 == 2303)
                {
                    num = 2;
                }
                else if (num17 == 2559)
                {
                    num = 3;
                }
                this.iLogger.LogMessage("\r\n");
                this.iLogger.LogMessage("Load . . . . . Import Tiles.");
                this.iLogger.StartTask();
                ImportTiles importTile = new ImportTiles(collections, this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text);
                this.iLogger.LogTimeStamp();
                this.iLogger.LogMessage("\r\n");
                this.iLogger.LogMessage("Write Mul Files.");
                this.iLogger.StartTask();
                str = string.Format("{0}/Map{1}.mul", this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text, num);
                this.iLogger.LogMessage(str);
                FileStream fileStream = new FileStream(str, FileMode.Create);
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                int num18 = width;
                for (int q = 0; q <= num18; q = checked(q + 8))
                {
                    int num19 = height;
                    for (int r = 0; r <= num19; r = checked(r + 8))
                    {
                        binaryWriter.Write(num16);
                        int num20 = 0;
                        do
                        {
                            int num21 = 0;
                            do
                            {
                                mapCell[checked(q + num21), checked(r + num20)].WriteMapMul(binaryWriter);
                                num21++;
                            }
                            while (num21 <= 7);
                            num20++;
                        }
                        while (num20 <= 7);
                    }
                }
                binaryWriter.Close();
                fileStream.Close();
                str = string.Format("{0}/StaIdx{1}.mul", this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text, num);
                FileStream fileStream1 = new FileStream(str, FileMode.Create);
                this.iLogger.LogMessage(str);
                str = string.Format("{0}/Statics{1}.mul", this.mainMenu_groupBox01_createYourWorld_panel03_workBench_groupBox01_compileYourNewFacet_textBox01_projectPath.Text, num);
                FileStream fileStream2 = new FileStream(str, FileMode.Create);
                this.iLogger.LogMessage(str);
                BinaryWriter binaryWriter1 = new BinaryWriter(fileStream1);
                BinaryWriter binaryWriter2 = new BinaryWriter(fileStream2);
                int num22 = num8;
                for (int s = 0; s <= num22; s++)
                {
                    int num23 = num9;
                    for (int t = 0; t <= num23; t++)
                    {
                        int num24 = 0;
                        int position = checked((int)binaryWriter2.BaseStream.Position);
                        try
                        {
                            enumerator = collections[s, t].GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                ((StaticCell)enumerator.Current).Write(binaryWriter2);
                                num24 = checked(num24 + 7);
                            }
                        }
                        finally
                        {
                            if (enumerator is IDisposable)
                            {
                                ((IDisposable)enumerator).Dispose();
                            }
                        }
                        if (num24 == 0)
                        {
                            position = -1;
                        }
                        binaryWriter1.Write(position);
                        binaryWriter1.Write(num24);
                        binaryWriter1.Write(num16);
                    }
                }
                binaryWriter2.Close();
                binaryWriter1.Close();
                fileStream2.Close();
                fileStream1.Close();
                this.iLogger.LogTimeStamp();
                this.iLogger.LogMessage("Done.");
            }
            else
            {
                this.iLogger.LogMessage("Unable to find Transition Data files in the following path: ");
                this.iLogger.LogMessage(baseDirectory);
            }
        }

        public class RenderFacet
        {
            private Hashtable m_CornerEdge;
            private Hashtable m_LeftEdge;
            private Hashtable m_TopEdge;

            public RenderFacet()
            {
                string str;
                short num;
                IEnumerator enumerator = null;
                IEnumerator enumerator1 = null;
                IEnumerator enumerator2 = null;
                this.m_CornerEdge = new Hashtable();
                this.m_LeftEdge = new Hashtable();
                this.m_TopEdge = new Hashtable();
                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    #region Data Directory Modification

                    str = string.Format("{0}Data\\Engine\\RoughEdge\\Corner.xml", AppDomain.CurrentDomain.BaseDirectory);

                    #endregion

                    xmlDocument.Load(str);
                    try
                    {
                        enumerator2 = xmlDocument.SelectNodes("//Terrains/Corner").GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            XmlElement current = (XmlElement)enumerator2.Current;
                            num = XmlConvert.ToInt16(current.GetAttribute("TileID"));
                            this.m_CornerEdge.Add(num, num);
                        }
                    }
                    finally
                    {
                        if (enumerator2 is IDisposable)
                        {
                            ((IDisposable)enumerator2).Dispose();
                        }
                    }
                }
                catch (Exception exception)
                {
                    ProjectData.SetProjectError(exception);
                    Interaction.MsgBox(exception.Message, MsgBoxStyle.OkOnly, null);
                    ProjectData.ClearProjectError();
                }
                try
                {
                    #region Data Directory Modification

                    str = string.Format("{0}Data\\Engine\\RoughEdge\\Left.xml", AppDomain.CurrentDomain.BaseDirectory);

                    #endregion

                    xmlDocument.Load(str);
                    try
                    {
                        enumerator1 = xmlDocument.SelectNodes("//Terrains/Left").GetEnumerator();
                        while (enumerator1.MoveNext())
                        {
                            XmlElement xmlElement = (XmlElement)enumerator1.Current;
                            num = XmlConvert.ToInt16(xmlElement.GetAttribute("TileID"));
                            this.m_LeftEdge.Add(num, num);
                        }
                    }
                    finally
                    {
                        if (enumerator1 is IDisposable)
                        {
                            ((IDisposable)enumerator1).Dispose();
                        }
                    }
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Interaction.MsgBox(exception1.Message, MsgBoxStyle.OkOnly, null);
                    ProjectData.ClearProjectError();
                }
                try
                {
                    #region Data Directory Modification

                    str = string.Format("{0}Data\\Engine\\RoughEdge\\Top.xml", AppDomain.CurrentDomain.BaseDirectory);

                    #endregion

                    xmlDocument.Load(str);
                    try
                    {
                        enumerator = xmlDocument.SelectNodes("//Terrains/Top").GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            XmlElement current1 = (XmlElement)enumerator.Current;
                            num = XmlConvert.ToInt16(current1.GetAttribute("TileID"));
                            this.m_TopEdge.Add(num, num);
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable)
                        {
                            ((IDisposable)enumerator).Dispose();
                        }
                    }
                }
                catch (Exception exception2)
                {
                    ProjectData.SetProjectError(exception2);
                    Interaction.MsgBox(exception2.Message, MsgBoxStyle.OkOnly, null);
                    ProjectData.ClearProjectError();
                }
            }

            public short CheckCorner(short TileID)
            {
                //short num;
                //num = (this.m_CornerEdge[TileID] != null ? -5 : 0);
                //return num;

                return this.m_CornerEdge[(object)TileID] == null ? (short)0 : (short)-5;
            }

            public short CheckLeft(short TileID)
            {
                short num = 0;
                if (this.m_LeftEdge[TileID] != null)
                {
                    VBMath.Randomize();
                    float single = VBMath.Rnd() * 15f;
                    if (single == 0f)
                    {
                        num = -4;
                    }
                    else if (single >= 1f && single <= 3f)
                    {
                        num = -3;
                    }
                    else if (single >= 4f && single <= 8f)
                    {
                        num = -2;
                    }
                    else if (single == 9f)
                    {
                        num = -1;
                    }
                    else if (single == 10f)
                    {
                        num = 0;
                    }
                    else if (single >= 11f && single <= 13f)
                    {
                        num = 1;
                    }
                    else if (single == 14f)
                    {
                        num = 2;
                    }
                    else if (single == 15f)
                    {
                        num = 3;
                    }
                }
                else
                {
                    num = 0;
                }
                return num;
            }

            public short CheckTop(short TileID)
            {
                short num = 0;
                if (this.m_TopEdge[TileID] != null)
                {
                    VBMath.Randomize();
                    float single = VBMath.Rnd() * 15f;
                    if (single == 0f)
                    {
                        num = -4;
                    }
                    else if (single >= 1f && single <= 3f)
                    {
                        num = -3;
                    }
                    else if (single >= 4f && single <= 8f)
                    {
                        num = -2;
                    }
                    else if (single == 9f)
                    {
                        num = -1;
                    }
                    else if (single == 10f)
                    {
                        num = 0;
                    }
                    else if (single >= 11f && single <= 13f)
                    {
                        num = 1;
                    }
                    else if (single == 14f)
                    {
                        num = 2;
                    }
                    else if (single == 15f)
                    {
                        num = 3;
                    }
                }
                else
                {
                    num = 0;
                }
                return num;
            }
        }

        #endregion

        private void mainMenu_statusStrip01_linkLabel01_licenseType_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.gnu.org/licenses/old-licenses/lgpl-2.0.html");
            Process.Start(sInfo);
        }
    }
}