package main

import (
	"bufio"
	"log"
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
		total += val / 3 - 2
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

		fuel := val / 3 - 2
		for fuel > 0 {
			total += fuel
			fuel = fuel / 3 - 2
		}
	}
	log.Println("Part 2 - Fuel's fuel:", total)
}
