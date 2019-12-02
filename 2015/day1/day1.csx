var line = File.ReadAllText("input.txt");
var floors = 0;
var charArr = line.ToCharArray();
var reached = false;
for (int i = 0; i < charArr.Length; i++)
{
  if (line[i] == '(')
  {
   floors++; 
  } 
  else if (line[i] ==')')
  {
    floors--;
  } 

  if (floors == -1 && !reached)
  {
    Console.WriteLine("Reached the basement {0}", i + 1);
    reached = true;
  }
}

Console.WriteLine("Final Floor: {0}", floors);
