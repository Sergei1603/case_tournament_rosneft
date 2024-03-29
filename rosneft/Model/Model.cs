﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop;
using OfficeOpenXml;


namespace rosneft.Model
{
    internal class Model
    {
        string file = "Data.xlsx";
        int number_of_page = 1;


        private List<int> years;
        public List<int> Years
        {
            get { return years; }
        }


        private List<int> flow;
        public List<int> Flow
        {
            get { return flow; }
        }

        public Model()
        {
            Read();
        }

        void Read()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<int> year = new List<int>();
            List<int> flow = new List<int>();

            List<List<string>> data = new List<List<string>>();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(this.file)))
            {

                ExcelWorksheet worksheet = package.Workbook.Worksheets.ElementAt(this.number_of_page);
                if (worksheet != null)
                {
                    for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        List<string> row = new List<string>();
                        worksheet.Row(i);
                        for (int j = 1; j <= worksheet.Dimension.End.Column; j++)
                        {
                            row.Add(worksheet.Cells[i, j].Value.ToString());
                        }
                        year.Add(int.Parse(row[0]));
                        flow.Add(int.Parse(row[1]) - int.Parse(row[2]));
                    }
                }
            }
            this.years = year;
            this.flow = flow;
        }


        public double calculate(double discont, int last_year)
        {
            double answer = 0;
            int len = last_year - years[0] + 1;
            for (int i = 0; i < len; i++)
            {
                answer += flow[i] * 1 / Math.Pow(1 + discont, i+1);
            }
            return answer;
        }
    }
}
