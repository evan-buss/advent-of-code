const readline = require("readline");
const fs = require("fs");

let fuelCalc = (fuel) => Math.floor((fuel / 3) - 2)
let modules = []

let rd = readline.createInterface({
    input: fs.createReadStream("input.txt"),
});

rd.on('line', (line) => {
    // Parse numbers from each line in the input file
    modules.push(parseInt(line));
}).on('close', () => {
    // Part 1
    let total = 0;
    modules.forEach((val) => {
        total += fuelCalc(val);
    });

    // Part 2
    console.log("Total Modules Fuel: ", total);
    total = 0;
    modules.forEach((val) => {
        let fuel = Math.floor((val / 3) - 2);
        while (fuel > 0) {
            total += fuel;
            fuel = Math.floor((fuel / 3) - 2);
        }
    });
    console.log("Total with Fuel's fuel:", total);
});
