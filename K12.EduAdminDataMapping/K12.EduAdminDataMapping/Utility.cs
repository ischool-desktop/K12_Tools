﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K12.EduAdminDataMapping.DAO;
using Aspose.Cells;
using System.Windows.Forms;
using System.IO;
using FISCA.Presentation.Controls;

namespace K12.EduAdminDataMapping
{
    public class Utility
    {
        /// <summary>
        /// 取得國籍中英文對照,回傳中文,英文
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetNationalityMappingDict()
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            List<UDT_NationalityMapping> dataList = UDTTransfer.UDTNationalityMappingSelectAll();
            foreach (UDT_NationalityMapping data in dataList)
            {
                if (!retVal.ContainsKey(data.Name))
                    retVal.Add(data.Name, data.Eng_Name);
            }
            return retVal;
        }

        public static void CompletedXls(string inputReportName, Workbook inputXls)
        {
            string reportName = inputReportName;

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xls");

            Workbook wb = inputXls;

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                wb.Save(path, Aspose.Cells.FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".xls";
                sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName, Aspose.Cells.FileFormatType.Excel2003);

                    }
                    catch
                    {
                        MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
    }
}
