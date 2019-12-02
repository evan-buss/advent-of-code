package main

import (
	"bufio"
	"log"
	"math"
	"os"
	"strconv"
)

func main() {
	part1()
	part2()
}

func part1() {
	file, err := os.Open("input.txt")
	if err != nil {
		os.Exit(-1)
	}
	defer file.Close()

	total := 0
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		val, err := strconv.Atoi(scanner.Text())
		if err != nil {
			log.Fatal(err)
		}
		total += int(math.Round(float64(val/3)) - 2)
	}
	log.Println("Part 1 - Module Fuel:", total)

}

func part2() {
	file, err := os.Open("input.txt")
	if err != nil {
		os.Exit(-1)
	}
	defer file.Close()

	total := 0
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		val, err := strconv.Atoi(scanner.Text())
		if err != nil {
			log.Fatal(err)
		}

		fuel := int(math.Round(float64(val/3)) - 2)
		for fuel > 0 {
			total += fuel
			fuel = int(math.Round(float64(fuel/3)) - 2)
		}
	}
	log.Println("Part 2 - Fuel's fuel:", total)
}
