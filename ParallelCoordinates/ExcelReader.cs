using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel;
using System.IO;

namespace DataReader
{
    public class Pair<T1, T2>
    {
        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        public T1 First { get; set; }
        public T2 Second { get; set; }
    }

    public class DataEntry
    {
        public DataEntry(string ColumnName)
        {
            this.ColumnName = ColumnName;
            Data = new List<string>();
            DisplayFilter = new Pair<double, double>(-1, -1);
            NumberRange = new Pair<double, double>(-1, -1);
            AllNumbers = true;
            UniquEntries = -1;
            ContainsEmptyEntry = false;
            YPlacements = new Dictionary<string, int>();
        }

        public string ColumnName;
        public List<string> Data;

        public bool ContainsEmptyEntry;
        public int UniquEntries;

        public bool AllNumbers;
        public Pair<double, double> NumberRange;

        public bool Filtered;
        public Pair<double, double> DisplayFilter;

        public Dictionary<string, int> YPlacements { get; set; }
    }

    public class ExcelReader
    {
        public List<DataEntry> ds;

        public ExcelReader(string fileName)
        {
            ds = new List<DataEntry>();

            if (fileName.IndexOf(".txt") != -1)
            {
                readFromText(fileName);
            }
            else
            {
                readFromExcel(fileName);
            }
        }

        void readFromText(string fileName)
        {
            string[] dataFile = File.ReadAllLines(fileName);

            foreach (var columnName in dataFile[0].Trim().Split('\t'))
            {
                ds.Add(new DataEntry(columnName));
            }

            for (int i = 1; i < dataFile.Length; ++i)
            {
                string[] entries = dataFile[i].Trim().Replace("\t\t", "\t[Not Available]\t").Replace("\t\t", "\t[Not Available]\t").Split('\t');

                if (entries.Length == 0)
                {
                    continue;
                }

                for (int j = 0; j < entries.Length; ++j)
                {
                    string valueToAdd = entries[j].Replace(",", "").Trim() == "" || entries[j] == null ? "[Not Available]" : entries[j].Replace(",", "").Trim();
                    ds[j].Data.Add(valueToAdd != "-0" ? valueToAdd : "0");

                    if (ds[j].Data.Last()[0] == '[')
                    {
                        ds[j].ContainsEmptyEntry = true;
                    }

                    double parseValue;
                    valueToAdd = valueToAdd.Replace("$", "").Trim();

                    ds[j].AllNumbers = ds[j].AllNumbers && (double.TryParse(valueToAdd, out parseValue) || valueToAdd == "" || (valueToAdd[0] == '[' && valueToAdd[valueToAdd.Length - 1] == ']'));
                }
            }

            parseData();
        }

        void readFromExcel(string fileName)
        {
            int worksheetItter = 0;
            foreach (var worksheet in Workbook.Worksheets(fileName))
            {
                if (worksheetItter != 0)
                {
                    break;
                }

                // Retrieve column names
                for (int i = 0; i < worksheet.Rows.First().Cells.Length; ++i)
                {
                    // worksheet.Rows[0].Cells.Length provides an inconsistant length
                    // so this method of itteration condition is used
                    try
                    {
                        if (worksheet.Rows.First().Cells[i] == null)
                        {
                            break;
                        }

                        ds.Add(new DataEntry(worksheet.Rows.First().Cells[i].Text.Trim()));
                    }
                    catch (IndexOutOfRangeException)
                    {
                        break;
                    }
                }

                // Fill column data
                bool first = true;
                foreach (var row in worksheet.Rows)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }

                    int iter = 0;
                    foreach (var cell in row.Cells)
                    {
                        if (iter >= ds.Count)
                        {
                            break;
                        }

                        string valueToAdd = cell == null || cell.Text.Replace(",", "").Trim() == null ? "[Not Available]" : cell.Text.Replace(",", "").Trim();
                        ds[iter].Data.Add(valueToAdd == "-0" ? "0" : valueToAdd);

                        if (ds[iter].Data.Last()[0] == '[')
                        {
                            ds[iter].ContainsEmptyEntry = true;
                        }

                        double parseValue;
                        valueToAdd = valueToAdd.Replace("$", "").Trim();

                        ds[iter].AllNumbers = ds[iter].AllNumbers && (double.TryParse(valueToAdd, out parseValue) || valueToAdd == "" || (valueToAdd[0] == '[' && valueToAdd[valueToAdd.Length - 1] == ']'));
                        
                        ++iter;
                    }
                }

                parseData();
                ++worksheetItter;
            }
        }

        void parseData()
        {
            for (int i = 0; i < ds.Count; ++i)
            {
                ds[i].UniquEntries = ds[i].Data.Distinct().ToList().Count;

                if (ds[i].AllNumbers)
                {
                    foreach (var num in ds[i].Data)
                    {
                        double parseValue;
                        if (double.TryParse(num, out parseValue))
                        {
                            if (ds[i].NumberRange.First == -1 && ds[i].NumberRange.Second == -1)
                            {
                                ds[i].NumberRange.First = parseValue;
                                ds[i].NumberRange.Second = parseValue;
                            }
                            else
                            {
                                ds[i].NumberRange.First = Math.Min(ds[i].NumberRange.First, parseValue);
                                ds[i].NumberRange.Second = Math.Max(ds[i].NumberRange.Second, parseValue);
                            }
                        }
                    }
                }
            }
        }
    }
}