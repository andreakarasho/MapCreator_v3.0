using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MapEngine.Altitude;
using MapEngine.ImportTiles;
using MapEngine.Logger;
using MapEngine.Terrain;
using MapEngine.Transition;
using MapEngine.UltimaSDK;

namespace MapCreator
{
    public partial class titleScreen : Form
    {
        public titleScreen()
        {
            InitializeComponent();
        }

        private void titleScreen_pictureBox01_splashImage_MouseClick(object sender, MouseEventArgs e)
        {
            mainMenu mainMenuForm = new mainMenu();
            mainMenuForm.Show();

            this.Hide();
        }
    }
}
