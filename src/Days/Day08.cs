using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2020, 8)]
    public class Day08 : BaseDay
    {
        public override string PartOne(string input)
        {
            var vm = new BootCodeVM(input);
            var result = vm.Run();

            return result.ToString();
        }

        public override string PartTwo(string input)
        {
            var instructions = input.ParseLines(BootCodeVM.ParseInstruction).ToList();

            for (var i = 0; i < instructions.Count; i++)
            {
                var original = instructions[i];

                if (instructions[i].instruction == "nop")
                {
                    instructions[i] = ("jmp", instructions[i].arg);
                }
                else if (instructions[i].instruction == "jmp")
                {
                    instructions[i] = ("nop", instructions[i].arg);
                }
                else
                {
                    continue;
                }

                var vm = new BootCodeVM(instructions);
                var result = vm.Run();

                if (!vm.InfiniteLoop)
                {
                    return result.ToString();
                }

                instructions[i] = original;
            }

            throw new Exception();
        }

        public class BootCodeVM
        {
            private long _accumulator;
            private int _ip = 0;
            private List<(string instruction, int arg)> _instructions;

            public bool InfiniteLoop { get; private set; }

            public BootCodeVM(string program)
            {
                _instructions = program.ParseLines(ParseInstruction).ToList();
            }

            public BootCodeVM(List<(string instruction, int arg)> program)
            {
                _instructions = program;
            }

            public static (string instruction, int arg) ParseInstruction(string arg)
            {
                return (arg.Words().First(), int.Parse(arg.Words().Last()));
            }

            public long Run()
            {
                InfiniteLoop = false;
                var seen = new List<int>();

                while (!seen.Contains(_ip))
                {
                    seen.Add(_ip);
                    ExecuteInstruction(_instructions[_ip++]);

                    if (_ip == _instructions.Count)
                    {
                        return _accumulator;
                    }
                }

                InfiniteLoop = true;
                return _accumulator;
            }

            private void ExecuteInstruction((string instruction, int arg) instruction)
            {
                if (instruction.instruction == "acc")
                {
                    _accumulator += instruction.arg;
                }

                if (instruction.instruction == "jmp")
                {
                    _ip += instruction.arg - 1;
                }
            }
        }
    }
}
