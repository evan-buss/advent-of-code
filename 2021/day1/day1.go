package main

import (
	"fmt"
	"os"
	"strconv"
)

var input = []int{199, 200, 208, 210, 200, 207, 240, 269, 260, 263}

func main() {
	prev := -1000
	count := -1

	for _, v := range parseInputFile() {
		if v > prev {
			count++
		}
		prev = v
	}

	fmt.Println(count)
}

func parseInputFile() []int {
	output := make([]int, 0)

	lines, err := os.ReadFile("input.txt")
	if err != nil {
		panic(err)
	}

	for _, line := range lines {
		conv, err := strconv.Atoi(string(line)[:len(string(line))-1])
		if err != nil {
			panic(err)
		}

		output = append(output, conv)
	}

	return output
}
