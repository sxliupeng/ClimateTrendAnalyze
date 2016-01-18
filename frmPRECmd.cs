using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MyPluginEngine;

namespace ClimateTrendAnalyze
{
    class frmPRECmd : MyPluginEngine.ICommand
    {
        private MyPluginEngine.IApplication hk;
        private System.Drawing.Bitmap m_hBitmap;
        private ESRI.ArcGIS.SystemUI.ICommand cmd = null;
        frmPRE pfrmPOP;

        public frmPRECmd()
        {
            string str = @"..\Data\Image\TrendAnalyze\People.png";
            if (System.IO.File.Exists(str))
                m_hBitmap = new Bitmap(str);
            else
                m_hBitmap = null;
        }
        #region ICommand 成员
        public System.Drawing.Bitmap Bitmap
        {
            get { return m_hBitmap; }
        }

        public string Caption
        {
            get { return "人口增长趋势"; }
        }

        public string Category
        {
            get { return "TrendAnalyzeMenu"; }
        }

        public bool Checked
        {
            get { return false; }
        }

        public bool Enabled
        {
            get { return true; }
        }

        public int HelpContextId
        {
            get { return 0; }
        }

        public string HelpFile
        {
            get { return ""; }
        }

        public string Message
        {
            get { return "人口增长趋势"; }
        }

        public string Name
        {
            get { return "frmPOP"; }
        }

        public void OnClick()
        {
            //System.Windows.Forms.MessageBox.Show("模块正在开发中！");
            pfrmPOP = new frmPRE();
            pfrmPOP.ShowDialog();
        }

        public void OnCreate(IApplication hook)
        {
            if (hook != null)
            {
                this.hk = hook;
                pfrmPOP = new frmPRE();
                pfrmPOP.Visible = false;
            }
        }

        public string Tooltip
        {
            get { return "人口增长趋势"; }
        }
        #endregion
    }
}
