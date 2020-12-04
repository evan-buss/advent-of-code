using FluentValidation;
using System.Collections.Generic;

namespace Day4
{
    public class PassportValidator : AbstractValidator<Passport>
    {
        public PassportValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Byr).NotEmpty().InclusiveBetween(1920, 2002);
            RuleFor(x => x.Iyr).NotEmpty().InclusiveBetween(2010, 2020);
            RuleFor(x => x.Eyr).NotEmpty().InclusiveBetween(2020, 2030);
            RuleFor(x => x.Hgt).NotEmpty().NotNull().Must((hgt) => hgt.EndsWith("cm") || hgt.EndsWith("in"));
            RuleFor(x => x.Hgt).NotEmpty().Must((hgt) =>
            {
                if (hgt == null) return false;
                var height = int.Parse(hgt.Substring(0, hgt.Length - 2));
                if (hgt.EndsWith("cm") && (height < 150 || height > 193)) return false;
                if (hgt.EndsWith("in") && (height < 59 || height > 76)) return false;
                return true;
            });

            var validEcl = new HashSet<string>(new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" });

            RuleFor(x => x.Hcl).Matches("^#[a-f0-9]{6}$");
            RuleFor(x => x.Ecl).Must(x => validEcl.Contains(x));
            RuleFor(x => x.Pid).Matches("^[0-9]{9}$");
        }
    }
}