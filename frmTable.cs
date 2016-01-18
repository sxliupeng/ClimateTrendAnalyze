using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace ClimateTrendAnalyze
{
    public partial class frmTable : DevComponents.DotNetBar.OfficeForm
    {
        public frmTable()
        {
            InitializeComponent();
        }


        public frmTable(DataTable dt,string xName,string yName)
        {
            InitializeComponent();
            //禁用Glass主题
            this.EnableGlass = false;
            //不显示最大化最小化按钮
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //去除图标
            this.ShowIcon = false;

            this.dgvData.ReadOnly = false;
            //去掉第一列(OID)
            for(int i=0;i<dt.Columns.Count;i++)
            {
                if(dt.Columns[i].ColumnName.ToUpper()=="OID")
                    dt.Columns.RemoveAt(i);
            }
            
            DrawOverLayBar1(dt,xName,yName);
        }

        private void DrawOverLayBar1(DataTable _dt, string _xName, string _yName)
        {
            //行数
            int iRowCount = _dt.Rows.Count;
            //列数 去除第一列 分区名称
            int iColCount = _dt.Columns.Count - 1;

            double[,] value = new double[ iRowCount,iColCount];
            string[] labels = new string[iRowCount];
            string[] sColorNameArr = { "Blue", "BlueViolet", "Brown", "BurlyWood", "CadetBlue", "Chartreuse", "Chocolate", "Coral", "CornflowerBlue", "Cornsilk", "Crimson", "Cyan", "DarkBlue", "DarkCyan", "DarkGoldenrod", "DarkGray", "DarkGreen", "DarkKhaki", "DarkMagenta", "DarkOliveGreen", "DarkOrange", "DarkOrchid", "DarkRed", "DarkSalmon", "DarkSeaGreen", "DarkSlateBlue", "DarkSlateGray", "DarkTurquoise", "DarkViolet", "DeepPink", "DeepSkyBlue", "DimGray", "DodgerBlue", "Firebrick", "FloralWhite", "ForestGreen", "Fuchsia", "Gainsboro", "GhostWhite", "Gold", "Goldenrod", "Gray", "Green", "GreenYellow", "Honeydew", "HotPink", "IndianRed", "Indigo", "LightGray", "LightGreen", "LightPink", "LightSalmon", "LightSeaGreen", "LightSkyBlue", "LightSlateGray", "LightSteelBlue", "LightYellow", "Lime", "LimeGreen", "Linen", "Magenta", "Maroon", "MediumAquamarine", "MediumBlue", "MediumOrchid", "MediumPurple", "MediumSeaGreen", "MediumSlateBlue", "MediumSpringGreen", "MediumTurquoise", "MediumVioletRed", "MidnightBlue", "MintCream", "MistyRose", "Moccasin", "NavajoWhite", "Navy", "OldLace", "Olive", "OliveDrab", "Orange", "OrangeRed", "Orchid", "PaleGoldenrod", "PaleGreen", "PaleTurquoise", "PaleVioletRed", "PapayaWhip", "PeachPuff", "Peru", "Pink", "Plum", "PowderBlue", "Purple", "Red", "RosyBrown", "RoyalBlue", "SaddleBrown", "Salmon", "SandyBrown", "SeaGreen", "SeaShell", "Sienna", "Silver", "SkyBlue", "SlateBlue", "SlateGray", "Snow", "SpringGreen", "SteelBlue", "Tan", "Teal", "Thistle", "Tomato", "Turquoise", "Violet", "Wheat", "White", "WhiteSmoke", "Yellow", "YellowGreen" };

            //分区名称
            for (int i = 0; i < iRowCount; i++)
            {
                labels[i] = _dt.Rows[i][0].ToString();
            }
            //数据区域
            for (int i = 0; i < iRowCount; i++)
            {
                for (int j = 0; j < iColCount; j++)
                {
                    value[i,j] = Convert.ToDouble(_dt.Rows[i][j + 1].ToString()) / 10000000;
                    _dt.Rows[i][j + 1] = Convert.ToDouble(_dt.Rows[i][j + 1].ToString()) / 10000000;
                }
            }

            this.dgvData.DataSource = _dt;

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = _xName + "&" + _yName + "面积柱状图";
            myPane.YAxis.Title.Text = _yName + "(Km2)";
            myPane.XAxis.Title.Text = _xName;


            double[] list = null;
            list=new double[iRowCount];
            BarItem myBar = null;

            for (int i = 0; i < iColCount; i++)
            {
                for (int j = 0; j < iRowCount; j++)
                {
                    list[j] = value[j,i];
                }
                myBar = myPane.AddBar(_dt.Columns[i+1].ColumnName,null, list, Color.FromName(sColorNameArr[i]));
                myBar.Bar.Fill = new Fill( Color.FromName(sColorNameArr[i]));
            }

            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White,
                Color.FromArgb(255, 255, 166), 45.0F);

            myPane.AxisChange();

            myPane.IsFontsScaled = true;
            //myPane.XAxis.MajorTic.IsBetweenLabels = true;

            // Set the XAxis labels
            myPane.XAxis.Scale.TextLabels = labels;
            // Set the XAxis to Text type
            myPane.XAxis.Type = AxisType.Text;
            // expand the range of the Y axis slightly to accommodate the labels
            myPane.YAxis.Scale.Max += myPane.YAxis.Scale.MajorStep;

            // Create TextObj's to provide labels for each bar
            BarItem.CreateBarLabels(myPane, false, "f0");
        }
        private void DrawOverLayBar(DataTable _dt,string _xName,string _yName)
        {
            GraphPane myPane =zedGraphControl1.GraphPane;

            // Set the title and axis label
            myPane.Title.Text =_xName+"&"+_yName+ "柱状堆积图";
            myPane.YAxis.Title.Text =_yName+ "(Km2)";
            myPane.XAxis.Title.Text = _xName;
            // Enter some data values
            int iRowCount=_dt.Rows.Count;
            int iColCount=_dt.Columns.Count-1;

            double[,] value=new double[iColCount,iRowCount];
            string[] labels=new string[iRowCount];


            for (int i = 0; i < iRowCount; i++)
            {
                labels[i] = _dt.Rows[i][0].ToString();
            }

            for (int i = 0; i < iRowCount; i++)
            {
                for (int j = 0; j < iColCount; j++)
                {
                    value[j, i] = Convert.ToDouble(_dt.Rows[i][j + 1].ToString()) / 10000000;
                }
            }

            // Manually sum up the curves
            for (int i = 0; i < iRowCount; i++)
            {
                for(int j=0;j<iColCount-1;j++)
                {
                    value[j+1,i]+= value[j,i];
                }
            }


            // Generate a red bar with "Curve 1" in the legend
            int per=255/iColCount;
            double[] yy = new double[iRowCount];
            string[]  sColorNameArr={"Blue","BlueViolet","Brown","BurlyWood","CadetBlue","Chartreuse","Chocolate","Coral","CornflowerBlue","Cornsilk","Crimson","Cyan","DarkBlue","DarkCyan","DarkGoldenrod","DarkGray","DarkGreen","DarkKhaki","DarkMagenta","DarkOliveGreen","DarkOrange","DarkOrchid","DarkRed","DarkSalmon","DarkSeaGreen","DarkSlateBlue","DarkSlateGray","DarkTurquoise","DarkViolet","DeepPink","DeepSkyBlue","DimGray","DodgerBlue","Firebrick","FloralWhite","ForestGreen","Fuchsia","Gainsboro","GhostWhite","Gold","Goldenrod","Gray","Green","GreenYellow","Honeydew","HotPink","IndianRed","Indigo","LightGray","LightGreen","LightPink","LightSalmon","LightSeaGreen","LightSkyBlue","LightSlateGray","LightSteelBlue","LightYellow","Lime","LimeGreen","Linen","Magenta","Maroon","MediumAquamarine","MediumBlue","MediumOrchid","MediumPurple","MediumSeaGreen","MediumSlateBlue","MediumSpringGreen","MediumTurquoise","MediumVioletRed","MidnightBlue","MintCream","MistyRose","Moccasin","NavajoWhite","Navy","OldLace","Olive","OliveDrab","Orange","OrangeRed","Orchid","PaleGoldenrod","PaleGreen","PaleTurquoise","PaleVioletRed","PapayaWhip","PeachPuff","Peru","Pink","Plum","PowderBlue","Purple","Red","RosyBrown","RoyalBlue","SaddleBrown","Salmon","SandyBrown","SeaGreen","SeaShell","Sienna","Silver","SkyBlue","SlateBlue","SlateGray","Snow","SpringGreen","SteelBlue","Tan","Teal","Thistle","Tomato","Turquoise","Violet","Wheat","White","WhiteSmoke","Yellow","YellowGreen"};
            //===========================
            BarItem bar=null;
            int k = 0;
            for(int j=0;j<iColCount;j++)
            {
                for (int i = 0; i < iRowCount; i++)
                {
                    yy[i] = value[j, i];
                }
                // getColor(0, j * per, 0)
                bar = myPane.AddBar(_dt.Columns[j + 1].ColumnName, null, yy,Color.YellowGreen);
                if (j < sColorNameArr.Length)
                    k = j;
                else
                    k = j-sColorNameArr.Length;

                bar.Bar.Fill = new Fill(Color.FromName(sColorNameArr[k]), Color.WhiteSmoke, Color.FromName(sColorNameArr[k]));
            }
            //============================

            // Draw the X tics between the labels instead of at the labels
            myPane.XAxis.MajorTic.IsBetweenLabels = true;

            // Set the XAxis to the ordinal type
            myPane.XAxis.Type = AxisType.Ordinal;

            //Add Labels to the curves

            // Shift the text items up by 5 user scale units above the bars
            const float shift = 5;

            for (int i = 0; i <iRowCount; i++)
            {
                // format the label string to have 1 decimal place
                string lab = value[iColCount-1,i].ToString("F1");
                // create the text item (assumes the x axis is ordinal or text)
                // for negative bars, the label appears just above the zero value
                TextObj text = new TextObj(lab, (float)(i + 1), (float)(yy[i] < 0 ? 0.0 : value[iColCount-1, i]) + shift);
                // tell Zedgraph to use user scale units for locating the TextObj
                text.Location.CoordinateFrame = CoordType.AxisXYScale;
                // AlignH the left-center of the text to the specified point
                text.Location.AlignH = AlignH.Left;
                text.Location.AlignV = AlignV.Center;
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.Fill.IsVisible = false;
                // rotate the text 90 degrees
                text.FontSpec.Angle = 90;
                // add the TextObj to the list
                myPane.GraphObjList.Add(text);
            }

            myPane.XAxis.MajorTic.IsBetweenLabels = true;

            // Set the XAxis to Text type
            myPane.XAxis.Type = AxisType.Text;
            // Set the XAxis labels
            myPane.XAxis.Scale.TextLabels = labels;
            myPane.XAxis.Scale.FontSpec.Size = 10.0F;

            // Make the bars a sorted overlay type so that they are drawn on top of eachother
            // (without summing), and each stack is sorted so the shorter bars are in front
            // of the taller bars
            myPane.BarSettings.Type = BarType.SortedOverlay;

            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White,Color.White,45.0f);

            zedGraphControl1.AxisChange();
            //base.ZedGraphControl.AxisChange();

            // Add one step to the max scale value to leave room for the labels
            myPane.YAxis.Scale.Max += myPane.YAxis.Scale.MajorStep;
        }
    }
}