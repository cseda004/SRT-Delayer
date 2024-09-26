namespace srtdelayer {

    class Program {

        static List<section> sections = new List<section>();

        static void Main(string[] args) {

            Console.Write("Select a subtitle file: ");
            string input = Console.ReadLine().Replace("\u0022", "");

            string[] parts = File.ReadAllLines(input);

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "") parts[i] = "#####";
            }

            string text = "";
            foreach (var item in parts)
            {
                text += item + "\n";
            }

            File.WriteAllText("temp",text);

            string[] parts2 = File.ReadAllLines("temp");

            sections.Add(new section());
            foreach (var item in parts2)
            {
                if (item == "#####") sections.Add(new section());
                else {

                    if (sections[sections.Count - 1].index == -1) { sections[sections.Count - 1].index = int.Parse(item); }
                    else{
                        if (sections[sections.Count - 1].sH == -1)
                        {
                            string dat = item.Replace(",", ":");
                            string dat1 = dat.Split(" --> ")[0];
                            string dat2 = dat.Split(" --> ")[1];

                            string[] sParts = dat1.Split(':');
                            sections[sections.Count - 1].sH = int.Parse(sParts[0]);
                            sections[sections.Count - 1].sM = int.Parse(sParts[1]);
                            sections[sections.Count - 1].sS = int.Parse(sParts[2]);

                            if (sParts.Length > 3)
                                sections[sections.Count - 1].sMs = int.Parse(sParts[3]);

                            string[] eParts = dat2.Split(':');
                            sections[sections.Count - 1].eH = int.Parse(eParts[0]);
                            sections[sections.Count - 1].eM = int.Parse(eParts[1]);
                            sections[sections.Count - 1].eS = int.Parse(eParts[2]);

                            if (eParts.Length > 3)
                                sections[sections.Count - 1].eMs = int.Parse(eParts[3]);


                        }
                        else { sections[sections.Count - 1].text += item + "\n"; } 
                    }
                    



                }
            }


            Console.Write("Delay Hours With: ");
            int h = int.Parse(Console.ReadLine().Trim());

            Console.Write("Delay Minutes With: ");
            int m = int.Parse(Console.ReadLine().Trim());

            Console.Write("Delay Seconds With: ");
            int s = int.Parse(Console.ReadLine().Trim());

            Console.Write("Delay MS With: ");
            int ms = int.Parse(Console.ReadLine().Trim());

            foreach (var item in sections)
            {
                item.sH += h;
                item.sM += m;
                item.sS += s;
                item.sMs += ms;

                item.eH += h;
                item.eM += m;
                item.eS += s;
                item.eMs += ms;
            }

            foreach (var item in sections)
            {
                if (item.sMs >= 1000) { item.sS += dev(item.sMs, 1000); item.sMs -= (dev(item.sMs, 1000) * 1000); }
                if (item.sS >= 60) { item.sM += dev(item.sS, 60); item.sS -= (dev(item.sS, 60) * 60); }
                if (item.sM >= 60) { item.sH += dev(item.sM, 60); item.sM -= (dev(item.sM, 60) * 60); }

                if (item.eMs >= 1000) { item.eS += dev(item.eMs, 1000); item.eMs -= (dev(item.eMs, 1000) * 1000); }
                if (item.eS >= 60) { item.eM += dev(item.eS, 60); item.eS -= (dev(item.eS, 60) * 60); }
                if (item.eM >= 60) { item.eH += dev(item.eM, 60); item.eM -= (dev(item.eM, 60) * 60); }


                if (item.sMs <0) { item.sS -=1; item.sMs = 1000+item.sMs; }
                if (item.sS < 0) { item.sM -=1; item.sS = 60+item.sS; }

                if (item.eMs < 0) { item.eS -= 1; item.eMs = 1000 + item.eMs; }
                if (item.eS < 0) { item.eM -= 1; item.eS = 60 + item.eS; }

            }

            string output = "";

            foreach (var item in sections)
            {
                if (item.index != -1)
                {
                    output += item.index + "\n";
                    output += format(item.sH) + ":" + format(item.sM) + ":" + format(item.sS) + "," + format(item.sMs);
                    output += " --> ";
                    output += format(item.eH) + ":" + format(item.eM) + ":" + format(item.eS) + "," + format(item.eMs) + "\n";
                    output += item.text + "\n";
                }
            }

            Console.WriteLine(output);
            Console.WriteLine("-------------------");
            Console.Write("Save new file to: ");
            File.WriteAllText(Console.ReadLine().Replace("\u0022",""),output);





        }

        static string format(int n) {

            if (n.ToString().Length == 1) return "0" + n.ToString();
            else return n.ToString();
        
        
        
        
        }

        static int dev(int up, int down) { 
        
            double u = up; double d = down;

            double r = up / down;
            string temp = r.ToString().Replace(",", ".").Split('.')[0];
            return int.Parse(temp);
        
        }


    }


    public class section {

        public int index = -1;

        public int sH = -1;
        public int sM = -1;
        public int sS = -1;
        public int sMs = -1;

        public int eH = -1;
        public int eM = -1;
        public int eS = -1;
        public int eMs = -1;

        public string text = "";
    
    
    
    
    }


}