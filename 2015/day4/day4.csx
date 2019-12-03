using System.Security.Cryptography;
// Hexadecimal Hash Generator :)
const string SECRET = "iwrupvqb";
int targetZeroes = 6;
string target = new string('0', targetZeroes);
int seed = 0;
string hash;

// compute new hashes until we reache the required zero ammount
do
{
    seed++;
    hash = Hash(SECRET, seed);
}
while (hash.Substring(0, 6) != target);

Console.WriteLine("hash: " + hash + " seed: " + seed);

// Compute the hash of the combined secret + number
string Hash(string secret, int number)
{

    using var md5Hash = MD5.Create();
    var bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(secret + number.ToString()));
    StringBuilder sb = new StringBuilder();

    for (int i = 0; i < bytes.Length; i++)
    {
        sb.Append(bytes[i].ToString("x2"));
    }

    return sb.ToString();
}
