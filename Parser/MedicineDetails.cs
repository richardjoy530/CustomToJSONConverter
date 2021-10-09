using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public class MedicineDetails
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public int MedicineConut { get; set; }
        public List<Tuple<int, string>> Medicines { get; set; }
        public List<MedicineDetails> SubItems { get; set; }
        public bool HasParent { get; set; }
        public MedicineDetails Parent { get; set; }
        public MedicineDetails(string rawString)
        {
            HasParent = false;
            var tokens = rawString.Split('#');
            Level = int.Parse(tokens[1].Trim().Split(' ')[0].Trim());
            Name = string.Join(" ", tokens[1].Trim().Split(' ').Skip(1).ToArray()).Replace(":", string.Empty);
            MedicineConut = int.Parse(tokens[2].Trim());
            Medicines = new List<Tuple<int, string>>();
            SubItems = new List<MedicineDetails>();
            var rawMedicines = tokens[3].Split(',');
            foreach (var medicine in rawMedicines)
            {
                if (medicine.Trim() != "")
                {
                    var key = int.Parse(medicine.Trim().Split(':')[0]);
                    var value = medicine.Trim().Split(':')[1];
                    Medicines.Add(new Tuple<int, string>(key, value));
                }
            }
        }

        // Not in use
        public void Process()
        {
            // Recursion limit exceeded
            //if (!HasParent)
            //{
            //    Program.FinalList.Add(this);
            //}

            //while (Program.Enumerator.MoveNext())
            //{
            //    var next = new MedicineDetails(Program.Enumerator.Current);
            //    if (Level > next.Level)
            //    {
            //        break;
            //    }
            //    else if (Level == next.Level)
            //    {
            //        if (HasParent)
            //        {
            //            next.HasParent = true;
            //            next.Parent = Parent;
            //            Parent.SubItems.Add(next);
            //        }
            //        next.Process();
            //    }
            //    else if (Level < next.Level)
            //    {
            //        next.HasParent = true;
            //        next.Parent = this;
            //        SubItems.Add(this);
            //        next.Process();
            //    }
            //}
        }

        public void AddToParentSubItems(MedicineDetails details)
        {
            if (HasParent)
            {
                details.HasParent = true;
                details.Parent = Parent;
                Parent.SubItems.Add(details);
            }
            else
            {
                Program.FinalList.Add(details);
            }
        }

        public override string ToString()
        {
            var medicines = string.Empty;
            if (Medicines.Count != 0)
            {
                foreach (var medicine in Medicines)
                {
                    medicines += $"{{\"{medicine.Item1}\": \"{medicine.Item2}\"}},\n";
                }
                medicines = medicines.RemoveTrailingComma();
            }
            var subitems = string.Empty;
            if (SubItems.Count != 0)
            {
                foreach (var subItem in SubItems)
                {
                    subitems += subItem.ToString();
                }
                subitems = subitems.RemoveTrailingComma();
            }

            return 
                $"{{\n" +
                $"\"name\": \"{Name}\",\n" +
                $"\"medicineCount\": \"{MedicineConut}\",\n" +
                $"\"medicines\": [{medicines}],\n" +
                $"\"subitems\": [{subitems}]\n" +
                $"}},\n"; 
        }
    }
}
