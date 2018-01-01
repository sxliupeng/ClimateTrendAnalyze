using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using System.Data.OleDb;
using System.Configuration;
using System.Threading;

namespace ClimateTrendAnalyze
{
    public partial class frmTEMP : DevComponents.DotNetBar.OfficeForm
    {
        private string sConn = null;
        private OleDbConnection pConn = null;
        private DataTable dt = null;

        private PointPairList list1 = null, list2 = null, list3 = null, list4 = null;
        public frmTEMP()
        {
            InitializeComponent();
            this.dgDataSource.ReadOnly = true;
            this.dgDataSource.AllowUserToAddRows = false;
            //禁用Glass主题
            this.EnableGlass = false;
            //不显示最大化最小化按钮
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //去除图标
            this.ShowIcon = false;
            //模拟的按钮默认false，以免没点开始直接点模拟按钮
            btnSimulate.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            CreateMutiLineChart();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pConn.State == ConnectionState.Open)
                pConn.Close();
            if (pConn != null)
                pConn = null;

        }

        private void LoadData()
        {
            //sConn = ConfigurationSettings.AppSettings["Provider"];
            //sConn += Application.StartupPath;
            //sConn += ConfigurationSettings.AppSettings["DataSource"];
            //sConn += ConfigurationSettings.AppSettings["Pwd"];
            sConn = @"Provider = Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + @"\Data\IndustrialDV.mdb;Jet OLEDB:Database Password=";
            if (pConn == null)
                pConn = new OleDbConnection(sConn);
            if (pConn.State == ConnectionState.Closed)
                pConn.Open();
            OleDbCommand cmd = pConn.CreateCommand();
            cmd.CommandText = "Select id,years,rcp2,rcp4,rcp6,rcp8 From temperature";
            OleDbDataAdapter oda = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            oda.Fill(ds, "temperature");
            if (dt == null)
                dt = new DataTable();
            dt = ds.Tables["temperature"];
        }
        private void CreateMutiLineChart()
        {
            dgDataSource.DataSource = dt;

            int iRow = dt.Rows.Count;

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();
            myPane.GraphObjList.Clear();
            // Set up the title and axis labels
            myPane.Title.Text = "温度变化趋势分析";
            myPane.XAxis.Title.Text = "年份";
            myPane.YAxis.Title.Text = "温度值";

            //PointPairList list1 = null, list3 = null, list2 = null, list = null;
            // Make up some data arrays based on the Sine function
            list1 = new PointPairList();
            list3 = new PointPairList();
            list2 = new PointPairList();
            list4 = new PointPairList();

            //double x = double.Parse(dt.Rows[0]["year"].ToString().Trim());
            //double y1 = double.Parse(dt.Rows[0]["gdp1"].ToString().Trim());
            //double y3 = double.Parse(dt.Rows[0]["gdp3"].ToString().Trim());
            //double y2 = double.Parse(dt.Rows[0]["gdp2"].ToString().Trim());
            //double y = double.Parse(dt.Rows[0]["gdp"].ToString().Trim());
            //list.Add(x, y);
            //list1.Add( x, y1);
            //list2.Add( x, y2 );
            //list3.Add(x, y3);

            LineItem myCurve1 = null, myCurve2 = null, myCurve3 = null, myCurve4 = null;

            // Generate a red curve with diamond
            // symbols, and "GDP1" in the legend
            myCurve1 = myPane.AddCurve("rcp2.6",
                list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            // symbols, and "GDP2" in the legend
            myCurve2 = myPane.AddCurve("rcp4.5",
                list2, Color.Blue, SymbolType.Circle);

            // Generate a blue curve with circle
            // symbols, and "GDP3" in the legend
            myCurve3 = myPane.AddCurve("rcp6.0",
                list3, Color.Green, SymbolType.Star);

            // Generate a blue curve with circle
            // symbols, and "GDP" in the legend
            myCurve4 = myPane.AddCurve("rcp8.5",
                list4, Color.Orange, SymbolType.Square);


            // Change the color of the title
            myPane.Title.FontSpec.FontColor = Color.Green;

            // Add gridlines to the plot, and make them gray
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.LightGray;
            myPane.YAxis.MajorGrid.Color = Color.LightGray;

            // Move the legend location
            myPane.Legend.Position = ZedGraph.LegendPos.Bottom;

            // Make both curves thicker
            myCurve1.Line.Width = 1.0F;
            myCurve2.Line.Width = 1.0F;
            myCurve3.Line.Width = 1.0F;
            myCurve4.Line.Width = 1.0F;

            // Fill the area under the curves

            //myCurve1.Line.Fill = new Fill(Color.White, Color.Red, 45F );
            //myCurve2.Line.Fill = new Fill( Color.White, Color.Blue, 45F );
            //myCurve3.Line.Fill = new Fill(Color.White, Color.Green, 45F);

            // Increase the symbol sizes, and fill them with solid white
            myCurve1.Symbol.Size = 2.0F;
            myCurve2.Symbol.Size = 2.0F;
            myCurve3.Symbol.Size = 2.0F;
            myCurve4.Symbol.Size = 2.0F;

            myCurve1.Symbol.Fill = new Fill(Color.White);
            myCurve2.Symbol.Fill = new Fill(Color.White);
            myCurve3.Symbol.Fill = new Fill(Color.White);
            myCurve4.Symbol.Fill = new Fill(Color.White);

            // Add a background gradient fill to the axis frame
            myPane.Chart.Fill = new Fill(Color.White,
                Color.FromArgb(255, 255, 210), -45F);

            // Add a caption and an arrow
            TextObj myText = new TextObj("Interesting\nPoint", 230F, 70F);
            myText.FontSpec.FontColor = Color.Red;
            myText.Location.AlignH = AlignH.Center;
            myText.Location.AlignV = AlignV.Top;
            myPane.GraphObjList.Add(myText);
            ArrowObj myArrow = new ArrowObj(Color.Red, 12F, 230F, 70F, 280F, 55F);
            myPane.GraphObjList.Add(myArrow);

            myPane.AxisChange();
            zedGraphControl1.Refresh();
        }

        private void loadHistoryData()
        {
            //加载数据库的原始数据
            while (list1.Count > 1)
            {
                list1.RemoveAt(1);
                list2.RemoveAt(1);
                list3.RemoveAt(1);
                list4.RemoveAt(1);
            }


            for (int i = 0; i < 16; i++)
            {
                Thread.Sleep(100);

                //创建曲线图

                double x = double.Parse(dt.Rows[i]["years"].ToString().Trim());
                double y1 = double.Parse(dt.Rows[i]["rcp2"].ToString().Trim());
                double y2 = double.Parse(dt.Rows[i]["rcp4"].ToString().Trim());
                double y3 = double.Parse(dt.Rows[i]["rcp6"].ToString().Trim());
                double y4 = double.Parse(dt.Rows[i]["rcp8"].ToString().Trim());
                list1.Add(x, y1);
                list2.Add(x, y2);
                list3.Add(x, y3);
                list4.Add(x, y4);
                zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;

                this.zedGraphControl1.AxisChange();
                this.zedGraphControl1.Refresh();
            }
        }

        private void loadSimulationData()
        {
            //加载数据库的原始数据
            //id=1-17	year=1996-2012
            int iRowCount = dt.Rows.Count - 1;
            if (iRowCount > 16)
            {
                for (int i = iRowCount; i >= 16; i--)
                {
                    dt.Rows.RemoveAt(i);
                    list1.RemoveAt(i);
                    list2.RemoveAt(i);
                    list3.RemoveAt(i);
                    list4.RemoveAt(i);
                }
            }
            ////修改数据源dt中的值
            ////id	year	gdp1	gdp2	gdp3	pop1	pop2	pop3	gdp	pop
            //double dRate = 0;
            //try
            //{
            //    dRate = double.Parse(txtIncreaseRate.Text.Trim()) / 100;
            //}
            //catch
            //{
            //    MessageBox.Show("增长率必须是数值！");
            //    return;
            //}

            //模拟结束年份
            Int32 iEndYear = (Int32)txtEndYear.Value;
            double year;
            for (int i = 0; i < iEndYear - 2015 + 1; i++)
            {
                Thread.Sleep(200);
                //产生模拟数据
                int iRowMaxIndex = dt.Rows.Count - 1;
                DataRow dr = dt.NewRow();
                year = double.Parse(dt.Rows[iRowMaxIndex]["years"].ToString().Trim());

                dr["id"] = int.Parse(dt.Rows[iRowMaxIndex]["id"].ToString().Trim()) + 1;
                dr["years"] = int.Parse(dt.Rows[iRowMaxIndex]["years"].ToString().Trim()) + 1;
                //年均温表达式如下
                //rcp2.6: y = 144.18ln(x) - 1091.1
                //rcp4.5：y = 119.27ln(x) - 901.75
                //rcp6.0：y = 100.13ln(x) - 756.2
                //rcp8.5：y = 99.874ln(x) - 754.25              
                dr["rcp2"] = 144.18 * Math.Log(year) - 1091.1;
                dr["rcp4"] = 119.27 * Math.Log(year) - 901.75;
                dr["rcp6"] = 100.13 * Math.Log(year) - 756.2;
                dr["rcp8"] = 99.874 * Math.Log(year) - 754.25;
                dt.Rows.Add(dr);
                //创建曲线图

                double x = double.Parse(dt.Rows[iRowMaxIndex]["years"].ToString().Trim());
                double y1 = double.Parse(dt.Rows[iRowMaxIndex]["rcp2"].ToString().Trim());
                double y2 = double.Parse(dt.Rows[iRowMaxIndex]["rcp4"].ToString().Trim());
                double y3 = double.Parse(dt.Rows[iRowMaxIndex]["rcp6"].ToString().Trim());
                double y4 = double.Parse(dt.Rows[iRowMaxIndex]["rcp8"].ToString().Trim());
                list1.Add(x, y1);
                list2.Add(x, y2);
                list3.Add(x, y3);
                list4.Add(x, y4);
                zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;

                this.zedGraphControl1.AxisChange();
                this.zedGraphControl1.Refresh();

                this.dgDataSource.DataSource = dt;
                this.dgDataSource.Refresh();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            loadHistoryData();
            btnSimulate.Enabled = true;
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            loadSimulationData();
            btnStart.Enabled = false;           //把开始按钮false掉，以免再点开始出现BUG。
        }
    }
}