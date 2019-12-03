var lines = File.ReadAllLines("input.txt")
   .Where(line =>
   {
       int count = 0;
       var pairs = new Dictionary<string, int>();
       for (int i = 0; i < line.Length - 1; i++)
       {
           var window = line[i..(i + 2)];
           var exists = pairs.TryGetValue(window, out int index);
           if (exists)
           {
               if (i - index != 1 && i - index != 0)
               {
                   count++;
                   pairs.Remove(window);
               }
           }
           else
           {
               pairs.Add(window, i);
           }
       }
       return count >= 1;
   }).Where(line =>
   {
       for (int i = 0; i < line.Length - 2; i++)
       {
           if (line[i] == line[i + 2])
           {
               return true;
           }
       }
       return false;
   }).Count();

Console.WriteLine(lines);