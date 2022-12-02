package main

import (
	"bufio"
	"fmt"
	"os"
	"sort"
	"strconv"
)

func main() {
	file, _ := os.Open("part1.txt")
	scanner := bufio.NewScanner(file)
	calories := make([]int, 0)

	currentCals := 0
	for scanner.Scan() {
		line := scanner.Text()
		if line == "" {
			calories = append(calories, currentCals)
			currentCals = 0
			continue
		}

		foodCalories, _ := strconv.Atoi(line)
		currentCals += foodCalories
	}
	sort.Sort(sort.Reverse(sort.IntSlice(calories)))
	fmt.Printf("Part 1: %d\n", calories[0])
	fmt.Printf("Part 2: %d\n", calories[0]+calories[1]+calories[2])
}
