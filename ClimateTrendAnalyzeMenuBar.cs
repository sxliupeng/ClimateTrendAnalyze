using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClimateTrendAnalyze
{
    class ClimateTrendAnalyzeMenuBar : MyPluginEngine.IMenuDef
    {
        #region IMenuDef 成员
        public string Caption
        {
            get { return "趋势预测"; }
        }

        public string Name
        {
            get { return "ClimateTrendAnalyzeMenu"; }
        }

        public long ItemCount
        {
            get { return 2; }
        }

        public void GetItemInfo(int pos, MyPluginEngine.ItemDef itemDef)
        {
            switch (pos)
            {
                case 0:
                    itemDef.ID = "ClimateTrendAnalyze.frmTEMPCmd";
                    itemDef.Group = false;
                    break;
                case 1:
                    itemDef.ID = "ClimateTrendAnalyze.frmPRECmd";
                    itemDef.Group = false;
                    break;
                //case 2:
                //    itemDef.ID = "ClimateTrendAnalyze.frmTableCmd";
                //    itemDef.Group = false;
                //    break;
                default:
                    break;

            }
        }
        #endregion
    }
}
