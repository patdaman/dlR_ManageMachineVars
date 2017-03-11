using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SignalrWebService.Performance
{
    public class PerformanceCategoryHelpers
    {
        public static List<PerformanceCounter> categoryCounters { get; private set; }
        public static List<string> categoriesUsed = "System|Process|ASP.NET Apps v4.0.30319|Network Adapter|Memory|Processor Information|Paging File|Job Object Details|TCPIP Performance Diagnostics|LogicalDisk|PowerShell Workflow|Objects|Physical Network Interface Card Activity|Web Service Cache|Web Service".Split('|').ToList();
        public static bool outputToFile = false;
        public static string filePath { get; set; }

        private static List<PerformanceCounterCategory> categoryList;
        private static PerformanceCounter[] counters;
        private static PerformanceCounterCategory[] categories;

        public PerformanceCategoryHelpers()
        { }

        public void GetCategories()
        {
            if (categoriesUsed.Count() > 0)
                categories = PerformanceCounterCategory.GetCategories().Where(cat => categoriesUsed.Contains(cat.CategoryName)).ToArray();
            else
                categories = PerformanceCounterCategory.GetCategories().ToArray();
            categoryList = categories.ToList();

            List<string> lines = new List<string>();
            if (outputToFile)
                lines.Add("CategoryName, CategoryType, MachineName, CategoryHelp");

            List<string> counterLines = new List<string>();
            if (outputToFile)
                counterLines.Add("CategoryName, CounterName, CounterType, InstanceLifetime, InstanceName, CounterHelp");

            foreach (var x in categories)
            {
                var instanceData = x.ReadCategory();
                var instanceNames = x.GetInstanceNames();
                var instanceCount = instanceNames.Count();
                if (instanceCount > 1)
                {
                    for (int z = 0; z < instanceCount - 1; z++)
                    {
                        try
                        {
                            counters = x.GetCounters(instanceNames[z]);
                            try
                            {
                                //foreach (var y in counters.Where(s => !errorTypes.Contains(s.CounterType.ToString())))
                                foreach (var y in counters)
                                {
                                    if (outputToFile)
                                        counterLines.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", y.CategoryName ?? "", y.CounterName ?? "", y.CounterType.ToString() ?? "", y.InstanceLifetime.ToString() ?? "", y.InstanceName.ToString() ?? "", y.CounterHelp ?? ""));
                                    else
                                        categoryCounters.Add(y);
                                }
                            }
                            catch
                            {
                                foreach (var y in counters)
                                {
                                    if (outputToFile)
                                        counterLines.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", y.CategoryName ?? "", y.CounterName ?? "", "", y.InstanceLifetime.ToString() ?? "", y.InstanceName.ToString() ?? "", y.CounterHelp ?? ""));
                                    else
                                        categoryCounters.Add(y);
                                }
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    try
                    {
                        counters = x.GetCounters();
                        try
                        {
                            foreach (var y in counters)
                            {
                                if (outputToFile)
                                    counterLines.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", y.CategoryName ?? "", y.CounterName ?? "", y.CounterType.ToString() ?? "", y.InstanceLifetime.ToString() ?? "", y.InstanceName.ToString() ?? "", y.CounterHelp ?? ""));
                                categoryCounters.Add(y);
                            }
                        }
                        catch
                        {
                            foreach (var y in counters)
                            {
                                if (outputToFile)
                                    counterLines.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", y.CategoryName ?? "", y.CounterName ?? "", "", y.InstanceLifetime.ToString() ?? "", y.InstanceName.ToString() ?? "", y.CounterHelp ?? ""));
                                categoryCounters.Add(y);
                            }
                        }
                    }
                    catch { }
                }
            }
            if (outputToFile)
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                System.IO.File.WriteAllLines(string.Format(@"{0}\Categories.csv", filePath), lines.ToArray());
                System.IO.File.WriteAllLines(@"{0}\Counters.csv", counterLines.ToArray());
            }
        }
    }
}