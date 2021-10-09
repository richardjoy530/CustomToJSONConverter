using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HostCore
{
    public static class Program
    {
        public static List<MedicineDetails> FinalList;

        public static string InputPath;
        public static string OutputPath;
        static void Main(string[] args)
        {
            InputPath = args[0];
            OutputPath = args[1];

            FinalList = new List<MedicineDetails>();

            var parentStack = new Stack<MedicineDetails>();
            var allItems = new List<MedicineDetails>();

            foreach (var line in File.ReadLines(InputPath))
            {
                allItems.Add(new MedicineDetails(line));
            }
            var prevItem = allItems.First();
            foreach (var item in allItems)
            {
                if (prevItem.Level == item.Level)
                {
                    prevItem.AddToParentSubItems(item);
                    prevItem = item;
                }
                else if (prevItem.Level < item.Level)
                {
                    item.HasParent = true;
                    item.Parent = prevItem;
                    prevItem.SubItems.Add(item);
                    parentStack.Push(prevItem);
                    prevItem = item;
                }
                else if (prevItem.Level > item.Level)
                {
                    while (parentStack.Peek().Level > item.Level)
                    {
                        parentStack.Pop();
                    }
                    prevItem = parentStack.Peek();
                    prevItem.AddToParentSubItems(item);
                    prevItem = item;
                }
            }
            WriteToJSON();
        }

        public static void WriteToJSON()
        {
            using (StreamWriter fileStream = File.AppendText(OutputPath))
            {
                var json = string.Empty;
                foreach (var medicineDetail in FinalList)
                    json += medicineDetail;
                json = json.RemoveTrailingComma();
                fileStream.Write($"[{json}]");
            }
        }

        public static string RemoveTrailingComma(this string str)
        {
            return str.Remove(str.Length - 2, 2) + "\n";
        }
    }
}
