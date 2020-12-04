using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Day4
{
    public class Passport
    {
        [Required] public int? Byr { get; init; }
        [Required] public int? Iyr { get; init; }
        [Required] public int? Eyr { get; init; }
        [Required] public string Hgt { get; init; }
        [Required] public string Hcl { get; init; }
        [Required] public string Ecl { get; init; }
        [Required] public string Pid { get; init; }
        public int? Cid { get; init; }

        public Passport(string passport)
        {
            var tokens = passport.Replace("\n", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var token in tokens)
            {
                var key = token.Split(":")[0].Trim();
                var val = token.Split(":")[1].Trim();
                switch (key)
                {
                    case "byr": Byr = int.Parse(val); break;
                    case "iyr": Iyr = int.Parse(val); break;
                    case "eyr": Eyr = int.Parse(val); break;
                    case "hgt": Hgt = val; break;
                    case "hcl": Hcl = val; break;
                    case "ecl": Ecl = val; break;
                    case "pid": Pid = val; break;
                    case "cid": Cid = int.Parse(val); break;
                }
            }
        }

        public bool IsValidPart1()
        {
            var ctx = new ValidationContext(this);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(this, ctx, results);
        }

        public bool IsValidPart2()
        {
            var validator = new PassportValidator();
            return validator.Validate(this).IsValid;
        }
    }
}