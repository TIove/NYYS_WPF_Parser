using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net;
using System.IO;
using System.Globalization;

namespace WpfApp4
{
    public class Program
    {
        private void IterationOfRowParse()
        {

        }
        public static void ParseExcel(ref int numRowsOnPage, ref int maxPages, ref bool isParsed, List<Danger> dangers, List<string> changedDangers)
        {
            changedDangers.Clear();
            List<Danger> OldDangers = new List<Danger>();
            OldDangers.AddRange(dangers);
            dangers.Clear();
            string FileName = $"{Environment.CurrentDirectory}\\thrlist.xlsx";
            if (!(new FileInfo(FileName).Exists))
                throw new FileNotFoundException();
            object rOnly = true;
            object SaveChanges = false;
            object MissingObj = System.Reflection.Missing.Value;

            Excel.Application app = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Sheets sheets = null;
            try
            {
                workbooks = app.Workbooks;
                workbook = workbooks.Open(FileName, MissingObj, rOnly, MissingObj, MissingObj,
                                            MissingObj, MissingObj, MissingObj, MissingObj, MissingObj,
                                            MissingObj, MissingObj, MissingObj, MissingObj, MissingObj);

                sheets = workbook.Sheets;

                foreach (Excel.Worksheet worksheet in sheets)
                {
                    DateTime dateTimeNow = DateTime.Now;
                    
                    Excel.Range UsedRange = worksheet.UsedRange;
                   
                    Excel.Range urRows = UsedRange.Rows;
                    
                    Excel.Range urColums = UsedRange.Columns;

                    int RowsCount = urRows.Count;
                    int ColumnsCount = urColums.Count;
                    maxPages = RowsCount / numRowsOnPage + 1;

                    for (int i = 3; i <= RowsCount; i++)
                    {
                        string[] str = new string[ColumnsCount];
                        for (int j = 1; j <= ColumnsCount; j++)
                        {
                            Excel.Range CellRange = UsedRange.Cells[i, j];
                            
                            string CellText = (CellRange == null || CellRange.Value2 == null) ? null : CellRange.Value.ToString();

                            if (CellText != null)
                            {
                                str[j - 1] = CellText;
                            }
                        }
                        Danger danger = new Danger(Int32.Parse(str[0]), str[1], str[2], str[3], str[4], str[5] == "1" ? true : false, str[6] == "1" ? true : false, str[7] == "1" ? true : false, DateTime.Parse(str[9]));

                        if (isParsed && !OldDangers.Contains(danger)) {
                            bool isNewRecord = true;
                            foreach (var item in OldDangers)
                            {
                                if (item.ID == danger.ID)
                                {
                                    isNewRecord = false;
                                    changedDangers.Add(item.GetChangedFields(danger));
                                }
                            }
                            if (isNewRecord) 
                            {
                                changedDangers.Add(danger.ToString()); 
                            }
                        }

                        dangers.Add(danger);
                    }
                    isParsed = true;

                    if (urRows != null) Marshal.ReleaseComObject(urRows);
                    if (urColums != null) Marshal.ReleaseComObject(urColums);
                    if (UsedRange != null) Marshal.ReleaseComObject(UsedRange);
                    if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                }
            }
            catch (Exception)
            {
                
                maxPages = 0;
            }
            finally
            {
                if (sheets != null) Marshal.ReleaseComObject(sheets);
                if (workbook != null)
                {
                    workbook.Close(SaveChanges);
                    Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }

                if (workbooks != null)
                {
                    workbooks.Close();
                    Marshal.ReleaseComObject(workbooks);
                    workbooks = null;
                }
                if (app != null)
                {
                    app.Quit();
                    Marshal.ReleaseComObject(app);
                    app = null;
                }
                
            }
        }

        public static List<Danger> UpdateDataGridAccordingToPage(int numPage, ref int numRowsOnPage, List<Danger> dangers)
        {
            List<Danger> showableDangers = new List<Danger>();
            
            for (int i = (numPage - 1) * numRowsOnPage; i < numPage * numRowsOnPage && i < dangers.Count; i++)
            {
                showableDangers.Add(dangers[i]);
            }
            return showableDangers;
        }

        public static void DownloadDataFromInternet()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx",
                                    $"{Environment.CurrentDirectory}\\thrlist.xlsx");
                } catch (WebException)
                {
                    throw new WebException();
                }
                
            }
        }
    }
}
