using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ProgramArguments
{
    [Option('i')]
    public string inputFile { get; set; }
    [Option('o')]
    public string outFile { get; set; }
    [Option('u')]
    public string user { get; set; }
    [Option('k')]
    public string keyboard { get; set; }
    [Option('b')]
    public string blacklist { get; set; } = "";

}

