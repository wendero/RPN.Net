# RPN.Tool
RPN.Tool is a RPN Evaluator tool based on RPN.Net. 

A easy way to evaluate RPN at CLI.

## Installation
To install, run the following command:

```
> dotnet tool install -g RPN.Tool
```

## Usage
```
> rpn [options] <expression>
```

### Reading Files
RPN.Tool has a feature to parse text files. After parsing, the file will be added into Data list and can be access as any other parameter.

```
> rpn "$0 3 line" -f myfile.txt
```

## Limitations
RPN.Tool is just a wrapper for RPN.Net. Hence, all features and limitations from RPN.Net are also present at RPN.Tool.

### Character Escaping
At Linux/Unix based environments, some characters (i.e. \\, $ and \`) need to be escaped.
